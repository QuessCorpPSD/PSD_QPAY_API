using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Process;
using static QPay.UI.Models.Process.Process;

namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReimbursementCalendarController : ControllerBase
    {
        private readonly IReimbursementCalendarRepository _processRepository;
        public ReimbursementCalendarController(IReimbursementCalendarRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchReimbursementRequest searchRequest)
        {
            var ds = await _processRepository.SearchDetails(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }       
    }
}


