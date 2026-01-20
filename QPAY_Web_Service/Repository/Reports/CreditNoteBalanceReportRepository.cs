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
    public class CreditNoteBalanceReportRepository : ICreditNoteBalanceReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public CreditNoteBalanceReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(companywithdateFilter items)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@fromdate"] = items.FromDate,
                ["@todate"] = items.ToDate,
                ["@Company_id"] = items.CompanyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreditNoteBalanceReport", parameters, 1500);
        }
    }
}
