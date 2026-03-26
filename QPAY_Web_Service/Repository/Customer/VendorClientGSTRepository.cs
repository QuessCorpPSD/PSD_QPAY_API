using Dapper;
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
    public class VendorClientGSTRepository : IVendorClientGstRepository
    {
        private readonly DbRepository _dbRepository;

        public VendorClientGSTRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<VendorClientGSTGrid>> GetAllVendorClientGSTDetails(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Get");
            parameters.Add("@UserId ", userId);
            parameters.Add("@PageNo", 1);
            parameters.Add("@PageSize", 999999);


            var res = await this._dbRepository.GetItemsAsync("Proc_ManageVendorClientGst", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                try
                {
                    Console.WriteLine("Response JSON:");
                    Console.WriteLine(res);

                    return JsonConvert.DeserializeObject<List<VendorClientGSTGrid>>(res)
                           ?? new List<VendorClientGSTGrid>();
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

            return new List<VendorClientGSTGrid>();
        }

        public async Task<string> PostAddVendorClientGST(VendorClientGSTRequest Request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", Request.Action);
            parameters.Add("@UserId", Request.UserId);
            parameters.Add("@XmlData", Request.XmlData);
            parameters.Add("@VendorClientGstId", Request.VendorClientGstId);
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

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageVendorClientGst", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> PostDeleteVendorClientGST(int VendorClientGSTId, int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Delete");
            parameters.Add("@UserId", UserId);
            parameters.Add("@VendorClientGSTId", VendorClientGSTId);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageVendorClientGst", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<VendorClientGSTResponse> PostVendorClientGSTUpload(string xmlString, string userId)
        {
            VendorClientGSTResponse clientGSTDetails = new VendorClientGSTResponse();

            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Upload_VendorClientGST", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Rows Uploaded Successfully."))
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
        public DataSet VendorClientGSTExport(int userId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Export",
                ["@UserId"] = userId,
                ["@PageNo"] = 1,
                ["@PageSize"] = 99999,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageVendorClientGst", parameters, 1500);

        }
    }
}
