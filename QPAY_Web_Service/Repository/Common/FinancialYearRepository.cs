using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Common
{
    public class FinancialYearRepository : IFinancialYearRepository
    {
        private readonly DbRepository _dbRepository;

        public FinancialYearRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<List<FinancialYearUI>> GetFinancialYears()
        {

            var res =await this._dbRepository.QueryMultiAsync("select * from tbl_Financial_Year");
            if(res!="")
            {
                return JsonConvert.DeserializeObject<List<FinancialYearUI>>(res) ?? new List<FinancialYearUI>();
            }

            return new List<FinancialYearUI>();
        }
    }
}
