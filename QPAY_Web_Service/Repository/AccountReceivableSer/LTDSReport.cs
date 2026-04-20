using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using QPay.UI.Models.AccountReceivableMod;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static QPay.UI.Common.StandingDataEnum;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class LTDSReport : ILTDSReport
    {
        private readonly DbRepository _dbRepository;

        public LTDSReport(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<DataSet> GetLTDSReportType(string action)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = action
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_Manage_LTDS_Report",  
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetFinancialYear(int? financialYearId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@FinancialYearId"] = financialYearId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetFinancialYear",   
                parameters,
                1500
            );
        }
        public async Task<DataSet> ExportToExcel(LTDSExportRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@ReportTypeId"] = request.ReportTypeId,
                ["@FinancialYearId"] = request.FinancialYearId,
                ["@TanNumber"] = request.TanNumber,
                ["@FromDate"] = request.FromDate,
                ["@ToDate"] = request.ToDate,
                ["@Action"] = "Report",                   
                ["@businessUnitId"] = request.BusinessUnitName
            };

            var ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_Manage_LTDS_Report",
                parameters,
                1500
            );

            if (ds != null && ds.Tables.Count > 0)
                return ds;

            return new DataSet();
        }
        public async Task<DataSet> GetBusinessUnits(int reportTypeId)
        {
            if (reportTypeId != 1144)
            {
                return new DataSet();
            }

            var parameters = new Dictionary<string, object?>();

            var ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetAllBusinessUnits",
                parameters,
                1500
            );

            return ds ?? new DataSet();
        }
    }
}