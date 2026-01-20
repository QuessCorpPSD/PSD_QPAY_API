using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayFrequencyController : ControllerBase
    {
        private readonly IPayFrequencyRepository _IRepository;
        private readonly IConfiguration _configuration;
        public PayFrequencyController(IConfiguration configuration, IPayFrequencyRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("Search/{companyId}")]
        public async Task<IActionResult> Search(int? companyId)
        {
            var response = await _IRepository.Search(companyId);
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



        [HttpGet]
        [Route("ExportToExcel/{companyId}")]
        public async Task<IActionResult> ExportToExcel(int? companyId)
        {
            var response = await _IRepository.ExportToExcel(companyId);
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


        [HttpGet]
        [Route("GetGroupName/{companyId}")]
        public async Task<IActionResult> GetGroupName(int? companyId)
        {
            var response = await _IRepository.GetGroupName(companyId);
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

        [HttpGet]
        [Route("GetData/{startDate}/{endDate}")]
        public async Task<IActionResult> GetData(string startDate, string endDate)
        {
            var response = await _IRepository.GetData(startDate, endDate);
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

        [HttpGet]
        [Route("CheckPayFrequencyExists/{companyId}/{startDate}/{endDate}/{payPeriod}")]
        public async Task<IActionResult> CheckPayFrequencyExists(int companyId, string startDate, string endDate, string payPeriod)
        {
            var response = await _IRepository.CheckPayFrequencyExists(companyId, startDate, endDate, payPeriod);
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
        public async Task<IActionResult> Create([FromBody] PayFrequencyRequest request)
        {
            var response = await _IRepository.Create(request);
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

    }
}
