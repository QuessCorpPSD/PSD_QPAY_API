using Dapper;
using QPay.BAL.IRepository.ARKnockOff;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.ARKnockOff
{
    public class ARKnockOffRepository:IARKnockOffRepository
    {
        private readonly DbRepository _dbRepository;

        public ARKnockOffRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<string> SaveARDetails(string xml)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", xml);
            //parameters.Add("@mode", mode);
            //parameters.Add("@CreatedBy", createdBy);

            var res = await this._dbRepository.GetItemsAsync("Proc_SaveARDetails", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
    }
}
