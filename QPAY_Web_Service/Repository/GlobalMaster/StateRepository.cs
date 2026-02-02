using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Models.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.GlobalMaster
{
    public class StateRepository: IStateRepository
    {
        private readonly DbRepository _dbRepository;

        public StateRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<List<State>> GetAllState(string? stateName, int? regionId, int? stateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("StateName", stateName);
            parameters.Add("RegionId", regionId);
            parameters.Add("StateId", stateId);


            var res = await _dbRepository.GetItemsAsync("sp_GetAllStateByParam", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<State>>(res) ?? new List<State>();
            }

            return new List<State>();
        }

        public async Task<string> Create(string xml, string mode, int createdBy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", xml);
            parameters.Add("@mode", mode);
            parameters.Add("@CreatedBy", createdBy);

            var res = await this._dbRepository.GetItemsAsync("sp_CreateUpdateState", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public async Task<List<Region>> GetAllRegion()
        {
            var parameters = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync("sp_GetAllRegions", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Region>>(res) ?? new List<Region>();
            }

            return new List<Region>();
        }

    }
}
