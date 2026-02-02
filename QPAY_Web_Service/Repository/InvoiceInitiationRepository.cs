using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository
{
    public class InvoiceInitiationRepository: IInvoiceInitiationRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _config;
        
        public InvoiceInitiationRepository(DbRepository dbRepository, IConfiguration config)
        {
            this._dbRepository = dbRepository;
            this._config = config;
          
        }
        public async Task<List<CommonUI>> GetTaxTypes(string action)
        {
       
            string storeProcedure = "[dbo].[USP_CommonDropDowns]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", action);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<CommonUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<CommonUI>>(res);
                return list?.ToList() ?? new List<CommonUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<CommonUI>();
            }

        }

        public async Task<List<InvoiceInitiationUI>> Search(int? Company_Id, string PayPeriod, int? TaxTypeId)
        {
            string storeProcedure = "[dbo].[sp_GetAllInvoiceInitiateDetails2]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@PayPeriodId", PayPeriod ?? (object)DBNull.Value);
            parameter.Add("@CompanyId", Company_Id ?? (object)DBNull.Value);
            parameter.Add("@TaxTypeId", TaxTypeId ?? (object)DBNull.Value);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            var list = JsonConvert.DeserializeObject<List<InvoiceInitiationUI>>(res);
            return list?.ToList() ?? new List<InvoiceInitiationUI>();
            //return res?.ToList() ?? new List<InvoiceInitiationUI>
            //{
            //    Error_Message = string.Empty
            //};
        }

       
        public async Task<List<InitiationRequestUI>> InitiationSearch(InitiationRequestModel initiationRequestModel)
        {
            string storeProcedure = "[dbo].[SP_Invoice_Initiation_search]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", initiationRequestModel.Company_Id ?? (object)DBNull.Value);
            parameter.Add("@PayPeriod_Id", initiationRequestModel.PayPeriod_Id ?? (object)DBNull.Value);            
            parameter.Add("@InvoiceType", initiationRequestModel.InvoiceType ?? (object)DBNull.Value);
            parameter.Add("@ActionType", initiationRequestModel.ActionType ?? (object)DBNull.Value);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            var list = JsonConvert.DeserializeObject<List<InitiationRequestUI>>(res);
            return list?.ToList() ?? new List<InitiationRequestUI>();
        }

        public async Task<FileResponse> InitiationSearchExport(InitiationRequestModel initiationRequestModel)
        {
            string storeProcedure = "[dbo].[SP_Invoice_Initiation_search]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", initiationRequestModel.Company_Id ?? (object)DBNull.Value);
            parameter.Add("@PayPeriod_Id", initiationRequestModel.PayPeriod_Id ?? (object)DBNull.Value);
            parameter.Add("@InvoiceType", initiationRequestModel.InvoiceType ?? (object)DBNull.Value);
            parameter.Add("@ActionType", initiationRequestModel.ActionType ?? (object)DBNull.Value);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            DataTable list =(DataTable) JsonConvert.DeserializeObject<DataTable>(res);
            
            using var workbook = new XLWorkbook();
            {
                var ws = workbook.AddWorksheet(list, "Invoice");
                ws.Table(0).ShowAutoFilter = false;
                ws.Table(0).Theme = XLTableTheme.None;
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());
                    FileResponse fileResponse = new FileResponse();
                    fileResponse.FileName = "Invoice";
                    fileResponse.File = bytes;
                    return fileResponse;//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                }
            }
        }

        public async Task<InvoiceInitiationUI> InvoiceInitiate(int? TaxTypeId, string xml, string action, int userId)
        {
            string storeProcedure = "[dbo].[Proc_ManageGstInvoiceInitiate_Online]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@xmlInput", xml ?? (object)DBNull.Value);
            parameter.Add("@mode", action ?? (object)DBNull.Value);
            parameter.Add("@CreatedBy", userId);
            try
            {
                var res = await _dbRepository.GetItemsAsync<InvoiceInitiationUI>(storeProcedure, parameter);
                return res?.FirstOrDefault() ?? new InvoiceInitiationUI
                {
                    Error_Message = "GST Invoice not Initiated" //string.Empty
                };
            }
            catch(Exception ex)
            {
              return  new InvoiceInitiationUI
                {
                    Error_Message = "GST Invoice not Initiated" 
                };
            }            
        }
        public async Task<FileResponse> ExportToExcel(int? CompanyId, string PayPeriodId, int? TaxTypeId)
        {
            var fileResponse = new FileResponse();
            var parameter = new DynamicParameters();
            parameter.Add("@CompanyId",  CompanyId);
            parameter.Add("@PayPeriodId",  PayPeriodId);
            parameter.Add("@TaxTypeId",  TaxTypeId);

            try
            {
                // Get the JSON result from the repository
                var res = await _dbRepository.GetItemsAsync("Proc_GetAllInvoiceInitiateDetails2ExportToExcel", parameter);

                if (!string.IsNullOrEmpty(res))
                {
                    // Deserialize JSON into DataTable
                    var dt = JsonConvert.DeserializeObject<DataTable>(res) ?? new DataTable();

                    if (dt.Rows.Count > 0)
                    {
                        using var wb = new XLWorkbook();
                        wb.Worksheets.Add(dt, "InvoiceInitiate");

                        using var memoryStream = new MemoryStream();
                        wb.SaveAs(memoryStream);
                        var bytes = Convert.ToBase64String(memoryStream.ToArray());
                        fileResponse.File = bytes;
                        fileResponse.FileName = "InvoiceInitiate.xlsx";
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
                throw new Exception("Failed to export Invoice Initiate to Excel: " + ex.Message, ex);
            }

            return fileResponse;
        }
    }
    }

