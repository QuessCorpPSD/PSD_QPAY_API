using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IBankRepository
    {
        Task<DataSet> Search();
        
        Task<DataSet> Create(BankRequest request);

        //Task<DataSet> GetCriteria(int? CriteriaTypeId);
        //Task<List<CategoryUI>> GetCategory();
    }
}
