using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using System.Data;

namespace QPay.BAL.Repository
{
    public class ITCalenderRepository : IITCalenderRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public ITCalenderRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? companyId, int? financialYearId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = companyId,
                ["@FinancialYearId"] = financialYearId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllITCalendarDetails", parameters, 1500);
        }


        public async Task<DataSet> GetFinancialYear()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@FinancialYearId"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetFinancialYear", parameters, 1500);
        }


        public async Task<DataSet> Create(ITCalenderRequest items)
        {
            var itCalenderResponse = new ITCalenderResponse();
            itCalenderResponse.ITCalender = new ITCalenderDetails[1];
            itCalenderResponse.ITCalender[0] = items.parentDetail;
            string itCalenderResponseSerialize = GenericSerializer<ITCalenderResponse>.Serialize(itCalenderResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = itCalenderResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateITCalender1", parameters);
        }

    }
}
