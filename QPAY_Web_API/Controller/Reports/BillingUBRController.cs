using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.UI.Models.Reports;
using static QPay.UI.Report.EntityReport;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingUBRController : ControllerBase
    {
        private readonly IBillingUBRRepository _itimesheet;
        public BillingUBRController(IBillingUBRRepository itimesheet)
        {
            this._itimesheet = itimesheet;
        }

        [HttpPost]
        [Route("GetBillingReport")]
        public async Task<IActionResult> GetBillingReport(PayperiodFilter items)
        {
            var ds = await _itimesheet.GetBillingReport(items);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
