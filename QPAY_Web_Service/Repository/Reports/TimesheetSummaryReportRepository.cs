using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;


namespace QPay.BAL.Repository.Reports
{
    public class TimesheetSummaryReportRepository : ITimesheetSummaryReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public TimesheetSummaryReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetTSSummaryReport(string companyId, string siteId, string location,
                string payPeriodId, string status)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Group_Detail_Id"] = string.IsNullOrEmpty(siteId) ? "-1" : siteId,
                ["@Payperiod_ID"] = string.IsNullOrEmpty(payPeriodId) ? "-1" : payPeriodId,
                ["@City_Id"] = string.IsNullOrEmpty(location) ? "-1" : location,
                ["@Status"] = string.IsNullOrEmpty(status) ? "-1" : status
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetEmployeeWiseConsolidatedTSReport", parameters, 1500);
        }
    }
}
