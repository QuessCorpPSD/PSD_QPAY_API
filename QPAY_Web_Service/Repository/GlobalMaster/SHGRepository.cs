using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.GlobalMaster
{
    public class SHGRepository : ISHGRepository
    {

        private readonly DbRepository _dbRepository;

        public SHGRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search(string? effectiveDate)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@EffectiveDate"] = effectiveDate
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetSHGDetails", parameters); ;

        }

        public async Task<List<CategoryUI>> GetCategory()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("sp_Get_shg_category", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CategoryUI>>(res) ?? new List<CategoryUI>();
            }

            return new List<CategoryUI>();
        }


        public async Task<DataSet> Create(string strXmlDetails, string mode, int userId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = strXmlDetails,
                ["@mode"] = mode,
                ["@CreatedBy"] = userId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateSHG", parameters);
        }


    }
}
