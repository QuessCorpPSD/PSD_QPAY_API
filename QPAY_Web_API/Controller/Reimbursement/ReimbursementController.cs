using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.UI.Customer;
using QPay.UI.Reimbursements;

namespace QPay.API.Controller.Reimbursement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReimbursementController : ControllerBase
    {
        private readonly BAL.IRepository.IReimbursementRepository _IRepository;
        private readonly IConfiguration _configuration;
        public ReimbursementController(IConfiguration configuration, BAL.IRepository.IReimbursementRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }


        [HttpGet]
        [Route("Search/{companyId}/{financialYearId}/{employeeId}")]
        public async Task<IActionResult> Search(int? companyId, int? financialYearId, int? employeeId)
        {
            var response = await _IRepository.Search(companyId, financialYearId, employeeId);
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
        [Route("GetAllFrequency/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> GetAllFrequency(int? companyId, int? financialYearId)
        {
            var response = await _IRepository.GetAllFrequency(companyId, financialYearId);
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


        [Route("GetAllRembPaycodes/{companyId}")]
        public async Task<IActionResult> GetAllRembPaycodes(int? companyId)
        {
            var response = await _IRepository.GetAllRembPaycodes(companyId);
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

        [Route("GetReimbursementDetail/{reimbursementId}")]
        public async Task<IActionResult> GetReimbursementDetail(int? reimbursementId)
        {
            var response = await _IRepository.GetReimbursementDetail(reimbursementId);
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
        public async Task<IActionResult> Create([FromBody] ReimbursementRequest request)
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
