using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
using QPay.BAL.IRepository.ARKnockOff;
using QPay.UI.Models.ARKnockOff;


namespace QPay.API.Controller.ARKnockOff
{
    [Route("api/[controller]")]
    [ApiController]
    public class ARKnockOffController : ControllerBase
    {
        private readonly IARKnockOffRepository _IRepository;
        public ARKnockOffController(IARKnockOffRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpPost, Route("SaveARDetails")]
        public async Task<IActionResult> SaveARDetails(ARKnockOffclass request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _IRepository.SaveARDetails(xml);
            return Ok(response);
        }
    }

}
