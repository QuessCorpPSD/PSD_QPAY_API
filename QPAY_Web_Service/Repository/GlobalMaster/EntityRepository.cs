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

    public class EntityRepository : IEntityRepository
    {
        private readonly DbRepository _dbRepository;

        public EntityRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@EntityName"] = "",
            };
            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllEntityProfitDetail", parameters); ;

        }

        public async Task<DataSet> GetQuessLegalEntity()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = "GetQuessLegalEntity",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_CommonDropDowns", parameters);

        }
        public async Task<DataSet> Create(EntityRequest items)
        {

            var parentdata = GenericSerializer<Entity>.Serialize(items.parentDetail);
            var childdata = GenericSerializer<EntityProfitCenter>.Serialize(items.ChildDetail);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@xmlInputDetail"] = childdata,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateEntity", parameters);
        }


    }
}
