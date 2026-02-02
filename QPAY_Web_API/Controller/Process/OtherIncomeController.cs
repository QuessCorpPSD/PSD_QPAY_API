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
    public class OtherIncomeController : ControllerBase
    {
        private readonly IOtherIncomeRepository _processRepository;
        public OtherIncomeController(IOtherIncomeRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchOIRequest searchRequest)
        {
            var ds = await _processRepository.SearchDetails(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        

        [HttpPost]
        [Route("ImportOtherIncome")]
        public async Task<IActionResult> ImportOtherIncome(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.ImportOtherIncome(file, User);
            return Ok(result);
        }

        [HttpGet, Route("DeleteOtherIncome/{Other_Income_Id}/{CreatedBy}")]
        public async Task<IActionResult> DeleteOtherIncome(string Other_Income_Id, string CreatedBy)
        {
            var ds = await _processRepository.DeleteOtherIncome(Other_Income_Id, CreatedBy);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

    }
}

