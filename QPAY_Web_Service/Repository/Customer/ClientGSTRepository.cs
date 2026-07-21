using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.BAL.Repository.EInvoiceRepository;

namespace QPay.BAL.Repository.Customer
{
    public class ClientGSTRepository : IClientGSTRepository
    {
        private readonly DbRepository _dbRepository;

        public ClientGSTRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<ClientGSTGrid>> GetAllClientGSTDetails(ClientGSTSearch searchparams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Get");
            parameters.Add("@ClientGstId", searchparams.ClientGstId);
            parameters.Add("@Company_Code", searchparams.Company_Code);
            parameters.Add("@Group_Name", searchparams.Group_Name);
            parameters.Add("@State_Name", searchparams.State_Name);
            parameters.Add("@ClientInvoicingState_Name", searchparams.ClientInvoicingState_Name);
            parameters.Add("@InvoicingState_Name", searchparams.InvoicingState_Name);
            parameters.Add("@GstTypeName", searchparams.GstTypeName);
            parameters.Add("@GstNumber", searchparams.GstNumber);
            parameters.Add("@PanNumber", searchparams.PanNumber);
            parameters.Add("@TanNumber", searchparams.TanNumber);
            parameters.Add("@UserName", searchparams.UserName);
            parameters.Add("@SapCustomerCode", searchparams.SapCustomerCode);
            parameters.Add("@InvoiceCategory", searchparams.InvoiceCategory);
            parameters.Add("@PageNo", searchparams.PageNo);
            parameters.Add("@PageSize", searchparams.PageSize);
            parameters.Add("@TotalCount", searchparams.TotalCount);
            parameters.Add("@UserId", searchparams.UserId);
            var res = await this._dbRepository.GetItemsAsync("Proc_ManageClientGst", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                try
                {
                    Console.WriteLine("Response JSON:");
                    Console.WriteLine(res);

                    return JsonConvert.DeserializeObject<List<ClientGSTGrid>>(res)
                           ?? new List<ClientGSTGrid>();
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("❌ JSON Deserialization failed:");
                    Console.WriteLine($"Message: {ex.Message}");
                    //Console.WriteLine($"Path: {ex.Path}");
                    //Console.WriteLine($"Line: {ex.LineNumber}, Position: {ex.LinePosition}");
                    Console.WriteLine($"JSON: {res}");

                    throw;
                }
            }

            return new List<ClientGSTGrid>();
        }

        public async Task<string> PostAddClientGST(ClientGSTRequest Request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", Request.Action);
            parameters.Add("@UserId", Request.UserId);
            parameters.Add("@XmlData", Request.XmlData);
            parameters.Add("@ClientGstId", Request.ClientGstId);
            parameters.Add("@CompanyId", Request.CompanyId);
            parameters.Add("@StateId", Request.StateId);
            parameters.Add("@GstNumber", Request.GstNumber);
            parameters.Add("@PanNumber", Request.PanNumber);
            parameters.Add("@TanNumber", Request.TanNumber);
            parameters.Add("@InvoicingStateId", Request.InvoicingStateId);
            parameters.Add("@ClientInvoicingStateId", Request.ClientInvoicingStateId);
            parameters.Add("@CreatedBy", Request.CreatedBy);
            //parameters.Add("@CreatedOn", Request.CreatedOn);
            parameters.Add("@Group_Detail_Id", Request.Group_Detail_Id);
            parameters.Add("@PageNo", Request.PageNo);
            parameters.Add("@PageSize", Request.PageSize);
            parameters.Add("@SortField", Request.SortField);
            parameters.Add("@SortDirection", Request.SortDirection);
            parameters.Add("@TotalCount", Request.TotalCount);
            parameters.Add("@Company_Code", Request.Company_Code);
            parameters.Add("@State_Name", Request.State_Name);
            parameters.Add("@InvoicingState_Name", Request.InvoicingState_Name);
            parameters.Add("@ClientInvoicingState_Name", Request.ClientInvoicingState_Name);
            parameters.Add("@UserName", Request.UserName);
            parameters.Add("@Group_Name", Request.Group_Name);
            parameters.Add("@Remarks", Request.Remarks);
            parameters.Add("@GstTypeId", Request.GstTypeId);
            parameters.Add("@GstTypeName", Request.GstTypeName);
            parameters.Add("@SapCustomerCode", Request.SapCustomerCode);
            parameters.Add("@InvoiceCategoryId", Request.InvoiceCategoryId);
            parameters.Add("@InvoiceCategory", Request.InvoiceCategory);
            parameters.Add("@StateCode", Request.StateCode);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageClientGst", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> PostDeleteClientGST(int ClientGSTId, int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Delete");
            parameters.Add("@UserId", UserId);
            parameters.Add("@ClientGSTId", ClientGSTId);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageClientGst", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<ClientGSTResponse> PostClientGSTUpload(string xmlString, string flag, string userId)
        {
            ClientGSTResponse clientGSTDetails = new ClientGSTResponse();

            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@Flag", flag);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Upload_ClientGST", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Row(s) Uploaded Successfully."))
                    {
                        clientGSTDetails.response = message;
                    }
                    else
                    {
                        clientGSTDetails.response = "Failed to import.";
                        clientGSTDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    clientGSTDetails.response = "Error while processing response.";
                }
            }
            else
            {
                clientGSTDetails.response = "Failed";
            }
            return clientGSTDetails;
        }
        public DataSet ClientGSTExport(int userId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Export",
                ["@UserId"] = userId,
                ["@PageNo"] = 1,
                ["@PageSize"] = 99999,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageClientGst", parameters, 1500);

        }
    }
}
