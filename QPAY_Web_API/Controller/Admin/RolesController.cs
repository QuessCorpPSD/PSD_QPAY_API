using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Admin;
using QPay.UI.Models.Admin;

namespace QPay.API.Controller.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesRepository _IRepository;
        public RolesController(IRolesRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpPost]
        [Route("RolesCRUD")]
        public async Task<IActionResult> RolesCRUD(Roles _params)
        {
            var response = await _IRepository.RolesCRUD(_params);
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
