using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Admin;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.GlobalMaster;
using QPay.UI.Models.Admin;
using QPay.UI.Models.Invoice;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.API.Controller.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminmenuController : ControllerBase
    {

        private readonly IAdminmenuRepository _IRepository;
        public AdminmenuController(IAdminmenuRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("getroletypes")]
        public async Task<IActionResult> GetRoleTypes()
        {
            var response = await _IRepository.GetRoleTypes();
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

        [HttpGet]
        [Route("getreportingto")]
        public async Task<IActionResult> GetReportingTo()
        {
            var response = await _IRepository.GetReportingTo();
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
        [HttpGet]
        [Route("getaccesstype")]
        public async Task<IActionResult> GetAccessType()
        {
            var response = await _IRepository.GetAccessType();
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
        public async Task<IActionResult> Search(Adminmenu _params)
        {
            var response = await _IRepository.Search(_params.UserId, _params.UserName, _params.RoleId, _params.IsCheck);
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
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] adminmenurequest request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _IRepository.Create(xml, request.createdBy, request.mode, request.UserDetails.Salt);
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
