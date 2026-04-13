using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.UI.Customer;
using QPay.UI.Models.Customer;
namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorServiceChargeController : ControllerBase
    {
        private readonly IVendorServiceChargeRepository _IRepository;
        public VendorServiceChargeController(IVendorServiceChargeRepository IRepository)
        {
            this._IRepository = IRepository;
        }
      
     
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] VendorServiceChargeRequest request)
        {
            var res = await this._IRepository.Create(request);
            return Ok(res);
        }


        [HttpPost]
        [Route("FileUpload")]
        public async Task<IActionResult> FileUpload(IFormFile file, [FromForm] int CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.FileUpload(file, CreatedBy);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllVendorServiceCharge/{companyId}")]
        public async Task<IActionResult> GetAllVendorServiceCharge(int companyId)
        {
            var response = await _IRepository.GetAllVendorServiceCharge(companyId);
            if (response != null)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }
        [HttpGet, Route("GetAllBillingTypes")]
        public async Task<IActionResult> GetAllBillingTypes()
        {
            var response = await _IRepository.GetAllBillingTypes();
            return Ok(response);
        }

        [HttpGet, Route("GetAllVendorServiceType")]
        public async Task<IActionResult> GetAllVendorServiceType()
        {
            var response = await _IRepository.GetAllVendorServiceType();
            return Ok(response);
        }
    }
}
