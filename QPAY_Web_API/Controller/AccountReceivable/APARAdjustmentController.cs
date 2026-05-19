using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.UI.Models.AccountReceivableMod;

namespace QPay.API.Controller.AccountReceivableCont
{
    [Route("api/[controller]")]
    [ApiController]
    public class APARAdjustmentController : ControllerBase
    {
        private readonly IAPARAdjustmentRepository _iapar;

        public APARAdjustmentController(IAPARAdjustmentRepository iapar)
        {
            _iapar = iapar;
        }

        [HttpGet("APARAdjustmentSearch/{CompanyId}/{fromdate}/{todate}")]
        public async Task<IActionResult> APARAdjustmentSearch(int CompanyId, string fromdate, string todate)
        {
            var ds = await _iapar.SearchAPARAdjustmentUpdate(CompanyId, fromdate, todate);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }


        [HttpGet("APARAdjustmentEmployeeSearch")]
        public async Task<IActionResult> APARAdjustmentEmployeeSearch([FromQuery] string APARAdjustmentNo)
        {
            var ds = await _iapar.APARAdjustmentEmployeeSearch(APARAdjustmentNo);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpPost]
        [Route("APARAdjustmentExport")]
        public async Task<IActionResult> APARAdjustmentExport([FromBody] APARAdjustmentExport payload)
        {
            var ds = await _iapar.APARAdjustmentExportToExcel(payload);

            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }

        [HttpPost]
        [Route("UploadAPARAdjustment")]
        public async Task<IActionResult> UploadAPARAdjustment(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _iapar.UploadAPARAdjustment(file, User);

            return Ok(result);
        }


        [HttpPost]
        [Route("UploadAPARAdjustmentCancel")]
        public async Task<IActionResult> UploadAPARAdjustmentCancel(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iapar.UploadAPARAdjustmentCancel(file, User);

            return Ok(result);
        }


        [HttpPost]
        [Route("EditAPARAdjustment")]
        public async Task<IActionResult> EditAPARAdjustment([FromBody] APARAdjustmentEditRequest request)
        {
            var result = await _iapar.EditAPARAdjustment(request);
            return Ok(result);
        }
    }
}