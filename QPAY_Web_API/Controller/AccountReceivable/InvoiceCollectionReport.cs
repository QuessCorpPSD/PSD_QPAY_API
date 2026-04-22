using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.UI.Models.AccountReceivableMod;

namespace QPay.API.Controller.AccountReceivableCont
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceCollectionReportController : ControllerBase
    {
        private readonly IInvoiceCollectionReport _invoiceRepo;

        public InvoiceCollectionReportController(IInvoiceCollectionReport invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }

        [HttpPost]
        [Route("InvoiceCollectionExport")]
        public async Task<IActionResult> InvoiceCollectionExport([FromBody] InvoiceCollectionReport payload)
        {
            var ds = await _invoiceRepo.InvoiceCollectionExportToExcel(payload);

            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }
        [HttpGet]
        [Route("GetGENTabledata/{Description}/{Flag}")]
        public async Task<IActionResult> GetGENTabledata(string Description, string Flag)
        {
            var ds = await _invoiceRepo.GetGENTabledata(Description, Flag);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


    }
}