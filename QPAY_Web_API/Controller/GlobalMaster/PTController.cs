using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using static QPay.UI.Models.GlobalMaster.ESIClass;
using static QPay.UI.Models.GlobalMaster.PTClass;


namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class PTController : ControllerBase
    {
        private readonly IPTRepository _IRepository;
        public PTController(IPTRepository IRepository)
        {
            this._IRepository = IRepository;
        }


        [HttpGet, Route("PTType")]
        public async Task<IActionResult> PTType()
        {
            var response = await _IRepository.PTType();
            return Ok(response);
        }

        [HttpGet, Route("PTCategory")]
        public async Task<IActionResult> PTCategory()
        {
            var response = await _IRepository.PTCategory();
            return Ok(response);
        }

        [HttpGet, Route("PTCircle/{StateId}")]
        public async Task<IActionResult> PTCircle(int StateId)
        {
            var response = await _IRepository.PTCircle(StateId);
            return Ok(response);
        }


        [HttpPost, Route("PTSearch")]
        public async Task<IActionResult> PTSearch(PTSearchRequest request)
        {
            var ds = await _IRepository.PTSearch(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("PTExporttoExcel")]
        public async Task<IActionResult> PTExporttoExcel(PTSearchRequest request)
        {
            var ds = await _IRepository.PTExporttoExcel(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CreateUpdateDeletePT")]
        public async Task<IActionResult> CreateUpdateDeletePT(PTRequest request)
        {
            var result = await _IRepository.CreateUpdateDeletePT(request);
            return Ok(result);
        }

    }
}
