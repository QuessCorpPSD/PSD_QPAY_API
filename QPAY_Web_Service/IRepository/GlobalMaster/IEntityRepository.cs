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
    public interface IEntityRepository
    {
        Task<DataSet> Search();
        Task<DataSet> GetQuessLegalEntity();

        Task<DataSet> Create(EntityRequest request);

        //Task<DataSet> GetCriteria(int? CriteriaTypeId);
        //Task<List<CategoryUI>> GetCategory();
    }
}
