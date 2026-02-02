using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using static QPay.UI.Models.GlobalMaster.ESIClass;
using static QPay.UI.Models.GlobalMaster.PFClass;
using static QPay.UI.Models.GlobalMaster.PTClass;
using QPay.UI;

namespace QPay.BAL.Repository.GlobalMaster
{
    public class PFRepository : IPFRepository
    {
        private readonly DbRepository _dbRepository;

        public PFRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public async Task<List<PFPayCodesUI>> PFPayCodes()
        {
            var parameters = new DynamicParameters();
            var res = await _dbRepository.GetItemsAsync("sp_GetAllPaycode", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                var allPaycodes = JsonConvert.DeserializeObject<List<PFPayCodesUI>>(res) ?? new List<PFPayCodesUI>();

                // Filter only Page_Type_Value = "ESI"
                var filteredPaycodes = allPaycodes
                    .Where(p => string.Equals(p.Page_Type_Value, "PF", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return filteredPaycodes;
            }

            return new List<PFPayCodesUI>();
        }


        public async Task<List<PFCapTypeUI>> PFCapType()
        {
            var parameters = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync("SP_GetCapNonCapType", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PFCapTypeUI>>(res) ?? new List<PFCapTypeUI>();
            }

            return new List<PFCapTypeUI>();
        }

        public async Task<DataSet> PFSearch(string CapType)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CapType"] = string.IsNullOrEmpty(CapType)
                || CapType == "\"\""
                || CapType == "\""
                || CapType == "''"
                ? null : CapType,
                ["@Paycode"] = null
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllProvidentFund", parameters, 1500);
        }

        public async Task<DataSet> PFExporttoExcel(string CapType)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CapType"] = string.IsNullOrEmpty(CapType)
                || CapType == "\"\""
                || CapType == "\""
                || CapType == "''"
                ? null : CapType,
                ["@Paycode"] = null
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllProvidentFund_ExportToExcel", parameters, 1500);
        }

        public async Task<PFResponse> CreateUpdatePF(PFRequest request)
        {
            PFResponse responseDetails = new PFResponse();

            var xmlInput = SerializeToXml(new PFData { PF = request.PF });

            var xmlInputDetail = SerializeToXml(new PFDetailsResponse { PFDetail = request.PFDetail });

            string storeProcedure = "sp_CreateUpdateProvidentFund";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@xmlInputDetail", xmlInputDetail);
            parameters.Add("@mode", request.mode);
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (message.Contains("successfully") || message.Contains("Successfully"))
                    {
                        responseDetails.response = message;
                    }
                    else
                    {
                        responseDetails.response = "Failed to - "+message;
                    }
                }
                catch
                {
                    responseDetails.response = "Error while processing response.";
                }
            }
            else
            {
                responseDetails.response = "Failed";
            }
            return responseDetails;
        }

        public async Task<PFResponse> DeletePF(PFDeleteRequest request)
        {
            PFResponse responseDetails = new PFResponse();

            string storeProcedure = "sp_DeleteProvidentFund";
            var parameters = new DynamicParameters();
            parameters.Add("@ProvidentFundId", request.ProvidentFundId);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    responseDetails.response = "Deleted Successfully";
                }
                catch
                {
                    responseDetails.response = "Error while processing response.";
                }
            }
            else
            {
                responseDetails.response = "Failed";
            }
            return responseDetails;
        }

        public static string SerializeToXml<T>(T obj)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // Remove xmlns:xsi and xmlns:xsd

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true, // Remove <?xml version="1.0" ... ?>
                Indent = true              // Optional: format XML nicely
            };

            using (StringWriter stringWriter = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(writer, obj, ns);
                return stringWriter.ToString();
            }
        }
    }
}
