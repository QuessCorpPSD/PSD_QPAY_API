using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Invoice
{
    public interface IPOInvoiceInitiateRepository
    {
        Task<List<POInvoiceInitiate>> Search(int companyId, int payPeriodId);
        Task<DataSet> POInvoiceRequest(int companyId, int payPeriodId);

        Task<string> POInvoiceInitiate(string xml, int createdBy);
        DataSet POInvoiceInitiateExport(int companyId, int payPeriodId);
        Task<PoIntiateResponse> Upload(IFormFile file, [FromForm] string User);

    }
}
