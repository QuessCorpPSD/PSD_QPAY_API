using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Reports
{
    public interface IIncrementReportRepository
    {
        Task<DataSet> ExportToExcel(int? CompanyId, int? Pay_Period_Id, int? EmployeeId);
    }
}
