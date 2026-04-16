using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.BAL.Repository;
using QPay.DAL.Repository;
using System.Data;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvanceUtilizationReportController : ControllerBase
    {
        private readonly IAdvanceUtilizationRepository _IRepository;
        public AdvanceUtilizationReportController(IAdvanceUtilizationRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet, Route("ExportToExcel/{PayPeriod}")]
        public async Task<IActionResult> ExportToExcel(string? PayPeriod)
        {
            var ds = await _IRepository.ExportToExcel(PayPeriod);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}

