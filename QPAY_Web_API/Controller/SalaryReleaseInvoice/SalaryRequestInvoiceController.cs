using Azure.Core;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.UI.Common;
using QPay.UI.Models;
using QPay.UI.Models.SalaryReleaseInvoice;
using System.Data;
using System.Xml.Serialization;
//using static QRCoder.PayloadGenerator;

namespace QPay.API.Controller.SalaryReleaseInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryRequestInvoiceController : ControllerBase
    {
        private readonly ISalaryRequestInvoiceRepository _SalaryRequestInvoiceRepository;
        private readonly IConfiguration _configuration;
       
        public SalaryRequestInvoiceController(IConfiguration configuration, ISalaryRequestInvoiceRepository InvoiceRepository)
        {
            _SalaryRequestInvoiceRepository = InvoiceRepository;
            _configuration = configuration;
        }

        #region Salary Request start

        [HttpPost, Route("GetBankAdviceApproveList")]
        public async Task<IActionResult> GetBankAdviceApproveList(InvoiceCommon SRInvoiceCommon)
        {
            var catgory = await _SalaryRequestInvoiceRepository.GetBankAdviceApproveList(SRInvoiceCommon);
            return Ok(catgory);
        }

        [HttpPost, Route("CreateRequestSalaryRelease")]
        public async Task<IActionResult> CreateRequestSalaryRelease(BankAdviceApprovalRequest Request)
        {
            var catgory = await _SalaryRequestInvoiceRepository.CreateRequestSalaryRelease(Request);
            return Ok(catgory);
        }

        [HttpPost, Route("UploadSalaryReleaseRequest")]
        public async Task<IActionResult> UploadSalaryReleaseRequest([FromBody] BankAdviceRequest rdata)
        {
            var catgory = await _SalaryRequestInvoiceRepository.UploadSalaryReleaseRequest(rdata);
            return Ok(catgory);

        }

        
        [HttpGet, Route("SalaryReleaseTemplate/{Flag}/{QZoneUserName}")]
        public IActionResult SalaryReleaseTemplate(string Flag, string QZoneUserName)
        {
            var ds = _SalaryRequestInvoiceRepository.SalaryReleaseTemplate(Flag, QZoneUserName);
            // ds.Tables[0].TableName = "Template";
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        #endregion Salary Request end

        #region SalaryHold Request start

        [HttpPost, Route("InvoiceHoldList")]
        public IActionResult InvoiceHoldList(SalaryHoldCommon Data)
        {
           
            var ds = _SalaryRequestInvoiceRepository.InvoiceHoldList(Data);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


        [HttpPost, Route("HoldRequestUpload")]
        public async Task<IActionResult> HoldRequestUpload([FromBody] HoldSalaryRequest payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.HoldRequestUpload(payload);
            return Ok(catgory);
                       
        }

        [HttpPost, Route("SingleHoldRequest")]
        public async Task<IActionResult> SingleHoldRequest([FromBody] SingleHoldRequest payload)
        {
            var ds = _SalaryRequestInvoiceRepository.SingleHoldRequest(payload);
            var payload1 = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload1);

        }
        #endregion SalaryHold Request end

        #region SalaryHoldRelease Request start

        [HttpPost, Route("InvoiceHoldReleaseList")]
        public async Task<IActionResult> InvoiceHoldReleaseList(SalaryHoldReleaseCommon Data)
        {
            var catgory = await _SalaryRequestInvoiceRepository.InvoiceHoldReleaseList(Data);
            return Ok(catgory);
        }

        [HttpPost, Route("InvoiceHoldReleaseListExport")]
        public async Task<IActionResult> InvoiceHoldReleaseListExport(SalaryHoldReleaseCommon Data)
        {
            var catgory = await _SalaryRequestInvoiceRepository.InvoiceHoldReleaseListExport(Data);
            return Ok(catgory);
        }

        [HttpPost, Route("HoldReleaseRequest")]
        public async Task<IActionResult> HoldReleaseRequest(HoldReleaseRequest payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.HoldReleaseRequest(payload);
            return Ok(catgory);
        }

        #endregion SalaryHoldRelease Request end

        #region partila hold and release start

        [HttpPost, Route("PartialHoldRequest")]
        public async Task<IActionResult> PartialHoldRequest([FromBody] PartilHoldRequest payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.PartialHoldRequest(payload);
            return Ok(catgory);

        }
        [HttpPost, Route("PartialHoldRelease")]
        public async Task<IActionResult> PartialHoldRelease([FromBody] PartialRelease payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.PartialHoldRelease(payload);
            return Ok(catgory);

        }
        
        #endregion partila hold and release end

        #region DBT hold and release start

        [HttpPost, Route("DBTHoldRequest")]
        public async Task<IActionResult> DBTHoldRequest([FromBody] DBTHoldRequest rdata)
        {
            var catgory = await _SalaryRequestInvoiceRepository.DBTHoldRequest(rdata);
            return Ok(catgory);

        }

        [HttpPost, Route("DBTHoldRelease")]
        public async Task<IActionResult> DBTHoldRelease([FromBody] DBTRelease payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.DBTHoldRelease(payload);
            return Ok(catgory);

        }
       
        #endregion DBT hold and release end

        #region netpay summary start

        [HttpGet, Route("InvoiceNetPaysummary/{Company_Id}/{Pay_Period_Id}/{QZoneUserName}")]
        public IActionResult InvoiceNetPaysummary(int Company_Id, int Pay_Period_Id, string QZoneUserName)
        {

            var ds = _SalaryRequestInvoiceRepository.InvoiceNetPaysummary(Company_Id, Pay_Period_Id,  QZoneUserName);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("InvoiceWiseAssociateHoldList/{Company_Id}/{Pay_Period_Id}/{Flag}/{Invoice_No}/{QZoneUserName}")]
        public IActionResult InvoiceWiseAssociateHoldList(int Company_Id, int Pay_Period_Id, string Flag,string Invoice_No,string QZoneUserName)
        {

            var ds = _SalaryRequestInvoiceRepository.InvoiceWiseAssociateHoldList(Company_Id, Pay_Period_Id, Flag, Invoice_No, QZoneUserName);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("NetPaysummary/{Company_Id}/{Pay_Period_Id}/{QZoneUserName}")]
        public IActionResult NetPaysummary(int Company_Id, int Pay_Period_Id, string QZoneUserName)
        {
            DataSet ds = _SalaryRequestInvoiceRepository.NetPaysummary(Company_Id, Pay_Period_Id, QZoneUserName);
            //var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            //return Ok(payload);

            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                ds.Tables[0].TableName = "Net Pay Summary Report";
                ds.Tables[1].TableName = "Net Pay Summary Details";
                ds.Tables[2].TableName = "Partial Hold Summary Report";
                ds.Tables[3].TableName = "Gratuity Summary Report";
                ds.Tables[4].TableName = "DBT Hold Summary Report";
                ds.Tables[5].TableName = "Deduction Flush Out Report";


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
                    fileResponse.FileName = "Net Pay Summary Report" + fileName;
                    fileResponse.File = bytes;

                    return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
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

        #endregion netpay summary end

        #region Common drop down start

        [HttpGet, Route("GetCommonDropDownList/{Flag}/{QZoneUserName}")]
        public IActionResult GetCommonDropDownList(string Flag, string QZoneUserName)
        {
            var response = _SalaryRequestInvoiceRepository.GetCommonDropDownList(Flag, QZoneUserName);

            return Ok(response);
        }
        #endregion Common drop down end

        #region Bonus flush out start

        [HttpGet, Route("BonusDetailsSummary/{Company_Id}/{FromDate}/{ToDate}/{QZoneUserName}")]
        public IActionResult BonusDetailsSummary(int Company_Id, string FromDate, string ToDate, string QZoneUserName)
        {

            var ds = _SalaryRequestInvoiceRepository.BonusDetailsSummary(Company_Id, FromDate, ToDate, QZoneUserName);
            var result = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(result);

        }

        [HttpGet, Route("BonusAccumatedReport/{Company_Id}/{FromDate}/{ToDate}/{QZoneUserName}")]
        public IActionResult BonusAccumatedReport(int Company_Id, string FromDate, string ToDate, string QZoneUserName)
        {

            var ds = _SalaryRequestInvoiceRepository.BonusAccumatedReport(Company_Id, FromDate, ToDate, QZoneUserName);
            var result = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(result);
           
        }

        [HttpPost, Route("BonusReleaseUpload")]
        public async Task<IActionResult> BonusReleaseUpload([FromBody] BonusReleaseRequest payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.BonusReleaseUpload(payload);
            return Ok(catgory);

        }
        #endregion Bonus flush out end

        #region Deduction FlasuOut start

        [HttpGet, Route("DeductionFlasuOutSearch/{Company_Id}/{Pay_Period_Id}/{QZoneUserName}")]
        public IActionResult DeductionFlasuOutSearch(int Company_Id, int Pay_Period_Id, string QZoneUserName)
        {

            var ds = _SalaryRequestInvoiceRepository.DeductionFlasuOutSearch(Company_Id, Pay_Period_Id, QZoneUserName);
            var result = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(result);

        }

        [HttpPost, Route("DeductionFlasuOutUpload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> DeductionFlasuOutUpload([FromForm] DeductionReleaseRequest payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.DeductionFlasuOutUpload(payload);
            return Ok(catgory);

        }
        #endregion Deduction FlasuOut end

        #region Salary Advance start

        [HttpGet, Route("SalaryAdvanceTemplate/{Company_Code}/{Pay_Period_Id}/{QZoneUserName}")]
        public IActionResult SalaryAdvanceTemplate(string Company_Code, int Pay_Period_Id, string QZoneUserName)
        {
            DataSet ds = _SalaryRequestInvoiceRepository.SalaryAdvanceTemplate(Company_Code, Pay_Period_Id, QZoneUserName);

            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                // ds.Tables[0].TableName = "Salary Advance Request";

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
                    fileResponse.FileName = "Salary Advance Request" + fileName;
                    fileResponse.File = bytes;

                    return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
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

        [HttpPost, Route("SalaryAdvanceUpload")]
        public async Task<IActionResult> SalaryAdvanceUpload(IFormFile file, [FromForm] string QZoneUserName)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _SalaryRequestInvoiceRepository.SalaryAdvanceUpload(file, QZoneUserName);
            return Ok(result);
        }
        #endregion Salary advance end

        #region Van Payment request start

       
        [HttpPost, Route("ViewVanPaymentRequestList")]
        public IActionResult ViewVanPaymentRequestList(VanDetailsView payload)
        {          
            var ds = _SalaryRequestInvoiceRepository.ViewVanPaymentRequestList(payload);
            var result = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(result);
        }

        [HttpPost, Route("VanPaymentRequestUpload")]
        public async Task<IActionResult> VanPaymentRequestUpload([FromBody] VanRequest payload)
        {
            var ds = _SalaryRequestInvoiceRepository.VanPaymentRequestUpload(payload);
            var payload1 = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload1);

        }

        [HttpGet, Route("VANCompanyCodeList/{QZoneUserName}")]
        public async Task<IActionResult> VANCompanyCodeList(string QZoneUserName)
        {
            var catgory = await _SalaryRequestInvoiceRepository.VANCompanyCodeList(QZoneUserName);
            return Ok(catgory);
        }

        [HttpPost, Route("VANPayPeriodList")]
        public IActionResult VANPayPeriodList(VanPayPeriod data)
        {
            var ds =   _SalaryRequestInvoiceRepository.VANPayPeriodList(data);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        #endregion Van Payment request end

        #region Uan hold Release request start

        [HttpPost, Route("UanReleaseList")]
        public IActionResult UanReleaseList(UanReleaseCommon payload)
        {
            var ds = _SalaryRequestInvoiceRepository.UanReleaseList(payload);
            var result = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(result);

        }        
        
        [HttpPost, Route("UanReleaseRequest")]
        public async Task<IActionResult> UanReleaseRequest([FromBody] UanReleaseRequest payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.UanReleaseRequest(payload);
            return Ok(catgory);
        }
        #endregion Uan hold Release request end

        #region Reissue Request start

        [HttpPost, Route("ReissueRequest")]
        public async Task<IActionResult> ReissueRequest([FromBody] ReissueRequestData payload)
        {
            var catgory = await _SalaryRequestInvoiceRepository.ReissueRequest(payload);
            return Ok(catgory);

        }

        #endregion Reissue Request end
    }

}


