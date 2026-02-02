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
    public class ProcessEmployeeRepository : IProcessEmployeeRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ProcessEmployeeRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(PayperiodFilter items)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = 0,
                ["@Payperiod"] = items.PayPeriod,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ReportProcessemployees", parameters, 1500);
        }
        
    }
}
