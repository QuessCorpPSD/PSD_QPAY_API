using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.UI.Models.Reports;


namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceLeaveBalanceReportController : ControllerBase
    {
        private readonly IInvoiceLeaveBalanceReportRepository _ileave;
        public InvoiceLeaveBalanceReportController(
          IInvoiceLeaveBalanceReportRepository ileave)
        {
            this._ileave = ileave;
        }

        [HttpPost]
        [Route("GetLeaveBalance")]
        public async Task<IActionResult> GetLeaveBalance(InvoiceLeaveBalanceReportRequest request)
        {
            var ds = await _ileave.GetLeaveBalance(request.companyId, request.siteId, request.fromMonth,
                request.fromYear, request.toMonth, request.toYear);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
