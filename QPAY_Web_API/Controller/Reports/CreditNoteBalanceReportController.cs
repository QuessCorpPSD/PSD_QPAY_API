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
    public class CreditNoteBalanceReportController : ControllerBase
    {
        private readonly ICreditNoteBalanceReportRepository _IRepository;
        public CreditNoteBalanceReportController(ICreditNoteBalanceReportRepository IRepository)
        {
            this._IRepository = IRepository;
        }


        [HttpPost, Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(companywithdateFilter items)
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
