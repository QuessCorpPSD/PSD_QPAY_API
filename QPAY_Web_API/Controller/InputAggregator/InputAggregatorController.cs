using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;

namespace QPay.API.Controller.InputAggregator
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputAggregatorController : ControllerBase
    {
        private readonly BAL.IRepository.IInputAggregatorRepository _IRepository;
        private readonly IConfiguration _configuration;
        public InputAggregatorController(IConfiguration configuration, BAL.IRepository.IInputAggregatorRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }

        #region for mapping 

        [HttpGet]
        [Route("QuessAttributeMaster")]
        public async Task<IActionResult> QuessAttributeMaster()
        {
            var response = await _IRepository.QuessAttributeMaster();
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
        [Route("ClientAttributes/{companyId}")]
        public async Task<IActionResult> ClientAttributes(int? companyId)
        {
            var response = await _IRepository.ClientAttributes(companyId);
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

        #region for billing report generation

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy);
            return Ok(result);
        }

        #endregion for billing report generation
    }
}
