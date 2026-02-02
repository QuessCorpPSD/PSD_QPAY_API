using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Process;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayRegisterUploadController : ControllerBase
    {
        private readonly IPayRegisterUploadRepository _processRepository;
        public PayRegisterUploadController(IPayRegisterUploadRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost, Route("DownloadTemplate")]
        public async Task<IActionResult> SearchDetails(SearchPayRegisterRequest searchRequest)
        {
            var ds = await _processRepository.DownloadTemplate(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("ExporttoExcel")]
        public async Task<IActionResult> ExporttoExcel(SearchPayRegisterRequest exporttoExcelRequest)
        {
            var ds = await _processRepository.ExporttoExcel(exporttoExcelRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ImportPayRegister")]
        public async Task<IActionResult> ImportPayRegister(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.ImportPayRegister(file, User);
            return Ok(result);
        }
    }
}
