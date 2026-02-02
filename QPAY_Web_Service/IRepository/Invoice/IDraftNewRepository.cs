using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        Task<InvoiceBackDatedUI> GetBackDated(int companyId, int payPeriod_Id);
        Task<DataSet> GetPerformaInvoice(int CompanyId, string PayPriod, string createdBy);
        Task<InvoiceResponse> PerformaInvoiceSplit(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
        Task<InvoiceResponse> PerformaInvoiceMerge(LotMergeRequest request);
        Task<InvoiceResponse> PerformaInvoiceInitiate(DraftInvoiceInitiate request);
        Task<InvoiceResponse> PerformaInvoiceSkip(DraftInvoiceInitiate request);
        Task<InvoiceResponse> UpdateMapName(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
        Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiodId, [FromForm] string CreatedBy);
        Task<InvoiceResponse> PerformaInvoiceMergeNew(List<MergeNewRequest> request);
        Task<DataSet> GetSplitTemplate(SplitParams splitParams);
        Task<List<InvoiceInitiateRequest>> GetDraftInformation(int CompanyId, int PayPriod, string createdBy);
        Task<List<InvoiceInitiateRequest>> PostInvoicePush(int company_id, int Pay_Period_Id, string xml, string CreatedBy, int DraftTypeId, string Action);

    }
}
