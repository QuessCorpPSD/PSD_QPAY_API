using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.BankNonInvoice;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldEmpSalaryController : ControllerBase
    {
        private readonly IHoldEmpSalaryRepository _ihold;

        public HoldEmpSalaryController(IHoldEmpSalaryRepository ihold)
        {
            _ihold = ihold;
        }
        [HttpGet]
        [Route("GetSalaryHoldType")]
        public async Task<IActionResult> GetSalaryHoldType()
        {
            var ds = await _ihold.GetSalaryHoldType();

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpGet]
        [Route("SearchHoldEmpSalary/{CompanyId}/{PayPeriodId}/{Status}")]
        public async Task<IActionResult> SearchHoldEmpSalary(
            int CompanyId,
            int PayPeriodId,
            string Status)
        {
            var ds = await _ihold.SearchHoldEmpSalary(CompanyId, PayPeriodId, Status);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpPost]
        [Route("HoldEmpSalaryExport")]
        public async Task<IActionResult> HoldEmpSalaryExport(
            [FromBody] HoldEmpSalaryExportRequest payload)
        {
            var ds = await _ihold.ExportToExcel(payload);

            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }

        [HttpPost]
        [Route("UploadHoldEmpSalary")]
        public async Task<IActionResult> UploadHoldEmpSalary([FromForm] IFormFile file,[FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _ihold.UploadHoldEmpSalary(file, User);

            return Ok(result);
        }
    }
}
