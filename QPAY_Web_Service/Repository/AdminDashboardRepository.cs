using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Admin;
using QPay.UI.Dashboard;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly DbRepository _dbRepository;
        public AdminDashboardRepository(DbRepository dbRepository) 
        {
        this._dbRepository = dbRepository;
        }

        public async Task<AdminDashboardUI> GetAdminDashboard()
        {

            AdminDashboardUI dashboardUI = new AdminDashboardUI();

            string storeProcedure = string.Format("SP_PSD_Admin_DashBoard");            
          
            var res =await this._dbRepository.GetItemsAsync(storeProcedure, null);
            if (res != "")
            {
                dashboardUI = JsonConvert.DeserializeObject<List<AdminDashboardUI>>(res).FirstOrDefault() ?? new AdminDashboardUI();
                return dashboardUI;
            }
            return dashboardUI;
        }

        public async Task<BreakTimeResponse> AddUpdateBreakDetail(BreakTimeDetailsUI breakTimeDetailsUI)
        {
            BreakTimeResponse breakTimeResponse=new BreakTimeResponse();
            var parameter = new DynamicParameters();
            string storeProcedure = "SP_tbl_BreakTimeDetails_AddAndUpdate" ?? "";

            parameter.Add("@BreakId", breakTimeDetailsUI.BreakId);
            parameter.Add("@Description", breakTimeDetailsUI.Description);
            parameter.Add("@TotalMinutes", breakTimeDetailsUI.TotalMinutes);            
            parameter.Add("@IsActive", breakTimeDetailsUI.IsActive);
            parameter.Add("@UserId", breakTimeDetailsUI.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<BreakTimeResponse>>(res).FirstOrDefault() ?? new BreakTimeResponse();
                }
                catch (JsonException ex)
                {
                    // Log exception or handle as needed
                    Console.WriteLine($"JSON Deserialization error: {ex.Message}");
                    return new BreakTimeResponse();
                }
            }

            return new BreakTimeResponse();

          
        }
        public async Task<List<EmployeeBreakUI>> GetEmployeeBreakByUserIdAndDate(int userId,DateTime currentDate)
        {
            List<EmployeeBreakUI> employeeBreaks = new List<EmployeeBreakUI>();
            var parameter = new DynamicParameters();
            string storeProcedure = "SP_GET_tbl_employee_BreakTimeDetails_AddAndUpdate" ?? "";

            parameter.Add("@UserId", userId);
            parameter.Add("@date", currentDate);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    employeeBreaks= JsonConvert.DeserializeObject<List<EmployeeBreakUI>>(res).ToList() ?? new List<EmployeeBreakUI>();
                }
                catch (JsonException ex)
                {
                    // Log exception or handle as needed
                    Console.WriteLine($"JSON Deserialization error: {ex.Message}");
                    return new List<EmployeeBreakUI>();
                }
            }
            return employeeBreaks;
        }
       public async Task<List<BreakTimeDetailsUI>> GetBreakDetail()
        {
            const string query = "select * from tbl_BreakTimeDetails";
            var res = await _dbRepository.QueryMultiAsync(query);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<BreakTimeDetailsUI>>(res) ?? new List<BreakTimeDetailsUI>();
                }
                catch (JsonException ex)
                {
                    // Log exception or handle as needed
                    Console.WriteLine($"JSON Deserialization error: {ex.Message}");
                    return new List<BreakTimeDetailsUI>();
                }
            }
            return new List<BreakTimeDetailsUI>();
        }
        public async Task<List<DashboardDetailUI>> GetPendingLotAsync()
        {
            const string storeProcedure = "SP_InputLot_Pending_Status";
            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<DashboardDetailUI>>(res) ?? new List<DashboardDetailUI>();
                }
                catch (JsonException ex)
                {
                    // Log exception or handle as needed
                    Console.WriteLine($"JSON Deserialization error: {ex.Message}");
                    return new List<DashboardDetailUI>();
                }
            }

            return new List<DashboardDetailUI>();
        }

        public async Task<List<DashboardDetailUI>> GetAdminDashboardDetail(AdminDashboardParameterlUI adminDashboardParameterlUI)
        {
            List<DashboardDetailUI> adminDashboardDetails = new List<DashboardDetailUI>();
            string storeProcedure = string.Format("SP_PSD_Admin_DashBoard_Detail");
            var parameter = new DynamicParameters();
            parameter.Add("@FilterType", adminDashboardParameterlUI.FilterType??"");
            parameter.Add("@FinancialYear",  adminDashboardParameterlUI.FinancialYear??"");
            parameter.Add("@UserId",  adminDashboardParameterlUI.UserId??0);
            if(adminDashboardParameterlUI.FromDate.HasValue)
            {
                parameter.Add("@FromDate", adminDashboardParameterlUI.FromDate);
                parameter.Add("@ToDate", adminDashboardParameterlUI.ToDate == null ? DBNull.Value : adminDashboardParameterlUI.ToDate);
            }
            
          

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (res != "")
            {
                adminDashboardDetails = JsonConvert.DeserializeObject<List<DashboardDetailUI>>(res).ToList() ?? new List<DashboardDetailUI>();
                return adminDashboardDetails;
            }

            return adminDashboardDetails;
        }
    }
}
