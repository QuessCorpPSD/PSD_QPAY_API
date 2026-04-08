using System.Data;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.IRepository.AccountReceivable
{
    public interface IClientAdvancePaymentReportRepository
    {
        Task<DataSet> Search(int? CompanyId, string FromDate, string ToDate);
        Task<DataSet> ExportToExcel(CommonExport payload);
        Task<DataSet> GetDateTypeClientAdvPay(string Description, string Action);
    }
}