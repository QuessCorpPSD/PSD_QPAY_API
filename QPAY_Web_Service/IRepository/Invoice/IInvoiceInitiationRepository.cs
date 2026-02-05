using QPay.UI.Common;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace QPay.BAL.IRepository.Invoice
{
    public interface IInvoiceInitiationRepository
    {
        Task<List<CommonUI>> GetTaxTypes(string action);
        Task<List<InvoiceInitiationUI>> Search(int? Company_Id, string PayPeriod, int? TaxTypeId);
        Task<FileResponse> InitiationSearchExport(InitiationRequestModel initiationRequestModel);
        Task<InvoiceInitiationUI> InvoiceInitiate(int? TaxTypeId, string xml, string action, int userId);
        Task<List<InitiationRequestUI>> InitiationSearch(InitiationRequestModel initiationRequestModel);
        Task<FileResponse> ExportToExcel(int? CompanyId, string PayPeriodId, int? TaxTypeId);
        Task<List<RemarksResponse>> getRemarksByReqNo(RequestModel requestModel);
    }
}
