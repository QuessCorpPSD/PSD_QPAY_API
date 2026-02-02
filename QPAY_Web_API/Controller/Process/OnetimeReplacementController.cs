using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Process;
using static QPay.UI.Models.Process.Process;

namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnetimeReplacementController : ControllerBase
    {
        private readonly IOnetimeReplacementRepository _processRepository;
        public OnetimeReplacementController(IOnetimeReplacementRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchOnetimeReplacementRequest searchRequest)
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
        public async Task<IActionResult> ExporttoExcel(SearchOnetimeReplacementRequest exporttoExcelRequest)
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
        [Route("ImportOnetimeReplacement")]
        public async Task<IActionResult> ImportOnetimeReplacement(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.ImportOnetimeReplacement(file, User);
            return Ok(result);
        }

        [HttpGet, Route("DeleteOnetimeReplacement/{One_Time_Replacement_Id}/{CreatedBy}")]
        public async Task<IActionResult> DeleteOnetimeReplacement(string One_Time_Replacement_Id, string CreatedBy)
        {
            var ds = await _processRepository.DeleteOnetimeReplacement(One_Time_Replacement_Id, CreatedBy);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}


