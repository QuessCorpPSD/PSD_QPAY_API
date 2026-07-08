using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.BankNonInvoice;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankNEFTcultureNonInvoiceController : ControllerBase
    {
        private readonly IBankNEFTcultureNonInvoice _IRepository;
        public BankNEFTcultureNonInvoiceController(IBankNEFTcultureNonInvoice IRepository)
        {
            this._IRepository = IRepository;
        }
        [HttpGet]
        [Route("Getbankname/{Company_id}/{mode}")]
        public async Task<IActionResult> Getbankname(int? Company_id, string mode)
        {
            var response = await _IRepository.Getbankname(Company_id, mode);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpGet]
        [Route("GetSearchdata/{Company_id}/{Bank_Culture_Id}/{Mode}")]
        public async Task<IActionResult> GetSearchdata(int Company_id, int Bank_Culture_Id, string Mode)
        {
            var response = await _IRepository.GetSearchdata(Company_id, Bank_Culture_Id, Mode);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }
        [HttpPost, Route("NeftCultureSave")]
        public async Task<IActionResult> NeftCultureSave([FromBody] BankCulturesave payload)
        {
            var catgory = await _IRepository.NeftCultureSave(payload);
            return Ok(catgory);

        }

        [HttpGet]
        [Route("Getpayperiod")]
        public async Task<IActionResult> Getpayperiod()
        {
            var response = await _IRepository.Getpayperiod();
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpGet]
        [Route("exporttoexcel/{payperiod}")]
        public async Task<IActionResult> exporttoexcel(string payperiod)
        {
            var response = await _IRepository.ExportToExcel(payperiod);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }
    }
}
