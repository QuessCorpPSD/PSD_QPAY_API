using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository;

namespace QPay.API.Controller.Promotion
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {

        //

        private readonly BAL.IRepository.IPromotionIncrementRepository _IRepository;
        private readonly IConfiguration _configuration;
        public PromotionController(IConfiguration configuration, BAL.IRepository.IPromotionIncrementRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }


        [HttpGet]
        [Route("GetAllPayPeriodByCompanyID/{companyId}")]
        public async Task<IActionResult> GetAllPayPeriodByCompanyID(int? companyId)
        {
            var response = await _IRepository.GetAllPayPeriodByCompanyID(companyId);
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
        [Route("GetEmployeeDetailsByCompanyID/{companyId}")]
        public async Task<IActionResult> GetEmployeeDetailsByCompanyID(int? companyId)
        {
            var response = await _IRepository.GetEmployeeDetailsByCompanyID(companyId);
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
        [Route("Search/{companyId}/{employeeId}/{payPeriodId}")]
        public async Task<IActionResult> Search(int? companyId, int? employeeId, int? payPeriodId)
        {
            var response = await _IRepository.GetAllIncrementDetails(companyId, employeeId, payPeriodId);
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
        [Route("GetAllIncrementDetailsByIncrementID/{incrementId}")]
        public async Task<IActionResult> GetAllIncrementDetailsByIncrementID(int? incrementId)
        {
            var response = await _IRepository.GetAllIncrementDetailsByIncrementID(incrementId);
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
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy);
            return Ok(result);
        }



    }
}
