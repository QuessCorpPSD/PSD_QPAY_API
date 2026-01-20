using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.API.Models;
using QPay.BAL.IRepository.Invoice;
using QPay.UI.Invoice;
using static QPay.UI.Models.Invoice.Invoice;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvisionalInvoiceController : ControllerBase
    {
        private readonly IProvisionalInvoiceRepository _iprovisional;

        public ProvisionalInvoiceController(
           IProvisionalInvoiceRepository iprovisional)
        {
            _iprovisional = iprovisional;
        }

        [HttpGet]
        [Route("GetProvisionalInvoice/{CompanyId}/{payPeriodId}/{createdBy}")]
        public async Task<IActionResult> GetProvisionalInvoice(int CompanyId, string payPeriodId, string createdBy)
        {
            var ds = await _iprovisional.GetProvisionalInvoice(CompanyId, payPeriodId, createdBy);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ProvisionalInvoiceSplit")]
        public async Task<IActionResult> ProvisionalInvoiceSplit(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iprovisional.ProvisionalInvoiceSplit(file, CompanyId, payperiod, CreatedBy, payperiodId);
            return Ok(result);
        }

        //[HttpPost]
        //[Route("PerformaInvoiceMerge")]
        //public async Task<IActionResult> PerformaInvoiceMerge(MergeRequest request)
        //{
        //    var result = await _iinvoice.PerformaInvoiceMerge(request.CompanyId, request.PayPeriodId,
        //        request.MAP_NAME_ID, request.MergeLot, request.CreatedBy, request.Remarks,
        //        request.Merged_Input_No, request.Data_From);
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("ProvisionalInvoiceInitiate")]
        public async Task<IActionResult> ProvisionalInvoiceInitiate(ProvisionalInvoiceInitiateRequest provisionalrequest)
        {
            var result = await _iprovisional.ProvisionalInvoiceInitiate(provisionalrequest);
            return Ok(result);
        }

        //[HttpPost]
        //[Route("UpdateMapName")]
        //public async Task<IActionResult> UpdateMapName(IFormFile file, [FromForm] string CompanyId,
        //   [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        //{
        //    if (file == null || file.Length == 0)
        //        return Ok("File is missing.");

        //    var result = await _iinvoice.UpdateMapName(file, CompanyId, payperiod, CreatedBy, payperiodId);
        //    return Ok(result);
        //}

        //[HttpPost]
        //[Route("UploadAttributes")]
        //public async Task<IActionResult> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
        //   [FromForm] string payperiodId, [FromForm] string CreatedBy)
        //{
        //    if (file == null || file.Length == 0)
        //        return Ok("File is missing.");

        //    var result = await _iinvoice.UploadAttributes(file, CompanyId, payperiodId, CreatedBy);
        //    return Ok(result);
        //}


    }
}
