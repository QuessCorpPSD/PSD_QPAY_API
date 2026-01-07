using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using System.Data;

namespace QPay.BAL.Repository
{
    public class CorporateBankRepository : ICorporateBankRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public CorporateBankRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Bank_Name"] = "",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_SerarchCorporateBank_API", parameters, 1500);
        }

        public async Task<DataSet> Create(CorporateBankRequest items)
        {
            var parentdata = GenericSerializer<CorporateBank>.Serialize(items.parentDetail);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateCorporateBank", parameters);
        }

    }
}
