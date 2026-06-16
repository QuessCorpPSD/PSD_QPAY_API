using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
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
using static QPay.UI.Report.EntityReport;

namespace QPay.BAL.Repository.Reports
{
    public class CreditNoteReportRepository : ICreditNoteReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public CreditNoteReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(string? CompanyId, string FromDate, string ToDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,
                ["@FromDate"] = FromDate,
                ["@ToDate"] = ToDate
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_InternalCreditNoteDetail_ExportToExcel", parameters, 1500);
        }

    }
}
