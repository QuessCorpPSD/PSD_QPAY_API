using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.BankNonInvoice;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesalaryreleaseController : ControllerBase
    {
        private readonly IEmployeeSalaryReleaseRepository _iesr;

        public EmployeesalaryreleaseController(
            IEmployeeSalaryReleaseRepository iesr)
        {
            _iesr = iesr;
        }

        [HttpGet]
        [Route("SearchEmployeeSalaryRelease/{CompanyId}/{PayPeriodId}")]
        public async Task<IActionResult> SearchEmployeeSalaryRelease(
           int CompanyId,
           int PayPeriodId)
        {
            var ds = await _iesr.SearchEmployeeSalaryRelease(CompanyId, PayPeriodId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpPost]
        [Route("EmployeeSalaryReleaseExport")]
        public async Task<IActionResult> EmployeeSalaryReleaseExport(
            [FromBody] CommonExport payload)
        {
            var ds = await _iesr.ExportToExcel(payload);

            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }

        [HttpPost]
        [Route("UploadEmployeeSalaryRelease")]
        public async Task<IActionResult> UploadEmployeeSalaryRelease(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iesr.UploadEmployeeSalaryRelease(file, User);

            return Ok(result);
        }
    }
}
