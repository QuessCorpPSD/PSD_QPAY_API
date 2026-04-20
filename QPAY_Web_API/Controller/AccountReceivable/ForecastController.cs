using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.UI.Models.AccountReceivableMod;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.API.Controller.AccountReceivable
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private readonly IForecastRepository _forecast;

        public ForecastController(IForecastRepository forecast)
        {
            _forecast = forecast;
        }
        [HttpGet]
        [Route("Search/{CompanyId}/{PayPeriod}/{Mode}")]
        public async Task<IActionResult> Search(int? CompanyId, string PayPeriod, string Mode)
        {
            var ds = await _forecast.Search(CompanyId, PayPeriod, Mode);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ForecastExport")]
        public async Task<IActionResult> ForecastExportToExcel([FromBody] ForecastExport payload)
        {
            var ds = await _forecast.ExportToExcel(payload);
            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetSBU")]
        public async Task<IActionResult> GetSBU()
        {
            var ds = await _forecast.GetSBU();
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetRegion")]
        public async Task<IActionResult> GetRegion()
        {
            var ds = await _forecast.GetRegion();
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpGet]
        [Route("GetInvoiceNumber/{CompanyId}/{PayPeriodId}")]
        public async Task<IActionResult> GetInvoiceNumber(int? CompanyId, int? PayPeriodId)
        {
            var ds = await _forecast.GetInvoiceNumber(CompanyId, PayPeriodId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost("SaveUpdateDeleteForecast")]
        public async Task<IActionResult> SaveUpdateDeleteForecast([FromBody] ForecastRequest request)
        {
            var res = await _forecast.SaveUpdateDeleteForecast(request);
            return Ok(res);
        }

        [HttpPost]
        [Route("UploadForecast")]
        public async Task<IActionResult> UploadForecast(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _forecast.UploadForecast(file, User);

            return Ok(result);
        }




    }
}