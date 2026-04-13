using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.DAL.Repository;
using QPay.UI.Models.AccountReceivableMod;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class ForecastRepository : IForecastRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public ForecastRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }

        public async Task<DataSet> Search(int? CompanyId, string PayPeriod, string Mode)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,
                ["@PayPeriod"] = PayPeriod,   // ✅ CHANGED
                ["@Mode"] = Mode
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Search_Get_Forecast_Data",
                parameters,
                1500
            );
        }

        public async Task<DataSet> ExportToExcel(ForecastExport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = payload.CompanyId,
                ["@PayPeriodId"] = payload.PayPeriod,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Search_Forecast_ExptToExcel",
                parameters,
                1500
            );
        }


        public async Task<DataSet> GetSBU()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Description"] = null,
                ["@Action"] = "SBU",
                ["@CreatedBy"] = 0
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_CommonDropDowns",
                parameters,
                1500
            );
        }

        public async Task<DataSet> GetRegion()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Description"] = null,
                ["@Action"] = "REGION",
                ["@CreatedBy"] = 0
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_CommonDropDowns",
                parameters,
                1500
            );
        }

        public async Task<DataSet> GetInvoiceNumber(int? CompanyId, int? PayPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companyid"] = CompanyId,
                ["@PayPriodid"] = PayPeriodId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_GetAllInvoiceNumberBasedonCompanyPay",
                parameters,
                1500
            );
        }

        private string BuildForecastXml(ForecastRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<BankInvoiceForeCastDetails>");

            foreach (var item in request.forecast)
            {
                sb.Append("<InvoiceForeCast>");

                sb.AppendFormat("<Fore_Cast_Id>{0}</Fore_Cast_Id>", item.Fore_Cast_Id);
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", item.Company_Id);
                sb.AppendFormat("<Company_Code>{0}</Company_Code>", item.Company_Code);
                sb.AppendFormat("<Pay_Period_Id>{0}</Pay_Period_Id>", item.Pay_Period_Id);
                sb.AppendFormat("<Region_Id>{0}</Region_Id>", item.Region_Id);
                sb.AppendFormat("<Sbu_Id>{0}</Sbu_Id>", item.Sbu_Id);
                sb.AppendFormat("<Projection_Amount>{0}</Projection_Amount>", item.Projection_Amount);
                sb.AppendFormat("<Collected_Amount>{0}</Collected_Amount>", item.Collected_Amount);
                sb.AppendFormat("<Balance_Amount>{0}</Balance_Amount>", item.Balance_Amount);
                sb.AppendFormat("<Final_Projection>{0}</Final_Projection>", item.Final_Projection);
                sb.AppendFormat("<Invoice_Id>{0}</Invoice_Id>", item.Invoice_Id);

                sb.Append("</InvoiceForeCast>");
            }

            sb.Append("</BankInvoiceForeCastDetails>");
            return sb.ToString();
        }

        public async Task<ForecastResponse> SaveUpdateDeleteForecast(ForecastRequest request)
        {
            ForecastResponse response = new ForecastResponse();

            if (request == null || request.forecast == null || !request.forecast.Any())
            {
                response.response = "Invalid request.";
                return response;
            }

            var xmlInput = BuildForecastXml(request);

            string storeProcedure = "sp_CreateUpdateFore1Cast";

            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@mode", request.Mode);
            parameters.Add("@CreatedBy", request.Created_By);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                if (res.Contains("Successfully"))
                {
                    response.response = res;
                }
                else
                {
                    response.response = "Failed to " + request.Mode;
                    response.errors = res.Split('\n').ToList();
                }
            }
            else
            {
                response.response = "Failed";
            }

            return response;
        }


        public async Task<ForecastResponse> UploadForecast(IFormFile file, string User)
        {
            ForecastResponse response = new ForecastResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

                var DirName = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Forecast");

                if (!Directory.Exists(DirName))
                    Directory.CreateDirectory(DirName);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(DirName, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ✅ Read Excel
                DataTable dt = ReadExcelToDataTable(filePath);

                if (dt.Rows.Count == 0)
                {
                    response.response = "Excel sheet is empty.";
                    return response;
                }

                // ✅ Convert to XML (same as MVC)
                dt.TableName = "Table";
                DataSet ds = new DataSet("NewDataSet");
                ds.Tables.Add(dt);

                string xmlInput = "";
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                // ✅ CALL FORECAST SP
                string storeProcedure = "SP_Fore_Cast_Upload_New";

                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);   // ⚠️ IMPORTANT NAME
                parameters.Add("@CreatedBy", User);

                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.Contains("Successfully"))
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



    }
}