using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.Repository;
using QPay.BAL.Repository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI_Domain.Models.PurchaseOrder;
using System.Data;
using System.Xml.Serialization;
using static QPay.UI.Models.Invoice.POCulture;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class POCultureController : ControllerBase
    {
        private readonly IPOCultureRepository _POCulture;
        private readonly IConfiguration _configuration;
        public POCultureController(
                   IPOCultureRepository POCulture, IConfiguration configuration)
        {
            this._POCulture = POCulture;
            this._configuration = configuration;
        }
       
        [HttpGet]
        [Route("GetAllPoNumbers")]
        public async Task<IActionResult> GetAllPoNumbers(int companyId, int userId)
        {
            var result = await _POCulture.GetAllPONumbers(companyId, userId);

            return Ok(result);
        }
        [HttpGet]
        [Route("GetAllPOCulture/{companyId}/{userId}")]
        public async Task<IActionResult> GetAllPOCulture(int companyId, int userId)
        {
            var result = await _POCulture.GetAllPOCulture(companyId, userId);

            return Ok(result);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] POCultureRequest request)
        {
            var response = await _POCulture.Create(request, request.createdBy);

            if (response.Tables.Count > 0 && response.Tables[0].Rows.Count > 0)
            {
                string message = response.Tables[0].Rows[0]["Error_Message"]?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(message) && !message.Contains("Successfully"))
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("SerialNo", typeof(string));
                    dt.Columns.Add("Error_Message", typeof(string));
                    dt.Rows.Add("1", message);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    var output = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
                    return Ok(output);
                }

                var outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(outputResponse);
            }

            return Ok(new
            {
                StatusCode = "400",
                Message = "Details are not saved"
            });
        }
        [HttpPost]
        [Route("PostUploadPOCulture")]
        public async Task<IActionResult> PostUploadPOCulture(IFormFile file, [FromForm] string userId)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "POCulture";
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


            var response = await _POCulture.PostPOCulture(xmlInput, userId);

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
        [Route("POCultureExport/{companyId}/{userId}")]
        public IActionResult POCultureExport(int companyId, int userId)
        {
            DataSet ds = _POCulture.POCultureExport(companyId, userId);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("POCulture");
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
                    fileResponse.FileName = "POCulture_Export_" + fileName + ".xlsx";
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
