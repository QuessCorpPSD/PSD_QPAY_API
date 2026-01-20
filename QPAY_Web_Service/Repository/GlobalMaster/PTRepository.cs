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
using QPay.UI;
using static QPay.UI.Models.GlobalMaster.ESIClass;
using static QPay.UI.Models.GlobalMaster.LWFClass;
using static QPay.UI.Models.GlobalMaster.PTClass;


namespace QPay.BAL.Repository.GlobalMaster
{
    public class PTRepository : IPTRepository
    {
        private readonly DbRepository _dbRepository;

        public PTRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public async Task<List<PTTypeUI>> PTType()
        {
            var parameters = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync("Sp_GetAllProfessionalTaxType", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PTTypeUI>>(res) ?? new List<PTTypeUI>();
            }

            return new List<PTTypeUI>();
        }

        public async Task<List<PTCategoryUI>> PTCategory()
        {
            var parameters = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync("Sp_GetAllProfessionalTaxCategory", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PTCategoryUI>>(res) ?? new List<PTCategoryUI>();
            }

            return new List<PTCategoryUI>();
        }

        public async Task<List<PTCircleUI>> PTCircle(int StateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@State_id", StateId);

            var res = await _dbRepository.GetItemsAsync("Sp_GetCircelStateby", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PTCircleUI>>(res) ?? new List<PTCircleUI>();
            }

            return new List<PTCircleUI>();
        }

        public async Task<DataSet> PTSearch(PTSearchRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@StateID"] = request.StateID,
                ["@EffectiveDate"] = string.IsNullOrEmpty(request.EffectiveDate) ? null : request.EffectiveDate,
                ["@PT_Type"] = request.PT_Type,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPTDetailsByStID_EffDt_PTType", parameters, 1500);
        }

        public async Task<DataSet> PTExporttoExcel(PTSearchRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@StateID"] = request.StateID,
                ["@EffectiveDate"] = string.IsNullOrEmpty(request.EffectiveDate) ? null : request.EffectiveDate,
                ["@PT_Type"] = request.PT_Type,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPTDetailsByStID_EffDt_PTType_ExportToExcel", parameters, 1500);
        }

        public async Task<PTResponse> CreateUpdateDeletePT(PTRequest request)
        {
            PTResponse responseDetails = new PTResponse();

            var xmlInput = SerializeToXml(new PTData { PTSlab = request.PTSlab });

            var xmlInputDetail = SerializeToXml(new PTSlabDetailsResponse { PTSlabDetail = request.PTSlabDetail });

            string storeProcedure = "sp_CreateUpdatePTDetails";
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
