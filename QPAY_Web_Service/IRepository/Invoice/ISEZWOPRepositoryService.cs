using QPay.UI.Models.Invoice;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Common.Invoices
{
    public interface ISEZWOPRepositoryService
    {
        Task<List<SEZWOPRepository>> SearchAsync(int companyId, int payPeriodId, string invoiceNumbers, int year);
        Task<SEZWOPRepository> UploadfileAsync(string cancelledInvoiceRepositoryDetails, int userId, string action);
        Task<DataSet> ExportToExcelAsync(int? companyId, string statusId, string invoiceNumbers, int? year);
        //Task<List<DocumentTypeMaster>> GetDocumentTypeMasterAsync();
    }
}
