using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Models.Customer;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Invoice
{
    public class CreditNoteRepository: ICreditNoteRepository
    {
        private readonly DbRepository _dbRepository;

        public CreditNoteRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<CreditNotePurpose>> GetCreditNotePurpose(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyID", companyId);

            var res = await this._dbRepository.GetItemsAsync("sp_GetCreditNoteTypeByCompanyID", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CreditNotePurpose>>(res) ?? new List<CreditNotePurpose>();
            }

            return new List<CreditNotePurpose>();
        }

        public async Task<List<CreditNote>> GetCreditNoteSearch(CreditNoteSearch creditNoteSearch)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Purpose", creditNoteSearch.Purpose);
            parameters.Add("@Company_Id", creditNoteSearch.Company_id);
            parameters.Add("@RefId", creditNoteSearch.RefId);
            parameters.Add("@Pay_period_id", creditNoteSearch.Pay_period_id);
            parameters.Add("@screentype", creditNoteSearch.screentype);

            var res = await this._dbRepository.GetItemsAsync("sp_SearchCreditNoteDetail", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CreditNote>>(res) ?? new List<CreditNote>();
            }

            return new List<CreditNote>();
        }

        public async Task<string> UploadCreditNoteRequest(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@xml", xmlString);
            parameters.Add("@Createdby", userId);

            var res = await this._dbRepository.GetItemsAsync("Upload_CreditNote", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<DataSet> ExportCreditNoteRequest(CreditNoteSearch creditNoteSearch)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Purpose"] =creditNoteSearch.Purpose,
                ["@Company_id"] = creditNoteSearch.Company_id,
                ["@RefId"] = creditNoteSearch.RefId,
                ["@Pay_period_id"] = creditNoteSearch.Pay_period_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_SearchCreditNoteDetail_ExportToExcel_NEW", parameters);
        }

    }
}
