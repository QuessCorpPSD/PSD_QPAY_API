using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Admin;
using QPay.UI.Common;
using QPay.UI.Dashboard;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users = QPay.UI.Models.Users;

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

        public async Task<AutoAllottmentUI> SwapCategory(SwapCategoryUI swapCategoryUI)
        {
            string StoreProcedure = "SP_User_Swap_Category";
            var parameter = new DynamicParameters();
            AutoAllottmentUI autoAllottmentUI = new AutoAllottmentUI();
            parameter.Add("@UserId", swapCategoryUI.userId);
            parameter.Add("@Category", swapCategoryUI.Category);
            parameter.Add("@CreatedOn", swapCategoryUI.CreatedOn);
            var res = await this._dbRepository.GetItemsAsync(StoreProcedure, parameter);
            if (res != "")
            {
                autoAllottmentUI = JsonConvert.DeserializeObject<List<AutoAllottmentUI>>(res).FirstOrDefault() ?? new AutoAllottmentUI();
                return autoAllottmentUI;
            }
            return autoAllottmentUI;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            List<Users> users = new List<Users>();
            var parameter = new DynamicParameters();
            string storeProcedure = "SP_Get_All_UserList" ?? "";
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Users>>(res).ToList() ?? new List<Users>();
                }
                catch (Exception ex)
                {
                    return users;
                }
            }
            else
            {
                return users;
            }
               
        }
        public async Task<BreakTimeResponse> AddUpdateBreakDetail(BreakTimeDetailsUI breakTimeDetailsUI)
        {
            BreakTimeResponse breakTimeResponse=new BreakTimeResponse();
            var parameter = new DynamicParameters();
            string storeProcedure = "SP_tbl_BreakTimeDetails_AddAndUpdate" ?? "";
            parameter.Add("@BreakId", breakTimeDetailsUI.BreakId);
            parameter.Add("@ProcessCategory", breakTimeDetailsUI.ProcessCategory);
            parameter.Add("@Description", breakTimeDetailsUI.Description);            
            parameter.Add("@StartTime", breakTimeDetailsUI.starttime);
            parameter.Add("@EndTime", breakTimeDetailsUI.endtime);
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

        public async Task<List<EmployeeBreakUI>> EmployeeBulkBreakAddUpdate(List<EmployeeBreakUI> employeeBreakUI, int userId)
        {
            List<EmployeeBreakUI> employeeBreaks = new List<EmployeeBreakUI>();
            foreach (var item in employeeBreakUI)
            {
                var parameter = new DynamicParameters();
                string storeProcedure = "SP_tbl_employee_BreakTimeDetails_AddAndUpdate" ?? "";
                parameter.Add("@UserId", item.UserId);
                parameter.Add("@BreakTypeId", item.BreakId);
                parameter.Add("@StartTime", item.StartTime?.ToTimeSpan());
                parameter.Add("@EndTime", item.EndTime?.ToTimeSpan());
                parameter.Add("@remarks", item.Remarks);
                parameter.Add("@UserBreakId", item.UserBreakId);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
                if (!string.IsNullOrWhiteSpace(res))
                {

                    try
                    {
                        employeeBreaks = JsonConvert.DeserializeObject<List<EmployeeBreakUI>>(res).ToList() ?? new List<EmployeeBreakUI>();
                    }
                    catch (JsonException ex)
                    {
                        // Log exception or handle as needed
                        Console.WriteLine($"JSON Deserialization error: {ex.Message}");
                        return new List<EmployeeBreakUI>();
                    }
                }

            }




            return employeeBreaks;
        }
        public async Task<List<EmployeeBreakUI>> EmployeeBreakAddUpdate(EmployeeBreakUI item)
        {
            List<EmployeeBreakUI> employeeBreaks = new List<EmployeeBreakUI>();
            var parameter = new DynamicParameters();
            string storeProcedure = "SP_tbl_employee_BreakTimeDetails_AddAndUpdate" ?? "";
            parameter.Add("@UserId", item.UserId);
            parameter.Add("@BreakTypeId", item.BreakId);
            parameter.Add("@StartTime", item.StartTime?.ToTimeSpan());
            parameter.Add("@EndTime", item.EndTime?.ToTimeSpan());
            parameter.Add("@remarks", item.Remarks);
            parameter.Add("@UserBreakId", item.UserBreakId);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (!string.IsNullOrWhiteSpace(res))
            {

                try
                {
                    employeeBreaks = JsonConvert.DeserializeObject<List<EmployeeBreakUI>>(res).ToList() ?? new List<EmployeeBreakUI>();
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
        public async Task<List<EmployeeBreakUI>> GetEmployeeBreakByUserIdAndDate(int userId,DateTime currentDate)
        {
            List<EmployeeBreakUI> employeeBreaks = new List<EmployeeBreakUI>();
            var parameter = new DynamicParameters();
            string storeProcedure = "SP_GET_tbl_employee_BreakTimeDetails" ?? "";

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
        public async Task<List<UserUI>> GetAllManager(int RoleId,int UserId)
        {
            List<UserUI> managers = new List<UserUI>();            
            string storeProcedure = string.Format("SP_Get_AllManager");
            var parameter = new DynamicParameters();
            parameter.Add("@Role", RoleId);
            parameter.Add("@UserID", UserId);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (res != "")
            {
                managers = JsonConvert.DeserializeObject<List<UserUI>>(res).ToList() ?? new List<UserUI>();
                return managers;
            }

            return managers;
        }
        public List<PayperiodDD> GetCurrentPayperiod(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = this._dbRepository.GetItemsAsync("Proc_GetCurrentPayperiod", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }
        public async Task<List<CompanyPicker>> GetallCompanyCodes(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@USER_ID", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Get_AllPSLCompany_Code_New", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CompanyPicker>>(res) ?? new List<CompanyPicker>();
            }

            return new List<CompanyPicker>();
        }
        public async Task<List<PayperiodDD>> GetAllPayperiod(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllPayperiodByCompanyId", parameters);
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }

        public async Task<List<UserUI>> GetTeamLeaderByMangerId( int UserId)
        {
            List<UserUI> managers = new List<UserUI>();
            string storeProcedure = string.Format("SP_Get_AllTeamLeader");
            var parameter = new DynamicParameters();
            
            parameter.Add("@ManagerId", UserId);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (res != "")
            {
                managers = JsonConvert.DeserializeObject<List<UserUI>>(res).ToList() ?? new List<UserUI>();
                return managers;
            }

            return managers;
        }

        public async Task<List<UserUI>> GetEmployeeByTeamLeaderId(int UserId)
        {
            List<UserUI> managers = new List<UserUI>();
            string storeProcedure = string.Format("SP_Get_AllUserByTeamLeader");
            var parameter = new DynamicParameters();

            parameter.Add("@TeamLeader", UserId);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (res != "")
            {
                managers = JsonConvert.DeserializeObject<List<UserUI>>(res).ToList() ?? new List<UserUI>();
                return managers;
            }

            return managers;
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
