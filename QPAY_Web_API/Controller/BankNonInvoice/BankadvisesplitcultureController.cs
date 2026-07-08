using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.BankNonInvoice;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankadvisesplitcultureController : ControllerBase
    {
        private readonly Ibankadvisesplitculturerepository _IRepository;
        public BankadvisesplitcultureController(Ibankadvisesplitculturerepository IRepository)
        {
            this._IRepository = IRepository;
        }
        [HttpGet]
        [Route("getvendor/{filter}/{Company_id}")]
        public async Task<IActionResult> getvendor(string? filter, int Company_id)
        {
            var response = await _IRepository.getvendor(filter, Company_id);
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
        [Route("getgroupname/{Company_id}/{client_id}")]
        public async Task<IActionResult> getgroupname(int? Company_id, int client_id)
        {
            var response = await _IRepository.getgroupname(Company_id, client_id);
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
        [HttpPost, Route("createbankadvisesplitculture")]
        public async Task<IActionResult> createbankadvisesplitculture([FromBody] Bankadvisesplitculture payload)
        {
            var ds = await _IRepository.createbankadvisesplitculture(payload);
            var catgory = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(catgory);

        }
        [HttpPost, Route("getsearcheditdata")]
        public async Task<IActionResult> getsearcheditdata([FromBody] searcheditdata payload)
        {
            var ds = await _IRepository.getsearcheditdata(payload);
            var catgory = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(catgory);

        }

        [HttpPost, Route("getsearcheditdataExport")]
        public async Task<IActionResult> getsearcheditdataExport([FromBody] searcheditdata payload)
        {
            var ds = await _IRepository.getsearcheditdataExport(payload);
            var catgory = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(catgory);

        }

        [HttpPost]
        [Route("uploadbankadvisesplitculture")]
        public async Task<IActionResult> uploadbankadvisesplitculture(IFormFile file, [FromForm] int created_by)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.uploadbankadvisesplitculture(file, created_by);

            return Ok(result);
        }
    }
}
