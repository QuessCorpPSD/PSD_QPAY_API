using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.BAL.IRepository.SalaryReleaseInvoice;

namespace QPay.API.Controller.SalaryReleaseInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceBatchCosolidationController : ControllerBase
    {
        private readonly IinvoiceBatchConsolidationRepository _iapar;

        public InvoiceBatchCosolidationController(IinvoiceBatchConsolidationRepository iapar)
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
    }
}
