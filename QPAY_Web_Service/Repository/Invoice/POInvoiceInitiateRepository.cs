using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.BAL.Repository.Invoice.InvoiceRepository;

namespace QPay.BAL.Repository.Invoice
{
    public class POInvoiceInitiateRepository : IPOInvoiceInitiateRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _config;

        public POInvoiceInitiateRepository(DbRepository dbRepository, IConfiguration config)
        {
            this._dbRepository = dbRepository;
            this._config = config;

        }
        public async Task<List<POInvoiceInitiate>> Search(int companyId, int payPeriodId)
        {
            string storeProcedure = "[dbo].[Proc_ManageMagnaGstInvoiceInitiate]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "Search");
            parameter.Add("@Company_Id", companyId);
            parameter.Add("@Pay_Period_Id", payPeriodId);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (!string.IsNullOrEmpty(res))
            {
                var list = JsonConvert.DeserializeObject<List<POInvoiceInitiate>>(res);
                return list?.ToList() ?? new List<POInvoiceInitiate>();
            }
            else
            {
                return new List<POInvoiceInitiate>();
            }
        }

        public async Task<DataSet> POInvoiceRequest(int companyId, int payPeriodId, string flag)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = flag,
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_Invoice_Initiation_CostPlus", parameters, 1500);

        }

        public async Task<string> POInvoiceInitiate(string xml, int createdBy)
        {
            string storeProcedure = "[dbo].[Proc_ManageMagnaGstInvoiceInitiate]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@xmlData", xml ?? (object)DBNull.Value);
            parameter.Add("@Action", "Initiate" ?? (object)DBNull.Value);
            parameter.Add("@Created_By", createdBy);
            try
            {
                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
                if (!string.IsNullOrEmpty(res))
                {
                    return res;
                }
                else
                {
                    return "No data found";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DataSet POInvoiceInitiateExport(int companyId, int payPeriodId)
        {
            DataSet ds = this._dbRepository.POInvoiceInitiateExport(companyId, payPeriodId);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given Parameters.");
            }

        }

        public async Task<PoIntiateResponse> Upload(IFormFile file, [FromForm] string User)
        {
            PoIntiateResponse processDetails = new PoIntiateResponse();
            if (file == null || file.Length == 0)
            {
                processDetails.response = "File is missing.";
                return processDetails;
            }


            string DirName = "";

            DirName = Path.Combine(_config["ClaimDocPath"].ToString());
            DirName += "PO_INVOICE";
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            string fileExtention = Path.GetExtension(file.FileName.ToUpper());
            string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
            FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;

            string serverpath = DirName + FileName;

            using (var stream = new FileStream(serverpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            DataSet ds = new DataSet("NewDataSet");
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)
            {
                processDetails.response = "Excel sheet is empty or not formatted correctly.";
                return processDetails;
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                processDetails.response = "Excel sheet is empty or not formatted correctly.";
                return processDetails;

            }

            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();

            string storeProcedure = "spImportPOBillableDays";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@CreatedBy", User);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && (message.Contains("Successfully")
                        || message.Contains("successfully")))
                    {
                        processDetails.response = message;
                    }
                    else
                    {
                        processDetails.response = "Failed to import.";
                        processDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    processDetails.response = "Error while processing response.";
                }
            }
            else
            {
                processDetails.response = "Failed";
            }

            return processDetails;
        }

        //public async Task<PoIntiateResponse> Upload(string xmlData, string createdBy)
        //{
        //    PoIntiateResponse processDetails = new PoIntiateResponse();
        //    var parameter = new DynamicParameters();
        //    parameter.Add("@xmlInput", xmlData);
        //    parameter.Add("@CreatedBy", createdBy);
        //    var res = await _dbRepository.GetItemsAsync("spImportPOBillableDays", parameter);

        //    if (!string.IsNullOrWhiteSpace(res))
        //    {
        //        try
        //        {
        //            var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
        //            var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

        //            if (!string.IsNullOrWhiteSpace(message) && (message.Contains("Successfully")
        //                || message.Contains("successfully")))
        //            {
        //                processDetails.response = message;
        //            }
        //            else
        //            {
        //                processDetails.response = "Failed to import.";
        //                processDetails.errors = res
        //                    ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        //                    .ToList() ?? new List<string> { "Unknown error." };
        //            }
        //        }
        //        catch
        //        {
        //            processDetails.response = "Error while processing response.";
        //        }
        //    }
        //    else
        //    {
        //        processDetails.response = "Failed";
        //    }

        //    return processDetails;
        //}

    }
}
