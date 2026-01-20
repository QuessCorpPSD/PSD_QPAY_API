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

    public class VendorMasterRepository : IVendorMasterRepository
    {
        private readonly DbRepository _dbRepository;

        public VendorMasterRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@ClientName"] = "",
                ["@Client_Id"] = 0,
            };
            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetClientDetails", parameters); ;
        }

      
        public async Task<DataSet> Create(ClientRequest items)
        {
            var clntResponse = new ClientResponse();
            clntResponse.ClientDetails = new UI.GlobalMaster.Client[1];
            clntResponse.ClientDetails[0] = items.detail;
            string prkResponseSerialize = GenericSerializer<ClientResponse>.Serialize(clntResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = prkResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateClient", parameters);
        }


    }
}
