using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class POBalanceReportController : ControllerBase
    {
        private readonly IPOBalanceReportRepository _IRepository;
        public POBalanceReportController(IPOBalanceReportRepository IRepository)
        {
            this._IRepository = IRepository;
        }


        [HttpGet, Route("ExportToExcel/{CompanyId}")]
        public async Task<IActionResult> ExportToExcel(int? CompanyId)
        {
            var ds = await _IRepository.ExportToExcel(CompanyId);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
