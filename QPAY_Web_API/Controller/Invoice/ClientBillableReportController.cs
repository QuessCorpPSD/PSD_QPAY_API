using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.Repository.Invoice;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientBillableReportController : ControllerBase
    {
        private readonly IClientBillableReportRepository _IRepository;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;
        private readonly HttpClient _client;
        private readonly ICommonRepository _icommon;

        public ClientBillableReportController(ILoggerManager logger, HttpClient client, IClientBillableReportRepository gstinvoiceRepository
            , IConfiguration configuration, ICommonRepository icommon)
        {
            this._IRepository = gstinvoiceRepository;
            _configuration = configuration;
            this._logger = logger;
            this._client = client;
            this._icommon = icommon;
        }

        [HttpGet]
        [Route("Search/{entityId}/{startDate}/{endDate}")]
        public async Task<IActionResult> Search(int? entityId, string? startDate, string? endDate)
        {
            var response = await _IRepository.Search(entityId, startDate, endDate);
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
