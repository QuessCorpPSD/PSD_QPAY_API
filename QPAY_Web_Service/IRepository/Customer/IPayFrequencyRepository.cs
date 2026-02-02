using QPay.UI.Customer;
using System.Data;

namespace QPay.BAL.IRepository
{
    public interface IPayFrequencyRepository
    {       
        Task<DataSet> Search(int? companyId);
        Task<DataSet> ExportToExcel(int? companyId);
        Task<DataSet> GetGroupName(int? companyId);
        Task<DataSet> GetData( string startDate, string endDate);
        Task<DataSet> CheckPayFrequencyExists(int companyId, string startDate, string endDate, string payPeriod);

        Task<DataSet> Create(PayFrequencyRequest request);
    }
}
