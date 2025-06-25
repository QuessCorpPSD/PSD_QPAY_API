using QPay.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IFinancialYearRepository
    {
        Task<List<FinancialYearUI>> GetFinancialYears();
    }
}
