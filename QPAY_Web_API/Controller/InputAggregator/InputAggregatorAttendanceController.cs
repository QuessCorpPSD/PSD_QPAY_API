using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.UI.Models.Aggregator;

namespace QPay.API.Controller.InputAggregator
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputAggregatorAttendanceController : ControllerBase
    {
        private readonly BAL.IRepository.IInputAggregatorAttendanceRepository _IRepository;
        private readonly IConfiguration _configuration;
        public InputAggregatorAttendanceController(IConfiguration configuration, BAL.IRepository.IInputAggregatorAttendanceRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }

        #region for mapping 

        [HttpGet]
        [Route("QuessLeaveMaster")]
        public async Task<IActionResult> QuessLeaveMaster()
        {
            var response = await _IRepository.QuessLeaveMaster();
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
        [Route("leaveTypeMaster")]
        public async Task<IActionResult> leaveTypeMaster()
        {
            var response = await _IRepository.leaveTypeMaster();
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
        [Route("Createleavetype")]
        public async Task<IActionResult> Createleavetype([FromBody] leaveTypeMasterRequest request)
        {
            var response = await _IRepository.Createleavetype(request);
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
        [Route("Createleavemapping")]
        public async Task<IActionResult> Createleavemapping([FromBody] AttendanceAggregatorRequest request)
        {
            var response = await _IRepository.Createleavemapping(request);
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
        [Route("QuessAttendanceAttributeMaster")]
        public async Task<IActionResult> QuessAttendanceAttributeMaster()
        {
            var response = await _IRepository.QuessAttendanceAttributeMaster();
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
        [Route("ClientAttributesUpload")]
        public async Task<IActionResult> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.ClientAttributesUpload(file, CreatedBy);
            return Ok(result);
        }

        [HttpPost]
        [Route("AttributesMappingUpload")]
        public async Task<IActionResult> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.AttributesMappingUpload(file, CreatedBy);
            return Ok(result);
        }

        #endregion for mapping



        /*
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


        */

        #region for billing report generation

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy, CompanyId);
            return Ok(result);
        }


        [HttpGet]
        [Route("billableReport/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> billableReport(int? companyId, int? payPeriodId)
        {
            var response = await _IRepository.billableReport(companyId, payPeriodId);
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

        #endregion for billing report generation

        
    }
}
