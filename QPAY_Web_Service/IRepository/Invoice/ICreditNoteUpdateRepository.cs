using QPay.BAL.Repository.Invoice;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Invoice
{
    public interface ICreditNoteUpdateRepository
    {
        Task<List<CreditNote>> GetCreditNoteSearch(CreditNoteSearchApprove creditNoteSearchApprove);
        Task<string> UploadCreditNoteCancel(string xmlString, string userId);
        DataSet GetInvoiceData(int Company_Id, int Invoice_ID, int CreditNoteId, string InvoiceNumber, string PdfType);
    }
}
