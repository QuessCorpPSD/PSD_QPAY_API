using QPay.UI.Invoice;
using QPay.UI_Domain.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.BAL.IRepository.Invoice
{
    public interface IInvoiceCultureRepository
    {
        Task<List<ServiceChargeMastereDD>> GetAllServiceChargeMaster();
        Task<List<InvoiceTypeforCultureDD>> GetAllInvoiceType();
        Task<List<GenDD>> GetAllInvoiceCategories();      
        Task<DataSet> GetMapNameByService(int companyId);
        Task<DataSet> GetAllPayCodeFromCompany(int companyId);
        Task<DataSet> GetAllPayCodeFromCompanyOI(int companyId);
        Task<DataSet> Create(string xml, int createdBy, string mode, string invoiceType);

        Task<List<InvoiceStructure>> GetAllInvoiceCulture(int companyId);
        Task<string> PostInvoiceCulture(string xmlString, string userId);
        DataSet InvoiceCultureExport(int companyId);

    }
}
