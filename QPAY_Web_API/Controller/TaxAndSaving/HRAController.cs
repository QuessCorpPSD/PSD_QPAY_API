using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository;
using QPay.UI.Models.TaxAndSaving;

namespace QPay.API.Controller.TaxAndSaving
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRAController : ControllerBase
    {
        private readonly IHRARepository _IRepository;
        private readonly IConfiguration _configuration;
        public HRAController(IConfiguration configuration, IHRARepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }
        

        [HttpGet]
        [Route("GetDeclarationType")]
        public async Task<IActionResult> GetDeclarationType()
        {
            var response = await _IRepository.GetDeclarationType();
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
        [Route("Search/{companyId}/{EmployeeId}/{finYearId}")]
        public async Task<IActionResult> Search(int? companyId, int? employeeId, int? finYearId)
        {
            var response = await _IRepository.Search(companyId, employeeId, finYearId);
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
        [Route("GetEmployeeListAdd/{companyId}/{financialYrID}/{employeeID}")]
        public async Task<IActionResult> GetEmployeeListAdd(int? companyID, int? financialYrID, int? employeeID)
        {
            var response = await _IRepository.GetEmployeeListAdd(companyID, financialYrID, employeeID);
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
        public async Task<IActionResult> Create([FromBody] HRARequest request)
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
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] int CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy);
            return Ok(result);
        }

    }
}
