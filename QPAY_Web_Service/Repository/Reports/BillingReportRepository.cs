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
    public class BillingReportRepository:IBillingReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public BillingReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetBillingReport(string companyCode, string siteId, string payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyCode"] = companyCode,
                ["@SiteCode"] = siteId,
                ["@Payperiod"] = payPeriodId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_BillableReportfordownload", parameters, 1500);
        }
    }
}
