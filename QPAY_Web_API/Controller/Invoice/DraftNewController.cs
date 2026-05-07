using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Invoice;
using QPay.UI.Common;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System.Data;
using static QPay.UI.Models.Invoice.DraftNew;


namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftNewController : ControllerBase
    {
        private readonly IDraftNewRepository _iinvoice;

        public DraftNewController(
           IDraftNewRepository iinvoice)
        {
            _iinvoice = iinvoice;
        }

        [HttpGet, Route("InvoiceBackdated/{companyId}/{payPeriod_Id}")]
        public async Task<IActionResult> InvoiceBackdated(int companyId, int payPeriod_Id)
        {
            var stauts = await _iinvoice.GetBackDated(companyId, payPeriod_Id);
            return Ok(stauts);
        }

        [HttpGet]
        [Route("GetPerformaInvoice/{CompanyId}/{PayPriod}/{createdBy}")]
        public async Task<IActionResult> GetPerformaInvoice(int CompanyId, string PayPriod, string createdBy)
        {
            var ds = await _iinvoice.GetPerformaInvoice(CompanyId, PayPriod, createdBy);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("PerformaInvoiceSplit")]
        public async Task<IActionResult> PerformaInvoiceSplit(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

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
        [Route("PerformaInvoiceSkip")]
        public async Task<IActionResult> PerformaInvoiceSkip(DraftInvoiceInitiate request)
        {
            var result = await _iinvoice.PerformaInvoiceSkip(request);
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateMapName")]
        public async Task<IActionResult> UpdateMapName(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _iinvoice.UpdateMapName(file, CompanyId, payperiod, CreatedBy, payperiodId);
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadAttributes")]
        public async Task<IActionResult> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiodId, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _iinvoice.UploadAttributes(file, CompanyId, payperiodId, CreatedBy);
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadAttributesNew")]
        public async Task<IActionResult> UploadAttributesNew(IFormFile file, [FromForm] string CompanyId,
   [FromForm] string payperiodId, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _iinvoice.UploadAttributesNew(file, CompanyId, payperiodId, CreatedBy);
            return Ok(result);
        }

        [HttpPost, Route("GetSplitTemplate")]
        public async Task<IActionResult> GetSplitTemplate(SplitParams splitParams)
        {

            DataSet ds = await _iinvoice.GetSplitTemplate(splitParams);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], "Sheet" + i);
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "Split_Template" + fileName;
                        fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                    }
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

        [HttpGet]

        [Route("GetDraftInformation/{CompanyId}/{PayPriod}/{createdBy}")]

        public async Task<IActionResult> GetDraftInformation(int CompanyId, int PayPriod, string createdBy)
        {
            var draftInformation = await _iinvoice.GetDraftInformation(CompanyId, PayPriod, createdBy);
            var results = new List<DraftInvoice>();
            var draftTypes = draftInformation.Where(x => x.DraftType > 0).Select(item => item.DraftType).ToList();
            List<InvoiceInitiateRequest> initiateRequests = new List<InvoiceInitiateRequest>();

            if (draftTypes != null && draftTypes.Count > 0)
            {
                var draftInfoLookup = draftInformation.Where(x => x.DraftType > 0)
                         .GroupBy(x => x.DraftType)
                            .ToDictionary(g => g.Key, g => g.ToList());
                foreach (var (draftType, invoices) in draftInfoLookup)

                {

                    results.Add(new DraftInvoice
                    {
                        DraftType = Convert.ToInt16(draftType),
                        InvoiceInitiateRequests = invoices
                    });

                }
                return Ok(results);

            }
            return Ok(results);

        }



        [HttpPost]
        [Route("PostInvoicePush")]
        public async Task<IActionResult> PostInvoicePush(PushModel request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var draftInformation = await _iinvoice.PostInvoicePush(request.company_id, request.Pay_Period_Id, xml, request.CreatedBy, request.DraftTypeId, request.Action);
            // var draftInformation = await _iinvoice.GetDraftInformation(CompanyId, PayPriod, createdBy);
            //var results = new List<DraftInvoice>();
            //var draftTypes = draftInformation.Where(x => x.DraftType > 0).Select(item => item.DraftType).ToList();

            //List<InvoiceInitiateRequest> initiateRequests = new List<InvoiceInitiateRequest>();
            //if (draftTypes != null && draftTypes.Count > 0)
            //{

            //    var draftInfoLookup = draftInformation.Where(x => x.DraftType > 0)
            //             .GroupBy(x => x.DraftType)
            //                .ToDictionary(g => g.Key, g => g.ToList());

            //    foreach (var (draftType, invoices) in draftInfoLookup)
            //    {
            //        results.Add(new DraftInvoice
            //        {
            //            DraftType = Convert.ToInt16(draftType),
            //            InvoiceInitiateRequests = invoices
            //        });
            //    }
            //    return Ok(results);

            //}
            return Ok(draftInformation);
        }

        [HttpPost, Route("GetEmployeeReport")]
        public async Task<IActionResult> GetEmployeeReport(EmpExport empExport)
        {

            DataSet ds = await _iinvoice.GetEmployeeReport(empExport);
            if (ds != null && ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                ds.Tables[0].TableName = "Employee Details";

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
                    fileResponse.FileName = "EmployeeDetails" + fileName;
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

        [HttpPost]
        [Route("GetInvoiceCountDetails")]
        public async Task<IActionResult> GetInvoiceCountDetails([FromBody] InvoiceCountRequest request)
        {
            var ds = await _iinvoice.GetInvoiceCountDetails(request);
            var table = ds.Tables[0];

            var result = table.AsEnumerable()
                .Select(row =>
                    table.Columns.Cast<DataColumn>()
                        .ToDictionary(
                            col => col.ColumnName,
                            col => row[col] == DBNull.Value ? null : row[col]
                        )
                ).ToList();

            return Ok(result);
        }

        [HttpPost, Route("GetPassThroughTemplate")]
        public async Task<IActionResult> GetPassThroughTemplate(SplitParams splitParams)
        {

            DataSet ds = await _iinvoice.GetPassThroughTemplate(splitParams);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], "Sheet" + i);
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "PassThrough_Template" + fileName;
                        fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                    }
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

        [HttpPost]
        [Route("UploadPassThrough")]
        public async Task<IActionResult> UploadPassThrough(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _iinvoice.UploadPassThrough(file, CompanyId, payperiod, CreatedBy, payperiodId);
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadPush")]
        public async Task<IActionResult> UploadPush(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _iinvoice.UploadPush(file, CompanyId, payperiod, CreatedBy, payperiodId);
            return Ok(result);
        }
    }
}
