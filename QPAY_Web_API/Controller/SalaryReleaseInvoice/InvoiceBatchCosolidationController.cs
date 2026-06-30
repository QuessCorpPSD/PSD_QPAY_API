using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.UI.Models.SalaryReleaseInvoice;
using System.Data;

namespace QPay.API.Controller.SalaryReleaseInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceBatchConsolidationController : ControllerBase
    {
        private readonly IinvoiceBatchConsolidationRepository _iapar;

        public InvoiceBatchConsolidationController(IinvoiceBatchConsolidationRepository iapar)
        {
            _iapar = iapar;
        }

        [HttpGet("GetBusinessUnit")]
        public async Task<IActionResult> GetBusinessUnit()
        {
            var ds = await _iapar.GetBusinessUnitName();
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("InvoiceBatchConsolidationExport")]
        public async Task<IActionResult> InvoiceBatchConsolidationExport([FromBody] InvoiceBatchExport payload)
        {
            var ds = await _iapar.InvoiceBatchConsolidationExport(payload);

            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }

        [HttpPost("searchnew")]

        public async Task<IActionResult> Search(HTHBankTransferStatusDto request)

        {

            var ds = await _iapar.SearchHTHBankTransferStatus(request);


            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);


            return Ok(payload);

        }


        [HttpPost("exporttoexcel")]

        public async Task<IActionResult> ExportToExcel(HTHBankTransferStatusDto request)

        {

            var ds = await _iapar.ExportToExcelHTHBankTransferStatus(request);


            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);


            return Ok(payload);

        }


    }
}