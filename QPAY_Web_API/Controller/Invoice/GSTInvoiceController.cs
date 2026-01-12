using ClosedXML.Excel;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Mvc;
using QPay.API.LoggerService;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Invoice;
using QPay.UI.Models.Invoice;
using SelectPdf;
using System.Data;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class GSTInvoiceController : ControllerBase
    {
        private readonly IGSTInvoiceRepository _gstinvoiceRepository;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;
        private readonly HttpClient _client;
        private readonly ICommonRepository _icommon;
        public GSTInvoiceController(ILoggerManager logger, HttpClient client, IGSTInvoiceRepository gstinvoiceRepository
            , IConfiguration configuration, ICommonRepository icommon)
        {
            this._gstinvoiceRepository = gstinvoiceRepository;
            _configuration = configuration;
            this._logger = logger;
            this._client = client;
            this._icommon = icommon;
        }

        [HttpGet, Route("GetGSTInvoice/{userId}")]
        public async Task<IActionResult> GetGSTInvoice(int userId) =>
            Ok(await this._gstinvoiceRepository.GetGSTInvoice(userId));

        [HttpGet]
        [Route("Download/{invoiceId}")]
        public async Task<IActionResult> Download(int invoiceId)
        {
            if (invoiceId <= 0)
                return Ok("Invalid Invoice Id");

            string fileName;
            string invoiceHtml;
            bool applyDigitalSignature;
            bool isHeaderFooter;
            string QRImageText;
            string QRImageBase64 = "";
            string dateToDs;

            DataSet ds = GetInvoiceData(invoiceId);

            invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
            fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";

            invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);

            byte[] pdf = GetInvoicePdf(invoiceHtml, fileName);

            string dirPath = _configuration["GstInvoiceForOtherApp"].ToString();

            byte[] fileBytes = DownloadToFolder(pdf, dirPath, fileName);

            return File(fileBytes, "application/pdf", fileName);
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


        private DataSet GetInvoiceData(int invoiceId)
        {
            var ds = _gstinvoiceRepository.GetInvoiceData(invoiceId);
            return (ds);
        }

        private byte[] GetInvoicePdf(string invoiceHtml, string fileName = default(string))
        {
            HtmlToPdf converter = new HtmlToPdf();

            PdfDocument doc = converter.ConvertHtmlString(invoiceHtml);

            byte[] pdf = doc.Save();

            doc.Close();

            return pdf;
        }


        [HttpPost]
        [Route("BulkDownload")]
        public ActionResult BulkDownload([FromBody] BulkInvoices bulkInvoices)
        {
            string[] DownloadIds = bulkInvoices.invoiceIds.Select(id => id.ToString()).ToArray();
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3);
            byte[] bytes = null;

            foreach (var invoicedetails in DownloadIds)
            {
                string[] idetails = invoicedetails.Split('|');
                string invoiceId = idetails[0];
                string fileName;
                string invoiceHtml;
                bool applyDigitalSignature;
                bool isHeaderFooter;
                string QRImageText;
                string QRImageBase64 = "";
                string dateToDs;

                DataSet ds = GetInvoiceData(Convert.ToInt32(invoiceId));

                invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
                fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";
                var newEntry = new ZipEntry(fileName);
                newEntry.DateTime = DateTime.Now;

                zipStream.PutNextEntry(newEntry);

                byte[] pdf = GetInvoicePdf(invoiceHtml, fileName);

                string dirPath = _configuration["GstInvoiceForOtherApp"].ToString();

                byte[] fileBytes = DownloadToFolder(pdf, dirPath, fileName);
                MemoryStream inStream = new MemoryStream(fileBytes);
                StreamUtils.Copy(inStream, zipStream, new byte[4096]);
                inStream.Close();
                zipStream.CloseEntry();
            }

            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;

            return File(outputMemStream.ToArray(), "application/octet-stream", "Invoices.zip");
        }

        [HttpPost]
        [Route("PostCancelReject")]
        public async Task<IActionResult> PostCancelReject(IFormFile file, [FromForm] string userId)
        {

            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "GSTCancel";
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
            DataSet ds = new DataSet("Data");
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return Ok("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var response = await _gstinvoiceRepository.PostCancelReject(xmlInput, userId);

            return Ok(response);
        }
        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet("Data");
            
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

        //[HttpPost]
        //[Route("Create")]
        //public async Task<IActionResult> Create(DAL.Repository.GstInvoiceCreateRequest request)
        //{
        //    var result = await _gstinvoiceRepository.Create(request);
        //    return Ok(result);
        //}

        [HttpPost("Create")]
        public async Task<IActionResult> Create(GstInvoiceCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _gstinvoiceRepository.Create(request);
            return Ok(response);
        }

        [HttpGet, Route("GetGSTInvoiceType")]
        public async Task<IActionResult> GetGSTInvoiceType() =>
    Ok(await this._gstinvoiceRepository.GetGSTInvoiceType());

        [HttpGet, Route("GetGSTBillableType")]
        public async Task<IActionResult> GetGSTBillable_Type() =>
           Ok(await this._gstinvoiceRepository.GetGSTBillableType());

        [HttpGet, Route("GetGSTCtcDeductionType")]
        public async Task<IActionResult> GetGSTCtcDeductionType() =>
           Ok(await this._gstinvoiceRepository.GetGSTCtcDeductionType());

        [HttpGet, Route("GetGSTNetDeductionType")]
        public async Task<IActionResult> GetGSTNetDeductionType() =>
           Ok(await this._gstinvoiceRepository.GetGSTNetDeductionType());

    }
}
