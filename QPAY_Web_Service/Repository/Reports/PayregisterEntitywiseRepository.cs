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
    public class PayregisterEntitywiseRepository : IPayregisterEntitywiseRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public PayregisterEntitywiseRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(EntityPayregisterFilter items)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EntityId"] = items.EntityId,
                ["@Pay_Period_Id"] = items.PayPeriod,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_PayRegister_withUnprocessed_Entitywise", parameters, 1500);
        }


    }
}
