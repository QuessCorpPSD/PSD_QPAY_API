using ClosedXML.Excel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Invoice;
using QRCoder;
using SelectPdf;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using QPay.UI.Models;
using System.IO.Compression;
using QPay.UI.Common;


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
                var invoice = await _gstinvoiceRepository.GetInvoiceDetailByInvoiceId(invoiceId);
                if (invoice == null) continue;

                var companyDetail = await _gstinvoiceRepository.CompanyPayPeriod(invoice.Pay_Period_Id);

                //var dynamicFolder = Path.Combine(dynamic, invoice.Invoice_Number);
                //if (!Directory.Exists(dynamicFolder))
                //{
                //    Directory.CreateDirectory(dynamicFolder);
                //}
                //Directory.CreateDirectory(dynamic);
                if (companyDetail == null) continue;

                // ---------- INVOICE PDF ----------
                var IRNStatusFolder = invoice.IsGenerated_IRN == 0 ? "DraftInvoice" : "IRN";
                string files = string.Format("{0}\\{1}\\{2}\\{3}\\{4}",
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
                        var invoiceupdate = _gstinvoiceRepository.IRNStatusGenerationUpdate(invoice.Invoice_Number).Result;
                        if (invoiceDetails != null)
                        {
                            IRNStatusFolder = invoiceDetails.IsGenerated_IRN == 0 ? "DraftInvoice" : "IRN";
                            files = string.Format("{0}\\{1}\\{2}\\{3}\\{4}",
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
                _logger.LogError(ex+ $"Failed to delete temp folder: {dynamic}");
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

        [HttpPost]
        [Route("DownloadByInvoiceId")]
        public async Task<IActionResult> DownloadByInvoiceId(InvoiceNumberLotUI invoiceNumberLotUI)
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


                _logger.LogError("Invoice Received" + JsonConvert.SerializeObject(invoiceNumberLotUI));
                string fileName;
                string invoiceHtml;
                bool applyDigitalSignature;
                bool isHeaderFooter;
                string QRImageText;
                string QRImageBase64 = "";
                string dateToDs;
                bool isIRNGenerated;
                int invoiceId = (int)invoiceNumberLotUI.Invoice_Id;
                _logger.LogError("Invoice Received" + JsonConvert.SerializeObject(invoiceNumberLotUI));
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

                var company = await this._gstinvoiceRepository.CompanyPayPeriod(invoiceNumberLotUI.Pay_Period_id);
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
                this._logger.LogError("exception : " + ex.Message);
            }



            return Ok(fileResponse);
        }
        private byte[] GetInvoicePdf(string invoiceHtml, string dateToDs, bool applyDigitalSignature = false, bool IsHeaderFooter = false, bool isIRNGenerated = false, string fileName = default(string))
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                string certPath = _configuration["CertificatePath"].ToString();
                string signapi = _configuration["SignApiUrl"].ToString();

                string watermarkText = _configuration["WatermarkText"].ToString();

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
                    pdf = DigitalSignature.AddSingleDiagonalWatermark(pdf, watermarkText, fontSize: 35, transparency: 35);
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


        private DataSet GetInvoiceData(int invoiceId)
        {
            var ds = _gstinvoiceRepository.GetInvoiceData(invoiceId);
            return (ds);
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
        [EnableCors("CorsPolicy")]
        [HttpGet]
        [Route("Download/{invoiceId}")]
        public async Task<IActionResult> Download(int invoiceId)
        {
            if (invoiceId <= 0)
                return BadRequest("Invalid Invoice Id");

            string fileName;
            string invoiceHtml;
            bool applyDigitalSignature;
            bool isHeaderFooter;
            string QRImageText;
            string QRImageBase64 = "";
            string dateToDs;
            bool isIRNGenerated;

            DataSet ds = GetInvoiceData(invoiceId);

            invoiceHtml = ds.Tables[0].Rows[0]["InvoiceHtml"].ToString();
            fileName = ds.Tables[0].Rows[0]["InvoiceNumber"] + ".pdf";
            applyDigitalSignature = ds.Tables[0].Columns.Contains("ApplyDigitalSignature")
                                    && Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyDigitalSignature"]);
            isHeaderFooter = ds.Tables[0].Columns.Contains("IsHeaderFooter")
                             && Convert.ToBoolean(ds.Tables[0].Rows[0]["IsHeaderFooter"]);
            isIRNGenerated = ds.Tables[0].Columns.Contains("IRN")
                             && Convert.ToBoolean(ds.Tables[0].Rows[0]["IRN"]);
            QRImageText = ds.Tables.Count > 1 && ds.Tables[1].Columns.Contains("QR_Image_Text")
                          ? ds.Tables[1].Rows[0]["QR_Image_Text"].ToString()
                          : "";

            if (!string.IsNullOrEmpty(QRImageText))
                QRImageBase64 = GenerateQRCodeBase64String(QRImageText);

            invoiceHtml = invoiceHtml.Replace("[QR_Image_Text]", QRImageBase64);

            dateToDs = ds.Tables[1].Rows[0]["date_to_ds"].ToString();

            byte[] pdf = GetInvoicePdf(invoiceHtml, dateToDs, applyDigitalSignature, isHeaderFooter, isIRNGenerated, fileName);
            var stream = new MemoryStream(pdf);
            stream.Position = 0;

            return File(stream, "application/pdf", fileName);


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

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DAL.Repository.GstInvoiceCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _gstinvoiceRepository.Create(request);
            return Ok(response);
        }

        [HttpGet, Route("GetGSTInvoiceType")]
        public async Task<IActionResult> GetGSTInvoiceType() =>
    Ok(await this._gstinvoiceRepository.GetGSTInvoiceType());

        [HttpPost, Route("GetGSTBillableType")]
        public async Task<IActionResult> GetGSTBillable_Type() =>
           Ok(await this._gstinvoiceRepository.GetGSTBillableType());
        [HttpPost, Route("GetInvoiceStatus")]
        public async Task<IActionResult> GetInvoiceStatus(UI.Models.Invoice.InvoiceStatusUI request) =>
       Ok(await this._gstinvoiceRepository.GetInvoiceStatus(request));

        [HttpGet, Route("GetGSTCtcDeductionType")]
        public async Task<IActionResult> GetGSTCtcDeductionType() =>
           Ok(await this._gstinvoiceRepository.GetGSTCtcDeductionType());

        [HttpGet, Route("GetGSTNetDeductionType")]
        public async Task<IActionResult> GetGSTNetDeductionType() =>
           Ok(await this._gstinvoiceRepository.GetGSTNetDeductionType());

        [HttpPost, Route("GetGstRates")]
        public async Task<IActionResult> GetGstRates(UI.Models.Invoice.GetGstRateRequest request) =>
      Ok(await this._gstinvoiceRepository.GetGstRates(request));


        [HttpPost, Route("GetParticulars")]
        public async Task<IActionResult> GetParticulars(UI.Models.Invoice.SendRequest request) =>
     Ok(await this._gstinvoiceRepository.GetParticulars(request));

        [HttpPost, Route("GetPayPeriod")]
        public async Task<IActionResult> GetPayPeriod(UI.Models.Invoice.PayPeriodRequest request) =>
  Ok(await this._gstinvoiceRepository.GetPayPeriod(request));


        [HttpPost, Route("Edit")]
        public async Task<IActionResult> Edit(GstInvoiceEditRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _gstinvoiceRepository.Edit(request);

            if (response == null)
                return NotFound("Invoice not found");

            return Ok(response);
        }

        [HttpPost]
        [Route("Reject")]
        public async Task<IActionResult> Reject(IFormFile file, [FromForm] string userId, [FromForm] string status)
        {

            if (file == null || file.Length == 0)
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "File is missing"
                });
            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "GSTReject";
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

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Excel sheet is empty or not formatted correctly"
                });
            ds.Tables[0].TableName = "GstInvoice";
          
            foreach (DataTable table in ds.Tables)
            {
                foreach (DataColumn col in table.Columns)
                {
                    col.ColumnName = col.ColumnName.Trim().Replace(" ", "_");
                }
            }
            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var repoResponse = await _gstinvoiceRepository.Reject(xmlInput, userId,status);
            return Ok(new
            {
                StatusCode = 200,
                Message = "Success",
                Data = new
                {
                    response = repoResponse
                }

            });
        }

        [HttpPost, Route("GetAllInvoiceCancelDetails")]
        public async Task<IActionResult> GetAllInvoiceCancelDetails()
        {
            var ds = await this._gstinvoiceRepository.GetAllInvoiceCancelDetails();

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpPost, Route("BulkApproveInvoice")]
        public async Task<IActionResult> BulkApproveInvoice([FromBody] UI.Models.Invoice.InvoiceCancelApprovalRequest request)
        {
            // Call repository
            var ds = await _gstinvoiceRepository.BulkApproveInvoice(request);

            // Only process credit note IRNs for SUCCESS invoices from backend
            if (ds?.CreditnoteInvoices?.InvoiceIds != null && ds.CreditnoteInvoices.InvoiceIds.Any())
            {
                string bulkInvoiceIds = string.Join(",", ds.CreditnoteInvoices.InvoiceIds);

                // Prepare payload for credit note IRN generation
                var invoiceDetails = InitiateCreditNoteIRN(bulkInvoiceIds, request.userId);

                string JsonString = JsonConvert.SerializeObject(invoiceDetails);

                try
                {
                    await CallFynamicsAPIForCreditNote(JsonString, bulkInvoiceIds, request.userId);
                }
                catch (Exception ex)
                {
                    // Log error, mark partial success
                    _logger.LogError(ex + "Credit note API call failed");
                    ds.Status = ds.Status == "SUCCESS" ? "PARTIAL_SUCCESS" : ds.Status;
                    ds.Message += " | Credit note API call failed";
                }
            }

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpPost, Route("BulkRejectCancelRequest")]
        public async Task<IActionResult> BulkRejectInvoice([FromBody] UI.Models.Invoice.InvoiceCancelApprovalRequest request)
        {
            var payload = await _gstinvoiceRepository.BulkRejectInvoice(request);
            return Ok(payload);
        }

        public EInvoice InitiateCreditNoteIRN(string invoiceIds, string userId)
        {
            var results = _gstinvoiceRepository.GetEInvoiceData(invoiceIds, userId, "GetEInvoiceCreditNoteData");

            return results;
        }

        public async Task<string> CallFynamicsAPIForCreditNote(
        string jsonString,
        string invoiceIds,
        string userId)
        {
            string message = "";

            try
            {
                string responseText = "";
                int statusCode;
                string responseMessage = "";
                string responseXml = "";

                string batchApiLink = _configuration["BatchApiLink"];

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // TLS 1.2
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    // ⚠️ Only if required (not recommended for prod)
                    ServicePointManager.ServerCertificateValidationCallback =
                        delegate { return true; };

                    var httpContent = new StringContent(
                        jsonString,
                        Encoding.UTF8,
                        "application/json");

                    var httpResponse = await httpClient.PostAsync(batchApiLink, httpContent);

                    statusCode = (int)httpResponse.StatusCode;
                    responseMessage = httpResponse.ReasonPhrase;

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        responseText = await httpResponse.Content.ReadAsStringAsync();

                        // Convert JSON → XML
                        var xmlDoc = JsonConvert.DeserializeXmlNode(responseText, "Response");
                        responseXml = xmlDoc?.InnerXml ?? "";
                    }
                    else
                    {
                        responseText = "Connection Failure";
                    }
                    message =  SaveBatchResponse(
                        statusCode,
                        responseMessage,
                        responseText,
                        responseXml,
                        invoiceIds,
                        "CreditNoteSaveBatchResponse",
                        userId);
                }
            }
            catch (Exception ex)
            {
                message = $"CallFynamicsAPI Error: {ex.Message}";
            }

            return message;
        }

        [HttpGet, Route("GetUploadedFile/{invoice_Id}")]
        public IActionResult GetUploadedFile(int invoice_Id)
        {
            try
            {
                var filejson = _gstinvoiceRepository.GetFilename(invoice_Id);
                var fileList = JsonConvert.DeserializeObject<List<FileJson>>(filejson);
                if (fileList == null || fileList.Count == 0 || string.IsNullOrEmpty(fileList[0]?.FilePath))
                {
                    return BadRequest(new { message = "FilePath not found." });
                }
                //string? fileName = fileList?[0].FileName;
                string? filePath = fileList?[0].FilePath;
                string? fileName = Path.GetFileName(filePath);
                string? fullPath = filePath.Replace(@"\", @"\\");
                this._logger.LogInfo("CancelDocPath"+fullPath);
                if (!System.IO.File.Exists(fullPath))
                {
                    this._logger.LogInfo("CancelDoc File Not found");
                    return BadRequest(new { message = "File not found." });
                }
                this._logger.LogInfo(fullPath);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string base64String = Convert.ToBase64String(fileBytes);
                FileResponse fileResponse = new FileResponse();
                fileResponse.FileName = fileName;
                fileResponse.File = base64String;

                return Ok(fileResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost, Route("GetAllAttribute")]
        public async Task<IActionResult> GetAllAttribute(UI.Models.Invoice.AttributeUI attributeUI)
        {
            var payload = await _gstinvoiceRepository.GetAllAttribute(attributeUI);

            var attribute = payload.Select(x => new UI.Models.Invoice.SelectedItems()
            {
                value = x.AttributeName,
                text = x.AttributeName
            }).ToList();

            return Ok(attribute.ToList());
        }

        [HttpPost]
        [Route("UploadAttributes")]
        public async Task<IActionResult> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
          [FromForm] string payperiodId, [FromForm] string CreatedBy)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _gstinvoiceRepository.UploadAttributes(file, CompanyId, payperiodId, CreatedBy);
            return Ok(result);
        }

        [HttpPost, Route("GetConsolidateInvoiceSummary")]
        public async Task<IActionResult> GetConsolidateInvoiceSummary(DownloadRegister downloadRegister)
        {
            DataTable dt = await _gstinvoiceRepository.GetConsolidateInvoiceSummary(downloadRegister.Company_Id, downloadRegister.Pay_Period_Id);
           // DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0) 
            {
                using var workbook = new XLWorkbook();
                {                    
                    var ws = workbook.AddWorksheet(dt, "InvoiceSummary");
                    ws.Table(0).ShowAutoFilter = false;
                    ws.Table(0).Theme = XLTableTheme.None;

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);

                        // 🔴 THIS LINE IS MANDATORY
                        stream.Position = 0;

                        var base64 = Convert.ToBase64String(stream.ToArray());

                        var fileResponse = new FileResponse
                        {
                            FileName = "Consolidated_InvoiceSummary_" +
                                       DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx",
                            File = base64
                        };

                        return Ok(fileResponse);
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
        [Route("EInvoiceError/{invoiceId}")]
        public async Task<IActionResult> EInvoiceError(int invoiceId)
        {
            FileResponse fileResponse = new FileResponse();
            try
            {
                DataSet ds = await _gstinvoiceRepository.GetEInvoiceError(invoiceId);

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
            DataSet ds = await _gstinvoiceRepository.GetEInvoiceErrorHover(invoiceId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("InitiateIRN")]
        public ActionResult InitiateIRN(InitiateIRN initiateIRN)
        {
            //string invoiceIds, int CompanyId, int PayPeriodId, string UserId
            //string[] invoiceIds = initiateIRN.invoiceIds.Select(id => id.ToString()).ToArray();
            string invoiceIds = string.Join(",", initiateIRN.invoiceIds);
            string UserId = initiateIRN.userId;
            var invoiceDetails = GetEInvoiceData(invoiceIds, UserId, "GetEInvoiceData");
            string JsonString = JsonConvert.SerializeObject(invoiceDetails).ToString();
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(invoiceDetails);


            Task<string> task = Task.Run(async () => await CallFynamicsAPI(JsonString, invoiceIds, UserId));
            string Response = task.Result;

            var payload = ResponseWrapManager.ResponseWrapper(Response, HttpContext);
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
            var Message = _gstinvoiceRepository.SaveBatchResponse(StatusCode, ResponseMessage, Response, ResponseXml, InvoiceIds, Mode, UserId);
            return Message;
        }


        public EInvoice GetEInvoiceData(string invoiceIds, string UserId, string Action)
        {
            var result = _gstinvoiceRepository.GetEInvoiceData(invoiceIds, UserId, Action);
            return result;
        }

        [HttpPost]
        [Route("PayRegisterDownload")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> PayRegisterDownload(DownloadRegister downloadRegister)
        {
            FileResponse fileResponse = new FileResponse();
            fileResponse = _gstinvoiceRepository.PayRegisterDownload(downloadRegister.Company_Id, downloadRegister.Pay_Period_Id, downloadRegister.Pay_Period);
            return Ok(fileResponse);
        }

        [HttpGet]
        [Route("GetAllInvoiceTypeColors")]
        public async Task<IActionResult> GetAllInvoiceTypeColors()
        {
            var ds = await _gstinvoiceRepository.GetAllInvoiceTypeColors();

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
    }
}
