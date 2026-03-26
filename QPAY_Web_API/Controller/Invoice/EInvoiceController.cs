using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Invoice;
using QPay.UI.Common;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using QRCoder;
using SelectPdf;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class EInvoiceController : ControllerBase
    {
        private readonly IEInvoiceRepository _ieinvoice;
        private readonly IConfiguration _configuration;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly HttpClient _httpClient;
        private ILogger<EInvoiceController> _logger;

        public EInvoiceController(IHttpClientFactory httpClientFactory, IEInvoiceRepository ieinvoice,  ILogger<EInvoiceController> logger, IInvoiceRepository invoiceRepository,  IConfiguration configuration)
        {
            _ieinvoice = ieinvoice;
            _configuration = configuration;            
            _invoiceRepository = invoiceRepository;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        [Route("GetAllInvoiceDetails/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> GetAllInvoiceDetails(int companyId, int payPeriodId)
        {
            var ds = await _ieinvoice.GetAllInvoiceDetails(companyId, payPeriodId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("EInvoiceExport/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> EInvoiceExport(int companyId, int payPeriodId)
        {
            var ds = await _ieinvoice.EInvoiceExport(companyId, payPeriodId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        public static byte[] AddSingleDiagonalWatermark(byte[] pdfBytes, string watermarkText, int fontSize = 45, int transparency = 50)
        {
            if (pdfBytes == null) throw new ArgumentNullException(nameof(pdfBytes));
            if (string.IsNullOrWhiteSpace(watermarkText)) throw new ArgumentNullException(nameof(watermarkText));
            PdfDocument doc = new PdfDocument(new MemoryStream(pdfBytes));
            try
            {
                foreach (PdfPage page in doc.Pages)
                {
                    PdfCanvas canvas = page;
                    var rect = page.ClientRectangle;
                    float pageW = page.ClientRectangle.Width;
                    float pageH = page.ClientRectangle.Height;
                    PdfFont font = doc.AddFont(PdfStandardFont.Helvetica);
                    font.Size = fontSize;
                    SizeF size = canvas.MeasureString(watermarkText, font);
                    PdfTextElement txt = new PdfTextElement(0, 0, watermarkText, font)
                    {
                        ForeColor = new PdfColor(180, 180, 180),
                        Transparency = transparency
                    };
                    txt.ForeColor = System.Drawing.Color.Gray;
                    float tx = (page.ClientRectangle.Width / 2f) - (size.Width / 2f);
                    float ty = (page.ClientRectangle.Height / 2f) - (size.Height / 2f);
                    txt.Translate(tx, ty);
                    txt.Rotate(-45f);
                    page.Add(txt);
                }
                using (var ms = new MemoryStream())
                {
                    doc.Save(ms);
                    return ms.ToArray();
                }
            }
            finally
            {
                doc.Close();
            }
        }





        //[HttpGet]
        //[Route("Download/{invoiceId}")]
        //public async Task<IActionResult> Download(int invoiceId)
        //{
        //    if (invoiceId <= 0)
        //        return BadRequest("Invalid Invoice Id");

        //    string fileName;
        //    string invoiceHtml;
        //    bool applyDigitalSignature;
        //    bool isHeaderFooter;
        //    string QRImageText;
        //    string QRImageBase64 = "";
        //    string dateToDs;
        //    bool isIRNGenerated;
        //    /*File Generated from Path*/
        //    var invoice = _invoiceRepository.GetInvoiceDetailByInvoiceId(invoiceId).Result;
        //    if (invoice != null)
        //    {
        //        var companydetail = _payrollRepository.CompanyPayPeriod(invoice.Pay_Period_Id).Result;
        //        if (companydetail != null)
        //        {
        //            var FilePath = "";
        //            var fileNames = string.Format("{0}_{1}_{2}", companydetail.Company_Code, companydetail.Pay_Period, invoice.Invoice_Number);
        //            if (invoice.IsGenerated_IRN == 0)

        //                FilePath = string.Format("{0}{1}/{2}/DraftInvoice/{3}.pdf",
        //                    _configuration["ClaimDocPath"].ToString(),
        //                    companydetail.Company_Code, companydetail.Pay_Period, fileNames);
        //            else
        //                FilePath = string.Format("{0}{1}/{2}/IRN/{3}.pdf",
        //                    _configuration["ClaimDocPath"].ToString(),
        //                    companydetail.Company_Code, companydetail.Pay_Period, fileNames);
        //            if (!System.IO.File.Exists(FilePath))
        //            {
        //                InvoiceNumberLotUI invoicedetails = new InvoiceNumberLotUI()
        //                {
        //                    Company_Id = invoice.Company_Id,
        //                    Pay_Period_id = invoice.Pay_Period_Id,
        //                    Invoice_Number = invoice.Invoice_Number

        //                };
        //                await DownloadByInvoiceId(invoicedetails);
        //            }
        //            byte[] existpdf = System.IO.File.ReadAllBytes(FilePath);
        //            return File(existpdf, "application/pdf", string.Format("{0}.pdf", invoice.Invoice_Number));
        //        }
        //    }

        //    DataSet ds = GetInvoiceData(invoiceId);

        //    invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
        //    fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";
        //    applyDigitalSignature = ds.Tables[0].Columns.Contains("ApplyDigitalSignature")
        //                            && Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyDigitalSignature"]);
        //    isHeaderFooter = ds.Tables[0].Columns.Contains("IsHeaderFooter")
        //                     && Convert.ToBoolean(ds.Tables[0].Rows[0]["IsHeaderFooter"]);
        //    isIRNGenerated = ds.Tables[0].Columns.Contains("IRN")
        //                     && Convert.ToBoolean(ds.Tables[0].Rows[0]["IRN"]);
        //    QRImageText = ds.Tables.Count > 1 && ds.Tables[1].Columns.Contains("QR_Image_Text")
        //                  ? ds.Tables[1].Rows[0]["QR_Image_Text"].ToString()
        //                  : "";

        //    if (!string.IsNullOrEmpty(QRImageText))
        //        QRImageBase64 = GenerateQRCodeBase64String(QRImageText);

        //    invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);

        //    dateToDs = ds.Tables[1].Rows[0]["date_to_ds"].ToString();

        //    byte[] pdf = GetInvoicePdf(invoiceHtml, dateToDs, applyDigitalSignature, isHeaderFooter, isIRNGenerated, fileName);

        //    var path = _configuration["ClaimDocPath"].ToString();

        //    //byte[] zipBytes = GetInvoicePdfAndPayRegisterZip(invoiceId,invoiceHtml, dateToDs,applyDigitalSignature, isHeaderFooter, isIRNGenerated, fileName);



        //    //string dirPath = _configuration["GstInvoiceForOtherApp"].ToString();

        //    //byte[] fileBytes = DownloadToFolder(pdf, dirPath, fileName);

        //    return File(pdf, "application/pdf", fileName);
        //}


        [HttpPost]
        [Route("DownloadByInvoiceId")]
        public async Task<IActionResult> DownloadByInvoiceId(UI.Invoice.InvoiceNumberLotUI invoiceNumberLotUI)
        {
            FileResponse fileResponse = new FileResponse() { File = "N", FileName = string.Format("{0}.pdf", invoiceNumberLotUI.Invoice_Number) };
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, "debug.txt");
            System.IO.File.AppendAllText(filePath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] PDF generation started{Environment.NewLine}",
            Encoding.UTF8);
            try
            {
                if (invoiceNumberLotUI.Invoice_Id <= 0)
                    return BadRequest("Invalid Invoice Id");


                this._logger.LogInformation("Invoice Received" + JsonConvert.SerializeObject(invoiceNumberLotUI));
                string fileName;
                string invoiceHtml;
                bool applyDigitalSignature;
                bool isHeaderFooter;
                string QRImageText;
                string QRImageBase64 = "";
                string dateToDs;
                bool isIRNGenerated;
                int invoiceId = (int)invoiceNumberLotUI.Invoice_Id;
                this._logger.LogInformation("Invoice Received" + JsonConvert.SerializeObject(invoiceNumberLotUI));
                System.IO.File.AppendAllText(filePath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {JsonConvert.SerializeObject(invoiceNumberLotUI)}{Environment.NewLine}",
            Encoding.UTF8);
                System.IO.File.AppendAllText(filePath,
               $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] SP Excution Started...{Environment.NewLine}",
           Encoding.UTF8);
                DataSet ds = GetInvoiceData(invoiceId);
                System.IO.File.AppendAllText(filePath,
             $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] SP Excution Completed...{Environment.NewLine}",
         Encoding.UTF8);
                invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
                fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";
                applyDigitalSignature = ds.Tables[0].Columns.Contains("ApplyDigitalSignature")
                                        && Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyDigitalSignature"]);
                isHeaderFooter = ds.Tables[0].Columns.Contains("IsHeaderFooter")
                                 && Convert.ToBoolean(ds.Tables[0].Rows[0]["IsHeaderFooter"]);
                isIRNGenerated = invoiceNumberLotUI.IsGenerated_IRN == 1 ? true : false;
                //ds.Tables[0].Columns.Contains("IRN") && Convert.ToBoolean(ds.Tables[0].Rows[0]["IRN"]);
                QRImageText = ds.Tables.Count > 1 && ds.Tables[1].Columns.Contains("QR_Image_Text")
                              ? ds.Tables[1].Rows[0]["QR_Image_Text"].ToString()
                              : "";

                if (!string.IsNullOrEmpty(QRImageText))
                    QRImageBase64 = GenerateQRCodeBase64String(QRImageText);

                invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);

                dateToDs = ds.Tables[1].Rows[0]["date_to_ds"].ToString();
                System.IO.File.AppendAllText(filePath,
             $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Converting Pdf started...{Environment.NewLine}",
         Encoding.UTF8);
                byte[] pdf = GetInvoicePdf(invoiceHtml, dateToDs, applyDigitalSignature, isHeaderFooter, isIRNGenerated, fileName);
                System.IO.File.AppendAllText(filePath,
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Converting Pdf compated...{Environment.NewLine}",
        Encoding.UTF8);
                var path = _configuration["ClaimDocPath"].ToString();

                var company = await this._ieinvoice.CompanyPayPeriod(invoiceNumberLotUI.Pay_Period_id);
                var companyPath = Path.Combine(_configuration["ClaimDocPath"].ToString(), company.Company_Code);
                var payperiodPath = Path.Combine(companyPath, company.Pay_Period);
                if (invoiceNumberLotUI.IsGenerated_IRN == 0)
                {
                    var Invoicepath = Path.Combine(payperiodPath, "DraftInvoice");

                    if (!Directory.Exists(Invoicepath))
                    {
                        Directory.CreateDirectory(Invoicepath);
                    }
                    string fileNames = string.Format("{0}_{1}_{2}{3}",
                               company.Company_Code,
                               company.Pay_Period,
                              invoiceNumberLotUI.Invoice_Number,
                               ".pdf");
                    Invoicepath = Invoicepath + "\\" + fileNames;

                    using (var fs = new FileStream(Invoicepath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(pdf, 0, pdf.Length);
                    }
                    fileResponse.File = "Y";
                    fileResponse.FileName = fileNames;
                }
                else
                {
                    var Invoicepath = Path.Combine(payperiodPath, "IRN");

                    if (invoiceNumberLotUI.Regenerate == 1)
                    {
                        if (Directory.Exists(Invoicepath))
                        {
                            Directory.Delete(Invoicepath, true);
                        }
                        Directory.CreateDirectory(Invoicepath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Invoicepath);
                    }
                    string fileNames = string.Format("{0}_{1}_{2}{3}",
                               company.Company_Code,
                               company.Pay_Period,
                              invoiceNumberLotUI.Invoice_Number,
                               ".pdf");
                    Invoicepath = Invoicepath + "\\" + fileNames;

                    using (var fs = new FileStream(Invoicepath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(pdf, 0, pdf.Length);
                    }
                    fileResponse.File = "Y";
                    fileResponse.FileName = fileNames;
                }

            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(filePath,
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.Message}...{Environment.NewLine}",
        Encoding.UTF8);
                this._logger.LogInformation("exception : " + ex.Message);
            }



            return Ok(fileResponse);
        }

        public static byte[] DownloadToFolder(byte[] byteToWrite, string dirPath, string fileName)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                string fullPath = System.IO.Path.Combine(dirPath, fileName);

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                System.IO.File.WriteAllBytes(fullPath, byteToWrite);

                return byteToWrite; // No need to re-read
            }
            catch
            {
                return null;
            }
        }


        private DataSet GetInvoiceData(int invoiceId)
        {
            var ds = _ieinvoice.GetInvoiceData(invoiceId);
            return (ds);
        }

        private byte[] GetInvoicePdfAndPayRegisterZip(int invoiceId, string invoiceHtml, string dateToDs, bool applyDigitalSignature = false,
            bool IsHeaderFooter = false, bool isIRNGenerated = false, string fileName = default(string))
        {
            try
            {
                byte[] pdfBytes = GetInvoicePdf(invoiceHtml, dateToDs, applyDigitalSignature, IsHeaderFooter, isIRNGenerated, fileName);
                if (pdfBytes == null)
                    throw new Exception("PDF generation failed.");
                string excelFolder = _configuration["PayRegisterDownloadPath"] ?? "";
                string excelFile = Path.Combine(excelFolder, $"PayRegister_{invoiceId}.xlsx");
                if (!System.IO.File.Exists(excelFile))
                    throw new FileNotFoundException("Excel file not found", excelFile);
                byte[] excelBytes = System.IO.File.ReadAllBytes(excelFile);
                using (MemoryStream zipStream = new MemoryStream())
                {
                    using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                    {
                        var pdfEntry = zip.CreateEntry($"Invoice_{invoiceId}.pdf", CompressionLevel.Fastest);
                        using (var entryStream = pdfEntry.Open())
                        {
                            entryStream.Write(pdfBytes, 0, pdfBytes.Length);
                        }
                        var excelEntry = zip.CreateEntry($"PayRegister_{invoiceId}.xlsx", CompressionLevel.Fastest);
                        using (var entryStream = excelEntry.Open())
                        {
                            entryStream.Write(excelBytes, 0, excelBytes.Length);
                        }
                    }
                    return zipStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating ZIP with PDF and Excel: " + ex.Message);
            }
        }


        private byte[] GetInvoicePdf(string invoiceHtml, string dateToDs, bool applyDigitalSignature = false, bool IsHeaderFooter = false, bool isIRNGenerated = false, string fileName = default(string))
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                string certPath = _configuration["CertificatePath"].ToString();
                string signapi = _configuration["SignApiUrl"].ToString();
                string watermarkImagePath = _configuration["WatermarkImagePath"].ToString();
                if (IsHeaderFooter == true)
                {
                    string FilePath = _configuration["PDFHeaderImg"].ToString();
                    string PDFFootertext = _configuration["PDFFootertext"].ToString();

                    string imgFile = System.IO.Path.Combine(FilePath);
                    // header settings
                    converter.Options.DisplayHeader = true;
                    converter.Header.DisplayOnFirstPage = true;
                    converter.Header.DisplayOnOddPages = true;
                    converter.Header.DisplayOnEvenPages = true;
                    converter.Header.Height = 80;
                    // create image element from file path with real image size
                    PdfImageSection headerHtml = new PdfImageSection(500, 0, 80, imgFile);
                    converter.Header.Add(headerHtml);
                    // header settings
                    converter.Options.DisplayFooter = true;
                    converter.Footer.DisplayOnFirstPage = true;
                    converter.Footer.DisplayOnOddPages = true;
                    converter.Footer.DisplayOnEvenPages = true;
                    converter.Footer.Height = 80;
                    //HttpUtility.HtmlDecode
                    string Footertext = HttpUtility.HtmlDecode(PDFFootertext).ToString();
                    PdfHtmlSection footerHtml = new PdfHtmlSection(50, 0, Footertext, string.Empty);
                    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    converter.Footer.Add(footerHtml);
                }

                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(invoiceHtml);
                byte[] pdf = doc.Save();
                doc.Close();
                string pfxFile = Path.Combine(certPath, "Certificate.pfx");
                string pfxPassword = "Pradeep@123";

                if (isIRNGenerated == false)
                {
                    pdf = DigitalSignature.AddSingleDiagonalWatermark(pdf, watermarkImagePath, fontSize: 35, transparency: 35);
                }
                // string tempPdfPath = Path.Combine(Path.GetTempPath(), "temp.pdf");
                //System.IO.File.WriteAllBytes(tempPdfPath, pdf);

                // // 4. Sign PDF using PFX

                //pdf = DigitalSignature.SignPdfDocument(pdf, pfxFile, pfxPassword,
                //   reason: "Approved",
                //   location: "Chennai",
                //   contactInfo: "qa@example.com");



                if (applyDigitalSignature)

                {

                    // pdf = DocumentSigner.DigitallySignPDFFileAdvanced(pdf, certPath, dateToDs);


                    Task<byte[]> task = Task.Run(async () => await CallSignApi(signapi, certPath, dateToDs, pdf));

                    pdf = task.Result;


                }
                return pdf;
            }
            catch { return null; }
        }


        public async Task<Byte[]> CallSignApi(string url, string certPath, string dsDate, byte[] pdfBytes)
        {
            var request = new
            {
                PdfBase64 = Convert.ToBase64String(pdfBytes),
                CertPath = certPath,
                DsToDate = dsDate,
                //WaterMarkImagePath=watermarkImagePath
            };

            string jsonString = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );

                // TLS 1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                byte[] signedPdfBytes = await response.Content.ReadAsByteArrayAsync();
                return signedPdfBytes;
            }
        }
        //private string GenerateQRCodeBase64String(string qrcodeText)
        //{
        //    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        //    {
        //        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcodeText, QRCodeGenerator.ECCLevel.Q);
        //        using (QRCode qrCode = new QRCode(qrCodeData))
        //        using (Bitmap qrBitmap = qrCode.GetGraphic(20))
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            qrBitmap.Save(ms, ImageFormat.Png);
        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //}
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

        [HttpGet]
        [Route("EInvoiceError/{invoiceId}")]
        public async Task<IActionResult> EInvoiceError(int invoiceId)
        {
            FileResponse fileResponse = new FileResponse();
            try
            {
                DataSet ds = await _ieinvoice.GetEInvoiceError(invoiceId);

                if (ds == null || ds.Tables.Count == 0)
                {
                    fileResponse.File = "No";
                }
                else
                {

                    using var workbook = new XLWorkbook();
                    {

                        var ws = workbook.AddWorksheet(ds.Tables[0], "IRNError");
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;

                        using (MemoryStream stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var bytes = Convert.ToBase64String(stream.ToArray());

                            fileResponse.FileName = "InputLot";
                            fileResponse.File = bytes;
                            //return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                fileResponse.File = "No";
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
            return Ok(fileResponse);
        }

        [HttpGet]
        [Route("EInvoiceErrorHover/{invoiceId}")]
        public async Task<IActionResult> EInvoiceErrorHover(int invoiceId)
        {
            DataSet ds = await _ieinvoice.GetEInvoiceErrorHover(invoiceId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("BulkDownload")]
        public async Task<ActionResult> BulkDownload([FromBody] UI.Models.Invoice.BulkInvoices bulkInvoices)
        {
            var basePath = _configuration["ClaimDocPath"]!;
            var dynamic = Path.Combine(basePath, Guid.NewGuid().ToString());

            // Ensure temp folder exists
            Directory.CreateDirectory(dynamic);

            foreach (var invoiceId in bulkInvoices.invoiceIds)
            {
                var invoice = await _ieinvoice.GetInvoiceDetailByInvoiceId(invoiceId);
                if (invoice == null) continue;

                var companyDetail = await _ieinvoice.CompanyPayPeriod(invoice.Pay_Period_Id);

                //var dynamicFolder = Path.Combine(dynamic, invoice.Invoice_Number);
                //if (!Directory.Exists(dynamicFolder))
                //{
                //    Directory.CreateDirectory(dynamicFolder);
                //}
                //Directory.CreateDirectory(dynamic);
                if (companyDetail == null) continue;

                // ---------- INVOICE PDF ----------
                var IRNStatusFolder = invoice.IsGenerated_IRN == 0 ? "Draft" : "IRN";
                string files = string.Format("{0}{1}\\{2}\\{3}\\{4}",
                            basePath,
                             companyDetail.Company_Code,
                             companyDetail.Pay_Period,
                             IRNStatusFolder,
                            string.Format("{0}_{1}_{2}.pdf", companyDetail.Company_Code, companyDetail.Pay_Period, invoice.Invoice_Number));


                if (!System.IO.File.Exists(files) || invoice.Regenerate == 1)
                {
                    var invoiceDetails = new InvoiceNumberLotUI
                    {
                        Company_Id = invoice.Company_Id,
                        Pay_Period_id = invoice.Pay_Period_Id,
                        Invoice_Number = invoice.Invoice_Number,
                        Invoice_Id = invoiceId,
                        IsGenerated_IRN = invoice.IsGenerated_IRN,
                        Regenerate = invoice.Regenerate
                    };

                    var result = await DownloadByInvoiceId(invoiceDetails); // should save PDF to invoiceFilePath
                    if (result is OkObjectResult okResult)
                    {
                        var value = okResult.Value;

                        // If anonymous/dynamic
                        dynamic data = value;
                        string InvoiceNumber = data.FileName;
                        var invoiceupdate = _ieinvoice.IRNStatusGenerationUpdate(invoice.Invoice_Number).Result;
                        if (invoiceDetails != null)
                        {
                            IRNStatusFolder = invoiceDetails.IsGenerated_IRN == 0 ? "DraftInvoice" : "IRN";
                            files = string.Format("{0}{1}\\{2}\\{3}\\{4}",
                                        basePath,
                                         companyDetail.Company_Code,
                                         companyDetail.Pay_Period,
                                         IRNStatusFolder,
                                        string.Format("{0}_{1}_{2}.pdf", companyDetail.Company_Code, companyDetail.Pay_Period, invoice.Invoice_Number));
                        }
                    }

                }


                var InvoiceSourcePath = Path.Combine(files);
                var InvoiceTargetPath = Path.Combine(dynamic, $"{invoice.Invoice_Number}.pdf");
                var invoiceFilePath = Path.Combine(files, $"{invoice.Invoice_Number}.pdf");
                if (System.IO.File.Exists(InvoiceSourcePath))
                {
                    System.IO.File.Copy(InvoiceSourcePath, InvoiceTargetPath, true);
                }

                // ---------- PAY REGISTER EXCEL ----------
                //var excelSourceFolder = Path.Combine(
                //    basePath,
                //    companyDetail.Company_Code,
                //    companyDetail.Pay_Period,
                //    invoice.Invoice_Number
                //);

                //var excelFileName = $"{companyDetail.Company_Code}_{companyDetail.Pay_Period}_{invoice.Invoice_Number}.xlsx";
                //var excelSourcePath = Path.Combine(excelSourceFolder, excelFileName);
                //var excelTargetPath = Path.Combine(dynamicFolder, excelFileName);

                //if (!System.IO.File.Exists(excelSourcePath))
                //{
                //    var invoiceDetails = new InvoiceNumberLotUI
                //    {
                //        Company_Id = invoice.Company_Id,
                //        Pay_Period_id = invoice.Pay_Period_Id,
                //        Invoice_Number = invoice.Invoice_Number,
                //        Data_from= invoice.Data_from
                //    };

                //    await _payregister.InvoicePayRegister(invoiceDetails);
                //}



                //if (System.IO.File.Exists(excelSourcePath))
                //{
                //    System.IO.File.Copy(excelSourcePath, excelTargetPath, true);
                //}
                //DataTable invoicesumary_dt = await _ieinvoice.GetInvoiceSummaryByInvoiceId(invoice.Invoice_Number);
                //var InvSourceFolder = Path.Combine(
                //    basePath,
                //    companyDetail.Company_Code,
                //    companyDetail.Pay_Period,
                //    invoice.Invoice_Number
                //);

                //var SummaryFileName = $"{invoice.Invoice_Number}_Summary.xlsx";
                //var SummarySourcePath = Path.Combine(""); // excelSourceFolder, excelFileName);
                //var SummaryTargetPath = Path.Combine(""); // dynamicFolder, excelFileName);
                //FileResponse fileResponse = new FileResponse();
                //using (MemoryStream stream = new MemoryStream())
                //{
                //    using var workbook = new XLWorkbook();
                //    {
                //        var ws = workbook.AddWorksheet(invoicesumary_dt, "Invoice Summary");
                //        workbook.SaveAs(stream);
                //        var bytes_summary = (stream.ToArray());
                //      System.IO.File.WriteAllBytes(dynamicFolder+"\\"+ SummaryFileName, bytes_summary);
                //        //  FileResponse fileResponse = new FileResponse();
                //        //fileResponse.FileName = "PayRegister.xlsx";
                //        //fileResponse.File = bytes_summary;
                //    }
                //}

                //var bytes = Convert.FromBase64String(fileResponse.File);

                //using (var fs = new FileStream(SummaryTargetPath, FileMode.Create, FileAccess.Write))
                //{
                //    fs.Write(bytes, 0, bytes.Length);
                //}
            }

            // ---------- ZIP CREATION ----------
            using var memoryStream = new MemoryStream();

            //using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            //{
            //    foreach (var file in Directory.GetFiles(dynamic))
            //    {
            //        zip.CreateEntryFromFile(file, Path.GetFileName(file));
            //    }
            //}

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var files = Directory.GetFiles(dynamic, "*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    var entryName = Path.GetRelativePath(dynamic, file);
                    var zipEntry = archive.CreateEntry(entryName, CompressionLevel.Fastest);

                    using var entryStream = zipEntry.Open();
                    using var fileStream = System.IO.File.OpenRead(file);
                    fileStream.CopyTo(entryStream);
                }
            }

            //return File(
            //    memoryStream.ToArray(),
            //    "application/zip",
            //    "Invoices.zip"
            //);
            //memoryStream.Position = 0;

            // ---------- CLEANUP ----------
            try
            {
                if (Directory.Exists(dynamic))
                    Directory.Delete(dynamic, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete temp folder: {dynamic}");
            }

            return File(memoryStream.ToArray(), "application/zip", "Invoices.zip");



            //string[] DownloadIds = bulkInvoices.invoiceIds.Select(id => id.ToString()).ToArray();

            //MemoryStream outputMemStream = new MemoryStream();

            //ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            //zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

            //byte[] bytes = null;

            //// loops through the PDFs I need to create
            //byte[] invoiceFileByte = null;
            //string fileName="";
            //foreach (var invoicedetails in DownloadIds)
            //{
            //    int invoiceId = Convert.ToInt32(invoicedetails);
            //    ZipEntry newEntry = null;

            //    var invoice = _invoiceRepository.GetInvoiceDetailByInvoiceId(invoiceId).Result;
            //    if (invoice != null)
            //    {
            //        var companydetail = _payrollRepository.CompanyPayPeriod(invoice.Pay_Period_Id).Result;
            //        if (companydetail != null)
            //        {
            //            var FilePath = "";
            //            fileName = invoice.Invoice_Number + ".pdf";
            //            newEntry= new ZipEntry(fileName);
            //            newEntry.DateTime = DateTime.Now;

            //            zipStream.PutNextEntry(newEntry);
            //            var fileNames = string.Format("{0}_{1}_{2}", companydetail.Company_Code, companydetail.Pay_Period, invoice.Invoice_Number);
            //            if (invoice.IsGenerated_IRN == 0)

            //                FilePath = string.Format("{0}{1}/{2}/DraftInvoice/{3}.pdf",
            //                    _configuration["ClaimDocPath"].ToString(),
            //                    companydetail.Company_Code, companydetail.Pay_Period, fileNames);
            //            else
            //                FilePath = string.Format("{0}{1}/{2}/IRN/{3}.pdf",
            //                    _configuration["ClaimDocPath"].ToString(),
            //                    companydetail.Company_Code, companydetail.Pay_Period, fileNames);
            //            if (!System.IO.File.Exists(FilePath))
            //            {
            //                InvoiceNumberLotUI invoice_details = new InvoiceNumberLotUI()
            //                {
            //                    Company_Id = invoice.Company_Id,
            //                    Pay_Period_id = invoice.Pay_Period_Id,
            //                    Invoice_Number = invoice.Invoice_Number
            //                };
            //                 DownloadByInvoiceId(invoice_details);
            //            }

            //            invoiceFileByte = System.IO.File.ReadAllBytes(FilePath);
            //           // return File(existpdf, "application/pdf", string.Format("{0}.pdf", invoice.Invoice_Number));
            //        }
            //    }
            //    else
            //    {



            //      //  string fileName;

            //        string invoiceHtml;

            //        bool applyDigitalSignature;

            //        bool isHeaderFooter;

            //        string QRImageText;

            //        string QRImageBase64 = "";

            //        string dateToDs;

            //        bool isIRNGenerated;

            //        DataSet ds = GetInvoiceData(Convert.ToInt32(invoiceId));
            //        invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
            //        fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";
            //        string companyCode = Clean(ds.Tables[0].Rows[0]["CompanyCode"].ToString());
            //        string payPeriod = Clean(ds.Tables[0].Rows[0]["PayPeriod"].ToString());
            //        string invoiceNumber = Clean(ds.Tables[0].Rows[0]["InvoiceNumber"].ToString());
            //        applyDigitalSignature = ds.Tables[0].Columns.Contains("ApplyDigitalSignature")
            //        && Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyDigitalSignature"]);
            //        isHeaderFooter = ds.Tables[0].Columns.Contains("IsHeaderFooter")
            //        && Convert.ToBoolean(ds.Tables[0].Rows[0]["IsHeaderFooter"]);

            //        isIRNGenerated = ds.Tables[0].Columns.Contains("IRN") && Convert.ToBoolean(ds.Tables[0].Rows[0]["IRN"]);

            //        QRImageText = ds.Tables.Count > 1 && ds.Tables[1].Columns.Contains("QR_Image_Text")
            //                      ? ds.Tables[1].Rows[0]["QR_Image_Text"].ToString()

            //                      : "";

            //        if (!string.IsNullOrEmpty(QRImageText))

            //            QRImageBase64 = GenerateQRCodeBase64String(QRImageText);

            //        invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);

            //        dateToDs = ds.Tables[1].Rows[0]["date_to_ds"].ToString();

            //        newEntry = new ZipEntry(fileName);
            //        newEntry.DateTime = DateTime.Now;
            //        zipStream.PutNextEntry(newEntry);

            //        invoiceFileByte = GetInvoicePdf(invoiceHtml, dateToDs, applyDigitalSignature, isHeaderFooter, isIRNGenerated, fileName);
            //        // byte[] zipBytes = GetInvoicePdfAndPayRegisterZip(invoiceId, invoiceHtml, dateToDs, applyDigitalSignature, isHeaderFooter, isIRNGenerated, companyCode, payPeriod, invoiceNumber, fileName);


            //        //return File(zipBytes, "application/zip", $"Invoice_{invoiceNumber}.zip");

            //    }
            //    var invoiceFileBytes = DownloadToFolder(invoiceFileByte, _configuration["GstInvoiceForOtherApp"], fileName);
            //    MemoryStream inStream = new MemoryStream(invoiceFileBytes);
            //    StreamUtils.Copy(inStream, zipStream, new byte[4096]);
            //    inStream.Close();
            //    zipStream.CloseEntry();
            //}

            //zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.

            //zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            //outputMemStream.Position = 0;

            //return File(outputMemStream.ToArray(), "application/octet-stream", "Invoices.zip");

        }
        private byte[] GetInvoicePdfAndPayRegisterZip(int invoiceId, string invoiceHtml, string dateToDs, bool applyDigitalSignature = false,
        bool IsHeaderFooter = false, bool isIRNGenerated = false, string companyCode = default(string), string payPeriod = default(string), string invoiceNumber = default(string), string fileName = default(string))
        {
            byte[] bytes = null;
            try
            {
                byte[] pdfBytes = GetInvoicePdf(invoiceHtml, dateToDs, applyDigitalSignature, IsHeaderFooter, isIRNGenerated, fileName);
                if (pdfBytes != null)
                {
                    string excelFolder = _configuration["ClaimDocPath"] ?? "";
                    excelFolder = Path.Combine(excelFolder, companyCode, payPeriod, invoiceNumber).Replace("\\", "/");
                    string excelFileName = $"{companyCode}_{payPeriod}_{invoiceNumber}.xlsx";
                    using (MemoryStream zipStream = new MemoryStream())
                    {
                        using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                        {
                            var pdfEntry = zip.CreateEntry($"Invoice_{invoiceNumber}.pdf", CompressionLevel.Fastest);
                            using (var entryStream = pdfEntry.Open())
                            {
                                entryStream.Write(pdfBytes, 0, pdfBytes.Length);
                            }
                            string excelFile = Path.Combine(excelFolder, excelFileName).Replace("\\", "/");
                            if (System.IO.File.Exists(excelFile))
                            {
                                byte[] excelBytes = System.IO.File.ReadAllBytes(excelFile);
                                if (excelBytes != null)
                                {
                                    var excelEntry = zip.CreateEntry($"PayRegister_{invoiceNumber}.xlsx", CompressionLevel.Fastest);
                                    using (var entryStream = excelEntry.Open())
                                    {
                                        entryStream.Write(excelBytes, 0, excelBytes.Length);
                                    }
                                }
                            }


                        }

                        return zipStream.ToArray();

                    }
                }
                return null;
            }

            catch (Exception ex)

            {

                throw new Exception("Error creating ZIP with PDF and Excel: " + ex.Message);

            }

        }



        //[HttpPost]
        //[Route("BulkDownload")]
        //public ActionResult BulkDownload([FromBody] BulkInvoices bulkInvoices)
        //{
        //    string[] DownloadIds = bulkInvoices.invoiceIds.Select(id => id.ToString()).ToArray();
        //    MemoryStream outputMemStream = new MemoryStream();
        //    ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

        //    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
        //    byte[] bytes = null;

        //    // loops through the PDFs I need to create

        //    foreach (var invoicedetails in DownloadIds)
        //    {
        //        string[] idetails = invoicedetails.Split('|');
        //        int invoiceId =Convert.ToInt32(idetails[0]);
        //        //string companyCode = idetails[1];
        //        //string payPeriod = idetails[2];
        //        string fileName;
        //        string invoiceHtml;
        //        bool applyDigitalSignature;
        //        bool isHeaderFooter;
        //        string QRImageText;
        //        string QRImageBase64 = "";
        //        string dateToDs;
        //        bool isIRNGenerated;

        //        DataSet ds = GetInvoiceData(Convert.ToInt32(invoiceId));

        //        invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
        //        fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";
        //        applyDigitalSignature = ds.Tables[0].Columns.Contains("ApplyDigitalSignature")
        //                                && Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyDigitalSignature"]);
        //        isHeaderFooter = ds.Tables[0].Columns.Contains("IsHeaderFooter")
        //                         && Convert.ToBoolean(ds.Tables[0].Rows[0]["IsHeaderFooter"]);
        //        isIRNGenerated = ds.Tables[0].Columns.Contains("IRN")
        //                   && Convert.ToBoolean(ds.Tables[0].Rows[0]["IRN"]);
        //        QRImageText = ds.Tables.Count > 1 && ds.Tables[1].Columns.Contains("QR_Image_Text")
        //                      ? ds.Tables[1].Rows[0]["QR_Image_Text"].ToString()
        //                      : "";

        //        if (!string.IsNullOrEmpty(QRImageText))
        //            QRImageBase64 = GenerateQRCodeBase64String(QRImageText);

        //        invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);

        //        dateToDs = ds.Tables[1].Rows[0]["date_to_ds"].ToString();
        //        var newEntry = new ZipEntry(fileName);
        //        newEntry.DateTime = DateTime.Now;

        //        zipStream.PutNextEntry(newEntry);

        //        //  byte[] pdf = GetInvoicePdf(invoiceHtml, dateToDs, applyDigitalSignature, isHeaderFooter, fileName);

        //        //string dirPath = _configuration["GstInvoiceForOtherApp"].ToString();

        //        //byte[] fileBytes = DownloadToFolder(pdf, dirPath, fileName);
        //        //MemoryStream inStream = new MemoryStream(fileBytes);
        //        //StreamUtils.Copy(inStream, zipStream, new byte[4096]);
        //        //inStream.Close();
        //        //zipStream.CloseEntry();
        //        byte[] zipBytes = GetInvoicePdfAndPayRegisterZip(invoiceId, invoiceHtml, dateToDs, applyDigitalSignature, isHeaderFooter, isIRNGenerated, fileName);

        //        return File(zipBytes, "application/pdf", fileName);
        //    }

        //    zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
        //    zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

        //    outputMemStream.Position = 0;

        //    return File(outputMemStream.ToArray(), "application/octet-stream", "Invoices.zip");
        //}
        [HttpPost]
        [Route("InitiateIRN")]
        public async Task<ActionResult> InitiateIRN(InitiateIRN initiateIRN)
        {
            string UserId = initiateIRN.userId ?? "0";
            IRNModelRequest request = new IRNModelRequest()
            {
                invoiceIds = initiateIRN.invoiceIds,
                Mode = "GetEInvoiceData",
                userId = UserId
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_configuration["IRNRequestURL"], content);
            var result = await response.Content.ReadAsStringAsync();
            var cleartaxRespose = Newtonsoft.Json.JsonConvert.DeserializeObject<ClearTaxResponse>(result);            
            var payload = ResponseWrapManager.ResponseWrapper(cleartaxRespose, HttpContext);
            return Ok(payload);
        }
        public EInvoice GetEInvoiceData(string invoiceIds, string UserId, string Action)
        {
            var result = _ieinvoice.GetEInvoiceData(invoiceIds, UserId, Action);
            return result;
        }
        [HttpGet]
        [Route("GetEInvoiceError/{invoiceId}")]
        public async Task<IActionResult> GetEInvoiceError(int invoiceId)
        {
            var ds = await _ieinvoice.GetEInvoiceError(invoiceId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);


        }
        public async Task<string> CallFynamicsAPI(string JsonString, string InvoiceIds, string UserId)
        {
            string Message = "";
            try
            {
                //ErrorLogException.ErrorLog().LogWebAPI("CallFynamicsAPI", "Starts", JsonString);

                string Response = "";
                int StatusCode;
                string ResponseMessage = "";
                string ResponseXml = "";
                string BatchApiLink = _configuration["BatchApiLink"].ToString();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //SecurityProtocolType.Ssl3;
                    // Skip validation of SSL/TLS certificate
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    var httpContent = new StringContent(JsonString, Encoding.UTF8, "application/json");
                    var responseMessage = await httpClient.PostAsync(BatchApiLink, httpContent);

                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        StatusCode = (int)responseMessage.StatusCode;
                        ResponseMessage = responseMessage.ReasonPhrase;
                        Response = responseMessage.Content.ReadAsStringAsync().Result;
                        var doc = JsonConvert.DeserializeXmlNode(Response, "Response");
                        ResponseXml = doc.InnerXml;

                        /*--------------DO NOT DELETE BELOW COMMENTED PART--------------------
                        Response responsejson = new Response();
                        responsejson = JsonConvert.DeserializeObject<Response>(Response);

                        XmlDocument xmlDoc = new XmlDocument();
                        XmlSerializer serializer = new XmlSerializer(responsejson.GetType());
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        using (MemoryStream ms = new MemoryStream())
                        {
                            serializer.Serialize(ms, responsejson, ns);
                            ms.Position = 0;
                            xmlDoc.Load(ms);

                            foreach (XmlNode node in xmlDoc)
                            {
                                if (node.NodeType == XmlNodeType.XmlDeclaration)
                                {
                                    xmlDoc.RemoveChild(node);
                                }
                            }
                            ResponseXml = xmlDoc.InnerXml;
                        }*/
                    }
                    else
                    {
                        StatusCode = (int)responseMessage.StatusCode;
                        ResponseMessage = responseMessage.ReasonPhrase;
                        Response = "Connection Failure";
                    }
                    Message = SaveBatchResponse(StatusCode, ResponseMessage, Response, ResponseXml, InvoiceIds, "SaveBatchResponse", UserId);
                }
            }
            catch (Exception ex)
            {
                Message = $"CallFynamicsAPI Error: {ex}";
            }
            return Message;
        }

        public string SaveBatchResponse(int StatusCode, string ResponseMessage, string Response, string ResponseXml, string InvoiceIds, string Mode, string UserId)
        {
            var Message = _ieinvoice.SaveBatchResponse(StatusCode, ResponseMessage, Response, ResponseXml, InvoiceIds, Mode, UserId);
            return Message;
        }

        [HttpGet]
        [Route("GetAllInvoiceTypeColors")]
        public async Task<IActionResult> GetAllInvoiceTypeColors()
        {
            var ds = await _ieinvoice.GetAllInvoiceTypeColors();

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        //[HttpPost]
        //[Route("PayRegisterDownload")]
        //[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        //public async Task<IActionResult> PayRegisterDownload(DownloadRegister downloadRegister)
        //{
        //    FileResponse fileResponse = new FileResponse();
        //    fileResponse = _ieinvoice.PayRegisterDownload(downloadRegister.Company_Id, downloadRegister.Pay_Period_Id, downloadRegister.Pay_Period);
        //    return Ok(fileResponse);
        //}

        [HttpPost]
        [Route("UploadAttributes")]
        public async Task<IActionResult> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
   [FromForm] string payperiodId, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _ieinvoice.UploadAttributes(file, CompanyId, payperiodId, CreatedBy);
            return Ok(result);
        }
        [HttpPost, Route("GetConsolidateInvoiceSummary")]
        public async Task<IActionResult> GetConsolidateInvoiceSummary(DownloadRegister downloadRegister)
        {
            DataSet ds = await _ieinvoice.GetConsolidateInvoiceSummary(downloadRegister.Company_Id, downloadRegister.Pay_Period_Id);
            DataTable dt = ds.Tables[0];

            FileResponse fileResponse = new FileResponse();

            if (dt.Rows.Count > 0) //&& dt1.Rows.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    var ws = workbook.AddWorksheet(dt, "InvoiceSummary");
                    ws.Table(0).ShowAutoFilter = false;
                    ws.Table(0).Theme = XLTableTheme.None;

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());

                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "Consolidated_InvoiceSummary" + fileName;
                        fileResponse.File = bytes;

                        return Ok(fileResponse);
                    }
                }
            }
            else
            {
                fileResponse.File = "No";
                return Ok(fileResponse);
            }

        }
        private string sheetName(int sheetId)
        {
            return sheetId switch
            {
                0 => "Net Pay Summary Report",
                1 => "Net Pay Summary Details",
                2 => "Partial Hold Summary Report",
                3 => "Gratuity Summary Report",
                4 => "DBT Hold Summary Report",
                5 => "Deduction Flush Out Report"
            };
            
        }

        [HttpGet, Route("GetNetPaySummary/{companyId}/{PayPeriodId}")]
        public async Task<IActionResult> GetNetPaySummary(int companyId,int PayPeriodId)
        {
            DataSet ds = await _ieinvoice.NetPaySummaryByCompanyIDAndPayperiodId(companyId, PayPeriodId);
            FileResponse fileResponse = new FileResponse();
            if (ds.Tables.Count > 0)
            {
                int i = 0;
                using var workbook = new XLWorkbook();
                {
                    foreach (DataTable table in ds.Tables)
                    {
                        var ws = workbook.AddWorksheet(table, sheetName(i));
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                        i++;
                    }
                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());

                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "Consolidated_InvoiceSummary" + fileName;
                        fileResponse.File = bytes;
                        return Ok(fileResponse);
                    }
                }
            }
            else
            {
                fileResponse.File = "N";
                return Ok(fileResponse);
            }
           



        }
    }
}
