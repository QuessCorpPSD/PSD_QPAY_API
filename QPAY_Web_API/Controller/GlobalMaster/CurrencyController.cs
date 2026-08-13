using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.GlobalMaster;
using QPay.UI.Models.GlobalMaster;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRepository _IRepository;
        public CurrencyController(ICurrencyRepository IRepository)
        {
            this._IRepository = IRepository;
        }


        [HttpGet]
        [Route("GetAllCurrency/{flag}")]
        public async Task<IActionResult> GetAllCurrency(string flag)
        {
            var response = await _IRepository.GetAllCurrency(flag);

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
        [Route("CurrencyConversion")]
        public async Task<IActionResult> CurrencyConversion(CurrencyConversionRequest request)
        {
            var response = await _IRepository.CurrencyConversion(request);

            if (response.Tables.Count > 0)
            {
                if (response.Tables[0].Rows.Count > 0)
                {
                    string message = response.Tables[0].Rows[0][0].ToString();
                    if (!(message.Contains("Successfully")))
                    {
                        return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0][0].ToString() });
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
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

    }
}
