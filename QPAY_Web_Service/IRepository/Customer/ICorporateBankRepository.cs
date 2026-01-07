using QPay.UI.Customer;
using System.Data;

namespace QPay.BAL.IRepository
{
    public interface ICorporateBankRepository
    {       
        Task<DataSet> Search();
        Task<DataSet> Create(CorporateBankRequest request);
    }
}
