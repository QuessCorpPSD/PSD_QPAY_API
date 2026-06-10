using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.UI.GlobalMaster;
using QPay.UI.Models.GlobalMaster;
using QPay.UI.Models.Invoice;
using System.Data;
using System.Linq;
using System.Net;

namespace QPay.API.Controller.GlobalMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaycodeController : ControllerBase
    {
        private readonly IPaycodeRepository _IRepository;
        public PaycodeController(
            IPaycodeRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search(PaycodeSearchParams _params)
        {
            var response = await _IRepository.Search(_params.paycode_Code, _params.PayTypeId, _params.IsTaxable, _params.PayId);
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
        [Route("GetPageType")]
        public async Task<IActionResult> GetPageType()
        {
            var response = await _IRepository.GetPageType();
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
        [Route("GetPayType")]
        public async Task<IActionResult> GetPayType()
        {
            var response = await _IRepository.GetPayType();
            if (response!=null)
            {
                return Ok(response);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }



        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(PaycodeCreateParams _params)
        {
            var response = await _IRepository.Create(_params.strXmlDetails, _params.mode, _params.userId);
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
                return Ok(new { StatusCode = "400", Message = "Paycode Created Failed" });
            }
        }

        [HttpPost]
        [Route("GetPayCode")]
        public async Task<IActionResult> GetPayCode(PaycodeModelRquest paycodeModelRquest)
        {
            var paycodes = await this._IRepository.GetPayCodeByCompanyId(paycodeModelRquest.company_Id, paycodeModelRquest.Culture_Id, paycodeModelRquest.Type);
            Paycodes PayCodeMapping=new Paycodes();
            DataTable available_dt = paycodes.Tables[0];
            DataTable selected_dt = paycodes.Tables[1];
            PayCodeMapping.availablePaycode = available_dt.AsEnumerable()
                        .Select(dr => new SelectedItems
                        {
                            value = dr["PayCode_Id"].ToString(),
                            text = dr["PayCodeName"].ToString()
                        })
                        .ToList();
            PayCodeMapping.MappedPaycode = selected_dt.AsEnumerable()
                       .Select(dr => new SelectedItems
                       {
                           value = dr["PayCode_Id"].ToString(),
                           text = dr["PayCodeName"].ToString()
                       })
                       .ToList();
            

            //foreach (var item in paycodes.Tables)
            //{

            //}
            //var paycode = paycodes.Select(x => new UI.Models.Invoice.SelectedItems()
            //{
            //value = x.PayCode_Id.ToString(),
            //text = x.PayCodeName
            // })
            //.ToList();
            return Ok(PayCodeMapping);
        }


    }
}
