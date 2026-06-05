using QPay.DTo.Models.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.IRepository.iRepository.Masters
{
    public interface IInvoiceRuleRepository
    {
        Task<List<InvoiceRule>> GetAllInvoiceRule(int? companyId, string? siteId);
        Task<string> PostAddInvoiceRule(InvoiceRuleAdd invoiceRuleAdd);
        Task<string> PostUpdateInvoiceRule(InvoiceRuleUpdate invoiceruleUpdate);
        Task<string> PostDeleteInvoiceRule(int invoicingRulesID);
        //DataSet GetInvoiceRuleTemplate(int companyId, string siteName);
        Task<string> PostInvoiceRuleUpload(string xmlString, string userId);
        DataSet InvoiceRuleExport(int companyId, int siteCode);
        DataSet GetInvoiceruleTemplate(int? companyId, string? siteName);

    }
}
