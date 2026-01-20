using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Report.EntityReport;

namespace QPay.BAL.Repository.Reports
{
    public class NetpaySummaryRepository : INetpaySummaryRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public NetpaySummaryRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(int? CompanyId, int? PayPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@PayPeriodID"] = PayPeriodId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_NetPaySummary", parameters, 1500);
        }

        public async Task<DataSet> ExportToExcel_Entity(EntityPayregisterFilter items)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EntityId"] = items.EntityId,
                ["@PayPeriodID"] = items.PayPeriod,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_NetPaySummary_EntityWise", parameters, 1500);
        }

    }
}
