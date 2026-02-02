using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository;
using QPay.UI.Models.TaxAndSaving;

namespace QPay.API.Controller.TaxAndSaving
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxDeclarationAndActualController : ControllerBase
    {
        //GetAllTaxCodes

        private readonly ITaxDeclarationAndActualRepository _IRepository;
        private readonly IConfiguration _configuration;
        public TaxDeclarationAndActualController(IConfiguration configuration, ITaxDeclarationAndActualRepository Repository)
        {
            this._IRepository = Repository;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllTaxCodes")]
        public async Task<IActionResult> GetAllTaxCodes()
        {
            var response = await _IRepository.GetAllTaxCodes();
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
        [Route("GetTaxCodes/{TaxCode}")]
        public async Task<IActionResult> GetTaxCodes(string TaxCode)
        {
            var response = await _IRepository.GetTaxCodes(TaxCode);
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
        [Route("GetEligibleAmtByEmpIDTaxCode/{Employee_Id}/{Financial_Year_Id}/{Computation_Rule_Id}")]
        public async Task<IActionResult> GetEligibleAmtByEmpIDTaxCode(int Employee_Id, int Financial_Year_Id, int Computation_Rule_Id)
        {
            var response = await _IRepository.GetEligibleAmtByEmpIDTaxCode(Employee_Id, Financial_Year_Id, Computation_Rule_Id);
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
        [Route("Search/{companyId}/{employeeId}")]
        public async Task<IActionResult> Search(int? companyId, int? employeeId)
        {
            var response = await _IRepository.Search(companyId, employeeId);
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


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] TaxDeclarationAndActualRequest request)
        {
            var response = await _IRepository.Create(request);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] int CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _IRepository.Upload(file, CreatedBy);
            return Ok(result);
        }




    }
}
