using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.UI.Models.Reports;


namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveBalanceReportController : ControllerBase
    {
        private readonly ILeaveBalanceReportRepository _ileave;
        public LeaveBalanceReportController(
          ILeaveBalanceReportRepository ileave)
        {
            this._ileave = ileave;
        }

        [HttpGet, Route("GetLeaveYear")]
        public async Task<IActionResult> GetLeaveYear()
        {
            var response = await _ileave.GetLeaveYear();
            return Ok(response);
        }

        [HttpPost]
        [Route("GetLeaveBalance")]
        public async Task<IActionResult> GetLeaveBalance(LeaveBalanceReportRequest request)
        {
            var ds = await _ileave.GetLeaveBalance(request.companyCode, request.siteId, request.year);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
