using ClosedXML.Excel;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Invoice;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Common.StandingDataEnum;

namespace QPay.BAL.Repository
{
    public class BillableDaysRepository : IBillableDaysRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public BillableDaysRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<string> BillableDaysUpload(string xmlData, string createdBy, int importType)
        {
            var storeProcedure = "";
            switch (importType)
            {
                case 1:
                    storeProcedure = "spImportArrearBillableDays";
                    break;
                case 2:
                    storeProcedure = "spImportBillableReport";
                    break;
                default:
                    storeProcedure = "spImportBillableDays";
                    break;
            }

            var parameter = new DynamicParameters();
            parameter.Add("@xmlInput", xmlData);
            parameter.Add("@CreatedBy", createdBy);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public async Task<List<BillableDaysUI>> SearchDetails(string mode, string value)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@mode", mode);
            parameter.Add("@Value2", value);

            var res = await _dbRepository.GetItemsAsync("sp_BillableDaysSearch", parameter);
            try
            {
                var list = JsonConvert.DeserializeObject<List<BillableDaysUI>>(res);
                return list?.ToList() ?? new List<BillableDaysUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<BillableDaysUI>();
            }

        }

        public async Task<FileResponse> ExportToExcel(string xml)
        {
            var fileResponse = new FileResponse();
            var parameter = new DynamicParameters();
            parameter.Add("@Value2", xml);

            try
            {
                // Get the JSON result from the repository
                var res = await _dbRepository.GetItemsAsync("sp_BillableDaysExportToExcel", parameter);

                if (!string.IsNullOrEmpty(res))
                {
                    // Deserialize JSON into DataTable
                    var dt = JsonConvert.DeserializeObject<DataTable>(res) ?? new DataTable();

                    if (dt.Rows.Count > 0)
                    {
                        using var wb = new XLWorkbook();
                        wb.Worksheets.Add(dt, "BillableDays");

                        using var memoryStream = new MemoryStream();
                        wb.SaveAs(memoryStream);
                        var bytes = Convert.ToBase64String(memoryStream.ToArray());
                        fileResponse.File = bytes;
                        fileResponse.FileName = "Billable Days Details.xlsx";
                    }
                    else
                    {
                        fileResponse.File = "No";
                        fileResponse.FileName = "NoData.xlsx";
                    }
                }
                else
                {
                    fileResponse.File = "No";
                    fileResponse.FileName = "NoData.xlsx";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to export Billable Days to Excel: " + ex.Message, ex);
            }

            return fileResponse;
        }
        public async Task<FileResponse> DownloadTemplate(int importType)
        {
            var fileResponse = new FileResponse();
            string fileName = GetTemplateName(importType);
            DataTable dt = GetExcelColumnNames(fileName);

            if (dt.Columns.Count > 0)
            {
                using var wb = new XLWorkbook();
                wb.Worksheets.Add(dt, "Template");

                using var memoryStream = new MemoryStream();
                wb.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var bytes = Convert.ToBase64String(memoryStream.ToArray());
                fileResponse.File = bytes;
                fileResponse.FileName = fileName + "_Template.xlsx";
            }
            else
            {
                fileResponse.File = "No";
                fileResponse.FileName = "NoData.xlsx";
            }

            return await Task.FromResult(fileResponse); // wrap in a Task for async signature
        }


        private DataTable GetExcelColumnNames(string fileName)
        {
            DataTable dt = new DataTable();
            try
            {
                List<string> list = GetColumns(fileName);
                list.ForEach(u => dt.Columns.Add(u));
            }
            catch (Exception ex)
            {
                throw; // keep original stack trace
            }
            return dt;
        }

        private List<string> GetColumns(string fileName)
        {
            string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "CustomJson", "UploadTemplates.json");
            JObject o1 = JObject.Parse(System.IO.File.ReadAllText(jsonPath));
            var colList = o1[fileName]?.Select(u => u.ToString()).ToList();

            if (colList == null || !colList.Any())
            {
                throw new Exception($"No column definitions found for {fileName} in UploadTemplates.json");
            }

            return colList;
        }

        // Stub method — replace with your actual logic
        private string GetTemplateName(int importType)
        {
            return importType switch
            {
                1 => "ArrearBillableDays",
                2 => "BillableReport",
                _ => "BillingDays"
            };
        }

        public async Task<List<UI.Models.Invoice.InvoiceTypeUI>> GetGSTInvoiceType()
        {
            string storeProcedure = "[dbo].[SP_GET_INVOICE_TYPE]" ?? "";
            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<UI.Models.Invoice.InvoiceTypeUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<UI.Models.Invoice.InvoiceTypeUI>>(res);
                return list?.ToList() ?? new List<UI.Models.Invoice.InvoiceTypeUI>();
            }
            catch (JsonException ex)
            {
                return new List<UI.Models.Invoice.InvoiceTypeUI>();
            }
        }
    }
}
