using Microsoft.AspNetCore.Mvc;
using QPay.UI.Dashboard;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
   public interface IDashboardRepository
    {
        DashboardUI GetAllottedLotsByUserId(int userId);
        Task<List<LotAllottmentPendingUI>> GetLotAllottmentPendings();
        Task<List<DashBoardCompledtedUI>> GetInputLotDetail(DashboardRequestModel dashboardRequestModel);
    }
}
