using QPay.UI.Invoice;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Invoice
{
    public interface ISezRepository
    {
        Task<List<SEZWOPRepositoryUI>> Search(int companyId, int payPeriodId, string InvoiceNumbers, int Year);
        FileResponse ExportToExcel(int? companyId, int payPeriodId, string InvoiceNumbers, int? Year);
        Task<SEZWOPRepositoryUI> Uploadfile(string CancelledInvoiceRepositoryDetails, int UserId, string Action);
    }
}
