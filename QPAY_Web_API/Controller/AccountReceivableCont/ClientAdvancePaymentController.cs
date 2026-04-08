using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.UI.Models;
using System.Data;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.API.Controller.AccountReceivable
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClientAdvancePaymentController : ControllerBase
    {
        private readonly IClientAdvancePaymentRepository _iclient;
        public ClientAdvancePaymentController(
          IClientAdvancePaymentRepository iclient)
        {
            this._iclient = iclient;
        }
        [HttpGet]
        [Route("Search/{CompanyId}/{FromDate}/{ToDate}")]
        public async Task<IActionResult> Search(int? CompanyId, string FromDate, string ToDate)
        {
            var ds = await _iclient.Search(CompanyId, FromDate, ToDate);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpPost]
        [Route("ClientAdvancePaymentExport")]
        public async Task<IActionResult> ClientAdvancePaymentExportToExcel([FromBody] CommonExport payload)
        {
            var ds = await _iclient.ExportToExcel(payload);   // <-- await is required
            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetModeOfCollections/{Action}")]
        public async Task<IActionResult> GetModeOfCollections(string Action)
        {
            var ds = await _iclient.GetModeOfCollections(Action);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetOnAccountTypes/{Action}")]
        public async Task<IActionResult> GetOnAccountTypes(string Action)
        {
            var ds = await _iclient.GetOnAccountTypes(Action);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetOnAccountNumbers/{Description}/{Action}")]
        public async Task<IActionResult> GetOnAccountNumbers(string Description, string Action)
        {
            var ds = await _iclient.GetOnAccountNumbers(Description, Action);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpGet]
        [Route("GetGroupNameByCompanyID/{CompanyId}")]
        public async Task<IActionResult> GetGroupNameByCompanyID(int? CompanyId)
        {
            var ds = await _iclient.GetGroupNameByCompanyID(CompanyId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetBankNameForOnAccount")]
        public async Task<IActionResult> GetBankNameForOnAccount()
        {
            var ds = await _iclient.GetBankNameForOnAccount();
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpPost("SaveUpdateDeleteClientAdvancePayment")]
        public async Task<IActionResult> SaveUpdateDeleteClientAdvancePayment([FromBody] ClientAdvancePaymentRequest request)
        {
            var res = await this._iclient.SaveUpdateDeleteClientAdvancePayment(request);
            return Ok(res);
        }

        [HttpPost("TransferClientAdvancePayment")]
        public async Task<IActionResult> TransferClientAdvancePayment([FromBody] ClientAdvancePaymentRequest request)
        {
            var res = await _iclient.TransferClientAdvancePayment(request);
            return Ok(res);
        }

        [HttpPost]
        [Route("UploadClientAdvancePayment")]
        public async Task<IActionResult> UploadClientAdvancePayment(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iclient.UploadClientAdvancePayment(file, User);
            return Ok(result);
        }


    }



}
