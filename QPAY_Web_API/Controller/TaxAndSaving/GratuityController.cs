using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository;
using QPay.UI.Models.TaxAndSaving;

namespace QPay.API.Controller.TaxAndSaving
{
    [Route("api/[controller]")]
    [ApiController]
    public class GratuityController : ControllerBase
    {
        private readonly IGratuityRepository _IRepository;
        private readonly IConfiguration _configuration;
        public GratuityController(IConfiguration configuration, IGratuityRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("GetEmployeeCodeForGratuity/{companyId}/{FinancialYrId}")]
        public async Task<IActionResult> GetEmployeeCodeForGratuity(int? companyId, int? FinancialYrId)
        {
            var response = await _IRepository.GetEmployeeCodeForGratuity(companyId, FinancialYrId);
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
        [Route("GetGratuityEmployeeByEmpId/{employeeId}")]
        public async Task<IActionResult> GetGratuityEmployeeByEmpId(int? employeeId)
        {
            var response = await _IRepository.GetGratuityEmployeeByEmpId(employeeId);
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
        [Route("GetBasicAmountByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetBasicAmountByEmployeeId(int? employeeId)
        {
            var response = await _IRepository.GetBasicAmountByEmployeeId(employeeId);
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
        [Route("GetDAAmountByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetDAAmountByEmployeeId(int? employeeId)
        {
            var response = await _IRepository.GetDAAmountByEmployeeId(employeeId);
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
        [Route("Search/{companyId}/{EmployeeId}")]
        public async Task<IActionResult> Search(int? companyId, int? EmployeeId)
        {
            var response = await _IRepository.Search(companyId, EmployeeId);
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
        public async Task<IActionResult> Create([FromBody] GratuityRequest request)
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
