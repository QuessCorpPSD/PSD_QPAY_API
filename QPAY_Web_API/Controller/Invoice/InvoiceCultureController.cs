using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.Repository;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI_Domain.Models.PurchaseOrder;
using System.Data;
using System.Xml.Serialization;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceCultureController : ControllerBase
    {
        private readonly IInvoiceCultureRepository _InvoiceCulture;
        private readonly IConfiguration _configuration;
        public InvoiceCultureController(
                    IInvoiceCultureRepository invoiceCulture, IConfiguration configuration)
        {
            this._InvoiceCulture = invoiceCulture;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllServiceChargeMaster")]
        public async Task<IActionResult> GetAllServiceChargeMaster()
        {
            var response = await _InvoiceCulture.GetAllServiceChargeMaster();
            return Ok(response);
        }

        [HttpGet, Route("GetAllInvoiceType")]
        public async Task<IActionResult> GetAllInvoiceType()
        {
            var response = await _InvoiceCulture.GetAllInvoiceType();
            return Ok(response);
        }

        [HttpGet, Route("GetAllInvoiceCategories")]
        public async Task<IActionResult> GetAllInvoiceCategories()
        {
            var response = await _InvoiceCulture.GetAllInvoiceCategories();
            return Ok(response);
        }

        [HttpGet, Route("GetAllInvoiceCulture/{companyId}")]
        public async Task<IActionResult> GetAllInvoiceCulture(int companyId)
        {
            var response = await _InvoiceCulture.GetAllInvoiceCulture(companyId);
            return Ok(response);
        }

        //[HttpGet, Route("GetMapNameByService/{companyId}")]
        //public async Task<IActionResult> GetMapNameByService(int companyId)
        //{
        //    var response = await _InvoiceCulture.GetMapNameByService(companyId);
        //    if (response.Tables[0].Rows.Count > 0)
        //    {
        //        var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
        //        return Ok(_outputResponse);
        //    }
        //    else
        //    {
        //        return Ok(new { StatusCode = "400", Message = "No Paycode Assigned to this Company" });
        //    }
        //}

        [HttpGet, Route("GetAllPayCodeFromCompany/{companyId}")]
        public async Task<IActionResult> GetAllPayCodeFromCompany(int companyId)
        {
            var response = await _InvoiceCulture.GetAllPayCodeFromCompany(companyId);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No Paycode Assigned to this Company" });
            }
        }

        [HttpGet, Route("GetAllPayCodeFromCompanyOI/{companyId}")]
        public async Task<IActionResult> GetAllPayCodeFromCompanyOI(int companyId)
        {
            var response = await _InvoiceCulture.GetAllPayCodeFromCompanyOI(companyId);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No Paycode Assigned to this Company" });
            }
        }

        //Proc_GetAllPayCodeFromCompanyOtherincomePayCode
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] InvoiceStructureRequest request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _InvoiceCulture.Create(xml, request.createdBy, request.mode, request.parentDetail.InvoiceType);
            if (response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Error_Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Details are not saved" });
            }
        }

        [HttpPost]
        [Route("PostUploadInvoiceCulture")]
        public async Task<IActionResult> PostUploadInvoiceCulture(IFormFile file, [FromForm] string userId)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "InvoiceCulture";
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


            var response = await _InvoiceCulture.PostInvoiceCulture(xmlInput, userId);

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

        [HttpPost]
        [Route("InvoiceCultureExport/{userId}")]
        public IActionResult InvoiceCultureExport(int userId)
        {
            DataSet ds = _InvoiceCulture.InvoiceCultureExport(userId);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("InvoiceCulture");
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
                    fileResponse.FileName = "InvoiceCulture_Export_" + fileName + ".xlsx";
                    fileResponse.File = bytes;
                    return Ok(fileResponse);
                }
            }
            else
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "Failure",
                    Data = "",
                    Error = ""
                };
                return Ok(response);
            }

        }


       
    }
}
