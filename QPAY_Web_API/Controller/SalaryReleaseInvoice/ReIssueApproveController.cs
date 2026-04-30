using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.UI.Models.SalaryReleaseInvoice;

namespace QPay.API.Controller.SalaryReleaseInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReIssueApproveController : ControllerBase
    {
        private readonly IReIssueApprove _repo;

        public ReIssueApproveController(IReIssueApprove repo)
        {
            _repo = repo;
        }
        // CONTROLLER

        [HttpGet]
        [Route("SearchReIssueApprove/{CompanyId}/{PayPeriodId}/{ReIssueTypes}/{FromDate}/{ToDate}/{param}/{Status}")]
        public async Task<IActionResult> SearchReIssueApprove(
            int CompanyId,
            int PayPeriodId,
            string ReIssueTypes,
            string FromDate,
            string ToDate,
            int param,
            string Status)
        {
            var ds = await _repo.SearchReIssueApprove(CompanyId,PayPeriodId,
          ReIssueTypes,
                FromDate,
                ToDate,
                param,
                Status);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpGet("GetDropdown/{flag}")]
        public async Task<IActionResult> GetDropdown(string flag)
        {
            var ds = await _repo.GetDropdown(flag);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpPost("ReissueProcessApproveBulkUpload")]
        public async Task<IActionResult> ReissueProcessApproveBulkUpload(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _repo.ReissueProcessApproveBulkUpload(file, User);
            return Ok(result);
        }
        [HttpPost]
        [Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel([FromBody] ReIssueApproveExportRequest payload)
        {
            var ds = await _repo.ExportToExcel(payload);
            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(res);
        }
        // CONTROLLER

        [HttpPost]
        [Route("CreateReIssueApproveReject")]
        public async Task<IActionResult> CreateReIssueApproveReject(
            [FromBody] ReIssueApproveRejectRequest request)
        {
            var result =
                await _repo.CreateReIssueApproveReject(request);

            return Ok(result);
        }
    }
}
