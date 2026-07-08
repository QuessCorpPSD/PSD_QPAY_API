using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.BankNonInvoice;
using QPay.UI.Models.BankNonInvoice;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class NIBatchGenerationController : ControllerBase
    {
        private readonly INIBatchGenerationRepository _BatchGenerationRepository;
        private readonly IConfiguration _configuration;

        public NIBatchGenerationController(IConfiguration configuration, INIBatchGenerationRepository Repository)
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


        [HttpGet, Route("GetSalaryreleaseProcessdata/{BatchType}/{EntityId}/{BatchCreationType}/{Status}/{UserId}")]
        public IActionResult GetSalaryreleaseProcessdata(string BatchType, int EntityId, int BatchCreationType,int Status, int UserId)
        {

            var ds = _BatchGenerationRepository.GetSalaryreleaseProcessdata(BatchType, EntityId,BatchCreationType, Status, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("GetSalaryreleaseProcessExport/{BatchType}/{EntityId}/{BatchCreationType}/{Status}/{UserId}")]
        public IActionResult GetSalaryreleaseProcessExport(string BatchType, int EntityId, int BatchCreationType, int Status, int UserId)
        {

            var ds = _BatchGenerationRepository.GetSalaryreleaseProcessExport(BatchType, EntityId, BatchCreationType, Status, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("BatchGenerate")]
        public async Task<IActionResult> BatchGenerate([FromBody] NIBatchGenerate payload)
        {

            var catgory = await _BatchGenerationRepository.BatchGenerate(payload);
            return Ok(catgory);
        }

        [HttpPost, Route("Rejectgroup/{BatchType}/{Salary_Process_Initiate_detail_Id}/{UserId}")]
        public async Task<IActionResult> Rejectgroup(string BatchType,int Salary_Process_Initiate_detail_Id, int UserId)
        {

            var catgory = await _BatchGenerationRepository.Rejectgroup(BatchType,Salary_Process_Initiate_detail_Id, UserId);
            return Ok(catgory);
        }

        [HttpPost, Route("UploadCollectionStatus")]
        public async Task<IActionResult> UploadCollectionStatus(IFormFile file, [FromForm] string BatchType, [FromForm] int UserId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _BatchGenerationRepository.UploadCollectionStatus(file, BatchType, UserId);
            return Ok(result);
        }

        #endregion BatchGenerate end

        #region Salaryreleaseprocess start

        [HttpGet, Route("GetSRPBatchList/{BatchType}/{UserId}")]
        public IActionResult GetSRPBatchList(string BatchType, int UserId)
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
        public IActionResult GetBatchList(string BatchType, string BatchDate, int UserId)
        {

            var response = _BatchGenerationRepository.GetBatchList(BatchType, BatchDate, UserId);

            return Ok(response);
        }

        [HttpGet, Route("DownloadBatchFile/{BatchId}")]
        public IActionResult DownloadBatchFile(string BatchId)
        {

            var basePath = _configuration["NoninvoiceBatch"];

            var fullPath = Path.Combine(basePath, BatchId, BatchId + ".rar");

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("Batch does not exist!");
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, "application/zip", BatchId + ".rar");

        }

        #endregion Download Batch end
    }
}
