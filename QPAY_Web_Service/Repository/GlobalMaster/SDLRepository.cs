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
    public class SDLRepository : ISDLRepository
    {

        private readonly DbRepository _dbRepository;

        public SDLRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object>
            {
             
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllSDLSlab", parameters); ;

        }

        public async Task<DataSet> GetPaycode()
        {
            var parameters = new Dictionary<string, object>
            {
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPaycode_SDL", parameters);


        }

        public async Task<DataSet> GetCriteria(int? CriteriaTypeId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@CriteriaTypeId"] = CriteriaTypeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetCriteriaType", parameters);
        }

        public async Task<DataSet> Create(string strXmlDetails, string mode, int userId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = strXmlDetails,
                ["@mode"] = mode,
                ["@CreatedBy"] = userId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateSDL", parameters);
        }


    }
}
