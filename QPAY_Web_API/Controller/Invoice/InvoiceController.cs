using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Invoice;
using QPay.UI.Models;
using static QPay.UI.Models.Invoice.Invoice;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _iinvoice;

        public InvoiceController(
           IInvoiceRepository iinvoice)
        {
            _iinvoice = iinvoice;
        }

        [HttpGet]
        [Route("GetPerformaInvoice/{CompanyId}/{PayPriod}/{InvoiceBillingType}/{createdBy}")]
        public async Task<IActionResult> GetPerformaInvoice(int CompanyId, string PayPriod, int InvoiceBillingType, string createdBy)
        {
            var ds = await _iinvoice.GetPerformaInvoice(CompanyId, PayPriod, InvoiceBillingType, createdBy);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("PerformaInvoiceSplit")]
        public async Task<IActionResult> PerformaInvoiceSplit(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iinvoice.PerformaInvoiceSplit(file, CompanyId, payperiod, CreatedBy, payperiodId);
            return Ok(result);
        }

        [HttpPost]
        [Route("PerformaInvoiceMerge")]
        public async Task<IActionResult> PerformaInvoiceMerge(LotMergeRequest request)
        {
            var result = await _iinvoice.PerformaInvoiceMerge(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("PerformaInvoiceMergeNew")]
        public async Task<IActionResult> PerformaInvoiceMergeNew(List<MergeNewRequest> request)
        {
            var result = await _iinvoice.PerformaInvoiceMergeNew(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("PerformaInvoiceInitiate")]
        public async Task<IActionResult> PerformaInvoiceInitiate(DraftInvoiceInitiate request)
        {
            var result = await _iinvoice.PerformaInvoiceInitiate(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateMapName")]
        public async Task<IActionResult> UpdateMapName(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iinvoice.UpdateMapName(file, CompanyId, payperiod, CreatedBy, payperiodId);
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadAttributes")]
        public async Task<IActionResult> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiodId, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _iinvoice.UploadAttributes(file, CompanyId, payperiodId, CreatedBy);
            return Ok(result);
        }

        [HttpGet]
        [Route("BillingDashboardByUserId/{userId}")]
        public async Task<IActionResult> BillingDashboardByUserId(int userId)
        {
            var dashboard =await _iinvoice.BillingDashboard(userId);
            return Ok(dashboard);
        }
        [HttpGet]
        [Route("DraftInvoiceEmployeeByRequestId/{reqNo}")]
        public async Task<IActionResult> DraftInvoiceEmployeeByRequestId(int reqNo)
        {
            var employee = await _iinvoice.DraftInvoiceEmployeeByRequestId(reqNo);
            if (employee.Rows.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    var ws = workbook.AddWorksheet(employee, "Sheet1");

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        fileResponse.FileName = "InputLot";
                        fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                    }
                }
            }
            else
            {
                FileResponse fileResponse = new FileResponse();
                fileResponse.FileName = "InputLot";
                fileResponse.File = "No";
                return Ok(fileResponse);
            }
        }
    }
}
