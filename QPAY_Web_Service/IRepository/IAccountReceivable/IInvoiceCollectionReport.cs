using System.Data;
using System.Threading.Tasks;
using QPay.UI.Models.AccountReceivableMod;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface IInvoiceCollectionReport
    {
        Task<DataSet> InvoiceCollectionExportToExcel(InvoiceCollectionReport payload);
        Task<DataSet> GetGENTabledata(string Description, string Flag);
    }
}