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

    public class InvoiceLegalEntityRepository : IInvoiceLegalEntityRepository
    {
        private readonly DbRepository _dbRepository;

        public InvoiceLegalEntityRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@EntityName"] = "",
            };
            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetQuessLegalEntityDetails", parameters); ;

        }

        public async Task<DataSet> Create(QuessLegalEntityRequest items)
        {
            var bnkResponse = new QuessLegalEntityResponse();
            bnkResponse.QuessLegalEntityDetails = new QuessLegalEntity[1];
            bnkResponse.QuessLegalEntityDetails[0] = items.parentDetail;
            string bnkResponseSerialize = GenericSerializer<QuessLegalEntityResponse>.Serialize(bnkResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = bnkResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_QuessLegalEntityAddAndDelete", parameters);
        }


    }
}
