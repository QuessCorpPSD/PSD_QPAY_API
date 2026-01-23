using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.UI.Invoice;
using QPay.UI.Models;

namespace QPay.API.Controller
{
    [Authorize]
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
        public async Task<IActionResult> Search(InvoiceSearchRequest request)
        {
          var search = await this._invoiceInitiationRepository.Search(request.companyId,request.Pay_Period, request.taxtypeId);
            return Ok(search);
        }
        [HttpPost,Route("InitiationSearch")]
        public async Task<IActionResult> InitiationSearch(InitiationRequestModel initiationRequestModel)
        {
            var invoicesearch = await this._invoiceInitiationRepository.InitiationSearch(initiationRequestModel);
            return Ok(invoicesearch);
        }

        [HttpPost, Route("InitiationSearchAllot")]
        public async Task<IActionResult> InitiationSearchAllot(InvoiceDetailModel invoiceDetailModel)
      {
            var invoicesearch = await this._invoiceInitiationRepository.InitiationSearchAllot(invoiceDetailModel);
            return Ok(invoicesearch);
        }

        [HttpPost]
        [Route("GetAllInvoiceAllotDetails")]
        public async Task<IActionResult> GetAllInvoiceAllotDetails(InvoiceDetailModel invoiceDetailModel)
        {
            var ds = await this._invoiceInitiationRepository.GetAllInvoiceAllotDetails(invoiceDetailModel);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpPost, Route("InitiationSearchExport")]
        public async Task<IActionResult> InitiationSearchExport(InitiationRequestModel initiationRequestModel)
        {
            var invoicesearch = await this._invoiceInitiationRepository.InitiationSearchExport(initiationRequestModel);
            return Ok(invoicesearch);
        }
        [HttpPost, Route("InvoiceInitiate")]
        public async Task<IActionResult> InvoiceInitiate(InvoiceInitiateRequestModel request)
        {
            string xml = XmlHelper.SerializeObjectToXml(request.invoiceInitiations, "Main");
            var result = await _invoiceInitiationRepository.InvoiceInitiate(
          request.TaxTypeId,
          xml,
          "Add",          // or make this request.Mode if dynamic
          request.CreatedBy
      );

            return Ok(result);
        }
        [HttpPost, Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(InvoiceSearchRequest requestModel)
        {
            try
            {
     
                var InvoiceInitiateExcel = await _invoiceInitiationRepository.ExportToExcel(requestModel.companyId,requestModel.Pay_Period, requestModel.taxtypeId);

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
