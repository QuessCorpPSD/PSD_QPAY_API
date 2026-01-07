using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.Models.GlobalMaster;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _IRepository;
        public CityController(ICityRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("GetAllCity")]
        public async Task<IActionResult> GetAllCity(string? cityName, int? stateId, int? cityId)
        {
            var response = await _IRepository.GetAllCity(cityName, stateId, cityId);
            return Ok(response);
        }

        [HttpPost, Route("AddCity")]
        public async Task<IActionResult> AddCity([FromBody] CityAddRequest request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _IRepository.Create(xml, request.mode, request.createdBy);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetAllCircle/{stateId}")]
        public async Task<IActionResult> GetAllCircle(int stateId)
        {
            var response = await _IRepository.GetAllCircle(stateId);
            return Ok(response);
        }
    }
}
