using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository;
using QPay.UI.Customer;
using QPay.UI.Models.TaxAndSaving;

namespace QPay.API.Controller.TaxAndSaving
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenEducationAllowanceController : ControllerBase
    {
        private readonly IChildrenEducationAllowanceRepository _IRepository;
        private readonly IConfiguration _configuration;
        public ChildrenEducationAllowanceController(IConfiguration configuration, IChildrenEducationAllowanceRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear()
        {
            var response = await _IRepository.sp_GetFinancialYear();
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
        [Route("GetAllType")]
        public async Task<IActionResult> GetAllType()
        {
            var response = await _IRepository.GetAllType();
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
        [Route("GetEmployeesList/{companyId}/{financialYearId}")]
        public async Task<IActionResult> GetEmployeesList(int? companyId, int? financialYearId)
        {
            var response = await _IRepository.GetEmployeesList(companyId, financialYearId);
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
        [Route("GetEligibleEmployee/{financialYearId}/{EmployeeId}")]
        public async Task<IActionResult> GetEligibleEmployee(int? financialYearId, int? EmployeeId)
        {
            var response = await _IRepository.GetEligibleEmployee(financialYearId, EmployeeId);
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
        [Route("GetEligibleChildren/{Effective_Date}/{Number_Of_Children}")]
        public async Task<IActionResult> GetEligibleChildren(string Effective_Date, int Number_Of_Children)
        {
            var response = await _IRepository.GetEligibleChildren(Effective_Date, Number_Of_Children);
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
        [Route("Search/{companyId}/{financialYearId}/{EmployeeId}")]
        public async Task<IActionResult> Search(int? companyId, int? financialYearId, int? EmployeeId)
        {
            var response = await _IRepository.Search(companyId, financialYearId, EmployeeId);
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
        public async Task<IActionResult> Create([FromBody] ChildrenEducationAllowanceRequest request)
        {
            var response = await _IRepository.Create(request);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("successfully")))
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
