using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.CreditNoteMatrix;
using QPay.UI.CreditNoteMatrix;

namespace QPay.API.Controller.CreditNoteMatrix
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditNoteMatrixController : ControllerBase
    {
        private readonly ICreditNoteMatrixRepository _creditNoteMatrixRepository;

        public CreditNoteMatrixController(ICreditNoteMatrixRepository creditNoteMatrixRepository)
        {
            _creditNoteMatrixRepository = creditNoteMatrixRepository;
        }

        [HttpGet, Route("Search")]
        public IActionResult Search()
        {
            var ds = _creditNoteMatrixRepository.Search("SEARCH", null, null);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create(CreditNoteMatrixRequest request)
        {
            var category = await _creditNoteMatrixRepository.Create(request);

            return Ok(category);
        }

        [HttpPut, Route("Update")]
        public async Task<IActionResult> Update(CreditNoteMatrixRequest request)
        {
            var category = await _creditNoteMatrixRepository.Update(request);

            return Ok(category);
        }

        [HttpDelete, Route("Delete")]
        public async Task<IActionResult> Delete(CreditNoteMatrixRequest request)
        {
            var category = await _creditNoteMatrixRepository.Delete(request);

            return Ok(category);
        }

        [HttpGet, Route("ExportToExcel")]
        public IActionResult ExportToExcel()
        {
            var ds = _creditNoteMatrixRepository.ExportToExcel();

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpGet, Route("GetCommonDropDownList/{Flag}/{UserId}")]
        public IActionResult GetCommonDropDownList(string Flag, int UserId)
        
        {
            var response = _creditNoteMatrixRepository.GetCommonDropDownList(Flag, UserId);

            return Ok(response);
        }
    }
}