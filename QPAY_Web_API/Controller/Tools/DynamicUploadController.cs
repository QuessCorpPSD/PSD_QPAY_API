using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.Tools;

namespace QPay.API.Controller.Tools
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicUploadController : ControllerBase
    {
        private readonly IDynamicUploadRepository _IRepository;
        public DynamicUploadController(IDynamicUploadRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet, Route("GetUploadType")]
        public async Task<IActionResult> GetUploadType()
        {
            var ds = await _IRepository.GetUploadType(0, 0);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpGet, Route("GetAllColumns/{UploadType}")]
        public async Task<IActionResult> GetAllColumns(int? UploadType)
        {
            var ds = await _IRepository.GetAllColumns(UploadType, 0);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpPost]
        [Route("FileUpload")]
        public async Task<IActionResult> FileUpload(IFormFile file, [FromForm] int UploadTypeId, [FromForm] int CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.FileUpload(file, UploadTypeId, CreatedBy);
            return Ok(result);
        }

    }
}
