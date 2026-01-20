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
    public interface IProvisionalInvoiceRepository
    {
            Task<DataSet> GetProvisionalInvoice(int CompanyId, string payPeriodId, string createdBy);
            Task<InvoiceResponse> ProvisionalInvoiceSplit(IFormFile file, [FromForm] string CompanyId,
                [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
            Task<InvoiceResponse> ProvisionalInvoiceInitiate(ProvisionalInvoiceInitiateRequest provisionalrequest);

            //Task<InvoiceResponse> UpdateMapName(IFormFile file, [FromForm] string CompanyId,
            //    [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId);
            //Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
            //   [FromForm] string payperiodId, [FromForm] string CreatedBy);
    }
}
