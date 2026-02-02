using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using System.Data;
using static QPay.UI.Report.EntityReport;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncrementReportController : ControllerBase
    {
        private readonly IIncrementReportRepository _IRepository;
        public IncrementReportController(IIncrementReportRepository IRepository)
        {
            this._IRepository = IRepository;
        }


        [HttpPost, Route("ExportToExcel/{companyId}/{payPeriodId}/{employeeId}")]
        public async Task<IActionResult> ExportToExcel(int? companyId, int? payPeriodId, int? employeeId)
        {
            var ds = await _IRepository.ExportToExcel(companyId, payPeriodId, employeeId);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


    }
}
