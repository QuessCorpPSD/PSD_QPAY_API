using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Invoice
{
    public interface ISEZRepositoryService
    {
        Task<List<SEZRepository>> Search(int companyId, int payPeriodId, string? InvoiceNumbers, int Year);
        string GetSEZFilename(int invoice_Id);
        Task<string> BulkApproveSEZ(ApproveRequest request);
    }
}
