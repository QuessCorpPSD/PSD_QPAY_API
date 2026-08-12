using QPay.UI.Invoice;
using QPay.UI_Domain.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.POCulture;

namespace QPay.BAL.IRepository.Invoice
{
    public interface IPOCultureRepository
    {

        Task<List<PoCulture>> GetAllPOCulture(int companyId, int userId);
         Task<List<PurchaseOrder>> GetAllPONumbers(int companyId, int userId);

        Task<DataSet> Create(POCultureRequest model, int createdBy);

        Task<string> PostPOCulture(string xmlString, string userId);

        DataSet POCultureExport(int companyId,int userId);

    }
}
