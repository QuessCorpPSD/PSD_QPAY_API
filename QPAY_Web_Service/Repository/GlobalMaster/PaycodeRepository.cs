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
        public async Task<DataSet> GetPayCodeByCompanyId(int companyId, int invoiceCultureId, string type)
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

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_CompanyPayCodeByCompanyId", parameters, 1500);

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
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPaycode", parameters); ;

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

        public async Task<PTStateExclude> CreateFlexidetails(string mode, string xml, Int32 userid)
        {
            PTStateExclude objFlexiRule = new PTStateExclude();
            var parameter = new DynamicParameters();
            parameter.Add("@xmlInput", xml);
            parameter.Add("@mode", mode);
            parameter.Add("@CreatedBy", userid);
            try
            {
                var res = await _dbRepository.GetItemsAsync<PTStateExclude>("Sp_Create_Update_PtStatexclude", parameter);
                if (res.Any())
                {
                    objFlexiRule = res.FirstOrDefault() ?? new PTStateExclude();
                }
            }
            catch (Exception ex)
            {

            }

            return objFlexiRule;
        }
        public async Task<List<PTStateExclude>> GetSearchdata(int? Company_Id, int? Band_Id,int? Flexi_Rule_Id,string? Mode)
        {
            List<PTStateExclude> listsearch = new List<PTStateExclude>();
            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", Company_Id);
            parameter.Add("@Band_Id", Band_Id);
            parameter.Add("@Flexi_Rule_Id", Flexi_Rule_Id);
            parameter.Add("@Mode", Mode);
            try
            {
                var res = await _dbRepository.GetItemsAsync<PTStateExclude>("Search_PtStatexclude", parameter);
                if (res.Any())
                {
                    listsearch = res.ToList() ?? new List<PTStateExclude>();
                }
            }
            catch (Exception ex)
            {

            }
            return listsearch;
        }

        //public async Task<List<PTStateExclude>> GetEditdata(int? Company_Id, int brand_Id, int? Flexi_Rule_Id, string mode)
        //{
        //    List<PTStateExclude> listsearch = new List<PTStateExclude>();
        //    var parameter = new DynamicParameters();
        //    parameter.Add("@Company_Id", Flexi_Rule_Id);
        //    parameter.Add("@Band_Id", brand_Id);
        //    parameter.Add("@Flexi_Rule_Id", Flexi_Rule_Id);
        //    parameter.Add("@Mode", mode);

        //    try
        //    {
        //        var res = await _dbRepository.GetItemsAsync<PTStateExclude>("Search_PtStatexclude", parameter);
        //        if (res.Any())
        //        {
        //            listsearch = res.ToList() ?? new List<PTStateExclude>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return listsearch;

        //}

        public async Task<List<CompanyPayCodeDetail>> Companypaycodes(int? company_Id)
        {
            List<CompanyPayCodeDetail> listsearch = new List<CompanyPayCodeDetail>();
            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", company_Id);
            
            try
            {
                var res = await _dbRepository.GetItemsAsync<CompanyPayCodeDetail>("sp_GetAllPtExcludePaycode", parameter);
                if (res.Any())
                {
                    listsearch = res.ToList() ?? new List<CompanyPayCodeDetail>();
                }
            }
            catch (Exception ex)
            {

            }
            return listsearch;

        }
    }
}
