using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Process;
using QPay.UI.Process;

namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayProcessController : ControllerBase
    {
        private readonly IPayProcessRepository _payProcessRepository;
        public PayProcessController(IPayProcessRepository payProcessRepository) {
        this._payProcessRepository = payProcessRepository;
        }

        [HttpPost,Route("GetITCalenderCompany")]
        public async Task<IActionResult> GetITCalenderCompany(PayProcessRequest payProcessRequest)
        {
            var status =await this._payProcessRepository.GetITCalenderCompany(payProcessRequest.company_Id, payProcessRequest.End_At);
            return Ok(status);
        }
        [HttpPost, Route("CheckPayPeriod")]
        public async Task<IActionResult> CheckPayPeriod(PayProcessPayperiodRequest payProcessRequest)
        {
            var status = await this._payProcessRepository.CheckPayPeriod(payProcessRequest.company_Id, payProcessRequest.payperiodId);
            return Ok(status);
        }
    }
}
