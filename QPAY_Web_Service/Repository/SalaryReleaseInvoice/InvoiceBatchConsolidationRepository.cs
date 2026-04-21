using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.DAL.Repository;
using QPay.UI.Models.SalaryReleaseInvoice;
using System.Data;

namespace QPay.BAL.Repository.SalaryReleaseInvoice
{
    public class InvoiceBatchConsolidationRepository : IinvoiceBatchConsolidationRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public InvoiceBatchConsolidationRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }

        public async Task<DataSet> GetBusinessUnitName()
        {
            var parameters = new Dictionary<string, object?>();

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetAllBusinessUnits",
                parameters,
                1500
            );
        }

        public async Task<DataSet> InvoiceBatchConsolidationExport(InvoiceBatchExport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = payload.ReportType,
                ["@From_Date"] = payload.FromDate,
                ["@To_Date"] = payload.ToDate,
                ["@txtsearch"] = payload.TxtSearch,
                ["@AllEntityId"] = payload.AllEntityId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
    "Proc_BatchConsolidationReport",
    parameters,
    1500
);
        }
    }
}