using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.Models.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.GlobalMaster
{

    public class PaycodeRepository : IPaycodeRepository
    {
        private readonly DbRepository _dbRepository;

        public PaycodeRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<DataSet> GetPayCodeByCompanyId(int companyId,int invoiceCultureId,string type)
        {

            //var paramerter=new DynamicParameters();
            //paramerter.Add("@CompanyId", companyId);
            //paramerter.Add("@InvoiceCulture_Id", invoiceCultureId);
            //paramerter.Add("@Type", type);
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@InvoiceCulture_Id"] = invoiceCultureId,
                ["@Type"] = type,
            };

         return   _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_CompanyPayCodeByCompanyId", parameters, 1500);

            //var res = await this._dbRepository.GetItemsAsync("SP_CompanyPayCodeByCompanyId", paramerter);
            //if(res.Any())
            //{
            //    return JsonConvert.DeserializeObject<List<PayCodeUI>>(res).ToList()?? new List<PayCodeUI>();
            //}
            //else
            //{
               //return new List<PayCodeUI>();
            //}
        }

        public async Task<DataSet> Search(string strPayCode, int? intPayTypeId, int? IsTaxable, int? PayId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@PayCode_Code"] = strPayCode,
                ["@PayTypeId"] = intPayTypeId,
                ["@IsTaxable"] = IsTaxable,
                ["@PayCode_Id"] = PayId
            };
            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPaycode", parameters); ;

        }

        public async Task<DataSet> GetPageType()
        {
            var parameters = new Dictionary<string, object>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPageType", parameters);

        }
        public async Task<string> GetPayType()
        {
            List<PayTypes> response = new List<PayTypes>();
            var res = _dbRepository.QueryMultiAsync("select * from tbl_PayType").Result;
            return res;
        }


        public async Task<DataSet> Create(string strXmlDetails, string mode, int userId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = strXmlDetails,
                ["@mode"] = mode,
                ["@CreatedBy"] = userId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdatePayCode", parameters); ;

        }


    }
}
