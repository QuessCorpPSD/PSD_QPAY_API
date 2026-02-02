using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;

namespace QPay.BAL.Repository.Reports
{
    public class PayregisterUnprocessedRepository : IPayregisterUnprocessedRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public PayregisterUnprocessedRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Exporttoexcel(int CompanyId, int PayperiodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,
                ["@Pay_Period_Id"] = PayperiodId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_PayRegister_withUnprocessed", parameters, 1500);
        }

    }
}
