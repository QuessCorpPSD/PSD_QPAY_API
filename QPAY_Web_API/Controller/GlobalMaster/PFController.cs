using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using static QPay.UI.Models.GlobalMaster.PFClass;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class PFController : ControllerBase
    {
        private readonly IPFRepository _IRepository;
        public PFController(IPFRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet, Route("PFPayCodes")]
        public async Task<IActionResult> PFPayCodes()
        {
            var response = await _IRepository.PFPayCodes();
            return Ok(response);
        }


        [HttpGet, Route("PFCapType")]
        public async Task<IActionResult> PFCapType()
        {
            var response = await _IRepository.PFCapType();
            return Ok(response);
        }

        [HttpGet, Route("PFSearch/{CapType}")]
        public async Task<IActionResult> PFSearch(string CapType)
        {
            var ds = await _IRepository.PFSearch(CapType);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("PFExporttoExcel/{CapType}")]
        public async Task<IActionResult> PFExporttoExcel(string CapType)
        {
            var ds = await _IRepository.PFExporttoExcel(CapType);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CreateUpdatePF")]
        public async Task<IActionResult> CreateUpdatePF(PFRequest request)
        {
            var result = await _IRepository.CreateUpdatePF(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("DeletePF")]
        public async Task<IActionResult> DeletePF(PFDeleteRequest request)
        {
            var result = await _IRepository.DeletePF(request);
            return Ok(result);
        }
    }
}
