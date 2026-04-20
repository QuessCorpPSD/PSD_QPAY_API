using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using QPay.UI.Models.AccountReceivableMod;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QPay.UI_Domain.Models.AccountReceivable;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class CollectionPendingReportRepository: ICollectionPendingReportRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public CollectionPendingReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
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

        public async Task<DataSet> GetEntity(string flag)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = flag
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_ManageLegalEntityMapping",
                parameters,
                1500
            );
        }

        public async Task<DataSet> CollectionPendingExportToExcel(CollectionPendingExport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@company_Id"] = payload.CompanyId,
                ["@Financial_Id"] = payload.FinancialId,
                ["@As_On_Date"] = payload.AsOnDate,
                ["@AllEntityId"] = payload.AllEntityId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_BankInvoice_CollectionPending_Report",
                parameters,
                1500
            );
        }


    }
}
