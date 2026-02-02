using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace QPay.BAL.IRepository.Reports
{
    public interface ITimesheetSummaryReportRepository
    {
        Task<DataSet> GetTSSummaryReport(string companyId, string siteId, string location,
                string payPeriodId, string status);
    }
}
