using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Reports
{
    public interface IInvoiceLeaveBalanceReportRepository
    {
        Task<DataSet> GetLeaveBalance(string companyId, string siteId, string fromMonth, string fromYear,
            string toMonth, string toYear);
    }
}
