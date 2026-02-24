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
    public class BankNeftCultureInvoiceRepository : IBankNeftCultureInvoiceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public BankNeftCultureInvoiceRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

      
        public DataSet NeftCulturesearch(int Company_Id, int UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Mode"] = "Search",
                ["@Company_Id"] = Company_Id,
                //["@Bank_Culture_Id"] = 0,
                //["@CreatedBy"] = UserId

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Search_EditNeftBankculture_Invoice_Data", parameters);
        }
        public DataSet NeftCultureExport(int Company_Id, int UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Mode"] = "Search",
                ["@Company_Id"] = Company_Id,                
                //["@CreatedBy"] = UserId

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Search_EditInvoiceNeftBankculture_ExptToExcel", parameters);
        }
        public List<NeftCulture> GetNeftBankculture(int Company_Id, string Mode, int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_id", Company_Id);          
            parameters.Add("@mode", Mode);

            var res = this._dbRepository.GetItemsAsync("sp_GetAllBankDetails_Invoice", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<NeftCulture>>(res) ?? new List<NeftCulture>();
            }

            return new List<NeftCulture>();
        }

        public async Task<List<CultureMessage>> NeftCultureSave(Culturesave payload)
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
                return new List<CultureMessage>
                {
                    new CultureMessage
                    {
                        Error_Message = "Invalid response "
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<CultureMessage>>(res);
                return list?.ToList() ?? new List<CultureMessage>();
            }
            catch (JsonException ex)
            {

                return new List<CultureMessage>
                  {
                    new CultureMessage
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
    }
}
