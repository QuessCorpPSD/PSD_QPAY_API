using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Reports.Payslip;

namespace QPay.BAL.IRepository.Reports
{
    public interface IPayslipReportRepository
    {
        Task<DataSet> GetEmployee(int CompanyId, int PayperiodId);
        Task<DataSet> DownloadPayslip(int EmployeeId, string payperiod);
    }
}
