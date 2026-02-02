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
using QPay.UI.Models.Reports;


namespace QPay.BAL.Repository.Reports
{
    public class LeaveBalanceReportRepository : ILeaveBalanceReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public LeaveBalanceReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<List<LeaveBalance>> GetLeaveYear()
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync("USP_Leave_YearBinding", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<LeaveBalance>>(res) ?? new List<LeaveBalance>();
            }

            return new List<LeaveBalance>();
        }

        public async Task<DataSet> GetLeaveBalance(string CompanyCode, string SiteId, string Year)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@company_code"] = CompanyCode,
                ["@GROUPNAME"] = SiteId,
                ["@YEAR"] = Year,
                ["@SearchDownload"] = "Search"
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_VIEW_LeaveBalance", parameters, 1500);
        }

    }
}
