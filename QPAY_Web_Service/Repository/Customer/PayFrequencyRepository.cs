using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using System.Data;
using System.Xml;
using System.Xml.Serialization;

namespace QPay.BAL.Repository
{
    public class PayFrequencyRepository : IPayFrequencyRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public PayFrequencyRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@StartDate"] = "",
                ["@EdnDate"] = "",
                ["@Pay_Frequency_Id"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPayFrequency_New", parameters, 1500);
        }

        public async Task<DataSet> ExportToExcel(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@StartDate"] = "",
                ["@EdnDate"] = "",
                ["@Pay_Frequency_Id"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetAllPayFrequency_NewExportToExcel", parameters, 1500);
        }

        public async Task<DataSet> GetGroupName(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetCompanyLocationAndGroupName", parameters, 1500);
        }

        public async Task<DataSet> GetData(string startDate, string endDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@PDateFrom"] = startDate,
                ["@PDateTo"] = endDate,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetDataForPayFrequency", parameters, 1500);
        }

        public async Task<DataSet> CheckPayFrequencyExists(int companyId, string startDate, string endDate, string payPeriod)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Starting_Date"] = startDate,
                ["@Ending_Date"] = endDate,
                ["@Pay_Period"] = payPeriod,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetCountForPayFreqNew", parameters, 1500);
        }

        public async Task<DataSet> Create([FromBody] PayFrequencyRequest items)
        {
            //var parentdata = GenericSerializer<PayFrequencyNew>.Serialize(items.parentDetail);
            //var childdata = GenericSerializer<List<PayFrequencyDetailNew>>.Serialize(items.ChildDetail);

            var parentdatawrapper = new UI.Customer.PayFrequencyNewWrapper
            {
                PayFrequency = new PayFrequencyNew
                {
                    Pay_Frequency_Id = items.parentDetail.Pay_Frequency_Id,
                    Company_Id = items.parentDetail.Company_Id,
                    Group_Id = items.parentDetail.Group_Id,
                    Starting_Date = items.parentDetail.Starting_Date,
                    Ending_Date = items.parentDetail.Ending_Date
                }
            };

            var childdatawrapper = new UI.Customer.PayFrequencyDetailNewWrapper
            {
                PayFrequencyDetail = items.ChildDetail.Select(child => new PayFrequencyDetailNew
                {
                    Pay_Frequency_Detail_Id = child.Pay_Frequency_Detail_Id,
                    Pay_Frequency_Id = child.Pay_Frequency_Id,
                    Pay_Sequence_Number = child.Pay_Sequence_Number,
                    Pay_Period = child.Pay_Period,
                    Start_At = child.Start_At,
                    End_At = child.End_At,
                    Salary_Date = child.Salary_Date,
                    Pay_Period_Days = child.Pay_Period_Days,
                    Weekly_Holidays = child.Weekly_Holidays,
                    Monthly_Holidays = child.Monthly_Holidays,
                    Other_Holidays=child.Other_Holidays,
                    Working_Days = child.Working_Days

                }).ToList()
            };

            string parentdata = ToXmlPayFrequencyNew(parentdatawrapper);
            string childdata = ToXmlPayFrequencyDetailNew(childdatawrapper);


            //PayFrequencyNew payFrequency = JsonConvert.DeserializeObject<PayFrequencyNew>(parentdata);
            //PayFrequencyDetailNew[] payFrequencyDetail = JsonConvert.DeserializeObject<PayFrequencyDetailNew[]>(childdata);
            //var payFrequencyResponse = new PayFrequencyResponseNew();
            //payFrequencyResponse.PayFrequencys = new PayFrequencyNew[1];
            //payFrequencyResponse.PayFrequencys[0] = payFrequency;

            //var payFrequencyDetailResponse = new PayFrequencyDetailResponseNew();
            //payFrequencyDetailResponse.PayFrequencyDetails = payFrequencyDetail;

            //string payFrequencyResponseSerialize = GenericSerializer<PayFrequencyResponseNew>.Serialize(payFrequencyResponse);
            //string payFrequencyDetailResponseSerialize = GenericSerializer<PayFrequencyDetailResponseNew>.Serialize(payFrequencyDetailResponse);
            //payFrequencyResponseSerialize = payFrequencyResponseSerialize == "<PayFrequencyDetailResponse />" ? "<PayFrequencyDetailResponse></PayFrequencyDetailResponse>" : payFrequencyResponseSerialize;


            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@xmlInputDetail"] = childdata,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdatePayFrequency_New", parameters);
        }

        public string ToXmlPayFrequencyDetailNew(UI.Customer.PayFrequencyDetailNewWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(UI.Customer.PayFrequencyDetailNewWrapper));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // remove xmlns

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,   // 🚀 remove the XML header
                Indent = true
            };

            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(writer, wrapper, ns);
                return sw.ToString();
            }
        }

        public string ToXmlPayFrequencyNew(UI.Customer.PayFrequencyNewWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(UI.Customer.PayFrequencyNewWrapper));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // remove xmlns

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,   // 🚀 remove the XML header
                Indent = true
            };

            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(writer, wrapper, ns);
                return sw.ToString();
            }
        }

    }
}
