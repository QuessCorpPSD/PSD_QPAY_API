using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Report.EntityReport;

namespace QPay.BAL.Repository.Reports
{
    public class BillingUBRRepository : IBillingUBRRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public BillingUBRRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetBillingReport(PayperiodFilter items)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Pay_Period"] = items.PayPeriod,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_Billing_UBRReport", parameters, 1500);
        }
    }
}
