using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.Models.GlobalMaster;
using System.Data;
using static QPay.UI.Models.GlobalMaster.GlobalMasters;

namespace QPay.BAL.Repository.GlobalMaster
{
    public class GstRepository : IGstRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public GstRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(string UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Get",
                ["@UserId"] = UserId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageGSTMaster", parameters, 1500);
        }

        public async Task<DataSet> ExporttoExcel(string UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Export",
                ["@UserId"] = UserId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageGSTMaster", parameters, 1500);
        }

        public async Task<GstMastersResponse> Create(GstRequest createRequest)
        {
            GstMastersResponse globalmastersResponse = new GstMastersResponse();

            string storeProcedure = "Proc_ManageGSTMaster";
            var parameters = new DynamicParameters();

            parameters.Add("@Action", "Add");
            parameters.Add("@UserId", createRequest.UserId);
            parameters.Add("@GstMasterId", 0);
            parameters.Add("@EffectiveDate", createRequest.EffectiveDate);
            parameters.Add("@GstNumber", createRequest.GstNumber);
            parameters.Add("@CompanyName", createRequest.CompanyName);
            parameters.Add("@CompanyAddress", createRequest.CompanyAddress);
            parameters.Add("@CreatedBy", createRequest.CreatedBy);
            parameters.Add("@Gst_Percentage", createRequest.Gst_Percentage);
            parameters.Add("@EntityId", createRequest.EntityId);
            parameters.Add("@Pincode", createRequest.Pincode);
            parameters.Add("@StateId", "1");
            parameters.Add("@LocationId", "76");


            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (message.Contains("successfully"))
                    {
                        // Success
                        globalmastersResponse.response = message;
                    }
                    else if (message.Contains("already exists"))
                    {
                        globalmastersResponse.response = message;
                    }
                    else
                    {
                        // Failure
                        globalmastersResponse.response = "Failed";
                    }
                }
                catch
                {
                    globalmastersResponse.response = "Error while processing response.";
                }
            }
            else
            {
                globalmastersResponse.response = "Failed";
            }
            return globalmastersResponse;
        }

        public async Task<GlobalMastersResponse> Edit(GstRequest createRequest)
        {
            GlobalMastersResponse globalmastersResponse = new GlobalMastersResponse();

            string storeProcedure = "Proc_ManageGSTMaster";
            var parameters = new DynamicParameters();

            parameters.Add("@Action", "Edit");
            parameters.Add("@UserId", createRequest.UserId);
            parameters.Add("@GstMasterId", createRequest.GstMasterId);
            parameters.Add("@EffectiveDate", createRequest.EffectiveDate);
            parameters.Add("@GstNumber", createRequest.GstNumber);
            parameters.Add("@CompanyName", createRequest.CompanyName);
            parameters.Add("@CompanyAddress", createRequest.CompanyAddress);
            parameters.Add("@CreatedBy", createRequest.CreatedBy);
            parameters.Add("@Gst_Percentage", createRequest.Gst_Percentage);
            parameters.Add("@EntityId", createRequest.EntityId);
            parameters.Add("@Pincode", createRequest.Pincode);
            parameters.Add("@StateId", "1");
            parameters.Add("@LocationId", "76");


            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (message.Contains("successfully"))
                    {
                        // Success
                        globalmastersResponse.response = message;
                    }
                    else if (message.Contains("already exists"))
                    {
                        globalmastersResponse.response = message;
                    }
                    else
                    {
                        // Failure
                        globalmastersResponse.response = "Failed";
                    }
                }
                catch
                {
                    globalmastersResponse.response = "Error while processing response.";
                }
            }
            else
            {
                globalmastersResponse.response = "Failed";
            }
            return globalmastersResponse;
        }

        public async Task<GlobalMastersResponse> Delete(int GstMasterId, int UserId)
        {
            GlobalMastersResponse globalmastersResponse = new GlobalMastersResponse();

            string storeProcedure = "Proc_ManageGSTMaster";
            var parameters = new DynamicParameters();

            parameters.Add("@Action", "Delete");
            parameters.Add("@GstMasterId", GstMasterId);
            parameters.Add("@UserId", UserId);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    globalmastersResponse.response = "Deleted Successfully";
                }
                catch
                {
                    globalmastersResponse.response = "Error while processing response.";
                }
            }
            else
            {
                globalmastersResponse.response = "Failed";
            }
            return globalmastersResponse;
        }

    }
}
