using QPay.UI.Admin;
using QPay.UI.Common;
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
        Task<AutoAllottmentUI> SwapCategory(SwapCategoryUI swapCategoryUI);
        Task<List<Users>> GetAllUsers();
        Task<List<CompanyPicker>> GetallCompanyCodes(string userId);
        Task<List<CategoryWiseAssignmentUI>> GetCategoryLotDetails(string AssigmentType);
        List<PayperiodDD> GetCurrentPayperiod(int companyId);
        Task<List<PayperiodDD>> GetAllPayperiod(int companyId);
        Task<List<DashboardDetailUI>> GetPendingLotAsync();
        Task<List<DashboardDetailUI>> GetAdminDashboardDetail(AdminDashboardParameterlUI adminDashboardParameterlUI);

        Task<BreakTimeResponse> AddUpdateBreakDetail(BreakTimeDetailsUI breakTimeDetailsUI);
        Task<List<BreakTimeDetailsUI>> GetBreakDetail();
        Task<List<EmployeeBreakUI>> GetEmployeeBreakByUserIdAndDate(int userId, DateTime currentDate);
        Task<List<EmployeeBreakUI>> EmployeeBreakAddUpdate(EmployeeBreakUI employeeBreakUI);
        Task<List<EmployeeBreakUI>> EmployeeBulkBreakAddUpdate(List<EmployeeBreakUI> employeeBreakUI, int userId);
        Task<List<UserUI>> GetAllManager(int RoleId, int UserId);
        Task<List<UserUI>> GetTeamLeaderByMangerId(int UserId);
        Task<List<UserUI>> GetEmployeeByTeamLeaderId(int UserId);
        Task<FileResponse> InputReconAndYettoCome(string flag);
        Task<AdminDashboardUI> GetInvoiceDashboard(string InvoiceType);
    }
}
