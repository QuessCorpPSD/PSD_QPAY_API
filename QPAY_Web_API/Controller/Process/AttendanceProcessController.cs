using Azure.Core;
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
    public class AttendanceProcessController : ControllerBase
    {
        private readonly IAttendanceProcessRepository _attendanceProcessRepository;
        public AttendanceProcessController(IAttendanceProcessRepository attendanceProcessRepository)
        {
            this._attendanceProcessRepository = attendanceProcessRepository;
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchRequest searchRequest)
        {
            var ds = await _attendanceProcessRepository.SearchDetails(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("ExporttoExcel")]
        public async Task<IActionResult> ExporttoExcel(ExporttoExcelRequest exporttoExcelRequest)
        {
            var ds = await _attendanceProcessRepository.ExporttoExcel(exporttoExcelRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ImportAttendnace")]
        public async Task<IActionResult> ImportAttendnace(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _attendanceProcessRepository.ImportAttendnace(file, User);
            return Ok(result);
        }

    }
}
