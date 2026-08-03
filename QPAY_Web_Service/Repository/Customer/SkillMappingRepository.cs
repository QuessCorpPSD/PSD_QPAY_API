using Azure.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Models.Customer;
using QPay.UI.Models.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Customer
{
    public class SkillMappingRepository : ISkillMappingRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public SkillMappingRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? companyId, int? siteId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_Id"] = companyId,
                ["@Site_Id"] = siteId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetAllConstruction_SkillMapping", parameters);
        }

        public async Task<string> CreateUpdateSkillMapping(SkillMappingRequest request)
        {
            string storeProcedure = "Proc_CreateUpdate_SkillMapping";
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", request.Company_Id);
            parameters.Add("@Company_code", request.Company_code);
            parameters.Add("@SiteId", request.SiteId);
            parameters.Add("@SiteName", request.SiteName);
            parameters.Add("@SkillType", request.SkillType);
            parameters.Add("@Amount", request.Amount);
            parameters.Add("@EffectiveDate", request.EffectiveDate);
            parameters.Add("@PO_Number", request.PO_Number);
            parameters.Add("@mode", request.Action);
            parameters.Add("@CreatedBy", request.UserId);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            { 
                return res;
            }
            else
            {
                return "No data returned from Database";
            }
        }

        public async Task<string> DeleteSkillMapping(int companyId, int siteId, string skillCategory, int userId)
        {
            string storeProcedure = "Proc_Delete_SkillMapping";
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@SiteId", siteId);
            parameters.Add("@SkillCategory", skillCategory);
            parameters.Add("@CreatedBy", userId);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                return res;
            }
            else
            {
                return "No data returned from Database";
            }
        }
    }
}
