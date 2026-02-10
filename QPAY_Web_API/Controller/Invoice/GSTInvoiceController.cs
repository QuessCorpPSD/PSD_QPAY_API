using ClosedXML.Excel;
using ClosedXML.Excel;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Models.Invoice;
using QPay.UI.Invoice;
using QRCoder;
using SelectPdf;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;


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
        public ActionResult BulkDownload([FromBody] QPay.UI.Models.Invoice.BulkInvoices bulkInvoices)
        {
            string[] DownloadIds = bulkInvoices.invoiceIds.Select(id => id.ToString()).ToArray();
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
            byte[] bytes = null;

            // loops through the PDFs I need to create

            foreach (var invoicedetails in DownloadIds)
            {
                string[] idetails = invoicedetails.Split('|');
                int invoiceId = Convert.ToInt32(idetails[0]);
                //string companyCode = idetails[1];
                //string payPeriod = idetails[2];
                string fileName;
                string invoiceHtml;
                bool applyDigitalSignature;
                bool isHeaderFooter;
                string QRImageText;
                string QRImageBase64 = "";
                string dateToDs;
                bool isIRNGenerated;

                DataSet ds = GetInvoiceData(Convert.ToInt32(invoiceId));

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
                var newEntry = new ZipEntry(fileName);
                newEntry.DateTime = DateTime.Now;

                zipStream.PutNextEntry(newEntry);

                byte[] pdf = GetInvoicePdf(invoiceHtml, dateToDs, applyDigitalSignature, isHeaderFooter, isIRNGenerated, fileName);

                string dirPath = _configuration["CertificatePath"].ToString();

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
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcodeText, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                using (Bitmap qrBitmap = qrCode.GetGraphic(20))
                using (MemoryStream ms = new MemoryStream())
                {
                    qrBitmap.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
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
        public async Task<IActionResult> GetInvoiceStatus(InvoiceStatusUI request) =>
       Ok(await this._gstinvoiceRepository.GetInvoiceStatus(request));

        [HttpGet, Route("GetGSTCtcDeductionType")]
        public async Task<IActionResult> GetGSTCtcDeductionType() =>
           Ok(await this._gstinvoiceRepository.GetGSTCtcDeductionType());

        [HttpGet, Route("GetGSTNetDeductionType")]
        public async Task<IActionResult> GetGSTNetDeductionType() =>
           Ok(await this._gstinvoiceRepository.GetGSTNetDeductionType());

        [HttpPost, Route("GetGstRates")]
        public async Task<IActionResult> GetGstRates(GetGstRateRequest request) =>
      Ok(await this._gstinvoiceRepository.GetGstRates(request));


        [HttpPost, Route("GetParticulars")]
        public async Task<IActionResult> GetParticulars(SendRequest request) =>
     Ok(await this._gstinvoiceRepository.GetParticulars(request));

        [HttpPost, Route("GetPayPeriod")]
        public async Task<IActionResult> GetPayPeriod(PayPeriodRequest request) =>
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
        public async Task<IActionResult> GetAllInvoiceCancelDetails([FromBody] CancelRequest request)
        {
            var ds = await this._gstinvoiceRepository.GetAllInvoiceCancelDetails(request.Company_Id, request.PayPeriod_Id);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpPost, Route("BulkApproveInvoice")]
        public async Task<IActionResult> BulkApproveInvoice([FromBody] InvoiceCancelApprovalRequest request)
        {
            // Call repository
            var ds = await _gstinvoiceRepository.BulkApproveInvoice(request);

            // Only process credit note IRNs for SUCCESS invoices from backend
            if (ds?.CreditnoteInvoices?.InvoiceIds != null && ds.CreditnoteInvoices.InvoiceIds.Any())
            {
                string bulkInvoiceIds = string.Join(",", ds.CreditnoteInvoices.InvoiceIds);

                // Prepare payload for credit note IRN generation
                var invoiceDetails = await InitiateCreditNoteIRN(bulkInvoiceIds, request.userId);

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


        public async Task<EInvoice> InitiateCreditNoteIRN(
    string invoiceIds,
    string userId)
        {
            return await _gstinvoiceRepository.GetEInvoiceData(
                invoiceIds,
                userId,
                "GetEInvoiceCreditNoteData"
            );
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
                    message = await SaveBatchResponse(
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

        public async Task<string> SaveBatchResponse(
     int statusCode,
     string responseMessage,
     string response,
     string responseXml,
     string invoiceIds,
     string mode,
     string userId)
        {
            var message = await _gstinvoiceRepository.SaveBatchResponse(
                statusCode,
                responseMessage,
                response,
                responseXml,
                invoiceIds,
                mode,
                userId
            );

            return message;
        }
    }
}
