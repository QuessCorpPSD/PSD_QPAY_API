using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.UI.Models.Reports;


namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingReportController : ControllerBase
    {
        private readonly IBillingReportRepository _itimesheet;
        public BillingReportController(
          IBillingReportRepository itimesheet)
        {
            this._itimesheet = itimesheet;
        }

        [HttpPost]
        [Route("GetBillingReport")]
        public async Task<IActionResult> GetBillingReport(BillingReportRequest request)
        {
            var ds = await _itimesheet.GetBillingReport(request.companyCode, request.siteId, request.payPeriodId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
