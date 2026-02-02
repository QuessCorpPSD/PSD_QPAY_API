using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Customer;
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

    public class BankRepository : IBankRepository
    {
        private readonly DbRepository _dbRepository;

        public BankRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@BankName"] = "",
                ["@Bank_Id"] = 0,
            };
            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetBankDetails", parameters); ;

        }

        public async Task<DataSet> Create(BankRequest items)
        {

            var bnkResponse = new BankResponse();
            bnkResponse.BankDetails = new Bank[1];
            bnkResponse.BankDetails[0] = items.detail;
            string bnkResponseSerialize = GenericSerializer<BankResponse>.Serialize(bnkResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = bnkResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateBank_New", parameters);
        }


    }
}
