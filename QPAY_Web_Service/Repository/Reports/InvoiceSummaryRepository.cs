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
    public class InvoiceSummaryRepository : IInvoiceSummaryRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public InvoiceSummaryRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> ExportToExcel(int? CompanyId, string FromDate, string ToDate, int? ReportTypeId,int? UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@FromDate"] = FromDate,
                ["@ToDate"] = ToDate,
                ["@ReportTypeId"] = ReportTypeId,
                ["@UserId"] = UserId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SpInvoiceDetails_Report", parameters, 1500);
        }

        public async Task<DataSet> ExportToExcel_Entity(EntityPayregister_Filter items)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EntityId"] = items.EntityId,
                ["@FromDate"] = items.FromDate,
                ["@ToDate"] = items.ToDate,
                ["@ReportTypeId"] = items.ReportTypeId,
                ["@UserId"] = items.UserId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SpInvoiceDetails_Report_EntityWise", parameters, 1500);
        }

        public async Task<DataSet> GetTaxTypes()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "GetTaxTypes"
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_CommonDropDowns", parameters, 1500);
        }

    }
}
