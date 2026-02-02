using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using QPay.UI_Domain.Models.PurchaseOrder;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayregisterUnprocessedController : ControllerBase
    {
        private readonly IPayregisterUnprocessedRepository _ipayregisterUnprocessedRepository;
        private readonly IConfiguration _configuration;

        public PayregisterUnprocessedController(
         IPayregisterUnprocessedRepository payregisterUnprocessedRepository)
        {
            this._ipayregisterUnprocessedRepository = payregisterUnprocessedRepository;
        }
       

        [HttpGet]
        [Route("Exporttoexcel/{companyId}/{payperiodId}")]
        public async Task<IActionResult> Exporttoexcel(int companyId, int payperiodId)
        {
            var ds = await _ipayregisterUnprocessedRepository.Exporttoexcel(companyId, payperiodId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
