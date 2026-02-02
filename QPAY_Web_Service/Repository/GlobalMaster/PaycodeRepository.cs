using Dapper;
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

    public class PaycodeRepository : IPaycodeRepository
    {
        private readonly DbRepository _dbRepository;

        public PaycodeRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
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
