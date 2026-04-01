using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
using QPay.BAL.IRepository.Invoice;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class SezRepositoryController : ControllerBase
    {
        private readonly ISezRepository _sezRepository;

        public SezRepositoryController(ISezRepository sezRepository)
        {
            this._sezRepository = sezRepository;
        }

        [HttpPost]
        [Route("SEZSearch")]
        public async Task<IActionResult> SEZSearch(SezModelRequest sezModelRequest)
        {
            var sezResult = await this._sezRepository.Search(sezModelRequest.Company_Id, sezModelRequest.PayPeriod_Id, sezModelRequest.InvoiceNumbers.ToString(), sezModelRequest.Year);
            return Ok(sezResult);
        }

        [HttpPost]
        [Route("Export")]
        public IActionResult Export(SezModelRequest sezModelRequest)
        {
            var sezResult = this._sezRepository.ExportToExcel(Convert.ToInt32(sezModelRequest.Company_Id),Convert.ToInt32(sezModelRequest.PayPeriod_Id), sezModelRequest.InvoiceNumbers.ToString(), sezModelRequest.Year);
            return Ok(sezResult);
        }
       
    }
}
