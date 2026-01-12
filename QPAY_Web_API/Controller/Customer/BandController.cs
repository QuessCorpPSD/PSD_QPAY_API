using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Customer;
using QPay.UI.Models.Customer;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly IBandRepository _iBand;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public BandController(
            IBandRepository iBand, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._iBand = iBand;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllBandDetails/{companyId}")]
        public async Task<IActionResult> GetAllBandDetails(string companyId)
        {
            var response = await _iBand.GetAllBandDetails(companyId);

            return Ok(response);
        }

        [HttpPost("SaveUpdateDeleteBand")]
        public async Task<IActionResult> SaveUpdateDeleteBand([FromBody] BandRequest request)
        {
            var res = await this._iBand.SaveUpdateDeleteBand(request);
            return Ok(res);
        }
    }
}
