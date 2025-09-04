using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.UI.Admin;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;

        public AdminController(IAdminDashboardRepository adminDashboardRepository)
        {
            this._adminDashboardRepository= adminDashboardRepository;
        }

        [HttpPost,Route("AddBreakDetail")]
        public async Task<IActionResult> AddBreakDetail(BreakTimeDetailRequest breakTimeDetailsUI)
        {
            BreakTimeDetailsUI breakTimeDetails = new BreakTimeDetailsUI()
            {
                BreakId = breakTimeDetailsUI.BreakId,
                Description = breakTimeDetailsUI.Description,
                starttime = TimeSpan.Parse(breakTimeDetailsUI.startTime),
                endtime = TimeSpan.Parse(breakTimeDetailsUI.EndTime),
                CreatedBy = breakTimeDetailsUI.CreatedBy,
                IsActive = breakTimeDetailsUI.IsActive,
                ProcessCategory = breakTimeDetailsUI.ProcessCategory
            };
            var res =await this._adminDashboardRepository.AddUpdateBreakDetail(breakTimeDetails);
            return Ok(res);
        }

        [HttpGet,Route("GetAllBreakDetail")]
        public async Task<IActionResult> GetAllBreakDetail()
        {            
            var res =await this._adminDashboardRepository.GetBreakDetail();
            return Ok(res);
        }
        [HttpPost,Route("GetEmployeeBreak")]
        public async Task<IActionResult> GetEmployeeBreak(EmployeeBreakRequest employeeBreakRequest)
        {
            var res = await this._adminDashboardRepository.GetEmployeeBreakByUserIdAndDate(employeeBreakRequest.userId, employeeBreakRequest.date);
            return Ok(res);
        }
        [HttpPost,Route("EmployeeBreakAdd")]
        public async Task<IActionResult> EmployeeBreakAdd(EmployeeBreakModelRequest employeeBreakRequest)
        {
            EmployeeBreakUI employeeBreakUI = new EmployeeBreakUI()
            {
                UserId = employeeBreakRequest.userId,
                Remarks = employeeBreakRequest.Remarks,
                StartTime = TimeOnly.Parse(employeeBreakRequest.StartTime),
                EndTime = TimeOnly.Parse(employeeBreakRequest.EndTime),
                Description = employeeBreakRequest.description,
                BreakId = employeeBreakRequest.breakId,
                UserBreakId = employeeBreakRequest?.breakTypeId ?? 0
            };
            var res = await this._adminDashboardRepository.EmployeeBreakAddUpdate(employeeBreakUI);
            return Ok(res);
        }
        [HttpPost, Route("BulkEmployeeBreakAdd")]
        public async Task<IActionResult> BulkEmployeeBreakAdd(EmployeeBreakBulkModelRequest employeeBulkBreakRequest)
        {

                     var employees = employeeBulkBreakRequest?.employeeBreakRequest?
                    .Select(x => new EmployeeBreakUI
                    {
                        UserId = employeeBulkBreakRequest.userId,
                        Remarks = x.Remarks,
                        StartTime = TimeOnly.Parse(x.StartTime),
                        EndTime = TimeOnly.Parse(x.EndTime),    
                        Description = x.description,
                        BreakId = x.breakId,
                        UserBreakId = x?.userBreakId ?? 0
                    })
                    .ToList();
            var res = await this._adminDashboardRepository.EmployeeBulkBreakAddUpdate(employees, employeeBulkBreakRequest.userId);
            EmployeeBreakUI employeeBreakUI = new EmployeeBreakUI()
            {
               
            };
            //return Ok(res);
            return Ok("");
        }
    }
}
