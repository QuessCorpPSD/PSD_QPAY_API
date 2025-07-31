using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            BreakTimeDetailsUI breakTimeDetails = new BreakTimeDetailsUI() { 
            BreakId= breakTimeDetailsUI.BreakId,
            Description= breakTimeDetailsUI.Description,
            TotalMinutes= (breakTimeDetailsUI.TotalMinutes),            
            CreatedBy= breakTimeDetailsUI.CreatedBy
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
        [HttpGet,Route("GetEmployeeBreak/{userId}/{date}")]
        public async Task<IActionResult> GetAllBreakDetail(int userId,DateTime date)
        {
            var res = await this._adminDashboardRepository.GetEmployeeBreakByUserIdAndDate(userId, date);
            return Ok(res);
        }
    }
}
