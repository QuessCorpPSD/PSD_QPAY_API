using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.Models.Customer;
using QPay.UI.Models.GlobalMaster;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillMappingController : ControllerBase
    {
        private readonly ISkillMappingRepository _IRepository;
        public SkillMappingController(ISkillMappingRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search(int? companyId, int? siteId)
        {
            var response = await _IRepository.Search(companyId, siteId);
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

        [HttpPost]
        [Route("CreateUpdateSkillMapping")]
        public async Task<IActionResult> CreateUpdateSkillMapping(SkillMappingRequest request)
        {
            var response = await _IRepository.CreateUpdateSkillMapping(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("DeleteSkillMapping/{companyId}/{siteId}/{skillCategory}/{userId}")]
        public async Task<IActionResult> DeleteSkillMapping(int companyId, int siteId, string skillCategory, int userId)
        {
            var response = await _IRepository.DeleteSkillMapping(companyId, siteId, skillCategory, userId);
            return Ok(response);
        }

    }
}
