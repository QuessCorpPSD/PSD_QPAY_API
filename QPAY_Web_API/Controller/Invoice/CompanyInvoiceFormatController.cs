using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Invoice;
using QPay.UI.Models.Invoice;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyInvoiceFormatController : ControllerBase
    {
        private readonly ICompanyInvoiceFormatRepository _icompany;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public CompanyInvoiceFormatController(
            ICompanyInvoiceFormatRepository icompany, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._icompany = icompany;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllCompanyInvoiceFormat/{userId}")]
        public async Task<IActionResult> GetAllCompanyInvoiceFormat(int userId)
        {
            var response = await _icompany.GetAllCompanyInvoiceFormat(userId);

            return Ok(response);
        }
        [HttpGet, Route("GetAllInvoiceType")]
        public async Task<IActionResult> GetAllInvoiceType()
        {
            var response = await _icompany.GetAllInvoiceType();

            return Ok(response);
        }
        [HttpGet, Route("GetAllInvoiceFormat")]
        public async Task<IActionResult> GetAllInvoiceFormat()
        {
            var response = await _icompany.GetAllInvoiceFormat();

            return Ok(response);
        }

        [HttpPost, Route("AddInvoiceFormat")]
        public async Task<IActionResult> AddInvoiceFormat([FromBody] InvoiceFormatAdd request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _icompany.Create(request);
            return Ok(response);
        }
    }
}
