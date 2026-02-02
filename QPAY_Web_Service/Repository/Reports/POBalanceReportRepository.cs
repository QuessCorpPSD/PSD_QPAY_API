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
    public class POBalanceReportRepository : IPOBalanceReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public POBalanceReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(int? CompanyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_PO_BALANCE_REPORT", parameters, 1500);
        }
    }
}
