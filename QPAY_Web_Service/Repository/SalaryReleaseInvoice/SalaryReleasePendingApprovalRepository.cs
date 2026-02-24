using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.DAL.Repository;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace QPay.BAL.Repository.SalaryReleaseInvoice
{
    public class SalaryReleasePendingApprovalRepository : ISalaryReleasePendingApprovalRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
      
        public SalaryReleasePendingApprovalRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
        public DataSet BankAdviceList(string BatchType, string CollectionStatus, string UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Search",
                ["@BatchType"] = BatchType,
                ["@CollectionStatus"] = CollectionStatus,
                ["@CreatedBy"] = UserId                

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_BankAdvice_Request", parameters);
        }

        public DataSet BankAdviceListExport(string BatchType, string CollectionStatus, string UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Export",
                ["@BatchType"] = BatchType,
                ["@CollectionStatus"] = CollectionStatus,
                ["@CreatedBy"] = UserId

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_BankAdvice_Request", parameters);
        }

        public async Task<List<BankadviceApprovalMessage>> BankAdviceApprove(ApproveBankAdvice payload)
        {
            const string storedProcedure = "Proc_Manage_BankAdvice_Request";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.approvedata, "NewDataSet", "Table");

            parameter.Add("@xmlInput", xml);
            parameter.Add("@CreatedBy", payload.UserId);
            parameter.Add("@Action", "Approve");
            parameter.Add("@BatchType", payload.BatchType);
            parameter.Add("@CollectionStatus", payload.CollectionStatus);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BankadviceApprovalMessage>
                {
                    new BankadviceApprovalMessage
                    {
                        Validation = "Invalid response "
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<BankadviceApprovalMessage>>(res);
                return list?.ToList() ?? new List<BankadviceApprovalMessage>();
            }
            catch (JsonException ex)
            {

                return new List<BankadviceApprovalMessage>
                  {
                    new BankadviceApprovalMessage
                      {
                        Validation = ex.Message
                      }
                 };
            }

        }

        public static string ConvertWithDynamicRoot<T>(IEnumerable<T> list, string rootName, string tableName)
        {
            var root = new XElement(rootName);

            foreach (var item in list)
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StringWriter();
                serializer.Serialize(writer, item);

                var doc = XDocument.Parse(writer.ToString());
                root.Add(new XElement(tableName, doc.Root.Elements()));
            }

            return root.ToString();
        }
    }
}