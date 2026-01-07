using QPay.UI.Customer;
using System.Data;

namespace QPay.BAL.IRepository
{
    public interface IITCalenderRepository
    {       
        Task<DataSet> Search(int? companyId, int? financialYearId);
        Task<DataSet> GetFinancialYear();
        Task<DataSet> Create(ITCalenderRequest request);
    }
}
