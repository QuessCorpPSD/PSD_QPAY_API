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
            FileResponse fileResponse = new FileResponse();
            try
            {
                var filejson = _iSEZ.GetSEZFilename(invoice_Id);
                var fileList = JsonConvert.DeserializeObject<List<SEZJson>>(filejson);
                if (fileList == null || fileList.Count == 0 || string.IsNullOrEmpty(fileList[0]?.FilePath))
                {
                    fileResponse.FileName = "FilePath not found.";
                    fileResponse.File = "N";
                    //return BadRequest(new { message = "FilePath not found." });
                }
                //string? fileName = fileList?[0].FileName;
                string? filePath = fileList?[0].FilePath;
                string? fileName = Path.GetFileName(filePath);
                string? fullPath = filePath; //.Replace(@"\", @"\\");
                if (!System.IO.File.Exists(fullPath))
                {
                    fileResponse.FileName = "FilePath not found.";
                    fileResponse.File = "N";
                    // return BadRequest(new { message = "File not found." });
                }
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string base64String = Convert.ToBase64String(fileBytes);                
                fileResponse.FileName = fileName;
                fileResponse.File = base64String;
            }
            catch (Exception ex)
            {
                fileResponse.FileName = "FilePath not found.";
                fileResponse.File = "N";
                // return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
            return Ok(fileResponse);
        }

        [HttpPost, Route("BulkApproveSEZ")]
        public async Task<IActionResult> BulkApproveSEZ(ApproveRequest request)
        {
            var result = await _iSEZ.BulkApproveSEZ(request);
            return Ok(result);
        }

        [HttpGet, Route("SearchSEZCertificate/{companyId}")]
        public async Task<IActionResult> SearchSEZCertificate(int companyId)
        {
            var stauts = await _iSEZ.SearchSEZCertificate(companyId);
            return Ok(stauts);
        }

        [HttpGet, Route("GetUploadedCertificate/{Id}")]
        public IActionResult GetUploadedCertificate(int Id)
        {
            FileResponse fileResponse = new FileResponse();
            try
            {
                var filejson = _iSEZ.GetUploadedCertificate(Id);
                var fileList = JsonConvert.DeserializeObject<List<SEZJson>>(filejson);
                if (fileList == null || fileList.Count == 0 || string.IsNullOrEmpty(fileList[0]?.FilePath))
                {
                    fileResponse.File = "N";
                    fileResponse.FileName = "FilePath not found.";
                    //return BadRequest(new { message = "FilePath not found." });
                }
                //string? fileName = fileList?[0].FileName;
                string? filePath = fileList?[0].FilePath;
                string? fileName = Path.GetFileName(filePath);
                string? fullPath = filePath; //.Replace(@"\", @"\\");
                if (!System.IO.File.Exists(fullPath))
                {
                    fileResponse.File = "N";
                    fileResponse.FileName = "FilePath not found.";
                    //return BadRequest(new { message = "File not found." });
                }
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string base64String = Convert.ToBase64String(fileBytes);
               
                fileResponse.FileName = fileName;
                fileResponse.File = base64String;

                return Ok(fileResponse);
            }
            catch (Exception ex)
            {
                fileResponse.File = "N";
                fileResponse.FileName = "FilePath not found.";
                return Ok(fileResponse);
                // return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("UploadSEZCertificate")]
        public async Task<IActionResult> UploadSEZDocument(
                        [FromForm] string companyId,
                        [FromForm] string userId,
                        [FromForm] string validFrom,
                        [FromForm] string validTo,
                        [FromForm] string remarks,
                        [FromForm] string AckNo,
                        [FromForm][Required] IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                FileDetails fileDetails = new FileDetails();

                fileDetails = await SaveFileAsync(file);

                Task<string> Error_Message = _iSEZ.SaveUploadData(companyId, userId, validFrom, validTo, remarks, AckNo, fileDetails.OriginalFileName
                    , fileDetails.FileName, fileDetails.FilePath, "SaveFilepath");

                return Ok(new
                {
                    Status = "Success",
                    Message = Error_Message
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private async Task<FileDetails> SaveFileAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            string? basePath = _configuration["ClaimDocPath"];

            if (string.IsNullOrWhiteSpace(basePath))
                throw new Exception("ClaimDocPath not configured.");

            string directoryPath = Path.Combine(basePath, "SEZCertificate");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string fileExtension = Path.GetExtension(file.FileName);

            string originalfileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileName = $"SEZCert_{originalfileName}{DateTime.UtcNow:yyyyMMddHHmmssfff}{fileExtension}";

            string filePath = Path.Combine(directoryPath, fileName);

            FileDetails fileDetails = new FileDetails
            {
                OriginalFileName = originalfileName,
                FileName = fileName,
                FilePath = filePath
            };

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileDetails;
        }

    }
}
