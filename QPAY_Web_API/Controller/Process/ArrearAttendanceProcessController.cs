using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Process;
using QPay.BAL.Repository.Process;
using static QPay.UI.Models.Process.AttendanceProcess;


namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrearAttendanceProcessController : ControllerBase
    {
        private readonly IArrearAttendanceProcessRepository _arrearAttendanceProcessRepository;
        public ArrearAttendanceProcessController(IArrearAttendanceProcessRepository arrearAttendanceProcessRepository)
        {
            this._arrearAttendanceProcessRepository = arrearAttendanceProcessRepository;
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchArrearRequest searchRequest)
        {
            var ds = await _arrearAttendanceProcessRepository.SearchDetails(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ImportArrearAttendnace")]
        public async Task<IActionResult> ImportArrearAttendnace(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _arrearAttendanceProcessRepository.ImportArrearAttendnace(file, User);
            return Ok(result);
        }
    }
}
