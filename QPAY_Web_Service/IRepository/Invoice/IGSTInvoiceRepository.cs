using Microsoft.AspNetCore.Mvc;
using QPay.DAL.Repository;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.Invoice;

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
        Task<List<GetGstRateUI>> GetGstRates(GetGstRateRequest request);
        Task<string> GetParticulars(SendRequest request);
        Task<string> GetInvoiceStatus(InvoiceStatusUI request);
        Task<List<PayPeriodUI>> GetPayPeriod(PayPeriodRequest request);
        Task<string> Edit(GstInvoiceEditRequest request);

    }
}
