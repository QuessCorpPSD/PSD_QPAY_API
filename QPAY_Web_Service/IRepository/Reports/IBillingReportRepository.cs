using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Reports
{
    public interface IBillingReportRepository
    {
        Task<DataSet> GetBillingReport(string companyCode, string siteId, string payPeriodId);
    }
}
