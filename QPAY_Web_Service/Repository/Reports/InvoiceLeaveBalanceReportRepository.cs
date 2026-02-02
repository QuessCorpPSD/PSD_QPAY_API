using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;


namespace QPay.BAL.Repository.Reports
{
    public class InvoiceLeaveBalanceReportRepository : IInvoiceLeaveBalanceReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public InvoiceLeaveBalanceReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
        public async Task<DataSet> GetLeaveBalance(string companyId, string siteId, string fromMonth, string fromYear,
            string toMonth, string toYear)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@company_ID"] = companyId,
                ["@site_Name"] = siteId,
                ["@From"] = fromMonth + " " + fromYear,
                ["@To"] = toMonth + " " + toYear
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_Qzone_LeaveBalanceReport", parameters, 1500);
        }
    }
}
