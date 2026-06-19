using Azure.Core;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.UI.Common;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System.Data;
using System.Xml.Linq;

namespace QPay.API.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceInitiationController : ControllerBase
    {
        private readonly IInvoiceInitiationRepository _invoiceInitiationRepository;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;
        private readonly HttpClient _client;
       // private readonly IHubContext<NotificationHub> _hub;
        public InvoiceInitiationController(ILoggerManager logger, HttpClient client, IInvoiceInitiationRepository invoiceInitiationRepository, IConfiguration configuration)
        {
            this._invoiceInitiationRepository = invoiceInitiationRepository;
            _configuration = configuration;
            this._logger = logger;
            this._client = client;
        }
        [HttpGet, Route("GetTaxTypes")]
        public async Task<IActionResult> GetTaxTypes() =>
            Ok(await this._invoiceInitiationRepository.GetTaxTypes("GetTaxTypes"));

        [HttpPost, Route("Search")]
        public async Task<IActionResult> Search(InvoiceSearchRequest request)
        {
          var search = await this._invoiceInitiationRepository.Search(request.companyId,request.Pay_Period, request.taxtypeId);
            return Ok(search);
        }
        [HttpPost,Route("InitiationSearch")]
        public async Task<IActionResult> InitiationSearch(InitiationRequestModel initiationRequestModel)
        {
            var invoicesearch = await this._invoiceInitiationRepository.InitiationSearch(initiationRequestModel);
            return Ok(invoicesearch);
        }

        [HttpPost, Route("InitiationSearchAllot")]
        public async Task<IActionResult> InitiationSearchAllot(InvoiceDetailModel invoiceDetailModel)
        {
            var invoicesearch = await this._invoiceInitiationRepository.InitiationSearchAllot(invoiceDetailModel);            
            return Ok(invoicesearch);
        }

        [HttpGet, Route("GetInvoiceQCDetail/{userId}")]
        public async Task<IActionResult> GetInvoiceQCDetail(int userId)
        {
            var invoicesearch = await this._invoiceInitiationRepository.InvoiceQCDetail(userId);
            // var invoicesearch = await this._invoiceInitiationRepository.InitiationSearchAllot(invoiceDetailModel);
            return Ok(invoicesearch);
        }

        [HttpPost, Route("PostInvoiceQC")]
        public async Task<IActionResult> PostInvoiceQC(BulkInvoiceQCModelRequest bulkInvoiceQCModel)
        {
            string xml = BAL.IRepository.XmlHelper.SerializeObjectToXml(bulkInvoiceQCModel.invoiceQCModels, "Main");
            var invoicesearch = await this._invoiceInitiationRepository.PostInvoiceQCDetail(xml,bulkInvoiceQCModel.CreatedBy);            
            return Ok(invoicesearch);
        }

        [HttpPost]
        [Route("GetAllInvoiceAllotDetails")]
        public async Task<IActionResult> GetAllInvoiceAllotDetails(InvoiceDetailModel invoiceDetailModel)
        {
            var ds = await this._invoiceInitiationRepository.GetAllInvoiceAllotDetails(invoiceDetailModel);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpPost, Route("InitiationSearchExport")]
        public async Task<IActionResult> InitiationSearchExport(IntiationExportRequest intiationExportRequest)
        {
            var invoicesearch = await this._invoiceInitiationRepository.InitiationSearchExport(intiationExportRequest);
            return Ok(invoicesearch);
        }
        [HttpGet,Route("RequestRevok/{ReqNo}/{InvoiceType}/{userId}")]
        public async Task<IActionResult> InvoiceRequestRevoke(int ReqNo,string InvoiceType,int userId)
        {
            var staus=await _invoiceInitiationRepository.InvoiceRequestRevoke(ReqNo, InvoiceType, userId);
            return Ok(staus);
        }
        [HttpPost, Route("InvoiceInitiate")]
      //  public async Task<IActionResult> InvoiceInitiate(InvoiceInitiateRequestModel request)
      //  {
      //      string xml = BAL.IRepository.XmlHelper.SerializeObjectToXml(request.invoiceInitiations, "Main");
      //      var result = await _invoiceInitiationRepository.InvoiceInitiate(
      //    request.TaxTypeId,
      //    xml,
      //    "Add",          // or make this request.Mode if dynamic
      //    request.CreatedBy
      //);

      //      return Ok(result);
      //  }

public async Task<IActionResult> InvoiceInitiate(InvoiceInitiateRequestModel request)
    {

            var withoutProInvoiceNumber = request.invoiceInitiations.Where(x => string.IsNullOrWhiteSpace(x.PRO_Invoice_Number)).ToList();
            var withProInvoiceNumber = request.invoiceInitiations.Where(x => !string.IsNullOrWhiteSpace(x.PRO_Invoice_Number)).ToList();

                InvoiceInitiationUI proInvoice_status = new InvoiceInitiationUI();
            InvoiceInitiationUI draftInvoice_status = new InvoiceInitiationUI();
            if (withProInvoiceNumber.Any())
        {

                string xml = BAL.IRepository.XmlHelper.SerializeObjectToXml(
               withProInvoiceNumber,
                "Main"
            );
                proInvoice_status = await _invoiceInitiationRepository.ProformaToActualInvoiceInitiate(
                request.TaxTypeId,
                xml,
                "Add",
                request.CreatedBy
            );
        }
            else if (withoutProInvoiceNumber.Any())
            {

                string xml = BAL.IRepository.XmlHelper.SerializeObjectToXml(
               withoutProInvoiceNumber,
                "Main"
            );
                draftInvoice_status = await _invoiceInitiationRepository.InvoiceInitiate(
                request.TaxTypeId,
                xml,
                "Add",
                request.CreatedBy
            );
               
            }
            if (proInvoice_status != null && draftInvoice_status != null)
            {
                {
                    var prostatus = proInvoice_status.Error_Message;
                    var draftstatus = draftInvoice_status.Error_Message;
                    if (prostatus == "GST Invoice Initiated Successfully" && draftstatus == "GST Invoice Initiated Successfully")
                    {
                        return Ok(new { Error_Message = "GST Invoice Initiated Successfully." });
                    }
                    else if (prostatus == "GST Invoice Initiated Successfully")
                    {
                        return Ok(new { Error_Message = "ProtoActual invoices initiated successfully, but Draft invoices failed." });
                    }
                    else if (draftstatus == "GST Invoice Initiated Successfully")
                    {
                        return Ok(new { Error_Message = "Draft invoices initiated successfully, but ProtoActual invoices failed." });
                    }
                    else
                    {
                        return Ok(new { Error_Message = "Both ProtoActual and Draft invoice initiation failed." });
                    }
                }
            }
            else
            {
                return Ok(new { Error_Message = "No invoices to initiate." });
            }
        }

        
    [HttpPost, Route("getRemarksByReqNo")]
        public async Task<IActionResult> getRemarksByReqNo(RequestModel requestModel)
        {
            var reqno = await this._invoiceInitiationRepository.getRemarksByReqNo(requestModel);
            return Ok(reqno);
        }
        [HttpPost, Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(InvoiceSearchRequest requestModel)
        {
            try
            {
     
                var InvoiceInitiateExcel = await _invoiceInitiationRepository.ExportToExcel(requestModel.companyId,requestModel.Pay_Period, requestModel.taxtypeId);

                if (string.IsNullOrEmpty(InvoiceInitiateExcel.File))
                {
                    return NotFound("No data available to export.");
                }

                return Ok(InvoiceInitiateExcel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while exporting the file: {ex.Message}");
            }
        }

        //[HttpPost]
        //[Route("ProvisionalInvoiceInitiate")]
        //public async Task<IActionResult> ProvisionalInvoiceInitiate(ProvisionalInvoiceInitiateRequestBulk request)
        //{
        //    string xml = XmlHelper2.SerializeObjectToXml(request);
        //    var result = await _invoiceInitiationRepository.ProvisionalInvoiceInitiate(xml, request.CreatedBy);
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("ProvisionalInvoiceInitiate")]
        public async Task<IActionResult> ProvisionalInvoiceInitiate(ProvisionalInvoiceInitiateRequestBulk request)
        {
            List<string> finalResponse = new List<string>();

            foreach (var item in request.request)
            {
                var result = await _invoiceInitiationRepository.ProvisionalInvoiceInitiate(item);

                finalResponse.Add(result);
            }

            return Ok(finalResponse);
        }

        [HttpPost]
        [Route("VendorInvoiceInitiate")]
        public async Task<IActionResult> VendorInvoiceInitiate(VendorInvoiceInitiateRequestBulk request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);
            var result = await _invoiceInitiationRepository.VendorInvoiceInitiate(xml, request.CreatedBy);
            return Ok(result);
        }
        [HttpPost]
        [Route("MiscInvoiceInitiate")]
        public async Task<IActionResult> MiscInvoiceInitiate(MiscInvoiceInitiateRequestBulk request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);
            var result = await _invoiceInitiationRepository.MiscInvoiceInitiate(xml, request.CreatedBy);
            return Ok(result);
        }
        

        [HttpPost, Route("DraftExporttoExcel")]
        public async Task<IActionResult> DraftExporttoExcel(InvoiceDetailModel invoiceDetailModel)
        {

            DataSet ds = await _invoiceInitiationRepository.DraftExporttoExcel(invoiceDetailModel);
            if (ds != null && ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                ds.Tables[0].TableName = "Invoice Details";

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    var ws = workbook.AddWorksheet(ds.Tables[i], ds.Tables[i].TableName);
                    ws.Table(0).ShowAutoFilter = false;
                    ws.Table(0).Theme = XLTableTheme.None;
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());
                    FileResponse fileResponse = new FileResponse();
                    string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                    fileResponse.FileName = "InvoiceDetails" + fileName;
                    fileResponse.File = bytes;

                    return Ok(fileResponse);
                }
            }
            else
            {
                var response = new APIResponse<object>
                {
                    statuscode = 400,
                    message = "Failure",
                    data = "",
                    error = ""
                };
                return Ok(response);
            }

        }

    }
}
