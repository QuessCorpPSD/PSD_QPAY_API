using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Extensions;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.BAL.Repository.SalaryReleaseInvoice;
using QPay.UI.Models.SalaryReleaseInvoice;

namespace QPay.API.Controller.SalaryReleaseInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchGenerationController : ControllerBase
    {
        private readonly IBatchGenerationRepository _BatchGenerationRepository;
        private readonly IConfiguration _configuration;

        public BatchGenerationController(IConfiguration configuration, IBatchGenerationRepository Repository)
        {
            _BatchGenerationRepository = Repository;
            _configuration = configuration;
        }

        #region BatchTypeLoad start

        [HttpGet, Route("GetBatchTypeList/{UserId}")]
        public IActionResult GetBatchTypeList(int UserId)
        {
            var response = _BatchGenerationRepository.GetBatchTypeList(UserId);

            return Ok(response);
        }

        [HttpGet, Route("GetTemplate/{Flag}/{UserId}")]
        public IActionResult GetTemplate(string Flag, int UserId)
        {
            var ds = _BatchGenerationRepository.GetTemplate(Flag, UserId);            
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        #endregion BatchTypeLoad end

        #region BatchGenerate start

        [HttpGet, Route("GetApproveInvoices/{BatchType}/{BatchCreationType}/{EntityId}/{UserId}")]
        public IActionResult GetApproveInvoices(string BatchType, int BatchCreationType, int EntityId,int UserId)
        {

            var ds = _BatchGenerationRepository.GetApproveInvoices(BatchType, BatchCreationType, EntityId, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetApproveInvoicesExport/{BatchType}/{BatchCreationType}/{EntityId}/{UserId}")]
        public IActionResult GetApproveInvoicesExport(string BatchType, int BatchCreationType, int EntityId, int UserId)
        {

            var ds = _BatchGenerationRepository.GetApproveInvoices(BatchType, BatchCreationType, EntityId, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("BatchGenerate")]
        public async Task<IActionResult> BatchGenerate([FromBody] BatchCreate payload)
        {

            var catgory = await _BatchGenerationRepository.BatchGenerate(payload);
            return Ok(catgory);
        }

        [HttpPost, Route("RejectBankAdvice")]
        public async Task<IActionResult> RejectBankAdvice([FromBody] RejectBankAdvice payload)
        {

            var catgory = await _BatchGenerationRepository.RejectBankAdvice(payload);
            return Ok(catgory);
        }

        [HttpGet, Route("EntityListbg/{UserId}")]
        public IActionResult EntityListbg(int UserId)
        {
            var response = _BatchGenerationRepository.EntityListbg(UserId);

            return Ok(response);
        }

        [HttpGet, Route("BatchCreationTypelist/{UserId}")]
        public IActionResult BatchCreationTypelist(int UserId)
        {
            var response = _BatchGenerationRepository.BatchCreationTypelist(UserId);

            return Ok(response);
        }

        #endregion BatchGenerate end

        #region Salaryreleaseprocess start

        [HttpGet, Route("GetSRPBatchList/{BatchType}/{UserId}")]
        public IActionResult GetSRPBatchList(string BatchType,int UserId)
        {

            var response = _BatchGenerationRepository.GetSRPBatchList(BatchType, UserId);

            return Ok(response);
        }

        [HttpGet, Route("GetSRPBatchData/{BatchType}/{BatchId}/{UserId}")]
        public IActionResult GetSRPBatchData(string BatchType, string BatchId, int UserId)
        {

            var ds = _BatchGenerationRepository.GetSRPBatchData(BatchType, BatchId, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("BatchIntitiate")]
        public IActionResult BatchIntitiate([FromBody] IntitiateBatch payload)
        {

            var fileBytes = _BatchGenerationRepository.BatchIntitiate(payload.BatchType, payload.BatchId, payload.UserId);

            if (fileBytes != null)
            {
               
                return File(fileBytes, "application/zip", payload.BatchId + ".rar");
            }
            //return NotFound("Batch does not exist!");
            return BadRequest("Batch does not exist!");           

        }

        #endregion Salaryreleaseprocess end

        #region Download Batch start

        [HttpGet, Route("GetBatchList/{BatchType}/{BatchDate}/{UserId}")]
        public IActionResult GetBatchList(string BatchType,string BatchDate, int UserId)
        {

            var response = _BatchGenerationRepository.GetBatchList(BatchType, BatchDate, UserId);

            return Ok(response);
        }

        [HttpGet, Route("DownloadBatchFile/{BatchId}")]
        public IActionResult DownloadBatchFile(string BatchId)
        {        

            var basePath = _configuration["invoiceBatch"];

            var fullPath = Path.Combine(basePath, BatchId, BatchId + ".rar");

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("Batch does not exist!");
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, "application/zip", BatchId + ".rar");            

        }

        #endregion Download Batch end

        #region Salary release status start

        [HttpGet, Route("GetSalaryReleaseStatusdata/{BatchType}/{FromDate}/{Todate}/{EmployeeCode}/{UserId}")]
        public IActionResult GetSalaryReleaseStatusdata(string BatchType, string FromDate, string Todate, string EmployeeCode, int UserId)
        {

            var ds = _BatchGenerationRepository.GetSalaryReleaseStatusdata(BatchType, FromDate, Todate, EmployeeCode, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetSalaryReleaseStatusdataExport/{BatchType}/{FromDate}/{Todate}/{EmployeeCode}/{UserId}")]
        public IActionResult GetSalaryReleaseStatusdataExport(string BatchType, string FromDate, string Todate, string EmployeeCode, int UserId)
        {

            var ds = _BatchGenerationRepository.GetSalaryReleaseStatusdataExport(BatchType, FromDate, Todate, EmployeeCode, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("UtrUpload")]
        public async Task<IActionResult> UtrUpload(IFormFile file, [FromForm] string BatchType, [FromForm] int UserId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _BatchGenerationRepository.UtrUpload(file, BatchType, UserId);
            return Ok(result);
        }

        #endregion Salary release status end
    }
}
