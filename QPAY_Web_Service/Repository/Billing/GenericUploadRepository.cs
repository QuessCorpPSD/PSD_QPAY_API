using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.Billing;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.Models.Customer;
using QPay.UI.Models.Invoice;
using QPay.UI.Utilities;
using QPay.UI_Domain.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.BAL.Repository.Invoice.ProvisionalInvoiceRepository;
using static QPay.UI.Billing.GenericUpload;
using static QPay.UI.Customer.Company;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;

namespace QPay.BAL.Repository.Billing
{
    public class GenericUploadRepository : IGenericUploadRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public GenericUploadRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
       
        public async Task<DataSet> masters(int userId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = "BankInvoiceGenericUploadTypes",
                ["@CreatedBy"] = userId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_CommonDropDowns", parameters);
        }

        public async Task<DataSet> GetGenericTemplate(string uploadType)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@UploadType"] = uploadType,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetGenericUpload_Template", parameters, 1500);
        }

        public async Task<InvoiceResponse> PostGenericUpload(string xmlString, string userId, string uploadType)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();
            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);
            parameters.Add("@UploadType", uploadType);


            var res = await this._dbRepository.GetItemsAsync("Proc_Upload_PartialHoldEmployeeSalary", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Row(s) Uploaded Successfully.", StringComparison.OrdinalIgnoreCase))
                    {
                        invoiceDetails.response = message;
                    }
                    else
                    {
                        invoiceDetails.response = "Failed to import.";
                        invoiceDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    invoiceDetails.response = "Error while processing response.";
                }
            }
            else
            {
                invoiceDetails.response = "Failed";
            }
            return invoiceDetails;
        }

    }
}
