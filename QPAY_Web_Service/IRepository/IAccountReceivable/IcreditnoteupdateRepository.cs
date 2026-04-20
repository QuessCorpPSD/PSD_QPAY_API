using Microsoft.AspNetCore.Http;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.AccountReceivableMod.creditnoteupdatemodel;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface IcreditnoteupdateRepository
    {
        Task<DataSet> CreditNoteSearch(int CompanyId, string fromdate, string todate);

        Task<DataSet> CreditNoteExportToExcel(CreditNoteExport payload);

        Task<CreditNoteUploadResponse> CreditNoteCancelUpload(IFormFile file, string User);

        DataSet GetInvoiceDetail(int companyId, int invoiceId, int creditNoteId, string invoiceNumber, string pdfType);

        Task<string> EditCreditNote(CreditNoteEditRequest request);
        Task<DataSet> CreditnoteEmployeeSearch(string creditNoteNo);
    }
}
