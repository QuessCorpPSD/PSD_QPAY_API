using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.SalaryReleaseInvoice
{
    public class InvoiceBatchConsolidationRepository: IinvoiceBatchConsolidationRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public InvoiceBatchConsolidationRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
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



    }
}
