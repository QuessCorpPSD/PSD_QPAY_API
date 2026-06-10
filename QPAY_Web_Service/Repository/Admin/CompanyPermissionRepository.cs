using QPay.BAL.IRepository.Admin;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Admin
{
    public class CompanyPermissionRepository: ICompanyPermissionRepository
    {
        private readonly DbRepository _dbRepository;

        public CompanyPermissionRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> GetEntityZoneEmployeeId()
        {
            var parameters = new Dictionary<string, object>
            {
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_GetAllUserName", parameters);
        }

        public async Task<DataSet> LoadCompany(int? BusinessUnitNameId, int? BusinessZonenName)
        {
            var parameters = new Dictionary<string, object>
            {

                ["@BusinessUnitNameId"] = BusinessUnitNameId,
                ["@BusinessZonenName"] = BusinessZonenName



            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_GetCompanyDetailsForPermission", parameters);
        }


        public async Task<DataSet> Search(int? Userid,  int? Businessunitnameid, int? CompanyPermissionId)
        {
            var parameters = new Dictionary<string, object>
            {
  
                ["@Userid"] = Userid,
                ["@Businessunitnameid"] = Businessunitnameid,
                ["@CompanyPermissionId"] = CompanyPermissionId


            
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_GetAllCompanyPermission", parameters);
        }

        public async Task<DataSet> Editdetails(int? Userid, int? Businessunitnameid, int? CompanyPermissionId)
        {
            var parameters = new Dictionary<string, object>
            {

                ["@Userid"] = Userid,
                ["@Businessunitnameid"] = Businessunitnameid,
                ["@CompanyPermissionId"] = CompanyPermissionId



            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_GetAllCompanyPermissionDetails", parameters);
        }

        public async Task<DataSet> CreateUpdateDelete(string xml, int createdBy, string mode)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = xml,
                ["@mode"] = mode,
                ["@CreatedBy"] = createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_CreateUpdateCompanyPermission", parameters);
        }
    }
}
