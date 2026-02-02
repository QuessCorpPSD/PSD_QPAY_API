using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.Reports;
using static QPay.UI.Report.EntityReport;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetpaySummaryController : ControllerBase
    {
        private readonly INetpaySummaryRepository _IRepository;
        public NetpaySummaryController(INetpaySummaryRepository IRepository)
        {
            this._IRepository = IRepository;
        }


        [HttpGet, Route("ExportToExcel/{CompanyId}/{PayPeriodId}")]
        public async Task<IActionResult> ExportToExcel(int? CompanyId,  int? PayPeriodId)
        {
            var ds = await _IRepository.ExportToExcel(CompanyId, PayPeriodId);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("ExportToExcel_Entity")]
        public async Task<IActionResult> ExportToExcel_Entity(EntityPayregisterFilter items)
        {
            var ds = await _IRepository.ExportToExcel_Entity(items);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
