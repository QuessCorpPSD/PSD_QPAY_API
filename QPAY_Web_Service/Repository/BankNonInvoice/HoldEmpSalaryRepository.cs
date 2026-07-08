using Azure;
using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.BankNonInvoice;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class HoldEmpSalaryRepository : IHoldEmpSalaryRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public HoldEmpSalaryRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }
        public async Task<DataSet> GetSalaryHoldType()
        {
            var parameters = new Dictionary<string, object?>();

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_GetSalaryHoldTypeList_QZONE",
                parameters,
                1500
            );
        }

        public async Task<DataSet> SearchHoldEmpSalary(int CompanyId, int PayPeriodId, string Status)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@PayPeriod_Id"] = PayPeriodId,
                ["@status"] = Status
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetAllEmployeeSalaryDetails",
                parameters,
                1500
            );
        }

        public async Task<DataSet> ExportToExcel(HoldEmpSalaryExportRequest payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = payload.CompanyId,
                ["@PayPeriod_Id"] = payload.PayPeriodId,
                ["@status"] = payload.Status
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetAllEmployeeSalaryDetails",
                parameters,
                1500
            );
        }

        public async Task<HoldEmpSalaryResponse> UploadHoldEmpSalary(IFormFile file, string User)
        {
            HoldEmpSalaryResponse result = new HoldEmpSalaryResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    result.response = "File not found";
                    return result;
                }

                var dirName = Path.Combine(
                    _configuration["SalaryReleaseKey"].ToString(),
                    "BankNonInvoice",
                    "HoldEmpSalary"
                );

                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(dirName, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ✅ SAME AS CLIENT ADVANCE
                DataSet ds = new DataSet("NewDataSet");
                ds = ExcelToDataSet(filePath);
                ds.Tables[0].TableName = "Table";

                if (ds.Tables.Count == 0)
                {
                    result.response = "Excel sheet is empty.";
                    return result;
                }              

                string xmlInput = "";
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                // ✅ SAME PARAM STYLE AS WORKING CODE
                var parameters = new DynamicParameters();
                parameters.Add("@xml", xmlInput);
                parameters.Add("@Createdby", 3); // same as old system
                parameters.Add("@QZoneUserName", User);

                var res = await _dbRepository.GetItemsAsync(
                    "Upload_HoldEmployeeSalary", parameters);

                // ✅ SAME RESPONSE HANDLING STYLE
                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success", StringComparison.OrdinalIgnoreCase))
                    {
                        result.response = res;
                    }
                    else
                    {
                        result.response = "Failed to import.";
                        result.errors = res
                            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();
                    }
                }
                else
                {
                    result.response = "Failed";
                }
            }
            catch (Exception ex)
            {
                result.response = ex.Message;
            }

            return result;
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable(worksheet.Name);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
                                        .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }
    }


}
