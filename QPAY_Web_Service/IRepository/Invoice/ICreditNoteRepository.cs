using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.Customer;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Invoice
{
    public interface ICreditNoteRepository
    {
        Task<List<CreditNotePurpose>> GetCreditNotePurpose(int companyId);
        Task<List<CreditNote>> GetCreditNoteSearch(CreditNoteSearch creditNoteSearch);
        Task<string> UploadCreditNoteRequest(string xmlString, string userId);
        Task<DataSet> ExportCreditNoteRequest(CreditNoteSearch creditNoteSearch);
    }
}
