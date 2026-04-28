using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using Qzone.IRepository.SplitCulture;
using QZone.DTo.SplitCulture;
using System.Data;
namespace Qzone.API.Controllers.SplitCulture
{
    [Route("api/[controller]")]
    [ApiController]
    public class SplitCultureController : ControllerBase
    {
        private readonly ISplitCultureRepository _splitCultureRepository;

        public SplitCultureController(ISplitCultureRepository splitCultureRepository)
        {
            _splitCultureRepository = splitCultureRepository;

        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SplitCultureSearchDto request)
        {
            var ds = await _splitCultureRepository.SearchBankAdviceSplitCulture(request);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet("getmapname/{companyId}")]
        public IActionResult GetMapName(int companyId)
         {
            var ds = _splitCultureRepository
                        .GetInvoiceBankCompanywiseMapname(companyId)
                        .Result;

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] BankCultureRequestDto request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var response = await _splitCultureRepository.CreateInvoiceBankCulture(request);

            return Ok(response);

        }

        [HttpPost]
        [Route("UploadBankInvoiceSplit")]
        public async Task<IActionResult> UploadBankInvoiceSplit(IFormFile file, [FromForm] int CreatedBy)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _splitCultureRepository.UploadBankInvoiceSplit(file, CreatedBy);
            return Ok(result);
        }
    }
}