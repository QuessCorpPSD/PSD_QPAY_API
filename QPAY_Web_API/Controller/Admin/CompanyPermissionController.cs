using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Admin;
using QPay.UI.Models.Admin;

namespace QPay.API.Controller.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyPermissionController : ControllerBase
    {

        private readonly ICompanyPermissionRepository _IRepository;
        public CompanyPermissionController(ICompanyPermissionRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("GetEntityZoneEmployeeId")]
        public async Task<IActionResult> GetEntityZoneEmployeeId()
        {
            var response = await _IRepository.GetEntityZoneEmployeeId();
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
        [Route("LoadCompany")]
        public async Task<IActionResult> LoadCompany(LoadCompany _params)
        {
            var response = await _IRepository.LoadCompany( _params.BusinessUnitNameId, _params.BusinessZonenName);
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
        [Route("Search")]
        public async Task<IActionResult> Search(CompanyPermission _params)
        {
            var response = await _IRepository.Search(_params.Userid,  _params.Businessunitnameid, _params.CompanyPermissionId);
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
        [Route("Editdetails")]
        public async Task<IActionResult> Editdetails(EditPermissiondetails _params)
        {
            var response = await _IRepository.Editdetails(_params.Userid, _params.Businessunitnameid, _params.CompanyPermissionId);
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
        [Route("CreateUpdateDelete")]
        public async Task<IActionResult> CreateUpdateDelete([FromBody] CompanyPermissionRequest request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _IRepository.CreateUpdateDelete(xml, request.createdBy, request.mode);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

    }
}
