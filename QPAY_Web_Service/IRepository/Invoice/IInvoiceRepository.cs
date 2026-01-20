using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.Invoice.Invoice;


namespace QPay.BAL.IRepository.Invoice
{
    public interface IInvoiceRepository
    {
        Task<DataSet> GetPerformaInvoice(int CompanyId, string PayPriod, int InvoiceBillingType, string createdBy);
        Task<InvoiceResponse> PerformaInvoiceSplit(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
        Task<InvoiceResponse> PerformaInvoiceMerge(LotMergeRequest request);
        Task<InvoiceResponse> PerformaInvoiceInitiate(DraftInvoiceInitiate request);
        Task<InvoiceResponse> UpdateMapName(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
        Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiodId, [FromForm] string CreatedBy);
        Task<InvoiceResponse> PerformaInvoiceMergeNew(List<MergeNewRequest> request);

    }
}
