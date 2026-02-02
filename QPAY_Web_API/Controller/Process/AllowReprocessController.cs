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
    public class AllowReprocessController : ControllerBase
    {
        private readonly IAllowReprocessRepository _processRepository;
        public AllowReprocessController(IAllowReprocessRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchAllowReprocessRequest searchRequest)
        {
            var ds = await _processRepository.SearchDetails(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("ExporttoExcel")]
        public async Task<IActionResult> ExporttoExcel(SearchAllowReprocessRequest exporttoExcelRequest)
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
        [Route("Create")]
        public async Task<IActionResult> Create(AllowReprocessCreateRequest createRequest)
        {
            var result = await _processRepository.Create(createRequest);
            return Ok(result);
        }
    }
}

