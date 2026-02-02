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
    public class IncrementReportRepository : IIncrementReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public IncrementReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(int? CompanyId, int? Pay_Period_Id, int? EmployeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@PayPeriodId"] = Pay_Period_Id,
                ["@EmployeeCode"] = EmployeeId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("PROC_ExportIncrementDetailsCompanywise", parameters, 1500);
        }
    }
}
