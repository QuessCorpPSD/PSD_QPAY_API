using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.UI.Models.SalaryReleaseInvoice;
using System.Threading.Tasks;

namespace QPay.API.Controller.SalaryReleaseInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReissueProcessReportController : ControllerBase
    {
        private readonly IReissueProcessRepository _ReissueProcessReportRepository;

        public ReissueProcessReportController(
            IReissueProcessRepository ReissueProcessReportRepository)
        {
            _ReissueProcessReportRepository =
                ReissueProcessReportRepository;
        }
        // CONTROLLER

        [HttpPost]
        [Route("ImportReissueProcess")]
        public async Task<IActionResult> ImportReissueProcess(
            IFormFile file,
            [FromForm] string createdBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _ReissueProcessReportRepository
                .ImportReissueProcess(file, createdBy);

            return Ok(result);
        }

        [HttpPost]
        [Route("ReissueProcessReportExport")]
        public IActionResult ReissueProcessReportExport(
            [FromBody] CommonExport payload)
        {
            var ds = _ReissueProcessReportRepository
                .ReissueProcessReportExportToExcel(payload);

            var response = ResponseWrapManager
                .ResponseWrapper(ds, HttpContext);

            return Ok(response);
        }

        [HttpGet]
        [Route("ReissueProcessSearch/{fromdate}/{todate}/{status}")]
        public IActionResult ReissueProcessSearch(
            string fromdate,
            string todate,
            string status)
        {
            var ds = _ReissueProcessReportRepository
                .ReissueProcessSearch(fromdate, todate, status);

            var response = ResponseWrapManager
                .ResponseWrapper(ds, HttpContext);

            return Ok(response);
        }
    }
}