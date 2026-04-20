using QPay.UI.Models.AccountReceivableMod;
using System.Data;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface ILTDSReport
    {
        Task<DataSet> GetLTDSReportType(string action);
        Task<DataSet> GetFinancialYear(int? financialYearId);
        Task<DataSet> LTDSReportExportToExcel(LTDSExportRequest request);
        Task<DataSet> GetBusinessUnits(int reportTypeId);
    }
}