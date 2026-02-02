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
    public class AttendanceBatchIdUpdateController : ControllerBase
    {
        private readonly IAttendanceBatchIdUpdateRepository _processRepository;
        public AttendanceBatchIdUpdateController(IAttendanceBatchIdUpdateRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost]
        [Route("ImportAttendanceBatchIdUpdate")]
        public async Task<IActionResult> ImportAttendanceBatchIdUpdate(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.ImportAttendanceBatchIdUpdate(file, User);
            return Ok(result);
        }
    }
}

