using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IBankNonInvoice;
using QPay.UI.Models.BankNonInvoice;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAdviceSplitCultureController : ControllerBase
    {

        private readonly IBankAdviceSplitCultureRepository _irepo;
        public BankAdviceSplitCultureController(IBankAdviceSplitCultureRepository irepo)
        {
            this._irepo = irepo;
        }
        [HttpGet("GetVendorname")]
        public async Task<IActionResult> GetVendorname(
    [FromQuery] string filter = "",
    [FromQuery] int Company_id = 0)
        {
            var ds = await _irepo.GetVendorname(filter, Company_id);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }

        [HttpGet]
        [Route("GetSearchEditdata/{Company_id}/{Vendor_id}/{Bank_Culture_Id}/{Mode}")]
        public async Task<IActionResult> GetSearchEditdata(int Company_id, int Vendor_id, int Bank_Culture_Id, string Mode)
        {
            var response = await _irepo.GetSearchEditdata(Company_id, Vendor_id, Bank_Culture_Id, Mode);

            if (response.Tables.Count > 0 &&
                response.Tables[0].Rows.Count > 0)
            {
                var outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);

                return Ok(outputResponse);
            }
            else
            {
                return Ok(new
                {
                    StatusCode = "400",
                    Message = "No records found"
                });
            }
        }
        [HttpPost]
        [Route("BankSplitCultureupload")]
        public async Task<IActionResult>
            BankSplitCultureupload(
                IFormFile file,
                [FromForm] int CreatedBy)
        {
            if (file == null || file.Length == 0)
            {
                return Ok(
                    new BankAdviceSplitCultureUploadResponse
                    {
                        response = "File is missing."
                    });
            }

            var result =
                await _irepo.BankSplitCultureupload(
                    file,
                    CreatedBy);

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateBankCulture")]
        [HttpPost("CreateBankCulture")]
        public async Task<IActionResult> CreateBankCulture(
    [FromBody] CreateBankCultureRequest request)
        {
            var res =
                await _irepo.CreateBankCulture(request);

            return Ok(res);
        }

        [HttpGet]
        [Route("Getgroupname/{Company_id}/{Client_id}")]
        public async Task<IActionResult> Getgroupname(
    int Company_id,
    int Client_id)
        {
            var ds = await _irepo.Getgroupname(
                Company_id,
                Client_id);

            var payload =
                ResponseWrapManager.ResponseWrapper(
                    ds,
                    HttpContext);

            return Ok(payload);
        }
    }
}
