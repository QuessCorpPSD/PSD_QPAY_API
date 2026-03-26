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
    public interface IFormulaRepository
    {
        Task<DataSet> Search(int? payCodeId);
        //Task<DataSet> GetFormulaPayCodeList();

        Task<DataSet> GetPayCategory(int companyId);

        Task<DataSet> Create(FormulasRequest request);
        Task<DataSet> CreateMC(MCFormulasRequest request);

        Task<DataSet> GetPayrollType();
        

        //Task<DataSet> GetCriteria(int? CriteriaTypeId);
        //Task<List<CategoryUI>> GetCategory();
    }
}
