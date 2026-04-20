using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.Repository.Invoice;
using QPay.UI.Models.AccountReceivableMod;
using QPay.UI.Models.Invoice;
using QRCoder;
using SelectPdf;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Text;
using static QPay.UI.Models.AccountReceivableMod.creditnoteupdatemodel;

namespace QPay.API.Controller.AccountReceivableCont
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditNoteUpdateController : ControllerBase
    {
        private readonly IcreditnoteupdateRepository _icredit;
        private readonly ILoggerManager _logger;
        private readonly HttpClient _client;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public CreditNoteUpdateController(IcreditnoteupdateRepository icredit, ILoggerManager logger, HttpClient client, ICommonRepository icommon, IConfiguration configuration)
        {
            _icredit = icredit;
            _configuration = configuration;
            this._logger = logger;
            this._client = client;
            this._icommon = icommon;
        }

        [HttpGet]
        [Route("CreditnoteSearch/{CompanyId}/{fromdate}/{todate}")]
        public async Task<IActionResult> CreditnoteSearch(int CompanyId, string fromdate, string todate)
        {
            var ds = await _icredit.CreditNoteSearch(CompanyId, fromdate, todate);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("CreditNoteExport")]
        public async Task<IActionResult> CreditNoteExport([FromBody] CreditNoteExport payload)
        {
            var ds = await _icredit.CreditNoteExportToExcel(payload);
            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(res);
        }

        [HttpPost]
        [Route("CreditNoteCancelUpload")]
        public async Task<IActionResult> CreditNoteCancelUpload(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _icredit.CreditNoteCancelUpload(file, User);
            return Ok(result);
        }


        [HttpPost]
        [Route("BulkDownload")]
        public async Task<IActionResult> BulkDownload([FromBody] creditnoteupdatemodel.BulkDownloadRequest request)
        {
            var basePath = _configuration["CreditNoteBulkDownload"];
            var tempFolder = Path.Combine(basePath, Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempFolder);

            foreach (var item in request.Items)
            {
                int CreditNoteId = item.CreditNoteId;
                int CompanyId = item.CompanyId;
                string InvoiceNumber = item.InvoiceNumber;
                int InvoiceId = item.InvoiceId;

                string PdfType = request.PdfType;

                if (CreditNoteId <= 0) continue;

                DataSet ds = _icredit.GetInvoiceDetail(
                    CompanyId, InvoiceId, CreditNoteId, InvoiceNumber, PdfType);

                string invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"]?.ToString();

                string fileName;

                if (string.IsNullOrEmpty(invoiceHtml))
                {
                    invoiceHtml = ds.Tables[2].Rows[0]["InvoiceHtml"].ToString();
                    fileName = ds.Tables[2].Rows[0]["InvoiceNumber"] + ".pdf";
                }
                else
                {
                    fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";
                }

                fileName = fileName.Replace("/", "-");

                string QRImageText = ds.Tables.Count > 3 &&
                                     ds.Tables[3].Columns.Contains("QR_Image_Text")
                    ? ds.Tables[3].Rows[0]["QR_Image_Text"].ToString()
                    : "";

                string QRImageBase64 = string.IsNullOrEmpty(QRImageText)
                    ? ""
                    : GenerateQRCodeBase64String(QRImageText);

                invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);

                string EmployeeHtml = "";
                if (ds.Tables[1].Rows.Count > 0)
                {
                    EmployeeHtml = ExportDatatableToHtml(ds.Tables[1]);
                }

                invoiceHtml = invoiceHtml.Replace("[Employee_detail]", EmployeeHtml);

                //string date_to_ds = ds.Tables[3].Rows[0]["date_to_ds"].ToString();

                string date_to_ds = "20260415";

                HtmlToPdf converter = new HtmlToPdf();
                var doc = converter.ConvertHtmlString(invoiceHtml);
                byte[] pdf = doc.Save();
                doc.Close();

                bool applyDigitalSignature =
                    ds.Tables[2].Columns.Contains("ApplyDigitalSignature") &&
                    Convert.ToBoolean(ds.Tables[2].Rows[0]["ApplyDigitalSignature"]);

                if (applyDigitalSignature)
                {
                    pdf = await DigitalSign(pdf, date_to_ds);
                }

                string filePath = Path.Combine(tempFolder, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, pdf);
            }

            using var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var files = Directory.GetFiles(tempFolder);

                foreach (var file in files)
                {
                    var entry = archive.CreateEntry(Path.GetFileName(file));

                    using var entryStream = entry.Open();
                    using var fileStream = System.IO.File.OpenRead(file);

                    await fileStream.CopyToAsync(entryStream);
                }
            }

            try
            {
                Directory.Delete(tempFolder, true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Temp delete failed: " + ex.Message);
            }

            return File(memoryStream.ToArray(), "application/zip", request.PdfType + ".zip");
        }
        [HttpGet]
        [Route("Download")]
        public IActionResult Download(int CreditNoteid, string ComapanyId, string InvoiceNumber, string InvoiceID, string PdfType)
        {
            int Company_Id = Convert.ToInt32(ComapanyId);
            int Invoice_ID = Convert.ToInt32(InvoiceID);

            DataSet ds = _icredit.GetInvoiceDetail(
                Company_Id, Invoice_ID, CreditNoteid, InvoiceNumber, PdfType);

            string invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"]?.ToString();
            string fileName;

            if (string.IsNullOrEmpty(invoiceHtml))
            {
                invoiceHtml = ds.Tables[2].Rows[0]["InvoiceHtml"].ToString();
                fileName = ds.Tables[2].Rows[0]["InvoiceNumber"] + ".pdf";
            }
            else
            {
                fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";
            }

            // QR
            string QRImageText = ds.Tables.Count > 3 &&
                                 ds.Tables[3].Columns.Contains("QR_Image_Text")
                ? ds.Tables[3].Rows[0]["QR_Image_Text"].ToString()
                : "";

            string QRImageBase64 = string.IsNullOrEmpty(QRImageText)
                ? ""
                : GenerateQRCodeBase64String(QRImageText);

            invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);

            // Employee
            string EmployeeHtml = ds.Tables[1].Rows.Count > 0
                ? ExportDatatableToHtml(ds.Tables[1])
                : "";

            invoiceHtml = invoiceHtml.Replace("[Employee_detail]", EmployeeHtml);

            string date_to_ds = ds.Tables[3].Rows[0]["date_to_ds"].ToString();

           

            HtmlToPdf converter = new HtmlToPdf();
            var doc = converter.ConvertHtmlString(invoiceHtml);
            byte[] pdf = doc.Save();
            doc.Close();

            bool applyDigitalSignature =
                ds.Tables[2].Columns.Contains("ApplyDigitalSignature") &&
                Convert.ToBoolean(ds.Tables[2].Rows[0]["ApplyDigitalSignature"]);

            if (applyDigitalSignature)
            {
                pdf = DigitalSign(pdf, date_to_ds).Result;
            }

            return File(pdf, "application/pdf", fileName);
        }

        // ================================
        // 🔧 Helpers (same logic)
        // ================================

        private async Task<byte[]> DigitalSign(byte[] pdf, string date)
        {
            // Call your signing API here (same pattern as invoice)
            return pdf;
        }

        private string ExportDatatableToHtml(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='1'>");

            sb.Append("<tr>");
            foreach (DataColumn col in dt.Columns)
                sb.Append($"<td>{col.ColumnName}</td>");
            sb.Append("</tr>");

            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn col in dt.Columns)
                    sb.Append($"<td>{row[col]}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            return sb.ToString();
        }

        private string GenerateQRCodeBase64String(string qrcodeText)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrcodeText, QRCodeGenerator.ECCLevel.Q);

            int modules = qrCodeData.ModuleMatrix.Count;

            int pixelsPerModule = (int)Math.Floor(250.0 / modules);

            if (pixelsPerModule < 1)
                pixelsPerModule = 1;

            using (var qrCode = new QRCode(qrCodeData))
            {
                using (Bitmap bitmap = qrCode.GetGraphic(
                    pixelsPerModule,
                    Color.Black,
                    Color.White,
                    drawQuietZones: true))
                {
                    using (Bitmap finalBitmap = new Bitmap(250, 250))
                    {
                        using (Graphics g = Graphics.FromImage(finalBitmap))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                            g.DrawImage(bitmap, 0, 0, 250, 250);
                        }

                        using (MemoryStream ms = new MemoryStream())
                        {
                            finalBitmap.Save(ms, ImageFormat.Png);
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
        }

        [HttpPost]
        [Route("EditCreditNote")]
        public async Task<IActionResult> EditCreditNote([FromBody] CreditNoteEditRequest request)
        {
            var result = await _icredit.EditCreditNote(request);
            return Ok(result);
        }

        [HttpGet("CreditnoteEmployeeSearch")]
        public async Task<IActionResult> CreditnoteEmployeeSearch([FromQuery] string creditNoteNo)
        {
            var ds = await _icredit.CreditnoteEmployeeSearch(creditNoteNo);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }


    }
}