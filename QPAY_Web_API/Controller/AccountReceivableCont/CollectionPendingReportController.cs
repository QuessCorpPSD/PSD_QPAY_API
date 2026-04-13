using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.UI_Domain.Models.AccountReceivable;

namespace QPay.API.Controller.AccountReceivable
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionPendingReportController : ControllerBase
    {
        private readonly ICollectionPendingReportRepository _icollection;
        public CollectionPendingReportController(
          ICollectionPendingReportRepository iclient)
        {
            this._icollection = iclient;
        }

        [HttpGet("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear(int? financialYearId)
        {
            var ds = await _icollection.GetFinancialYear(financialYearId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpGet("GetEntity/{flag}")]
        public async Task<IActionResult> GetEntity(string flag)
        {
            var ds = await _icollection.GetEntity(flag);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CollectionPendingExport")]
        public async Task<IActionResult> CollectionPendingExport([FromBody] CollectionPendingExport payload)
        {
            var ds = await _icollection.CollectionPendingExportToExcel(payload);
            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(res);
        }




    }
}
