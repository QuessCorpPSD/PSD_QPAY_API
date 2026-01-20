using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPFController : ControllerBase
    {
        private readonly ICPFRepository _IRepository;
        public CPFController(ICPFRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search(CpfSearchParams _params)
        {
            var response = await _IRepository.Search(_params.Paycode, _params.Category);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CPFCreateParams _params)
        {
            var response = await _IRepository.Create(_params.strXmlDetails, _params.mode, _params.userId);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }


        [HttpGet]
        [Route("GetCriteria")]
        public async Task<IActionResult> GetCriteria()
        {
            var response = await _IRepository.GetCriteria(0);
            if (response != null)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpGet]
        [Route("GetCategory")]
        public async Task<IActionResult> GetCategory()
        {
            var response = await _IRepository.GetCategory();
            if (response != null)
            {

                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }


        [HttpGet]
        [Route("GetPaycode")]
        public async Task<IActionResult> GetPaycode()
        {
            var response = await _IRepository.GetPaycode();
            if (response != null)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }


    }
}
