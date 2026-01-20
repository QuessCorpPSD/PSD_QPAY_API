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

namespace QPay.BAL.Repository.Reports
{
    public class OtherIncomeProcessEmployeeRepository : IOtherIncomeProcessEmployeeRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public OtherIncomeProcessEmployeeRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(string? PayPeriod)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@PayPeriod"] = PayPeriod
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(@"sp_ProcessOtherIncome_Report_ExportToExcel", parameters, 1500);
        }


    }
}
