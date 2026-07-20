using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;
using QPay.BAL.Repository;
using QPay.UI.Dashboard;
using QPay.UI.Models;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly ILogger<DashBoardController> _logger;

        public DashBoardController(ILogger<DashBoardController> logger,IDashboardRepository dashboardRepository, IAdminDashboardRepository adminDashboardRepository)
        {
            _logger = logger;
            this._dashboardRepository=dashboardRepository;
            this._adminDashboardRepository=adminDashboardRepository;

        }
        [HttpGet,Route("GetDashBoardByUserId/{userId}")]
        public  IActionResult GetDashBoardByUserId(int userId)
        {
            var data = this._dashboardRepository.GetAllottedLotsByUserId(userId);
            return Ok(data);
        }

        [HttpGet,Route("GetAdminDashBoard")]
        public async Task<IActionResult> GetAdminDashBoard()
        {
            var data =await this._adminDashboardRepository.GetAdminDashboard();
            return Ok(data);
        }
        [HttpPost, Route("GetAdminDashBoardDetail")]
        public async Task<IActionResult> GetAdminDashBoardDetail(AdminDashboardParameterlUI adminDashboardParameterlUI)
        {
            var data = await this._adminDashboardRepository.GetAdminDashboardDetail(adminDashboardParameterlUI);
            return Ok(data);
        }

        [HttpGet,Route("GetPendingLotDetail")]
        public  async Task<IActionResult> GetPendingLotDetail()
        {
            var data = await this._dashboardRepository.GetLotAllottmentPendings();
            return Ok(data);
        }

        [HttpPost,Route("GetInputLotCompletedDetail")]
        public  async Task<IActionResult> GetInputLotCompletedDetail(DashboardRequestModel dashboardRequestModel)
        {
            var completed =await this._dashboardRepository.GetInputLotDetail(dashboardRequestModel);

            return Ok(completed);
        }
        [HttpGet]
        [Route("ReconNotYettoCome/{flag}")]
        public async Task<IActionResult> ReconNotYettoCome(string flag)
        {
            var files = await this._adminDashboardRepository.InputReconAndYettoCome(flag);
            return Ok(files);
        }
        

        [HttpGet("PendingLot")]
        public async Task<ActionResult<List<PendingLotsUI>>> GetPendingLot()
        {
            try
            {
                var pendingLots = await _adminDashboardRepository.GetPendingLotAsync();          

                return Ok(pendingLots);
            }
            catch (Exception ex)
            {
                // Optional: Replace with ILogger logging
                return new List<PendingLotsUI>();
            }
        }

        [HttpGet,Route("CategoryLotDetail/{assignmentType}")]
        public async Task<IActionResult> CategoryLotDetail(string assignmentType)
        {
            var status=await this._adminDashboardRepository.GetCategoryLotDetails(assignmentType);
            return Ok(status);
        }

        [HttpPost,Route("DownloadPayRegister")]
        public IActionResult PayRegisterDownload()
        {
            FileResponse fileResponse = new FileResponse();

            return Ok(fileResponse);
        }

        [HttpGet, Route("SaveInvoiceAllotEdit/{reqNo}/{userId}")]
        public async Task<IActionResult> SaveInvoiceAllotEdit(string reqNo,int userId)
        {
            var status = await this._dashboardRepository.SaveInvoiceAllotEdit(reqNo, userId);
            return Ok(status);
        }
    }
}
