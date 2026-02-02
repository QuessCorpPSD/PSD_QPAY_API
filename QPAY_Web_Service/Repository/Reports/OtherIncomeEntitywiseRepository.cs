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
    public class OtherIncomeEntitywiseRepository : IOtherIncomeEntitywiseRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public OtherIncomeEntitywiseRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(EntityPayregisterFilter items)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EntityId"] = items.EntityId,
                ["@PayPeriod"] = items.PayPeriod,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(@"sp_OtherIncome_Report_ExportToExcel_Entitywise", parameters, 1500);
        }

        public async Task<DataSet> GetEntity(string Action)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = Action
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(@"Proc_ManageLegalEntityMapping", parameters, 1500);
        }


    }
}
