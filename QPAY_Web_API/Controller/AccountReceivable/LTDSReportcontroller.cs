using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.UI.Models.AccountReceivableMod;
using System.Data;
using System.Threading.Tasks;

namespace QPay.API.Controller.AccountReceivableCont
{
    [Route("api/[controller]")]
    [ApiController]
    public class LTDSReportcontroller : ControllerBase
    {
        private readonly ILTDSReport _repo;

        public LTDSReportcontroller(ILTDSReport repo)
        {
            _repo = repo;
        }

        [HttpGet("GetLTDSReportType")]
        public async Task<IActionResult> GetLTDSReportType()
        {
            var ds = await _repo.GetLTDSReportType("GetLTDSReportType");

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpGet("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear(int? id)
        {
            var ds = await _repo.GetFinancialYear(id);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpPost]
        [Route("LTDSReportExport")]
        public async Task<IActionResult> LTDSReportExport([FromBody] LTDSExportRequest payload)
        {
            var ds = await _repo.LTDSReportExportToExcel(payload);   // ✅ FIXED

            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }
        [HttpGet("GetBusinessUnits/{reportTypeId}")]
        public async Task<IActionResult> GetBusinessUnits(int reportTypeId)
        {
            var ds = await _repo.GetBusinessUnits(reportTypeId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

    }
}