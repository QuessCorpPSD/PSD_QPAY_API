using ClosedXML.Excel;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.Invoice;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace QPay.BAL.Repository
{
    public class BillingPayFrequencyRepository : IBillingPayFrequencyRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public BillingPayFrequencyRepository(DbRepository dbRepository, IConfiguration configuration)
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
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllBillingPayFrequency_New", parameters, 1500);
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
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetAllBillingPayFrequencyExportToExcel", parameters, 1500);
        }

        public async Task<DataSet> GetGroupName(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetBillingCompanyLocationAndGroupName", parameters, 1500);
        }

        public async Task<DataSet> GetData(string startDate, string endDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@PDateFrom"] = startDate,
                ["@PDateTo"] = endDate,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetDataForBillingPayFrequency", parameters, 1500);
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
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CheckBillingPayFrequencyExists", parameters, 1500);
        }


        public async Task<DataSet> Create(BillingPayFrequencyRequest items)
        {
            // var parentdata = GenericSerializer<BillingPayFrequency>.Serialize(items.parentDetail);
            // var childdata = GenericSerializer<List<BillingPayFrequencyDetail>>.Serialize(items.ChildDetail);

            //string resultMessage = string.Empty;
            //BillingPayFrequency payFrequency = JsonConvert.DeserializeObject<BillingPayFrequency>(parentdata);
            //BillingPayFrequencyDetail[] payFrequencyDetail = JsonConvert.DeserializeObject<BillingPayFrequencyDetail[]>(childdata);
            //var payFrequencyResponse = new BillingPayFrequencyResponse();
            //payFrequencyResponse.PayFrequencys = new BillingPayFrequency[1];
            //payFrequencyResponse.PayFrequencys[0] = payFrequency;

            //var payFrequencyDetailResponse = new BillingPayFrequencyDetailResponse();
            //payFrequencyDetailResponse.PayFrequencyDetails = payFrequencyDetail;

            //string payFrequencyResponseSerialize = GenericSerializer<BillingPayFrequencyResponse>.Serialize(payFrequencyResponse);
            //string payFrequencyDetailResponseSerialize = GenericSerializer<BillingPayFrequencyDetailResponse>.Serialize(payFrequencyDetailResponse);
            //payFrequencyResponseSerialize = payFrequencyResponseSerialize == "<PayFrequencyDetailResponse />" ? "<PayFrequencyDetailResponse></PayFrequencyDetailResponse>" : payFrequencyResponseSerialize;

            var parentdatawrapper = new UI.Invoice.PayFrequencyNewWrapper
            {
                PayFrequency = new BillingPayFrequency
                {
                    Pay_Frequency_Id = items.parentDetail.Pay_Frequency_Id,
                    Company_Id = items.parentDetail.Company_Id,
                    Group_Id = items.parentDetail.Group_Id,
                    Starting_Date = items.parentDetail.Starting_Date,
                    Ending_Date = items.parentDetail.Ending_Date
                }
            };

            var childdatawrapper = new UI.Invoice.PayFrequencyDetailNewWrapper
            {
                PayFrequencyDetail = items.ChildDetail.Select(child => new BillingPayFrequencyDetail
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


            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@xmlInputDetail"] = childdata,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateBillingPayFrequency", parameters);
        }

        public string ToXmlPayFrequencyDetailNew(UI.Invoice.PayFrequencyDetailNewWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(UI.Invoice.PayFrequencyDetailNewWrapper));

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

        public string ToXmlPayFrequencyNew(UI.Invoice.PayFrequencyNewWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(UI.Invoice.PayFrequencyNewWrapper));

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
