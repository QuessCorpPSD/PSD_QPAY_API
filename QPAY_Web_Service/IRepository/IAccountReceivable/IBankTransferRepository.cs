using System.Data;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface IBankTransferRepository
    {
        Task<DataSet> Search(string FromDate, string ToDate);

        Task<DataSet> ExportToExcel(string FromDate, string ToDate);
    }
}