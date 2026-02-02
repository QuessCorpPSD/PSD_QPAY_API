using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using static QPay.UI.Report.EntityReport;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayregisterEntitywiseController : ControllerBase
    {
        private readonly IPayregisterEntitywiseRepository _IRepository;
        public PayregisterEntitywiseController(IPayregisterEntitywiseRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpPost, Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(EntityPayregisterFilter items)
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
