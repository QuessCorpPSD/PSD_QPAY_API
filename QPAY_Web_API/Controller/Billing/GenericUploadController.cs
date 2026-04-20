using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Billing;
using QPay.BAL.IRepository.Customer;

namespace QPay.API.Controller.Billing
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericUploadController : ControllerBase
    {
        private readonly IGenericUploadRepository _IRepository;
        public GenericUploadController(IGenericUploadRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("masters/{userId}")]
        public async Task<IActionResult> masters(int userId)
        {
            var response = await _IRepository.masters(userId);
            if (response != null)
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
        [Route("DownloadTemplate/{UploadType}")]
        public async Task<IActionResult> DownloadTemplate(string UploadType)
        {
            string UploadTypeTrim = string.Empty;

            if (!string.IsNullOrEmpty(UploadType))
            {
                UploadTypeTrim = UploadType.Replace(" ", "");
            }

            var response = await _IRepository.DownloadTemplate(UploadTypeTrim);

            if (response != null)
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
        [Route("FileUpload")]
        public async Task<IActionResult> FileUpload(IFormFile file, [FromForm] string uploadType, [FromForm] int createdBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.FileUpload(file, uploadType, createdBy);
            return Ok(result);
        }


    }
}
