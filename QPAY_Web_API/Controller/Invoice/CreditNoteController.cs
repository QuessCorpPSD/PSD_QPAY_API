using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.Invoice;
using QPay.UI.Models.Invoice;
using System.Data;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditNoteController : ControllerBase
    {
        private readonly ICreditNoteRepository _iCreditNote;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public CreditNoteController(
            ICreditNoteRepository iCreditNote, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._iCreditNote = iCreditNote;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetCreditNotePurpose/{companyId}")]
        public async Task<IActionResult> GetCreditNotePurpose(int companyId)
        {
            var response = await _iCreditNote.GetCreditNotePurpose(companyId);

            return Ok(response);
        }

        [HttpPost, Route("GetCreditNoteSearch")]
        public async Task<IActionResult> GetCreditNoteSearch(CreditNoteSearch creditNoteSearch)
        {
            var response = await _iCreditNote.GetCreditNoteSearch(creditNoteSearch);

            return Ok(response);
        }

        [HttpPost, Route("UploadCreditNoteRequest")]
        public async Task<IActionResult> UploadCreditNoteRequest(IFormFile file, [FromForm] string userId)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "CreditNote";
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


            var response = await _iCreditNote.UploadCreditNoteRequest(xmlInput, userId);

            return Ok(response);
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable("Table");
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

        [HttpPost, Route("ExportCreditNoteRequest")]
        public async Task<IActionResult> ExportCreditNoteRequest(CreditNoteSearch creditNoteSearch)
        {
            var response = await _iCreditNote.ExportCreditNoteRequest(creditNoteSearch);

            return Ok(response);
        }
    }
}
