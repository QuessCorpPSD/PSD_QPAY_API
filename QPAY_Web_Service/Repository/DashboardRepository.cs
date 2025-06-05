using Dapper;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DbRepository _dbRepository;
        public DashboardRepository(DbRepository dbRepository)
        {
            this._dbRepository=dbRepository;
        }

        public DashboardUI GetAllottedLotsByUserId(int userId)
        {
            DashboardUI dashboardUI = new DashboardUI();

            string storeProcedure = string.Format("SP_Profile_Dashboard_process");
            PasswordSalt salt = new PasswordSalt();
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                dashboardUI = JsonConvert.DeserializeObject<DashboardUI[]>(res).FirstOrDefault();
                return dashboardUI;
            }
            return dashboardUI;
        }
    }
}
