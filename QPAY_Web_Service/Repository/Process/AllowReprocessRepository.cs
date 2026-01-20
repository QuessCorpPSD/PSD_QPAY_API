using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Process;
using QPay.DAL.Repository;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.Repository.Process
{
    public class AllowReprocessRepository : IAllowReprocessRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public AllowReprocessRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(SearchAllowReprocessRequest searchRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = searchRequest.Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetLockedPayPeriodsForReprocessing", parameters, 1500);
        }

        public async Task<DataSet> ExporttoExcel(SearchAllowReprocessRequest exporttoExcelRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = exporttoExcelRequest.Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetLockedPayPeriodsForReprocessing_ExportToExcel", parameters, 1500);
        }

        public async Task<ProcessResponse> Create(AllowReprocessCreateRequest createRequest)
        {
            ProcessResponse processDetails = new ProcessResponse();

            if (createRequest.allowReprocesses != null)
            {
                var request = new AllowReprocess
                {
                    Items = createRequest.allowReprocesses.Select(x => new AllowReprocessForLockedPayPeriods
                    {
                        AllowReprocessId = x.Allow_Reprocess_Id,
                        CompanyId = x.Company_Id,
                        PayFrequencyDetailId = x.Pay_Frequency_Detail_Id
                    }).ToList()
                };

                string xml = XmlHelper.SerializeObjectToXml(request, "AllowReprocess");

                string storeProcedure = "sp_CreateReprocess";
                var parameters = new DynamicParameters();

                parameters.Add("@xmlInput", xml);
                parameters.Add("@Mode", createRequest.Mode);
                parameters.Add("@CreatedBy", createRequest.CreatedBy);


                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) && message.Contains("Data Saved Successfully"))
                        {
                            processDetails.response = message;
                        }
                        else if (!string.IsNullOrWhiteSpace(message) && message.Contains("Data already exists"))
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

            }
            else
            {
                processDetails.response = "No data found";
            }
            return processDetails;
        }
    }

    [XmlRoot("AllowReprocess")]
    public class AllowReprocess
    {
        [XmlElement("AllowReprocessForLockedPayPeriods")]
        public List<AllowReprocessForLockedPayPeriods> Items { get; set; }
    }

    public class AllowReprocessForLockedPayPeriods
    {
        [XmlElement("Allow_Reprocess_Id")]
        public string AllowReprocessId { get; set; }

        [XmlElement("Company_Id")]
        public string CompanyId { get; set; }

        [XmlElement("Pay_Frequency_Detail_Id")]
        public string PayFrequencyDetailId { get; set; }
    }

    public class ResponseModel
    {
        public string? Result { get; set; }
        public string? Error_Message { get; set; }
        public string? ErrorMessage { get; set; }
    }


}
