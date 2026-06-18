using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.Reports;
using System.Data;
using static QPay.UI.Report.EntityReport;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProInvoiceSummaryController : ControllerBase
    {
        private readonly IProInvoiceSummaryRepository _IRepository;
        public ProInvoiceSummaryController(IProInvoiceSummaryRepository IRepository)
        {
            this._IRepository = IRepository;
        }

      

        [HttpGet, Route("ExportToExcel/{CompanyId}/{FromDate}/{ToDate}")]
        public async Task<IActionResult> ExportToExcel(int? CompanyId, string FromDate, string ToDate)
        {
            var ds = await _IRepository.ExportToExcel(CompanyId, FromDate, ToDate);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

    }
}
