using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using QPay.UI.Models.AccountReceivableMod;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class InvoiceCollectionreport : IInvoiceCollectionReport
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;


        public InvoiceCollectionreport(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> InvoiceCollectionExportToExcel(InvoiceCollectionReport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@DateTypeId"] = payload.DateTypeId,    
                ["@CompanyId"] = payload.CompanyId,
                ["@PayperiodId"] = payload.PayPeriodId,
                ["@FromDate"] = payload.FromDate,
                ["@ToDate"] = payload.ToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Sp_GetAllInvoiceCollectionReport_Export2Excel",
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetGENTabledata(string Description, string Flag)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Description"] = Description,
                ["@Action"] = Flag
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_CommonDropDowns", parameters, 1500);
        }
    }
}