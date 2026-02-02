using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.Repository.Invoice;
using QPay.UI.Models.Invoice;
using SelectPdf;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;
using QPay.BAL.IRepository.Common;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditNoteUpdateController : ControllerBase
    {
        private readonly ICreditNoteUpdateRepository _iCreditNote;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public CreditNoteUpdateController(
            ICreditNoteUpdateRepository iCreditNote, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._iCreditNote = iCreditNote;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpPost, Route("GetCreditNoteSearch")]
        public async Task<IActionResult> GetCreditNoteSearch(CreditNoteSearchApprove creditNoteSearchApprove)
        {
            var response = await _iCreditNote.GetCreditNoteSearch(creditNoteSearchApprove);

            return Ok(response);
        }

        [HttpPost, Route("UploadCreditNoteCancel")]
        public async Task<IActionResult> UploadCreditNoteCancel(IFormFile file, [FromForm] string userId)
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


            var response = await _iCreditNote.UploadCreditNoteCancel(xmlInput, userId);

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

        [HttpGet]
        [Route("Download/{CreditNoteid}/{ComapanyId}/{InvoiceNumber}/{InvoiceID}")]
        public async Task<IActionResult> Download(int CreditNoteid, string ComapanyId, string InvoiceNumber, string InvoiceID)
        {
            MemoryStream memoryStream = new MemoryStream();
            int Invoice_ID = 0;
            Int32.TryParse(InvoiceID, out Invoice_ID);
            int Company_Id = 0;
            Int32.TryParse(ComapanyId, out Company_Id);
            int CreditNoteId = 0;
            CreditNoteId = Convert.ToInt32(CreditNoteid);
            string fileName = string.Empty;
            string invoiceHtml = string.Empty;

            string EmployeeHtml = string.Empty;
            string QRImageText = string.Empty;
            string QRImageBase64 = string.Empty;
            bool applyDigitalSignature = false;
            string date_to_ds = string.Empty;


            if (CreditNoteid > 0)
            {
                DataSet ds = GetInvoiceData(Company_Id, Invoice_ID, CreditNoteId, InvoiceNumber, "CreditNote");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
                    if (invoiceHtml == null || invoiceHtml == "")
                    {
                        invoiceHtml = ds.Tables[2].Rows[0]["InvoiceHtml"].ToString();
                        fileName = ds.Tables[2].Rows[0]["InvoiceNumber"].ToString() + ".pdf";
                    }
                    else
                    {
                        invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
                        fileName = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString() + ".pdf";

                    }
                    QRImageText = ds.Tables.Count > 3 ? ds.Tables[3].Columns.Contains("QR_Image_Text") ? ds.Tables[3].Rows[0]["QR_Image_Text"].ToString() : string.Empty : string.Empty;
                    //QRImageBase64 = string.IsNullOrEmpty(QRImageText) ? string.Empty : GenerateQRCodeBase64String(QRImageText);
                    //invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);
                    //date_to_ds = ds.Tables[3].Rows[0]["date_to_ds"].ToString();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        EmployeeHtml = ExportDatatableToHtml(ds.Tables[1]);
                        invoiceHtml = invoiceHtml.Replace("[Employee_detail]", EmployeeHtml);
                    }
                    else
                    {
                        invoiceHtml = invoiceHtml.Replace("[Employee_detail]", EmployeeHtml);
                    }
                    // instantiate a html to pdf converter object
                    HtmlToPdf converter = new HtmlToPdf();

                    // create a new pdf document converting an url
                    SelectPdf.PdfDocument doc = converter.ConvertHtmlString(invoiceHtml);


                    // save pdf document
                    byte[] pdf = doc.Save();
                    string dirPath = _configuration["GstInvoiceForOtherApp"].ToString();

                    // close pdf document
                    doc.Close();

                    //applyDigitalSignature = ds.Tables[2].Columns.Contains("ApplyDigitalSignature") ? Convert.ToBoolean(ds.Tables[2].Rows[0]["ApplyDigitalSignature"]) : false;

                    //if (applyDigitalSignature)
                    //{
                    //    pdf = DocumentSigner.DigitallySignPDFFileAdvanced(pdf, date_to_ds);
                    //}
                    // return resulted pdf document

                    byte[] fileBytes = DownloadToFolder(pdf, dirPath, fileName);

                    return File(fileBytes, "application/pdf", fileName);
                }
                else
                {
                    return BadRequest("No data found for the given Credit Note Id");
                }
            }
            else
            {
                return BadRequest("Invalid Credit Note Id");
            }
        }

        private DataSet GetInvoiceData(int Company_Id, int Invoice_ID, int CreditNoteId, string InvoiceNumber, string PdfType)
        {
            var ds = _iCreditNote.GetInvoiceData(Company_Id, Invoice_ID, CreditNoteId, InvoiceNumber, PdfType);
            return (ds);
        }


        public static byte[] DownloadToFolder(byte[] byteToWrite, string dirPath, string fileName)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                string fullPath = Path.Combine(dirPath, fileName);

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                System.IO.File.WriteAllBytes(fullPath, byteToWrite);

                return byteToWrite;
            }
            catch
            {
                return null;
            }
        }

        protected string ExportDatatableToHtml(DataTable dt)
        {
            StringBuilder strHTMLBuilder = new StringBuilder();

            strHTMLBuilder.Append("<table  id='Anexxures' style='border - collapse: collapse; width: 100 %; ' border='1'>");


            strHTMLBuilder.Append("<tr >");
            foreach (DataColumn myColumn in dt.Columns)
            {
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append(myColumn.ColumnName);
                strHTMLBuilder.Append("</td>");

            }
            strHTMLBuilder.Append("</tr>");


            foreach (DataRow myRow in dt.Rows)
            {

                strHTMLBuilder.Append("<tr >");
                foreach (DataColumn myColumn in dt.Columns)
                {
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                    strHTMLBuilder.Append("</td>");

                }
                strHTMLBuilder.Append("</tr>");
            }

            //Close tags.  
            strHTMLBuilder.Append("</table>");

            string Htmltext = strHTMLBuilder.ToString();

            return Htmltext;

        }

    }
}
