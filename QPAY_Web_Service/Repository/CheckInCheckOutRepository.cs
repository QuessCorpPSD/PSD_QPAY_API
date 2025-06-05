using Dapper;
using Newtonsoft.Json;
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
   public class CheckInCheckOutRepository : ICheckInCheckOutRepository
    {
        private readonly DbRepository _dbRepository;
        public CheckInCheckOutRepository(DbRepository dbRepository) { 
            this._dbRepository = dbRepository;
        }
      public  CheckInCheckOutUI CheckIn(int userId, string Type)
        {
            CheckInCheckOutUI checkInCheckOutUI = new CheckInCheckOutUI();
            string storeProcedure = string.Format("SP_Timesheet_checkIn_checkout");
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);
            parameters.Add("@Type", Type);
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res != "")
            {
                checkInCheckOutUI = JsonConvert.DeserializeObject<List<CheckInCheckOutUI>>(res).FirstOrDefault();
            }
            return checkInCheckOutUI;

        }
    }
}
