using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Common.StandingDataEnum;

namespace QPay.BAL.IRepository
{
    public interface IBillableDaysRepository
    {
        Task<string> BillableDaysUpload(string xmlData, string createdBy, int importType);
        Task<List<BillableDaysUI>> SearchDetails(string mode, string value);
        Task<FileResponse> ExportToExcel(string xml);
        Task<FileResponse> DownloadTemplate(int importType);
        Task<List<UI.Models.Invoice.InvoiceTypeUI>> GetGSTInvoiceType();
    }
}
