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

    public class MaterialCodeRepository : IMaterialCodeRepository
    {
        private readonly DbRepository _dbRepository;

        public MaterialCodeRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = "Search",
                ["@Code"] = "",
            };
            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageMaterialCode", parameters); ;

        }

        public async Task<DataSet> Create(MaterialCodeMasterRequest items)
        {
            var InsResponse = new MaterialCodeMasterResponse();
            InsResponse.LstMaterialCodeMaster = new MaterialCodeMaster[1];
            InsResponse.LstMaterialCodeMaster[0] = items.detail;
            string responseSerialize = GenericSerializer<MaterialCodeMasterResponse>.Serialize(InsResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = responseSerialize,
                ["@Action"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageMaterialCode", parameters);
        }


    }
}
