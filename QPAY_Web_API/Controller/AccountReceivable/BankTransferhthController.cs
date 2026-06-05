using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.BAL.IRepository.IAccountReceivable;

namespace QPay.API.Controller.AccountReceivable
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankTransferhthController : ControllerBase
    {

        private readonly IBankTransferRepository _itransfer;
        public BankTransferhthController(
          IBankTransferRepository itransfer)
        {
            this._itransfer = itransfer;
        }
        [HttpGet]
        [Route("Search/{FromDate}/{ToDate}")]
        public async Task<IActionResult> Search(
            string FromDate,
            string ToDate)
        {
            var ds = await _itransfer.Search(FromDate, ToDate);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpGet]
        [Route("ExportToExcel/{FromDate}/{ToDate}")]
        public async Task<IActionResult> ExportToExcel(
            string FromDate,
            string ToDate)
        {
            var ds = await _itransfer.ExportToExcel(FromDate, ToDate);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }



    }
}
