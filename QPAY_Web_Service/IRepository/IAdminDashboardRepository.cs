using QPay.UI.Admin;
using QPay.UI.Dashboard;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IAdminDashboardRepository
    {
        Task<AdminDashboardUI> GetAdminDashboard();
        Task<List<DashboardDetailUI>> GetPendingLotAsync();
        Task<List<DashboardDetailUI>> GetAdminDashboardDetail(AdminDashboardParameterlUI adminDashboardParameterlUI);

        Task<BreakTimeResponse> AddUpdateBreakDetail(BreakTimeDetailsUI breakTimeDetailsUI);
        Task<List<BreakTimeDetailsUI>> GetBreakDetail();
        Task<List<EmployeeBreakUI>> GetEmployeeBreakByUserIdAndDate(int userId, DateTime currentDate);


    }
}
