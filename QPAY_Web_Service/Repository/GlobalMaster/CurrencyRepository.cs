using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Models.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.GlobalMaster
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly DbRepository _dbRepository;

        public CurrencyRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }


        public async Task<DataSet> GetAllCurrency(string flag)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@XmlData"] = "",
                ["@Action"] = flag,
                ["@UserId"] = ""
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_Currency", parameters); ;

        }

        public async Task<DataSet> CurrencyConversion(CurrencyConversionRequest request)
        {

            var parentdata = GenericSerializer<CurrencyConversion>.Serialize(request.currency);

            var parameters = new Dictionary<string, object>
            {
                ["@XmlData"] = parentdata,
                ["@Action"] = request.mode,
                ["@UserId"] = request.UserId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_Currency_Conversion", parameters);
        }

    }
}
