using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.Models.GlobalMaster;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateRepository _IRepository;
        public StateController(IStateRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("GetAllState")]
        public async Task<IActionResult> GetAllState(string? stateName, int? regionId, int? stateId)
        {
            var response = await _IRepository.GetAllState(stateName, regionId, stateId);
            return Ok(response);
        }

        [HttpPost, Route("AddState")]
        public async Task<IActionResult> AddState([FromBody] StateAddRequest request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _IRepository.Create(xml, request.mode, request.createdBy);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetAllRegion")]
        public async Task<IActionResult> GetAllRegion()
        {
            var response = await _IRepository.GetAllRegion();
            return Ok(response);
        }

    }
}
