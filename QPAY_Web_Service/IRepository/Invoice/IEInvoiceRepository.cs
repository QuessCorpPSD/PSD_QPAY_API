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
using static QPay.UI.Invoice.Invoice;

namespace QPay.BAL.IRepository.Invoice
{
    public interface IEInvoiceRepository
    {
        Task<InvoiceNumberLotUI> IRNStatusGenerationUpdate(string Invoice_Number);
        Task<UI.Models.Invoice.InvoiceDetail> GetInvoiceDetailByInvoiceId(int invoiceId);
        Task<ClientPeriodUI> CompanyPayPeriod(int payperiod);
        Task<DataSet> GetAllInvoiceDetails(int companyId, int payPeriodId);
        Task<DataSet> EInvoiceExport(int companyId, int payPeriodId);
        DataSet GetInvoiceData(int invoiceId);
        UI.Invoice.EInvoice GetEInvoiceData(string invoiceIds, string UserId, string Action);
        string SaveBatchResponse(int StatusCode, string ResponseMessage, string Response, string ResponseXml, string InvoiceIds, string Mode, string UserId);
        Task<DataSet> GetEInvoiceError(int invoiceId);
        Task<DataSet> GetEInvoiceErrorHover(int invoiceId);
        Task<List<UI.Invoice.InvoiceColors>> GetAllInvoiceTypeColors();
        //FileResponse PayRegisterDownload(int companyCode, int pay_period_Id, string payPeriod);
        Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
        [FromForm] string payperiodId, [FromForm] string CreatedBy);

        Task<DataTable> GetInvoiceSummaryByInvoiceId(string Invoice_Number);
        Task<DataSet> GetConsolidateInvoiceSummary(int companyId, int payperiodid);
        DataTable PayRegisterDownload(int companyId, int payperiodid, string payperiod);
        Task<DataSet> NetPaySummaryByCompanyIDAndPayperiodId(int companyId, int pay_period_Id);
        DataTable GetPayRegisterSummary(int companyCode, int pay_period_Id);



    }
}
