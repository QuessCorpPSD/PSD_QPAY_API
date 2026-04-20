using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.DAL.Repository;
using System.Data;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.Repository.AccountReceivable
{
    public class ClientAdvancePaymentRepositoryReport : IClientAdvancePaymentReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public ClientAdvancePaymentRepositoryReport(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? CompanyId, string FromDate, string ToDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,
                ["@From_Date"] = FromDate,
                ["@To_Date"] = ToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Sp_ClientAdvancePaymentReportSearch",
                parameters,
                1500
            );
        }

        public async Task<DataSet> ExportToExcel(CommonExport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Convert.ToInt32(payload.companyId),
                ["@From_Date"] = payload.fromDate,
                ["@To_Date"] = payload.toDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Sp_ClientAdvancePaymentReportExportToExcel",
                parameters,
                1500
            );
        }

        public async Task<DataSet> GetDateTypeClientAdvPay(string Description, string Action)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Description"] = Description,
                ["@Action"] = Action
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_CommonDropDowns", parameters, 1500);
        }
    }
}