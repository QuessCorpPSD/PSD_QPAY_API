using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QPay.API.Extensions;
using QPay.DTo.Models.Common;
using QPay.DTo.Models.Masters;
using QPay.IRepository.iRepository.Masters;
using System.Data;
using static QPay.UI.Models.Common.Common;

namespace QPay.API.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceRuleController : ControllerBase
    {
        private readonly IInvoiceRuleRepository _invoiceRule;
        private readonly IConfiguration _configuration;
        public InvoiceRuleController(
                    IInvoiceRuleRepository invoiceRule, IConfiguration configuration)
        {
            this._invoiceRule = invoiceRule;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllInvoiceRule/{companyId}/{siteId}")]
        public async Task<IActionResult> GetAllInvoiceRule(int? companyId, string? siteId)
        {
            var response = await _invoiceRule.GetAllInvoiceRule(companyId, siteId);

            return Ok(response);
        }
        [HttpPost("PostAddInvoiceRule")]
        public async Task<IActionResult> PostAddInvoiceRule([FromBody] InvoiceRuleAdd invoiceruleAdd)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _invoiceRule.PostAddInvoiceRule(invoiceruleAdd);
            return Ok(response);
        }

        [HttpPost("PostUpdateInvoiceRule")]
        public async Task<IActionResult> PostUpdateInvoiceRule([FromBody] InvoiceRuleUpdate invoiceruleUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _invoiceRule.PostUpdateInvoiceRule(invoiceruleUpdate);
            return Ok(response);
        }

        [HttpPost("PostDeleteInvoiceRule")]
        public async Task<IActionResult> PostDeleteInvoiceRule([FromBody] int invoicingRulesID)
        {

            var response = await _invoiceRule.PostDeleteInvoiceRule(invoicingRulesID);
            return Ok(response);

        }
        //[HttpPost]
        //[Route("GetInvoiceRuleTemplate")]
        //public IActionResult GetInvoiceRuleTemplate([FromForm] int companyId, [FromForm] string companyCode, [FromForm] string siteName)
        //{

        //    DataSet ds = _invoiceRule.GetInvoiceRuleTemplate(companyId, siteName);
        //    if (ds.Tables.Count > 0)
        //    {
        //        using var workbook = new XLWorkbook();
        //        var ws = workbook.AddWorksheet("InvoiceRule");
        //        ws.Cell(1, 1).InsertTable(ds.Tables[0], "NewDataSet", true);

        //        var table = ws.Table("NewDataSet");
        //        table.ShowAutoFilter = false;
        //        table.Theme = XLTableTheme.None;

        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var bytes = Convert.ToBase64String(stream.ToArray());

        //            FileResponse fileResponse = new FileResponse();
        //            string fileName = companyCode;
        //            fileResponse.FileName = "InvoiceRule_Template_" + fileName + ".xlsx";
        //            fileResponse.File = bytes;
        //            return Ok(fileResponse);
        //        }
        //    }
        //    else
        //    {
        //        var response = new APIResponse<object>
        //        {
        //            statuscode = 400,
        //            message = "Failure",
        //            data = "",
        //            error = ""
        //        };
        //        return Ok(response);
        //    }

        //}
        [HttpPost]
        [Route("PostInvoiceRuleUpload")]
        public async Task<IActionResult> PostInvoiceRuleUpload(IFormFile file, [FromForm] string userId)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "InvoiceRule";
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            string fileExtention = Path.GetExtension(file.FileName.ToUpper());
            string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
            FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;

            string serverpath = DirName + FileName;

            using (var stream = new FileStream(serverpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            DataSet ds = new DataSet("NewDataSet");
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return BadRequest("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var response = await _invoiceRule.PostInvoiceRuleUpload(xmlInput, userId);

            return Ok(response);
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable(worksheet.Name);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
                                        .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }
        [HttpPost]
        [Route("InvoiceRuleExport")]
        public IActionResult InvoiceRuleExport([FromForm] int companyId, [FromForm] int siteCode)
        {

            DataSet ds = _invoiceRule.InvoiceRuleExport(companyId, siteCode);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("InvoiceRule");
                ws.Cell(1, 1).InsertTable(ds.Tables[0], "NewDataSet", true);

                var table = ws.Table("NewDataSet");
                table.ShowAutoFilter = false;
                table.Theme = XLTableTheme.None;

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());

                    FileResponse fileResponse = new FileResponse();
                    string fileName = DateTime.Now.ToString();
                    fileResponse.FileName = "InvoiceRule_Export_" + fileName + ".xlsx";
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

        [HttpGet, Route("GetInvoiceruleTemplate/{companyId}/{siteName}")]
        public IActionResult GetInvoiceruleTemplate(int? companyId, string? siteName)
        {
            var ds = _invoiceRule.GetInvoiceruleTemplate(companyId, siteName);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
