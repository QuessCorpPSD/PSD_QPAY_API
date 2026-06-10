using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.MailApprovalProcess;
using QPay.DAL.Repository;
using QPay.UI.Models.MailApprovalProcess;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace QPay.BAL.Repository.MailApprovalProcess
{
    public class CustomerBlockApprovalRepository : ICustomerBlockApprovalRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public CustomerBlockApprovalRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        #region CustomerBlockApproval start
        public DataSet GetApproveClientList(string UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "ApprovalList",              
                ["@UserId"] = UserId

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageCustomerBlock", parameters);
        }

        public async Task<List<ErrorMessage>> ClientApproveReject(ClientApprove payload)
        {
            const string storedProcedure = "Proc_ManageCustomerBlock";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.ApproveList, "NewDataSet", "Table");

            parameter.Add("@Action", "BulkApprove");                   
            parameter.Add("@UserId", payload.UserId);
            parameter.Add("@IsApproved", payload.IsApproved);
            parameter.Add("@xmlInput", xml);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<ErrorMessage>
                {
                    new ErrorMessage
                    {
                        Message = "Invalid response"
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<ErrorMessage>>(res);
                return list?.ToList() ?? new List<ErrorMessage>();
            }
            catch (JsonException ex)
            {

                return new List<ErrorMessage>
                  {
                    new ErrorMessage
                      {
                        Message = ex.Message
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

        #endregion CustomerBlockApproval end
    }
}
