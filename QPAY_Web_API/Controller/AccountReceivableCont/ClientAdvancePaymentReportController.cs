using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System.Data;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.API.Controller.AccountReceivable
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAdvancePaymentReportController : ControllerBase
    {
        private readonly IClientAdvancePaymentReportRepository _ireport;

        public ClientAdvancePaymentReportController(
            IClientAdvancePaymentReportRepository ireport)
        {
            this._ireport = ireport;
        }

        [HttpGet]
        [Route("Search/{CompanyId}/{FromDate}/{ToDate}")]
        public async Task<IActionResult> Search(int? CompanyId, string FromDate, string ToDate)
        {
            var ds = await _ireport.Search(CompanyId, FromDate, ToDate);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

      
        [HttpPost]
        [Route("ClientAdvancePaymentReportExport")]
        public async Task<IActionResult> ClientAdvancePaymentReportExportToExcel([FromBody] CommonExport payload)
        {
            var ds = await _ireport.ExportToExcel(payload);
            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetDateTypeClientAdvPay/{Description}/{Action}")]
        public async Task<IActionResult> GetDateTypeClientAdvPay(string Description, string Action)
        {
            var ds = await _ireport.GetDateTypeClientAdvPay(Description, Action);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}