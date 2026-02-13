using Microsoft.AspNetCore.Mvc;
using QPay.DAL.Repository;
using QPay.UI.Models.Invoice;
using QPay.UI.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.Invoice;
using static QPay.UI.Invoice.Invoice;
using Microsoft.AspNetCore.Http;
using InvoiceResponse = QPay.UI.Models.Invoice.InvoiceResponse;

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
        Task<string> Reject(string xmlString, string userId,string status);
        Task<List<InvoiceCancelGrid>> GetAllInvoiceCancelDetails(int companyId, int payPeriod);
        Task<InvoiceCancelResponse> BulkApproveInvoice(InvoiceCancelApprovalRequest request);
        Task<string> BulkRejectInvoice(InvoiceCancelApprovalRequest request);
        Task<EInvoice> GetEInvoiceData(string invoiceIds, string UserId, string Action);
        Task<string> SaveBatchResponse(int StatusCode, string ResponseMessage, string Response, string ResponseXml, string InvoiceIds, string Mode, string UserId);
        string GetFilename(int invoice_Id);
        Task<InvoiceDetail> GetInvoiceDetailByInvoiceId(int invoiceId);
        Task<ClientPeriodUI> CompanyPayPeriod(int payperiod);
        Task<InvoiceNumberLotUI> IRNStatusGenerationUpdate(string Invoice_Number);
        Task<List<AttributeUI>> GetAllAttribute(AttributeUI attributeUI);
        Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiodId, [FromForm] string CreatedBy);
        Task<DataSet> GetConsolidateInvoiceSummary(int companyId, int payperiodid);

        Task<DataSet> GetEInvoiceError(int invoiceId);
        Task<DataSet> GetEInvoiceErrorHover(int invoiceId);
    }
}
