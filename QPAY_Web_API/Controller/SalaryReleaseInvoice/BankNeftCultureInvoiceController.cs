using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.UI.Models.SalaryReleaseInvoice;

namespace QPay.API.Controller.SalaryReleaseInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankNeftCultureInvoiceController : ControllerBase
    {
        private readonly IBankNeftCultureInvoiceRepository _BankNeftCultureInvoiceRepository;
        private readonly IConfiguration _configuration;

        public BankNeftCultureInvoiceController(IConfiguration configuration, IBankNeftCultureInvoiceRepository Repository)
        {
            _BankNeftCultureInvoiceRepository = Repository;
            _configuration = configuration;
        }



        [HttpGet, Route("NeftCulturesearch/{Company_Id}/{UserId}")]
        public IActionResult NeftCulturesearch(int Company_Id, int UserId)
        {

            var ds = _BankNeftCultureInvoiceRepository.NeftCulturesearch(Company_Id, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("NeftCultureExport/{Company_Id}/{UserId}")]
        public IActionResult NeftCultureExport(int Company_Id, int UserId)
        {

            var ds = _BankNeftCultureInvoiceRepository.NeftCultureExport(Company_Id, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetNeftBankculture/{Company_Id}/{Mode}/{UserId}")]
        public IActionResult GetNeftBankculture(int Company_Id, string Mode, int UserId)
        {
            var response = _BankNeftCultureInvoiceRepository.GetNeftBankculture(Company_Id, Mode, UserId);
            return Ok(response);
        }

        [HttpPost, Route("NeftCultureSave")]
        public async Task<IActionResult> NeftCultureSave([FromBody] Culturesave payload)
        {
            var catgory = await _BankNeftCultureInvoiceRepository.NeftCultureSave(payload);
            return Ok(catgory);

        }
    }
}
