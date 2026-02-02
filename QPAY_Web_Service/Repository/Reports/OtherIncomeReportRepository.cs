using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Reports
{
    public class OtherIncomeReportRepository : IOtherIncomeReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public OtherIncomeReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetInputno(int? CompanyId, int? payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,
                ["@pay_Frequency_detail_id"] = payPeriodId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(@"sp_GetOterIncomeInputno", parameters, 1500);
        }

        public async Task<DataSet> ExportToExcel(int? companyId, int? paySequenceNo, int? payCodeId, string? inputNo)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_ID"] = companyId,
                ["@Pay_Frequency_Detail_Id"] = paySequenceNo,
                ["@PayCode"] = payCodeId,
                ["@Inputno"] = inputNo,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(@"sp_OtherIncome_Report_ExportToExcel", parameters, 1500);
        }


    }
}
