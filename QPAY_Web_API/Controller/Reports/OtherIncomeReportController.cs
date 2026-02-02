using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherIncomeReportController : ControllerBase
    {
        private readonly IOtherIncomeReportRepository _IRepository;
        public OtherIncomeReportController(IOtherIncomeReportRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet, Route("GetInputno/{CompanyId}/{payPeriodId}")]
        public async Task<IActionResult> GetInputno(int? CompanyId,  int? payPeriodId)
        {
            var ds = await _IRepository.GetInputno(CompanyId, payPeriodId);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpGet, Route("ExportToExcel/{companyId}/{paySequenceNo}/{payCodeId}/{inputNo}")]
        public async Task<IActionResult> ExportToExcel(int? companyId, int? paySequenceNo, int? payCodeId, string? inputNo)
        {
            var ds = await _IRepository.ExportToExcel(companyId, paySequenceNo, payCodeId, inputNo);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


    }
}
