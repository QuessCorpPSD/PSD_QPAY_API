using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QPay.UI.Models.Reports;


namespace QPay.BAL.IRepository.Reports
{
    public interface ILeaveBalanceReportRepository
    {
        Task<List<LeaveBalance>> GetLeaveYear();
        Task<DataSet> GetLeaveBalance(string CompanyCode, string SiteId,string Year);
    }
}
