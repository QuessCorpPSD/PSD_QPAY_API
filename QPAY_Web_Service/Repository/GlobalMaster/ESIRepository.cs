using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using static QPay.UI.Models.GlobalMaster.ESIClass;


namespace QPay.BAL.Repository.GlobalMaster
{
    public class ESIRepository : IESIRepository
    {
        private readonly DbRepository _dbRepository;

        public ESIRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<List<EsiBlockUI>> GetBlocks()
        {
            var parameters = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync("sp_GetAllBlock", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<EsiBlockUI>>(res) ?? new List<EsiBlockUI>();
            }

            return new List<EsiBlockUI>();
        }

        public async Task<List<EsiMonthsUI>> GetMonths()
        {
            var parameters = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync("sp_GetAllMonths", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<EsiMonthsUI>>(res) ?? new List<EsiMonthsUI>();
            }

            return new List<EsiMonthsUI>();
        }

        public async Task<DataSet> GetEsiblockSearch(string EffectiveDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@mode"] = "Search",
                ["@value1"] = string.IsNullOrEmpty(EffectiveDate) ? null : EffectiveDate,
                ["@value2"] = null
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetEsiblockDetailsData", parameters, 1500);
        }

        public async Task<DataSet> GetEsiblockExporttoExcel(string EffectiveDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@mode"] = "Search",
                ["@value1"] = string.IsNullOrEmpty(EffectiveDate) ? null : EffectiveDate,
                ["@value2"] = null
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetEsiblockDetailsData_ExportToExcel", parameters, 1500);
        }

        public async Task<EsiResponse> CreateUpdateDeleteEsiblock(EsiblockRequest request)
        {
            EsiResponse responseDetails = new EsiResponse();

            var serializer = new XmlSerializer(typeof(EsiblockMain));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true // optional
            };
            using var sw = new StringWriter();
            using var writer = XmlWriter.Create(sw, settings);

            serializer.Serialize(writer, request.main, ns);

            string xmlInput = sw.ToString();

            string storeProcedure = "sp_CreateUpdate_ESIBlock";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@mode", request.mode);
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<QPay.UI.ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Message ?? resultList?.FirstOrDefault()?.ErrorMessagee ?? string.Empty;

                    if (message.Contains("successfully") || message.Contains("Successfully"))
                    {
                        responseDetails.response = message;
                    }
                    else
                    {
                        responseDetails.response = message;
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


        public async Task<List<PaycodeUI>> GetPaycodes()
        {
            var parameters = new DynamicParameters();
            var res = await _dbRepository.GetItemsAsync("sp_GetAllPaycode", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                var allPaycodes = JsonConvert.DeserializeObject<List<PaycodeUI>>(res) ?? new List<PaycodeUI>();

                // Filter only Page_Type_Value = "ESI"
                var filteredPaycodes = allPaycodes
                    .Where(p => string.Equals(p.Page_Type_Value, "ESI", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return filteredPaycodes;
            }

            return new List<PaycodeUI>();
        }

        public async Task<List<EsiStateUI>> GetStates()
        {
            var parameters = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync("sp_GetAllStateByParam", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<EsiStateUI>>(res) ?? new List<EsiStateUI>();
            }

            return new List<EsiStateUI>();
        }

        public async Task<List<EsiCityUI>> GetCity(int StateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@State_Id", StateId);

            var res = await _dbRepository.GetItemsAsync("sp_GetStateandCity", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<EsiCityUI>>(res) ?? new List<EsiCityUI>();
            }
            return new List<EsiCityUI>();
        }

        public async Task<List<EsiCriteriaTypeUI>> GetCriteriaType()
        {
            var parameters = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync("sp_GetCriteriaType", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<EsiCriteriaTypeUI>>(res) ?? new List<EsiCriteriaTypeUI>();
            }

            return new List<EsiCriteriaTypeUI>();
        }

        public async Task<DataSet> GetEsiLocationSlabSearch(EsiLocationSlabSearchRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@FromDate"] = string.IsNullOrEmpty(request.FromDate) ? null : request.FromDate,
                ["@ToDate"] = string.IsNullOrEmpty(request.ToDate) ? null : request.ToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllESILocationSlab", parameters, 1500);
        }

        public async Task<DataSet> GetEsiLocationSlabExporttoExcel(EsiLocationSlabSearchRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@FromDate"] = string.IsNullOrEmpty(request.FromDate) ? null : request.FromDate,
                ["@ToDate"] = string.IsNullOrEmpty(request.ToDate) ? null : request.ToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllESILOCATIONSLAB_ExportToExcel", parameters, 1500);
        }

        public async Task<EsiResponse> CreateUpdateDeleteEsiLocationSlab(EsiLocationSlabRequest request)
        {
            EsiResponse responseDetails = new EsiResponse();

            var xmlInput = SerializeToXml(new ESILocationSlabsData { ESILocationSlab = request.ESILocationSlab });

            var xmlInputDetail = SerializeToXml(new ESILocationSlabDetailResponse { ESILocationSlabDetails = request.ESILocationSlabDetails });

            string storeProcedure = "sp_CreateUpdateESILocationSlab";
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
                        responseDetails.response = "Failed to - " + message;
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

        public async Task<DataSet> GetEsiSlabSearch(EsiSlabSearchRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@StartDate"] = string.IsNullOrEmpty(request.FromDate) ? null : request.FromDate,
                ["@EdnDate"] = string.IsNullOrEmpty(request.ToDate) ? null : request.ToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllESISlab", parameters, 1500);
        }

        public async Task<DataSet> GetEsiSlabExporttoExcel(EsiSlabSearchRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@StartDate"] = string.IsNullOrEmpty(request.FromDate) ? null : request.FromDate,
                ["@EdnDate"] = string.IsNullOrEmpty(request.ToDate) ? null : request.ToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllESISlab_ExportToExcel", parameters, 1500);
        }

        public async Task<EsiResponse> CreateUpdateDeleteEsiSlab(EsiSlabRequest request)
        {
            EsiResponse responseDetails = new EsiResponse();

            var xmlInput = SerializeToXml(new ESISlabsDetails { EsiSlab = request.ESISlab });

            var xmlInputDetail = SerializeToXml(new ESISlabDetailResponse { ESISlabDetail = request.ESISlabDetail });

            string storeProcedure = "sp_CreateUpdateESISlab";
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
                        responseDetails.response = "Failed to - " + message;
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
