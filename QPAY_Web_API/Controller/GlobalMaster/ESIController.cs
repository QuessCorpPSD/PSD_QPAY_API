using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using static QPay.UI.Models.GlobalMaster.ESIClass;



namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ESIController : ControllerBase
    {
        private readonly IESIRepository _IRepository;
        public ESIController(IESIRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        // ESI Block

        [HttpGet, Route("GetBlocks")]
        public async Task<IActionResult> GetBlocks()
        {
            var response = await _IRepository.GetBlocks();
            return Ok(response);
        }

        [HttpGet, Route("GetMonths")]
        public async Task<IActionResult> GetMonths()
        {
            var response = await _IRepository.GetMonths();
            return Ok(response);
        }
       

        [HttpGet]
        [Route("GetEsiblockSearch")]
        public async Task<IActionResult> GetEsiblockSearch(string? EffectiveDate)
        {
            var ds = await _IRepository.GetEsiblockSearch(EffectiveDate);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetEsiblockExporttoExcel")]
        public async Task<IActionResult> GetEsiblockExporttoExcel(string? EffectiveDate)
        {
            var ds = await _IRepository.GetEsiblockExporttoExcel(EffectiveDate);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CreateUpdateDeleteEsiblock")]
        public async Task<IActionResult> CreateUpdateDeleteEsiblock(EsiblockRequest request)
        {
            var result = await _IRepository.CreateUpdateDeleteEsiblock(request);
            return Ok(result);
        }

        // ESI Location Slab

        [HttpGet, Route("GetPaycodes")]
        public async Task<IActionResult> GetPaycodes()
        {
            var response = await _IRepository.GetPaycodes();
            return Ok(response);
        }

        [HttpGet, Route("GetStates")]
        public async Task<IActionResult> GetStates()
        {
            var response = await _IRepository.GetStates();
            return Ok(response);
        }

        [HttpGet, Route("GetCity/{StateId}")]
        public async Task<IActionResult> GetCity(int StateId)
        {
            var response = await _IRepository.GetCity(StateId);
            return Ok(response);
        }

        [HttpGet, Route("GetCriteriaType")]
        public async Task<IActionResult> GetCriteriaType()
        {
            var response = await _IRepository.GetCriteriaType();
            return Ok(response);
        }

        [HttpPost]
        [Route("GetEsiLocationSlabSearch")]
        public async Task<IActionResult> GetEsiLocationSlabSearch(EsiLocationSlabSearchRequest request)
        {
            var ds = await _IRepository.GetEsiLocationSlabSearch(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("GetEsiLocationSlabExporttoExcel")]
        public async Task<IActionResult> GetEsiLocationSlabExporttoExcel(EsiLocationSlabSearchRequest request)
        {
            var ds = await _IRepository.GetEsiLocationSlabExporttoExcel(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CreateUpdateDeleteEsiLocationSlab")]
        public async Task<IActionResult> CreateUpdateDeleteEsiLocationSlab(EsiLocationSlabRequest request)
        {
            var result = await _IRepository.CreateUpdateDeleteEsiLocationSlab(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetEsiSlabSearch")]
        public async Task<IActionResult> GetEsiSlabSearch(EsiSlabSearchRequest request)
        {
            var ds = await _IRepository.GetEsiSlabSearch(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("GetEsiSlabExporttoExcel")]
        public async Task<IActionResult> GetEsiSlabExporttoExcel(EsiSlabSearchRequest request)
        {
            var ds = await _IRepository.GetEsiSlabExporttoExcel(request);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CreateUpdateDeleteEsiSlab")]
        public async Task<IActionResult> CreateUpdateDeleteEsiSlab(EsiSlabRequest request)
        {
            var result = await _IRepository.CreateUpdateDeleteEsiSlab(request);
            return Ok(result);
        }

    }
}
