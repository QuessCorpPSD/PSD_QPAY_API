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
    public class ReleaseholdemployeesalaryRepository : IReleaseholdemployeesalaryRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ReleaseholdemployeesalaryRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
        public async Task<DataSet> search(int Company_Id, int Pay_Period_Id, int? Employee_Id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_Id,
                ["@Pay_Period_Id"] = Pay_Period_Id,
                ["@Employee_Id"] = Employee_Id
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllReleaseEmployeeSalaryDetail", parameters, 1500);
        }

        public async Task<DataSet> ExportToExcel(CommonExports payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Convert.ToInt32(payload.Company_Id),
                ["@Pay_Period_Id"] = payload.Pay_Period_Id,
                ["@Employee_Id"] = payload.Employee_Id
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllReleaseEmployeeSalaryDetail", parameters, 1500);
        }

        public async Task<ReleaseHoldSalaryResponse> UploadReleaseholdsalary(IFormFile file, string CreatedBy, string action)
        {
            ReleaseHoldSalaryResponse response = new ReleaseHoldSalaryResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

                var DirName = Path.Combine(_configuration["SalaryReleaseKey"].ToString(), "ReleaseHoldSalary", "ReleaseHoldEmpSalary");

                if (!Directory.Exists(DirName))
                    Directory.CreateDirectory(DirName);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(DirName, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Read Excel
                DataSet ds = new DataSet("NewDataSet");
                ds = ExcelToDataSet(filePath);
                ds.Tables[0].TableName = "Table";

                if (ds.Tables.Count == 0)
                {
                    response.response = "Excel sheet is empty.";
                    return response;
                }               

                string xmlInput = "";
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                string storeProcedure = "USP_ReleaseHoldEmployeeSalaryStatus";

                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", 3);
                parameters.Add("@Action", action);
                parameters.Add("@QZoneUserName", CreatedBy);


                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success",StringComparison.OrdinalIgnoreCase))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response = "Failed to import.";
                        response.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                else
                {
                    response.response = "Failed";
                }
            }
            catch (Exception ex)
            {
                response.response = ex.Message;
            }

            return response;
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

        private string BuildReleaseHoldSalaryXml(ReleaseHoldSalaryRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<NewDataSet>");

            foreach (var item in request.Data)
            {
                sb.Append("<Table>");
                sb.AppendFormat("<Company_Code>{0}</Company_Code>", item.Company_Code);
                sb.AppendFormat("<Employee_Code>{0}</Employee_Code>", item.Employee_Code);
                sb.AppendFormat("<Pay_Period>{0}</Pay_Period>", item.Pay_Period);
                sb.AppendFormat("<PURPOSE>{0}</PURPOSE>", item.PURPOSE);
                sb.AppendFormat("<BatchID>{0}</BatchID>", item.BatchID);
                sb.AppendFormat("<INPUT_NO>{0}</INPUT_NO>", item.INPUT_NO);
                sb.Append("</Table>");
            }

            sb.Append("</NewDataSet>");
            return sb.ToString();
        }
        public async Task<ReleaseHoldSalaryResponse> SaveReleaseHoldSalary(ReleaseHoldSalaryRequest request)
        {
            ReleaseHoldSalaryResponse response = new ReleaseHoldSalaryResponse();

            if (request == null || request.Data == null || !request.Data.Any())
            {
                response.response = "Invalid request.";
                return response;
            }

            var xmlInput = BuildReleaseHoldSalaryXml(request);

            string storeProcedure = "USP_ReleaseHoldEmployeeSalaryStatus";

            var parameters = new DynamicParameters();
            parameters.Add("XML_File", xmlInput);
            parameters.Add("CreatedBy", request.CreatedBy);
            parameters.Add("Action", request.Action);
            parameters.Add("QZoneUserName", request.User);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    if (res.Contains("Successfully" , StringComparison.OrdinalIgnoreCase) || res.Contains("successfully", StringComparison.OrdinalIgnoreCase))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response = "Failed to " + request.Action;
                        response.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    response.response = "Error while processing response.";
                }
            }
            else
            {
                response.response = "Failed";
            }

            return response;
        }
    }
}
