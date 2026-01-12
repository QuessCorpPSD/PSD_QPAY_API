using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Dashboard;
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
        
        public async Task<List<DashBoardCompledtedUI>> GetInputLotDetail(DashboardRequestModel dashboardRequestModel)
        {
            List<DashBoardCompledtedUI> completed = new List<DashBoardCompledtedUI>();
            string ProcedureName = "SP_PSD_Completed_InputLot";
            var parameter = new DynamicParameters();
            parameter.Add("@FilterType", dashboardRequestModel.FilterType);
            parameter.Add("@UserId", dashboardRequestModel.UserId);
            parameter.Add("@financialyear", dashboardRequestModel.financialyear);

            if (dashboardRequestModel.FromDate !=null && dashboardRequestModel.ToDate!=null)
            {
                parameter.Add("@FromDate", dashboardRequestModel.FromDate);
                parameter.Add("@ToDate", dashboardRequestModel.ToDate);
            }

            var res = await this._dbRepository.GetItemsAsync(ProcedureName, parameter);
            if (res != "")
            {
                completed = JsonConvert.DeserializeObject<List<DashBoardCompledtedUI>>(res).ToList();
                return completed;
            }
            return completed;

        }

        public async Task<List<LotAllottmentPendingUI>> GetLotAllottmentPendings()
        {
            List<LotAllottmentPendingUI> lotAllottmentPendings = new List<LotAllottmentPendingUI>();

            string storeProcedure = string.Format("SP_Process_Dashboard");
            PasswordSalt salt = new PasswordSalt();
            var parameters = new DynamicParameters();
            var res =await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (res != "")
            {
                lotAllottmentPendings = JsonConvert.DeserializeObject<List<LotAllottmentPendingUI>>(res).ToList();
                return lotAllottmentPendings;
            }
            return lotAllottmentPendings;
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
