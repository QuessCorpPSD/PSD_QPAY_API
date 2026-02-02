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
    public class ProcessEmployeeController : ControllerBase
    {
        private readonly IProcessEmployeeRepository _IRepository;
        public ProcessEmployeeController(IProcessEmployeeRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet, Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(PayperiodFilter items)
        {
            var ds = await _IRepository.ExportToExcel(items);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

    }
}
