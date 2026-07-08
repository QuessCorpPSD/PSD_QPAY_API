using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.BankNonInvoice;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReleaseholdemployeesalaryController : ControllerBase
    {
        private readonly IReleaseholdemployeesalaryRepository _irelease;
        public ReleaseholdemployeesalaryController(
          IReleaseholdemployeesalaryRepository _irelease)
        {
            this._irelease = _irelease;
        }
        [HttpGet]
        [Route("search/{Company_Id}/{Pay_Period_Id}/{Employee_Id}")]
        public async Task<IActionResult> search(int Company_Id, int Pay_Period_Id, int? Employee_Id)
        {
            var ds = await _irelease.search(Company_Id, Pay_Period_Id, Employee_Id);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ReleaseHoldSalaryExport")]
        public async Task<IActionResult> ReleaseHoldSalaryExport([FromBody] CommonExports payload)
        {
            var ds = await _irelease.ExportToExcel(payload);   // <-- await is required
            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(res);
        }

        [HttpPost]
        [Route("UploadReleaseholdsalary")]
        public async Task<IActionResult> UploadReleaseholdsalary(IFormFile file, [FromForm] string CreatedBy, [FromForm] string action)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _irelease.UploadReleaseholdsalary(file, CreatedBy, action);
            return Ok(result);
        }

        [HttpPost("SaveReleaseHoldSalary")]
        public async Task<IActionResult> SaveReleaseHoldSalary([FromBody] ReleaseHoldSalaryRequest request)
        {
            var res = await _irelease.SaveReleaseHoldSalary(request);
            return Ok(res);
        }
    }
}
