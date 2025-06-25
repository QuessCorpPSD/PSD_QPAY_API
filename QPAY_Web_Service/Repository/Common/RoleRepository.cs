using DocumentFormat.OpenXml.Drawing.Diagrams;
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
    public class RoleRepository : IRoleRepository
    {
        private readonly DbRepository _dbConnection;

        public RoleRepository(DbRepository dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<List<RoleUI>> GetAllActiveRole()
        {
            List<RoleUI> roleUIList = new List<RoleUI>(); 


            try
            {
                string sql = string.Format("SELECT RoleId, RoleName FROM [Role] WHERE IsActive = {0} ORDER BY RoleName",1);
                var status = await this._dbConnection.QueryMultiAsync(sql);
              return  roleUIList = JsonConvert.DeserializeObject<List<RoleUI>>(status)
                                                 ?? new List<RoleUI>();
            }
            catch (JsonException ex)
            {
                // Log the error if needed
                return roleUIList = new List<RoleUI>();
            }
           
        }
    }
}
