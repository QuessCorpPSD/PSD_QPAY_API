using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.Models.GlobalMaster;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class GstController : ControllerBase
    {
        private readonly IGstRepository _processRepository;
        public GstController(IGstRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpGet, Route("SearchDetails/{UserId}")]
        public async Task<IActionResult> SearchDetails(string UserId)
        {
            var ds = await _processRepository.SearchDetails(UserId);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetGSTtype")]
        public async Task<IActionResult> GetGSTtype()
        {
            var ds = await _processRepository.GetGSTtype();
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("ExporttoExcel/{UserId}")]
        public async Task<IActionResult> ExporttoExcel(string UserId)
        {
            var ds = await _processRepository.ExporttoExcel(UserId);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(GstRequest createRequest)
        {
            var result = await _processRepository.Create(createRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(GstRequest createRequest)
        {
            var result = await _processRepository.Edit(createRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("Delete/{GstMasterId}/{UserId}")]
        public async Task<IActionResult> Delete(int GstMasterId, int UserId)
        {
            var result = await _processRepository.Delete(GstMasterId, UserId);
            return Ok(result);
        }
    }
}
