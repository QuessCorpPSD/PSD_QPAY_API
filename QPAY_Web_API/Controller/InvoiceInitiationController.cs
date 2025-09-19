using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.LoggerService;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.UI.Models;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]




    [ApiController]
    public class InvoiceInitiationController : ControllerBase
    {
        private readonly IInvoiceInitiationRepository _invoiceInitiationRepository;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;
        private readonly HttpClient _client;
        public InvoiceInitiationController(ILoggerManager logger, HttpClient client, IInvoiceInitiationRepository invoiceInitiationRepository, IConfiguration configuration)
        {
            this._invoiceInitiationRepository = invoiceInitiationRepository;
            _configuration = configuration;
            this._logger = logger;
            this._client = client;
        }
        [HttpGet, Route("GetTaxTypes")]
        public async Task<IActionResult> GetTaxTypes() =>
            Ok(await this._invoiceInitiationRepository.GetTaxTypes("GetTaxTypes"));
        [HttpPost, Route("Search")]
        public async Task<IActionResult> Search(InvoiceInitiationUI request)
        {
          var search = await this._invoiceInitiationRepository.Search(request.Company_Id,request.Pay_Period,request.TaxTypeId);
            return Ok(search);
        }
        [HttpPost, Route("InvoiceInitiate")]
        public async Task<IActionResult> InvoiceInitiate(InvoiceInitiationUI request)
        {
            string xml = XmlHelper.SerializeObjectToXml(request, "Main");
            var result = await _invoiceInitiationRepository.InvoiceInitiate(
          request.TaxTypeId,
          xml,
          "Add",          // or make this request.Mode if dynamic
          request.CreatedBy
      );

            return Ok(result);
        }
        [HttpPost, Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(InvoiceInitiationUI requestModel)
        {
            try
            {
     
                var InvoiceInitiateExcel = await _invoiceInitiationRepository.ExportToExcel(requestModel.Company_Id,requestModel.Pay_Period,requestModel.TaxTypeId);

                if (string.IsNullOrEmpty(InvoiceInitiateExcel.File))
                {
                    return NotFound("No data available to export.");
                }

                return Ok(InvoiceInitiateExcel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while exporting the file: {ex.Message}");
            }
        }

    }
}
