using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;
using QPay.BAL.Repository;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashBoardController(IDashboardRepository dashboardRepository)
        {
            this._dashboardRepository=dashboardRepository;

        }
        [HttpGet,Route("GetDashBoardByUserId/{userId}")]
        public  IActionResult GetDashBoardByUserId(int userId)
        {
            var data = this._dashboardRepository.GetAllottedLotsByUserId(userId);
            return Ok(data);
        }
    }
}
