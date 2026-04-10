using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlTypes;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Invoice;
using QPay.UI.Models.Invoice;
using QPay.UI.Models;
using System.ComponentModel.DataAnnotations;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class SEZRepositoryApprovalController : ControllerBase
    {
        private readonly ISEZRepositoryService _iSEZ;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public SEZRepositoryApprovalController(
            ISEZRepositoryService iSEZ, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._iSEZ = iSEZ;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("Search/{companyId}/{payPeriodId}/{InvoiceNumbers}/{Year}")]
        public async Task<IActionResult> Search(int companyId, int payPeriodId, string? InvoiceNumbers, int Year)
        {
            var stauts = await _iSEZ.Search(companyId, payPeriodId, InvoiceNumbers, Year);
            return Ok(stauts);
        }

        [HttpGet, Route("GetUploadedFile/{invoice_Id}")]
        public IActionResult GetUploadedFile(int invoice_Id)
        {
            try
            {
                var filejson = _iSEZ.GetSEZFilename(invoice_Id);
                var fileList = JsonConvert.DeserializeObject<List<SEZJson>>(filejson);
                if (fileList == null || fileList.Count == 0 || string.IsNullOrEmpty(fileList[0]?.FilePath))
                {
                    return BadRequest(new { message = "FilePath not found." });
                }
                //string? fileName = fileList?[0].FileName;
                string? filePath = fileList?[0].FilePath;
                string? fileName = Path.GetFileName(filePath);
                string? fullPath = filePath; //.Replace(@"\", @"\\");
                if (!System.IO.File.Exists(fullPath))
                {
                    return BadRequest(new { message = "File not found." });
                }
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string base64String = Convert.ToBase64String(fileBytes);
                FileResponse fileResponse = new FileResponse();
                fileResponse.FileName = fileName;
                fileResponse.File = base64String;

                return Ok(fileResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost, Route("BulkApproveSEZ")]
        public async Task<IActionResult> BulkApproveSEZ(ApproveRequest request)
        {
            var result = await _iSEZ.BulkApproveSEZ(request);
            return Ok(result);
        }
    }
}
