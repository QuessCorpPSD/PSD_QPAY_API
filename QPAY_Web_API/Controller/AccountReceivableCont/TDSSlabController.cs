using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IAccountReceivable;
using static QPay.UI.Models.AccountReceivableMod.TDSSlabModels;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.API.Controller.AccountReceivableCont
{
    [Route("api/[controller]")]
    [ApiController]
    public class TDSSlabController : ControllerBase
    {
        private readonly ITDSSlab _repo;
        public TDSSlabController(ITDSSlab repo)
        {
            _repo = repo;
        }

        [HttpGet("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear(int? id)
        {
            var ds = await _repo.GetFinancialYear(id);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpGet]
        [Route("Search/{CompanyId}/{FinancialYearId}")]
        public async Task<IActionResult> Search(int? CompanyId, int? FinancialYearId)
        {
            var ds = await _repo.Search(CompanyId, FinancialYearId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel([FromBody] CommonExport2 payload)
        {
            var ds = await _repo.ExportToExcel(payload);
            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(res);
        }

        [HttpPost]
        [Route("UploadTDSSlab")]
        public async Task<IActionResult> UploadTDSSlab(IFormFile file, [FromForm] string createdBy)
        {
            if (file == null || file.Length == 0)
                return Ok(new { response = "File is missing." });

            var result = await _repo.UploadTDSSlab(file, createdBy);
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadLTDSSlab")]
        public async Task<IActionResult> UploadLTDSSlab(IFormFile file, [FromForm] int userId)
        {
            if (file == null || file.Length == 0)
                return Ok(new { response = "File is missing." });

            var result = await _repo.UploadLTDSSlab(file, userId);
            return Ok(result);
        }

        [HttpPost]
        [Route("TdsSlabCreate")]
        public async Task<IActionResult> TdsSlabCreate([FromBody] TdsSlabSaveRequest request)
        {
            var result = await _repo.TdsSlabCreate(request);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCompanyNameByCode/{companyCode}")]
        public async Task<IActionResult> GetCompanyNameByCode(string companyCode)
        {
            var result = await _repo.GetCompanyNameByCode(companyCode);
            return Ok(result);
        }
    }
}