using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.BAL.IRepository.IBankNonInvoice;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static QPay.UI.Models.BankNonInvoice.BankNeftcultureNonInvoiceModel;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class BankNeftCultureNonInvoice : IBankNEFTcultureNonInvoice
    {
        private readonly DbRepository _dbRepository;

        public BankNeftCultureNonInvoice(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Getbankname(int? Company_id, string mode)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_id"] = Company_id,
                ["@mode"] = mode,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllBankDetails_Invoice", parameters); ;

        }
        public async Task<DataSet> GetSearchdata(int Company_id, int Bank_Culture_Id, string Mode)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_Id"] = Company_id,
                ["@Bank_Culture_Id"] = Bank_Culture_Id,
                ["@Mode"] = Mode,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Search_EditNeftBankculture_Invoice_Data", parameters); ;

        }

        public async Task<List<BankCultureMessage>> NeftCultureSave(BankCulturesave payload)
        {
            const string storedProcedure = "CreateNeftBankCultureInvoice";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.culturedatas, "BankNeftCultureDetailsResponse", "BankNeftCulture");

            parameter.Add("@GroupDetail", xml);
            parameter.Add("@Company_id", payload.Company_Id);
            parameter.Add("@CreatedBy", payload.UserId);
            parameter.Add("@Mode", payload.Mode);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BankCultureMessage>
                {
                    new BankCultureMessage
                    {
                        Error_Message = "Invalid response "
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<BankCultureMessage>>(res);
                return list?.ToList() ?? new List<BankCultureMessage>();
            }
            catch (JsonException ex)
            {

                return new List<BankCultureMessage>
                  {
                    new BankCultureMessage
                      {
                        Error_Message = ex.Message
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

        public async Task<DataSet> Getpayperiod()
        {
            var parameters = new Dictionary<string, object>();

            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_GetAllPayPeriod",
                parameters
            );
        }

        public async Task<DataSet> ExportToExcel(string payperiod)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@PayPeriod"] = payperiod
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("proc_FinanceHold_Report", parameters);
        }

    }

 }

