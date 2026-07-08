using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.BankNonInvoice;
using QPay.DAL.Repository;
using QPay.UI.BankNonInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class EmployeeSalaryReleaseRepository : IEmployeeSalaryReleaseRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public EmployeeSalaryReleaseRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }
        public async Task<DataSet> SearchEmployeeSalaryRelease(int CompanyId, int PayPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,
                ["@pay_frequency_Detail_id"] = PayPeriodId,
                ["@Mode"] = "Search"
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Search_EditSalaryProcessInitiation_Data_ExportToExcel",
                parameters,
                1500
            );
        }

        public async Task<DataSet> ExportToExcel(CommonExport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Convert.ToInt32(payload.companyId),
                ["@pay_frequency_Detail_id"] = Convert.ToInt32(payload.payPeriodId),
                ["@Mode"] = "Search"
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Search_EditSalaryProcessInitiation_Data_ExportToExcel",
                parameters,
                1500
            );
        }

        public async Task<EmployeeSalaryReleaseResponse> UploadEmployeeSalaryRelease(IFormFile file, string User)
        {
            EmployeeSalaryReleaseResponse response = new EmployeeSalaryReleaseResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

                var dirPath = Path.Combine(_configuration["SalaryReleaseKey"], "BankNonInvoice", "EmployeeSalaryRelease");               

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(dirPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ✅ SAME AS CLIENT ADVANCE → DataTable
                //DataTable dt = ReadExcelToDataTable(filePath);

                DataSet ds = new DataSet("NewDataSet");
                ds = ExcelToDataSet(filePath);
                ds.Tables[0].TableName = "Table";

                if (ds.Tables.Count == 0)
                {
                    response.response = "Excel sheet is empty.";
                    return response;
                }

                // ✅ IMPORTANT: MATCH SP XML FORMAT
                //dt.TableName = "Table";
               // DataSet ds = new DataSet("NewDataSet");
                //ds.Tables.Add(dt);

                string xmlInput = "";
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                // ✅ CALL SP (no hardcode changes — same as your current SP)
                var parameters = new DynamicParameters();
                parameters.Add("@xml", xmlInput);
                parameters.Add("@Createdby", User);

                var result = await _dbRepository.GetItemsAsync(
                    "Upload_NonInvoice_Salary_initiate",
                    parameters
                );

                // ✅ SAME RESPONSE HANDLING AS CLIENT ADVANCE
                if (!string.IsNullOrWhiteSpace(result))
                {
                    if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
                    {
                        response.response = result;
                    }
                    else
                    {
                        response.response = "Failed to import.";
                        response.errors = result
                            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();
                    }
                }
                else
                {
                    response.response = "No response from server.";
                }
            }
            catch (Exception ex)
            {
                response.response = "Error occurred.";
                response.errors = new List<string> { ex.Message };
            }

            return response;
        }
        private DataTable ReadExcelToDataTable(string filePath)
        {
            DataTable dt = new DataTable();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                            dt.Columns.Add(cell.Value.ToString().Trim());

                        firstRow = false;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;

                        foreach (var cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString().Trim();
                            i++;
                        }
                    }
                }
            }

            return dt;
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
