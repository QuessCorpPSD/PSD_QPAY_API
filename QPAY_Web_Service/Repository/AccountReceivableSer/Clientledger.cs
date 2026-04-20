using QPay.BAL.IRepository.IAccountReceivable;
using QPay.BAL.Models;
using QPay.DAL.Repository;
using QPay.UI.Models.AccountReceivableMod;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class Clientledger : IClientledger
    {
        private readonly DbRepository _dbRepository;
        public Clientledger(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
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
        public async Task<DataSet> ExportClientLedger(ClientLedgerExportRequest request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = request.CompanyId,
                ["@FromDate"] = request.FromDate,
                ["@ToDate"] = request.ToDate,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Sp_ClientLedger_Report",
                parameters,
                1500
            );
        }
    }
}
