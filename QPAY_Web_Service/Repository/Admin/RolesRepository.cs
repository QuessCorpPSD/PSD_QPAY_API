using QPay.BAL.IRepository.Admin;
using QPay.DAL.Repository;
using QPay.UI.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Admin
{
    public class RolesRepository:IRolesRepository
    {

        private readonly DbRepository _dbRepository;

        public RolesRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<DataSet> RolesCRUD(Roles _params)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = _params.Action,
                ["@UserId"] = _params.UserId,
                ["@RoleId"] = _params.RoleId,
                ["@RoleName"] = _params.RoleName,
                ["@Description"] = _params.Description,
                ["@IsSysAdmin"] = _params.IsSysAdmin,
                ["@IsActive"] = _params.IsActive,
                ["@PageNo"] = 1,
                ["@PageSize"] = 10000

            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Admin_ManageRole_NewUI", parameters); ;

        }
    }
}
