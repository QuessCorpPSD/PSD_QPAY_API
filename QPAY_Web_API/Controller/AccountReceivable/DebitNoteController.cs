using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.DebitNote;
using QPay.UI.DebitNote;

namespace QPay.API.Controller.DebitNote
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitNoteController : ControllerBase
    {
        private readonly IDebitNoteRepository _DebitNoteRepository;

        public DebitNoteController(
            IDebitNoteRepository DebitNoteRepository)
        {
            _DebitNoteRepository = DebitNoteRepository;
        }

        [HttpGet]
        [Route("Search/{ClientName}/{EmpCode}/{FromDate}/{ToDate}")]
        public async Task<IActionResult> Search(
    string ClientName,
    string EmpCode,
    string FromDate,
    string ToDate)
        {
            var ds = await _DebitNoteRepository.Search(
                ClientName,
                EmpCode,
                FromDate,
                ToDate
            );

            var payload =
                ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpPost]
        [Route("DebitNoteExport")]
        public async Task<IActionResult> DebitNoteExportToExcel(
            [FromBody] DebitNoteExport payload)
        {
            var ds =
                await _DebitNoteRepository.DebitNoteExportToExcel(
                    payload
                );

            var res =
                ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }
    }
}