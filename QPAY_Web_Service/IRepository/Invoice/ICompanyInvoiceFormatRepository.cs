using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Invoice
{
    public interface ICompanyInvoiceFormatRepository
    {
        Task<List<CompanyInvoiceFormat>> GetAllCompanyInvoiceFormat(int userId);
        Task<List<InvoiceTypeModel>> GetAllInvoiceType();
        Task<List<InvoiceFormat>> GetAllInvoiceFormat();
        Task<string> Create(InvoiceFormatAdd invoiceFormat);
    }
}
