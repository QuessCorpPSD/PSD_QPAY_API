using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Invoice;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.DraftNew;



namespace QPay.BAL.IRepository.Invoice
{
    public interface IDraftNewRepository
    {
        Task<UI.Models.Invoice.InvoiceBackDatedUI> GetBackDated(int companyId, int payPeriod_Id);
        Task<DataSet> GetPerformaInvoice(int CompanyId, string PayPriod, string createdBy);
        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> PerformaInvoiceSplit(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> PerformaInvoiceMerge(UI.Models.Invoice.DraftNew.LotMergeRequest request);
        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> PerformaInvoiceInitiate(UI.Models.Invoice.DraftNew.DraftInvoiceInitiate request);
        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> PerformaInvoiceSkip(UI.Models.Invoice.DraftNew.DraftInvoiceInitiate request);

        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> UpdateMapName(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiodId, [FromForm] string CreatedBy);
        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> UploadAttributesNew(IFormFile file, [FromForm] string CompanyId,
   [FromForm] string payperiodId, [FromForm] string CreatedBy);

        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> PerformaInvoiceMergeNew(List<UI.Models.Invoice.DraftNew.MergeNewRequest> request);
        Task<DataSet> GetSplitTemplate(UI.Models.Invoice.SplitParams splitParams);
        Task<List<UI.Models.Invoice.DraftNew.InvoiceInitiateRequest>> GetDraftInformation(int CompanyId, int PayPriod, string createdBy);
        Task<string> PostInvoicePush(int company_id, int Pay_Period_Id, string xml, string CreatedBy, int DraftTypeId, string Action);
        Task<InvoiceDetail> GetInvoiceDetailByInvoiceId(int invoiceId);
        Task<InvoiceNumberLotUI> IRNStatusGenerationUpdate(string Invoice_Number);
        Task<DataSet> GetEmployeeReport(EmpExport empExport);
        Task<DataSet> GetInvoiceCountDetails(InvoiceCountRequest request);
        Task<DataSet> GetPassThroughTemplate(SplitParams splitParams);
        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> UploadPassThrough(IFormFile file, [FromForm] string CompanyId,
    [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
        Task<UI.Models.Invoice.DraftNew.InvoiceResponse> UploadPush(IFormFile file, [FromForm] string CompanyId,
[FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
    }
}
