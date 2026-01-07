using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository;
using QPay.UI.Customer;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ITCalenderController : ControllerBase
    {
        private readonly IITCalenderRepository _IRepository;
        private readonly IConfiguration _configuration;
        public ITCalenderController(IConfiguration configuration, IITCalenderRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("Search/{companyId}/{financialYearId}")]
        public async Task<IActionResult> Search(int? companyId, int? financialYearId)
        {
            var response = await _IRepository.Search(companyId,financialYearId);
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
        [Route("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear()
        {
            var response = await _IRepository.GetFinancialYear();
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
        public async Task<IActionResult> Create([FromBody] ITCalenderRequest request)
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
