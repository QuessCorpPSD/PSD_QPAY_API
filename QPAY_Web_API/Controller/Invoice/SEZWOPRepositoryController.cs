
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QPay.BAL.IRepository.Common.Invoices;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SEZWOPRepositoryController : ControllerBase
    {
        private readonly ISEZWOPRepositoryService _service;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SEZWOPRepositoryController> _logger;

        public SEZWOPRepositoryController(
            ISEZWOPRepositoryService service,
            IConfiguration configuration,
            ILogger<SEZWOPRepositoryController> logger)
        {
            _service = service;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Search SEZ WOP Repository records
        /// GET api/SEZWOPRepository/Search?companyId=0&payPeriodId=0&InvoiceNumbers=&Year=0
        /// </summary>
        [HttpGet("Search")]
        public async Task<IActionResult> Search(
            [FromQuery] int companyId,
            [FromQuery] int payPeriodId,
            [FromQuery] string InvoiceNumbers,
            [FromQuery] int Year)
        {
            try
            {
                var result = await _service.SearchAsync(companyId, payPeriodId, InvoiceNumbers, Year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SEZWOPRepository Search failed");
                return StatusCode(500, new { Error_Message = "An error occurred during search." });
            }
        }

        /// <summary>
        /// Upload file with selected records
        /// POST api/SEZWOPRepository/Uploadfile
        /// </summary>
        [HttpPost("Uploadfile")]
        public async Task<IActionResult> Uploadfile([FromForm] IFormFile DocumentFile,
            [FromForm] string Remark,
            [FromForm] string Obselete_Document_FilePath,
            [FromForm] string selectedrecord)
        {
            var cancelledInvoiceRepository = new SEZWOPRepository();

            try
            {
                // Get UserId from claims (replaces Session["UserId"])
                var userIdClaim = User.FindFirst("UserId")?.Value
                    ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    cancelledInvoiceRepository.Error_Message = "Session expired. Please login again.";
                    return Unauthorized(cancelledInvoiceRepository);
                }

                int userId = Convert.ToInt32(userIdClaim);

                // Deserialize selected records
                cancelledInvoiceRepository.selectedrecordsList = 
                    JsonSerializer.Deserialize<List<SelectedRecords>>(selectedrecord ?? "[]");
                cancelledInvoiceRepository.Remark = Remark;
                cancelledInvoiceRepository.Obselete_Document_FilePath = Obselete_Document_FilePath;

                string dirPath = _configuration["AppSettings:RepositoryUpload"];

                if (DocumentFile != null && DocumentFile.Length > 0)
                {
                    string documentName = Path.GetFileNameWithoutExtension(DocumentFile.FileName);
                    string fileExtension = Path.GetExtension(DocumentFile.FileName);
                    string fileName = documentName + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + fileExtension;
                    string filePath = Path.Combine(dirPath, fileName);

                    // Validate path to prevent directory traversal
                    string fullPath = Path.GetFullPath(filePath);
                    if (!fullPath.StartsWith(Path.GetFullPath(dirPath)))
                    {
                        return BadRequest(new { Error_Message = "Invalid file path." });
                    }

                    // Delete existing file if present
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // Delete obsolete file if exists
                    if (!string.IsNullOrEmpty(cancelledInvoiceRepository.Obselete_Document_FilePath))
                    {
                        string obseleteFilePath = Path.Combine(dirPath, cancelledInvoiceRepository.Obselete_Document_FilePath);
                        string fullObsoletePath = Path.GetFullPath(obseleteFilePath);
                        if (fullObsoletePath.StartsWith(Path.GetFullPath(dirPath)) && System.IO.File.Exists(obseleteFilePath))
                        {
                            System.IO.File.Delete(obseleteFilePath);
                        }
                    }

                    cancelledInvoiceRepository.Document_Name = documentName + fileExtension;
                    cancelledInvoiceRepository.Document_FilePath = fileName;

                    // Serialize to XML for stored procedure
                    var response = new SEZWOPRepositoryResponse
                    {
                        SEZWOPRepositoryDetails = new[] { cancelledInvoiceRepository }
                    };
                    string serializedXml = SerializeToXml(response);

                    cancelledInvoiceRepository = await _service.UploadfileAsync(serializedXml, userId, "Add");

                    // Save file to disk
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await DocumentFile.CopyToAsync(stream);
                    }
                }

                return Ok(cancelledInvoiceRepository);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SEZWOPRepository Uploadfile failed");
                return StatusCode(500, new { Error_Message = "An error occurred during upload." });
            }
        }

        /// <summary>
        /// Delete a repository record and its file
        /// POST api/SEZWOPRepository/Delete
        /// </summary>
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromForm] int id, [FromForm] string fileName)
        {
            var objRepository = new SEZWOPRepository();

            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value
                    ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { Error_Message = "Session expired. Please login again." });
                }

                int userId = Convert.ToInt32(userIdClaim);
                string dirPath = _configuration["AppSettings:RepositoryUpload"];

                // Delete physical file
                string filePath = Path.Combine(dirPath, fileName);
                string fullPath = Path.GetFullPath(filePath);
                if (fullPath.StartsWith(Path.GetFullPath(dirPath)) && System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                objRepository.Id = id;
                var response = new SEZWOPRepositoryResponse
                {
                    SEZWOPRepositoryDetails = new[] { objRepository }
                };
                string serializedXml = SerializeToXml(response);

                objRepository = await _service.UploadfileAsync(serializedXml, userId, "Delete");

                return Ok(objRepository.Error_Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SEZWOPRepository Delete failed");
                return StatusCode(500, new { Error_Message = "An error occurred during delete." });
            }
        }

        /// <summary>
        /// Export data to Excel file
        /// POST api/SEZWOPRepository/ExportToExcel
        /// </summary>
        [HttpPost("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel([FromBody] ExportToExcelRequestDto request)
        {
            try
            {
                var ds = await _service.ExportToExcelAsync(
                    request.CompanyId, request.StatusId, request.InvoiceNumbers, request.Year);
                DataTable dt = new DataTable();
                dt=(ds.Tables[0]);
                if (dt != null && dt.Rows.Count > 0)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "sez");
                        using (var stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            stream.Position = 0;
                            return File(
                                stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "SEZWOPRepositoryDetails.xlsx");
                        }
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SEZWOPRepository ExportToExcel failed");
                return StatusCode(500, new { Error_Message = "An error occurred during export." });
            }
        }

        /// <summary>
        /// Get uploaded file details
        /// POST api/SEZWOPRepository/FilesDetails
        /// </summary>
        [HttpPost("FilesDetails")]
        public IActionResult FilesDetails([FromBody] FilesDetailsRequestDto request)
        {
            var filedata = GetUploadFiles(request.Document_Name, request.Document_Remarks, request.Empid);
            return Ok(filedata);
        }

        /// <summary>
        /// Download a file
        /// GET api/SEZWOPRepository/Download?fileName=xxx
        /// </summary>
        [HttpGet("Download")]
        public IActionResult Download([FromQuery] string fileName)
        {
            try
            {
                string dirPath = _configuration["AppSettings:RepositoryUpload"];
                string filePath = Path.Combine(dirPath, fileName);

                // Validate path to prevent directory traversal
                string fullPath = Path.GetFullPath(filePath);
                if (!fullPath.StartsWith(Path.GetFullPath(dirPath)))
                {
                    return Forbid();
                }

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { Error_Message = "File not found." });
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/force-download", fileName);
            }
            catch (Exception)
            {
                return Forbid();
            }
        }

        /// <summary>
        /// View/Download a PDF file
        /// GET api/SEZWOPRepository/GetPdf?filename=xxx
        /// </summary>
        [HttpGet("GetPdf")]
        public IActionResult GetPdf([FromQuery] string filename)
        {
            string dirPath = _configuration["AppSettings:RepositoryUpload"];
            string filePath = Path.Combine(dirPath, filename);

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", Path.GetFileName(filename));
            }

            return NotFound(new { Error_Message = "Employee Document does not exist!" });
        }

        /// <summary>
        /// Get Document Type Master list
        /// GET api/SEZWOPRepository/DocumentTypeMaster
        /// </summary>
        //[HttpGet("DocumentTypeMaster")]
        //public async Task<IActionResult> GetDocumentTypeMaster()
        //{
        //    try
        //    {
        //        var result = await _service.GetDocumentTypeMasterAsync();
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "GetDocumentTypeMaster failed");
        //        return StatusCode(500, new { Error_Message = "An error occurred." });
        //    }
        //}

        #region Private Helpers

        private List<DocumentUploadsFiles> GetUploadFiles(string documentName, string documentRemarks, string empId)
        {
            var list = new List<DocumentUploadsFiles>();
            string[] fileNames = documentName.Split('?');
            string[] uploadedFileNames = documentRemarks.Split(',');

            for (int i = 0; i < fileNames.Length; i++)
            {
                list.Add(new DocumentUploadsFiles
                {
                    Document_Name = fileNames[i].TrimEnd(),
                    Document_Remarks = i < uploadedFileNames.Length ? uploadedFileNames[i].TrimEnd() : string.Empty,
                    EmpployeeID = empId
                });
            }
            return list;
        }

        private string SerializeToXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        #endregion
    }
}
