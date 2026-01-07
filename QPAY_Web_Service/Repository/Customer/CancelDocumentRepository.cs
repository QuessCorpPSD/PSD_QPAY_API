using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Models;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Customer
{
    public class CancelDocumentRepository: ICancelDocumentRepository
    {
        private readonly DbRepository _dbRepository;

        public CancelDocumentRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<List<CancelDocument>> Search(int companyId, int payPeriodId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@Payperiod_Id", payPeriodId);
            parameters.Add("@Action", "Search");

            var res = await this._dbRepository.GetItemsAsync("Proc_Manage_CancelledInvoiceDocument", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CancelDocument>>(res) ?? new List<CancelDocument>();
            }

            return new List<CancelDocument>();
        }

        public async Task<string> UploadDocument(string xml, int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", xml);
            parameters.Add("@Action", "Add");
            parameters.Add("@CreatedBy", userId);


            var res = await this._dbRepository.GetItemsAsync("Proc_Manage_CancelledInvoiceDocument_NewUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
    }
}
