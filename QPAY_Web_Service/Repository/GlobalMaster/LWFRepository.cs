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
using QPay.DAL.Repository;
using QPay.BAL.IRepository.GlobalMaster;
using static QPay.UI.Models.GlobalMaster.LWFClass;
using QPay.UI;


namespace QPay.BAL.Repository.GlobalMaster
{
    public class LWFRepository : ILWFRepository
    {
        private readonly DbRepository _dbRepository;

        public LWFRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<DataSet> GetLWFSlabSearch(LWFSearchRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@StateID"] = string.IsNullOrEmpty(request.StateID) ? null : request.StateID,
                ["@EffectiveDate"] = string.IsNullOrEmpty(request.EffectiveDate) ? null : request.EffectiveDate,

            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllLWFSlabDetail", parameters, 1500);
        }

        public async Task<DataSet> GetLWFSlabExporttoExcel(LWFSearchRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@StateID"] = string.IsNullOrEmpty(request.StateID) ? null : request.StateID,
                ["@EffectiveDate"] = string.IsNullOrEmpty(request.EffectiveDate) ? null : request.EffectiveDate,

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllLWFSlabDetail_ExportToExcel", parameters, 1500);
        }

        public async Task<LWFResponse> CreateUpdateDeleteLWFSlab(LWFSlabRequest request)
        {
            LWFResponse responseDetails = new LWFResponse();

            var xmlInput = SerializeToXml(new LWFDetails { LWFSlab = request.LWFSlab });

            var xmlInputDetail = SerializeToXml(new LabourWelfareFareFundDetailsResponse { LWFSlabDetails = request.LWFSlabDetails });

            string storeProcedure = "sp_CreateUpdateLWFSlabDetails";
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
                        responseDetails.response = "Failed to - "+ message;
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
