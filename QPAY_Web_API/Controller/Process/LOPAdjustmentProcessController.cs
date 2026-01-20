using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Process;
using QPay.BAL.Repository.Process;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class LOPAdjustmentProcessController : ControllerBase
    {
        private readonly ILOPAdjustmentProcessRepository _processRepository;
        public LOPAdjustmentProcessController(ILOPAdjustmentProcessRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchLOPRequest searchRequest)
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
        public async Task<IActionResult> ExporttoExcel(ExporttoExcelxml exporttoExcelRequest)
        {
            var ds = await _processRepository.ExporttoExcel(exporttoExcelRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("DeleteLOPAdjustment/{LOP_Adjustment_Id}/{CreatedBy}")]
        public async Task<IActionResult> DeleteLOPAdjustment(string LOP_Adjustment_Id,string CreatedBy)
        {
            var ds = await _processRepository.DeleteLOPAdjustment(LOP_Adjustment_Id, CreatedBy);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ImportLOPAdjustment")]
        public async Task<IActionResult> ImportLOPAdjustment(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.ImportLOPAdjustment(file, User);
            return Ok(result);
        }


        [HttpPost]
        [Route("BulkPOCreate")]
        public async Task<IActionResult> BulkPOCreate(IFormFile file, [FromForm] string flag,
          [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.BulkPOCreate(file, flag, CreatedBy);
            return Ok(result);
        }
    }
}
