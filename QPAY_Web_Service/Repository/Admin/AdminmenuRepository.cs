using QPay.BAL.IRepository.Admin;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Admin
{
    public class AdminmenuRepository: IAdminmenuRepository
    {

        private readonly DbRepository _dbRepository;

        public AdminmenuRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> GetRoleTypes()
        {
            var parameters = new Dictionary<string, object>
            {
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_GET_ROLES", parameters);
        }

        public async Task<DataSet> GetReportingTo()
        {
            var parameters = new Dictionary<string, object>
            {
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_GET_ReportingToNames", parameters);
        }

        public async Task<DataSet> GetAccessType()
        {
            var parameters = new Dictionary<string, object>
            {
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_GET_Access_Type", parameters);
        }

        public async Task<DataSet> Search(int? UserId, String? UserName,int? RoleId,int? IsCheck)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@UserId"] = UserId,
                ["@UserName"] = UserName,
                ["@RoleId"] = RoleId,
                ["@IsCheck"] = IsCheck


            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllUserDetails", parameters); ;

        }

        public async Task<DataSet> Create(string xml, int createdBy, string mode, string UserDetails)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = xml,
                ["@mode"] = mode,
                ["@CreatedBy"] = createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateUserDetails", parameters);
        }


    }
}
