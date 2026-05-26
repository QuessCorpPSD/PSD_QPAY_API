using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.AccountReceivableRepository
{
    public class BankTransferhthRepository : IBankTransferRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public BankTransferhthRepository(
            DbRepository dbRepository,
            IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        // SEARCH

        public async Task<DataSet> Search(
            string FromDate,
            string ToDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Search",
                ["@From_Date"] = FromDate,
                ["@To_Date"] = ToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_Manage_hth_bank_transfer_status",
                parameters,
                1500
            );
        }

        // EXPORT EXCEL

        public async Task<DataSet> ExportToExcel(
            string FromDate,
            string ToDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Export",
                ["@From_Date"] = FromDate,
                ["@To_Date"] = ToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_Manage_hth_bank_transfer_status",
                parameters,
                1500
            );
        }
    }
}