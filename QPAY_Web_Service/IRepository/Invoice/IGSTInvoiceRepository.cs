using QPay.UI.Models.Invoice;
using System.Data;

namespace QPay.BAL.IRepository.Invoice
{
    public interface IGSTInvoiceRepository
    {
        Task<List<GstInvoiceGrid>> GetGSTInvoice(int userId);
        DataSet GetInvoiceData(int invoiceId);
        Task<string> PostCancelReject(string xmlString, string userId);
        //Task<GstInvoiceCreateResponse> Create(GstInvoiceCreateRequest request);
        Task<string> Create(GstInvoiceCreateRequest request);
        Task<List<UI.Models.Invoice.InvoiceTypeUI>> GetGSTInvoiceType();
        Task<List<BillingTypeUI>> GetGSTBillableType();
        Task<List<CtcDeductionUI>> GetGSTCtcDeductionType();
        Task<List<NewDeductionUI>> GetGSTNetDeductionType();
    }
}
