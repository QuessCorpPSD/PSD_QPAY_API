using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Invoice
{
    public class CreditNoteApproveRepository: ICreditNoteApproveRepository
    {
        private readonly DbRepository _dbRepository;

        public CreditNoteApproveRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<List<CreditNote>> GetCreditNoteSearch(CreditNoteSearchApprove creditNoteSearchApprove)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@fromdate", creditNoteSearchApprove.fromdate);
            parameters.Add("@todate", creditNoteSearchApprove.todate);
            parameters.Add("@CompanyId", creditNoteSearchApprove.companyId);

            var res = await this._dbRepository.GetItemsAsync("sp_BlankSearchCreditNoteDetail", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CreditNote>>(res) ?? new List<CreditNote>();
            }

            return new List<CreditNote>();
        }

        public async Task<string> UploadCreditNote(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Upload_CreditNotebulkApproval", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        //public async Task<DataSet> ExportCreditNoteRequest(CreditNoteSearch creditNoteSearch)
        //{
        //    var parameters = new Dictionary<string, object>
        //    {
        //        ["@Purpose"] = creditNoteSearch.Purpose,
        //        ["@Company_id"] = creditNoteSearch.Company_id,
        //        ["@RefId"] = creditNoteSearch.RefId,
        //        ["@Pay_period_id"] = creditNoteSearch.Pay_period_id
        //    };
        //    return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_SearchCreditNoteDetail_ExportToExcel_NEW", parameters);
        //}
    }
}
