using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.SalaryReleaseInvoice
{
    public interface IBankNeftCultureInvoiceRepository
    {
        
        DataSet NeftCulturesearch(int Company_Id, int UserId);

        DataSet NeftCultureExport(int Company_Id, int UserId);

        List<NeftCulture> GetNeftBankculture(int Company_Id,string Mode, int UserId);

        Task<List<CultureMessage>> NeftCultureSave(Culturesave payload);
    }
}
