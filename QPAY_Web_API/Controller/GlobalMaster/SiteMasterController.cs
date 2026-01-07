using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.Models.GlobalMaster;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteMasterController : ControllerBase
    {
        private readonly ISiteMasterRepository _IRepository;
        public SiteMasterController(ISiteMasterRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search(int? companyId, int? groupId)
        {
            var response = await _IRepository.Search(companyId, groupId);
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
        [Route("GetQuessLegalEntity")]
        public async Task<IActionResult> GetQuessLegalEntity()
        {
            var response = await _IRepository.GetQuessLegalEntity();
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

        //[HttpPost]
        //[Route("Create")]
        //public async Task<IActionResult> Create([FromBody] EntityRequest request)
        //{
        //    var response = await _IRepository.Create(request);
        //    if (response.Tables[0].Rows.Count > 0)
        //    {
        //        string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
        //        if (!(message.Contains("Successfully")))
        //        {
        //            return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
        //        }
        //        else
        //        {
        //            var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
        //            return Ok(_outputResponse);
        //        }
        //    }
        //    else
        //    {
        //        return Ok(new { StatusCode = "400", Message = "Details are not saved" });
        //    }
        //}

        [HttpGet]
        [Route("ExporttoExcel")]
        public async Task<IActionResult> ExporttoExcel(int? companyId, int? groupId)
        {
            var response = await _IRepository.ExporttoExcel(companyId, groupId);
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
        [Route("GetPortalPayslipFormat")]
        public async Task<IActionResult> GetPortalPayslipFormat()
        {
            var response = await _IRepository.GetPortalPayslipFormat();
            return Ok(response);
        }

        [HttpPost]
        [Route("CreateUpdateSiteMaster")]
        public async Task<IActionResult> CreateUpdateSiteMaster(CreateUpdateSitemasterRequest request)
        {
            var response = await _IRepository.CreateUpdateSiteMaster(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("UploadSiteMaster")]
        public async Task<IActionResult> UploadSiteMaster(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.UploadSiteMaster(file, User);
            return Ok(result);
        }
    }
}
