using QPay.API.Models;
using QPay.UI.Invoice;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IInvoiceInitiationRepository
    {
        Task<List<CommonUI>> GetTaxTypes(string action);
        Task<List<InvoiceInitiationUI>> Search(int? Company_Id, string PayPeriod, int? TaxTypeId);
        Task<FileResponse> InitiationSearchExport(InitiationRequestModel initiationRequestModel);
        Task<InvoiceInitiationUI> InvoiceInitiate(int? TaxTypeId, string xml, string action, int userId);
        Task<List<InitiationRequestUI>> InitiationSearch(InitiationRequestModel initiationRequestModel);
        Task<List<InitiationRequestUI>> InitiationSearchAllot(InvoiceDetailModel invoiceDetailModel);
        Task<FileResponse> ExportToExcel(int? CompanyId, string PayPeriodId, int? TaxTypeId);
        Task<List<InvoiceDashboardDto>> GetAllInvoiceAllotDetails(InvoiceDetailModel invoiceDetailModel);
    }
}
