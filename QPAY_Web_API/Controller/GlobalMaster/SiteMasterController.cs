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

        [HttpGet]
        [Route("GetSiteIncharge")]
        public async Task<IActionResult> GetSiteIncharge()
        {
            var response = await _IRepository.GetSiteIncharge();
            return Ok(response);
        }


        [HttpGet]
        [Route("GetCity/{keyword}")]
        public async Task<IActionResult> GetCity(string keyword)
        {
            var response = await _IRepository.GetCity(keyword);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPFCategory")]
        public async Task<IActionResult> GetPFCategory()
        {
            var response = await _IRepository.GetPFCategory();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetLeaveCategory")]
        public async Task<IActionResult> GetLeaveCategory()
        {
            var response = await _IRepository.GetLeaveCategory();
            return Ok(response);
        }


        [HttpGet]
        [Route("GetLeaveType")]
        public async Task<IActionResult> GetLeaveType()
        {
            var response = await _IRepository.GetLeaveType();
            return Ok(response);
        }

    }
}
