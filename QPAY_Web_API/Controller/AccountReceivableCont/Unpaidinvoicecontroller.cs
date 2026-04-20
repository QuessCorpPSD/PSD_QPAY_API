using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IAccountReceivable;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.API.Controller.AccountReceivableCont
{
    [Route("api/[controller]")]
    [ApiController]
    public class Unpaidinvoicecontroller : ControllerBase
    {
        private readonly IUnpaidinvoice _repo;

        public Unpaidinvoicecontroller(IUnpaidinvoice repo)
        {
            _repo = repo;
        }

        [HttpGet("GetEntity/{flag}")]
        public async Task<IActionResult> GetEntity(string flag)
        {
            var ds = await _repo.GetEntity(flag);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpPost]
        [Route("UnpaidInvoiceExport")]
        public async Task<IActionResult> UnpaidInvoiceExport([FromBody] CommonExport payload)
        {
            try
            {
                var ds = await _repo.ExportToExcel(payload);

                HttpContext.Response.StatusCode = 200;   

                var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

                return Ok(res);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;   

                var res = ResponseWrapManager.ResponseWrapper(null, HttpContext);

                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "Error",
                    Error = ex.Message  
                });
            }
        }
    }
}
