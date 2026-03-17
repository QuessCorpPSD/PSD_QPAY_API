using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.Models.Customer;
using System.Data;

namespace QPay.BAL.Repository.Customer
{
    public class ClientAddressRepository : IClientAddressRespository
    {
        private readonly DbRepository _dbRepository;

        public ClientAddressRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<ClientAddress>> GetAllClientAddressDetails(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Get");
            parameters.Add("@UserId ", userId);
            parameters.Add("@PageNo", 1);
            parameters.Add("@PageSize", 999999);


            var res = await this._dbRepository.GetItemsAsync("Proc_ManageClientAddress", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<ClientAddress>>(res) ?? new List<ClientAddress>();
            }

            return new List<ClientAddress>();
        }

        public async Task<string> PostAddClientAddress(AddressRequest addressRequest)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", addressRequest.Action);
            parameters.Add("@UserId", addressRequest.UserId);
            parameters.Add("@ClientAddressId", addressRequest.ClientAddressId);
            parameters.Add("@CompanyId", addressRequest.CompanyId);
            parameters.Add("@StateId", addressRequest.StateId);
            parameters.Add("@CostCenterMappingId", addressRequest.CostCenterMappingId);
            parameters.Add("@BillingClientName", addressRequest.BillingClientName);
            parameters.Add("@BillingAddress", addressRequest.BillingAddress);
            parameters.Add("@BillingStateId", addressRequest.BillingStateId);
            parameters.Add("@IsShippingAddressSameAsBilling", addressRequest.IsShippingAddressSameAsBilling);
            parameters.Add("@ShippingClientName", addressRequest.ShippingClientName);
            parameters.Add("@ShippingAddress", addressRequest.ShippingAddress);
            parameters.Add("@ShippingStateId", addressRequest.ShippingStateId);
            parameters.Add("@EffectiveDate", addressRequest.EffectiveDate);
            parameters.Add("@SEZ_Applicable", addressRequest.SEZ_Applicable);
            //parameters.Add("@SEZ_Document", addressRequest.SEZ_Document);
            parameters.Add("@SEZ_ExpiryDate", addressRequest.SEZ_ExpiryDate);
            parameters.Add("@LUT_Number", addressRequest.LUT_Number);
            parameters.Add("@LUT_Date", addressRequest.LUT_Date);
            parameters.Add("@LUT_ExpiryDate", addressRequest.LUT_ExpiryDate);
            parameters.Add("@VendorCode", addressRequest.VendorCode);
            parameters.Add("@SAC_Code", addressRequest.SAC_Code);
            parameters.Add("@GstNumber", addressRequest.GstNumber);
            parameters.Add("@CreatedBy", addressRequest.UserId);
            //parameters.Add("@CreatedOn", addressRequest.UserId);
            parameters.Add("@ModifiedBy", addressRequest.UserId);
            //parameters.Add("@ModifiedOn", addressRequest.UserId);
            //parameters.Add("@PageNo", addressRequest.UserId);
            //parameters.Add("@PageSize", addressRequest.UserId);
            //parameters.Add("@SortField", addressRequest.UserId);
            //parameters.Add("@SortDirection", addressRequest.UserId);
            //parameters.Add("@TotalCount", addressRequest.UserId);
            parameters.Add("@Company_Code", addressRequest.Company_Code);
            parameters.Add("@State_Name", addressRequest.State_Name);
            parameters.Add("@Map_Name", addressRequest.Map_Name);
            parameters.Add("@BillingStateName", addressRequest.BillingStateName);
            parameters.Add("@ShippingStateName", addressRequest.ShippingStateName);
            parameters.Add("@BillingLocationId", addressRequest.BillingLocationId);
            parameters.Add("@BillingPinCode", addressRequest.BillingPinCode);
            parameters.Add("@ShippingLocationId", addressRequest.ShippingLocationId);
            parameters.Add("@ShippingPinCode", addressRequest.ShippingPinCode);
            parameters.Add("@City_Name", addressRequest.City_Name);
            parameters.Add("@ShippingCity_Name", addressRequest.ShippingCity_Name);
            parameters.Add("@SapBillTo", addressRequest.SapBillTo);
            parameters.Add("@SapShipTo", addressRequest.SapShipTo);
            parameters.Add("@AddressCode", addressRequest.AddressCode);
            parameters.Add("@ClientGstNumber", addressRequest.ClientGstNumber);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageClientAddress", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> PostDeleteClientAddress(int ClientAddressId, int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Delete");
            parameters.Add("@UserId", UserId);
            parameters.Add("@ClientAddressId", ClientAddressId);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageClientAddress", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<ClientAddressResponse> PostClientAddressUpload(string xmlString, string userId)
        {
            ClientAddressResponse clientAddressDetails = new ClientAddressResponse();

            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Upload_ClientAddress", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) &&
                        message.Contains("Rows Uploaded Successfully."))
                    {
                        clientAddressDetails.response = message;
                    }
                    else
                    {
                        clientAddressDetails.response = "Failed to import.";
                        clientAddressDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    clientAddressDetails.response = "Error while processing response.";
                }
            }
            else
            {
                clientAddressDetails.response = "Failed";
            }
            return clientAddressDetails;

        }
        public DataSet ClientAddressExport(int userId)
        {
            DataSet ds = this._dbRepository.ClientAddressExport(userId);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given Parameters.");
            }

        }

    }
}
