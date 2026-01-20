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
    public class CPFRepository : ICPFRepository
    {

        private readonly DbRepository _dbRepository;

        public CPFRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search(int? PayCode, int? Category)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@CapType"] = Category,
                ["@Paycode"] = PayCode
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllProvidentFund", parameters); ;

        }

        public async Task<DataSet> GetPaycode()
        {
            var parameters = new Dictionary<string, object>
            {
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPaycode_CPF", parameters);
        }

        public async Task<DataSet> GetCriteria(int? CriteriaTypeId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@CriteriaTypeId"] = CriteriaTypeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetCriteriaType", parameters);
        }
        public async Task<List<CategoryUI>> GetCategory()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("Proc_GetCategory_CPF", parameters);

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
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateCPF", parameters);
        }


    }
}
