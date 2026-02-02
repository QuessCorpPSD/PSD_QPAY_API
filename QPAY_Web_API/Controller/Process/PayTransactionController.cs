using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Process;
using static QPay.UI.Models.Process.Process;

namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayTransactionController : ControllerBase
    {
        private readonly IPayTransactionRepository _processRepository;
        public PayTransactionController(IPayTransactionRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost, Route("GetEmployeeDetailsByCompanyID")]
        public async Task<IActionResult> GetEmployeeDetailsByCompanyID(SearchEmployeeRequest searchRequest)
        {
            var ds = await _processRepository.GetEmployeeDetailsByCompanyID(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpPost, Route("GetAllPayCodeByCompanyID")]
        public async Task<IActionResult> GetAllPayCodeByCompanyID(SearchEmployeeRequest searchRequest)
        {
            var ds = await _processRepository.GetAllPayCodeByCompanyID(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchPayTransactionRequest searchRequest)
        {
            var ds = await _processRepository.SearchDetails(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("Exporttoexcel")]
        public async Task<IActionResult> Exporttoexcel(SearchPayTransactionRequest searchRequest)
        {
            var ds = await _processRepository.Exporttoexcel(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ImportPayTransaction")]
        public async Task<IActionResult> ImportPayTransaction(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.ImportPayTransaction(file, User);
            return Ok(result);
        }

        [HttpGet, Route("DeletePayTransaction/{Pay_Transaction_Id}/{CreatedBy}")]
        public async Task<IActionResult> DeletePayTransaction(string Pay_Transaction_Id, string CreatedBy)
        {
            var ds = await _processRepository.DeletePayTransaction(Pay_Transaction_Id, CreatedBy);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}

