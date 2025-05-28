using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;
using QPAY_Web_API.Models;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionAnswerController : ControllerBase
    {
        private readonly IQARepository _qaRepository;
        public QuestionAnswerController(IQARepository qaRepository)
        {
            this._qaRepository = qaRepository;
        }


        [HttpGet, Route("GetCustomerSOPQuestionAnswer")]
        public async Task<IActionResult> GetCustomerSOPQuestionAnswer()
        {
            var res = this._qaRepository.GetCustomerSOPQuestionAnswer();
            return Ok(res);
        }
    }
}
