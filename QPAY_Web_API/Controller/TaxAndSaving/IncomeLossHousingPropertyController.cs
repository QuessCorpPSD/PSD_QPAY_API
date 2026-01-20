using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository;
using QPay.UI.Models.TaxAndSaving;

namespace QPay.API.Controller.TaxAndSaving
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeLossHousingPropertyController : ControllerBase
    {
        private readonly IIncomeLossHousingPropertyRepository _IRepository;
        private readonly IConfiguration _configuration;
        public IncomeLossHousingPropertyController(IConfiguration configuration, IIncomeLossHousingPropertyRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }


        [HttpGet]
        [Route("Search/{companyId}/{EmployeeId}")]
        public async Task<IActionResult> Search(int? companyId, int? employeeId)
        {
            var response = await _IRepository.Search(companyId, employeeId);
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
        public async Task<IActionResult> Create([FromBody] IncomeLossHousePropertyRequest request)
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


        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] int CreatedBy, [FromForm] string action)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy, action);
            return Ok(result);
        }

    }
}
