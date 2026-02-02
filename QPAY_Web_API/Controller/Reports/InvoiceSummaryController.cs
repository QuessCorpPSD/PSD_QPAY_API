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
    public class InvoiceSummaryController : ControllerBase
    {
        private readonly IInvoiceSummaryRepository _IRepository;
        public InvoiceSummaryController(IInvoiceSummaryRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        //

        [HttpGet, Route("GetTaxTypes")]
        public async Task<IActionResult> GetTaxTypes()
        {
            var ds = await _IRepository.GetTaxTypes();
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("ExportToExcel/{CompanyId}/{FromDate}/{ToDate}/{ReportTypeId}/{UserId}")]
        public async Task<IActionResult> ExportToExcel(int? CompanyId, string FromDate, string ToDate, int? ReportTypeId, int? UserId)
        {
            var ds = await _IRepository.ExportToExcel(CompanyId, FromDate, ToDate, ReportTypeId, UserId);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("ExportToExcel_Entity")]
        public async Task<IActionResult> ExportToExcel_Entity(EntityPayregister_Filter items)
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
