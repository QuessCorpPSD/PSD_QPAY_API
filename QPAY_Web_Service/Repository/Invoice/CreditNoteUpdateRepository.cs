using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class CreditNoteUpdateRepository: ICreditNoteUpdateRepository
    {
        private readonly DbRepository _dbRepository;

        public CreditNoteUpdateRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<List<CreditNote>> GetCreditNoteSearch(CreditNoteSearchApprove creditNoteSearchApprove)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@fromdate", creditNoteSearchApprove.fromdate);
            parameters.Add("@todate", creditNoteSearchApprove.todate);
            parameters.Add("@Company_id", creditNoteSearchApprove.companyId);

            var res = await this._dbRepository.GetItemsAsync("sp_SearchCreditNoteUpdate", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CreditNote>>(res) ?? new List<CreditNote>();
            }

            return new List<CreditNote>();
        }

        public async Task<string> UploadCreditNoteCancel(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Upload_BulkCreditNoteCancel", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public DataSet GetInvoiceData(int Company_Id, int Invoice_ID, int CreditNoteId, string InvoiceNumber, string PdfType)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_Id,
                ["@InvoiceId"] = Invoice_ID,
                ["@CreditNoteId"] = CreditNoteId,
                ["@InvoiceNum"] = InvoiceNumber,
                ["@PdfType"] = PdfType,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetCreditNotePdf", parameters, 1500);
        }
    }
}
