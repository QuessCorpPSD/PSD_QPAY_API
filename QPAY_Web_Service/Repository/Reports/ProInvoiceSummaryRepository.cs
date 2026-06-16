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
    public class ProInvoiceSummaryRepository : IProInvoiceSummaryRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ProInvoiceSummaryRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(int? CompanyId, string FromDate, string ToDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@FromDate"] = FromDate,
                ["@ToDate"] = ToDate
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_ProvisionalInvoiceReport", parameters, 1500);
        }

    }
}
