using Newtonsoft.Json;
using QPay.BAL.IRepository.Common;
using QPay.DAL.Repository;
using QPay.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Common
{
    public class AccesstypeRepository : IAccesstypeRepository
    {
        private readonly DbRepository dbRepository;

        public AccesstypeRepository(DbRepository dbRepository)
        {
            this.dbRepository = dbRepository;
        }
        public async Task<List<AccessTypeUI>> GetAllAccessType()
        {
           
            
            try
            {
                string sql = "select * from tbl_Access_type";
                var status = await this.dbRepository.QueryMultiAsync(sql);
                return  JsonConvert.DeserializeObject<List<AccessTypeUI>>(status)
                                                   ?? new List<AccessTypeUI>();
            }
            catch (JsonException ex)
            {
                // Log the error if needed
                return  new List<AccessTypeUI>();
            }
        }
    }
}
