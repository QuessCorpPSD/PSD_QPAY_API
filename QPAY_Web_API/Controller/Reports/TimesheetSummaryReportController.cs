using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.UI.Models.Reports;


namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetSummaryReportController : ControllerBase
    {
        private readonly ITimesheetSummaryReportRepository _itimesheet;
        public TimesheetSummaryReportController(
          ITimesheetSummaryReportRepository itimesheet)
        {
            this._itimesheet = itimesheet;
        }
       
        [HttpPost]
        [Route("GetTSSummaryReport")]
        public async Task<IActionResult> GetTSSummaryReport(TimesheetSummaryReportRequest request)
        {
            var ds = await _itimesheet.GetTSSummaryReport(request.companyId, request.siteId, request.location,
                request.payPeriodId, request.status);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
