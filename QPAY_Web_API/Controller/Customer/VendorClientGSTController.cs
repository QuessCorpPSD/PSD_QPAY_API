using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Customer;
using QPay.UI.Models.Customer;
using QPay.UI.Models;
using System.Data;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorClientGSTController : ControllerBase
    {
        private readonly IVendorClientGstRepository _igst;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;
        public VendorClientGSTController (IVendorClientGstRepository igst, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._igst = igst;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllVendorClientGSTDetails/{userId}")]
        public async Task<IActionResult> GetAllVendorClientGSTDetails(int userId)
        {
            var response = await _igst.GetAllVendorClientGSTDetails(userId);

            return Ok(response);
        }

        [HttpPost("PostAddVendorClientGST")]
        public async Task<IActionResult> PostAddVendorClientGST([FromBody] VendorClientGSTRequest Request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _igst.PostAddVendorClientGST(Request);
            return Ok(response);
        }

        [HttpGet("PostDeleteVendorClientGST/{VendorClientGSTId}/{UserId}")]
        public async Task<IActionResult> PostDeleteVendorClientGST(int VendorClientGSTId, int UserId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _igst.PostDeleteVendorClientGST(VendorClientGSTId, UserId);
            return Ok(response);
        }


        [HttpPost]
        [Route("PostVendorClientGSTUpload")]
        public async Task<IActionResult> PostVendorClientGSTUpload(IFormFile file, [FromForm] string userId)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "ClientGST";
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


            var response = await _igst.PostVendorClientGSTUpload(xmlInput, userId);

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
        [Route("VendorClientGSTExport/{userId}")]
        public IActionResult VendorClientGSTExport(int userId)
        {

            DataSet ds = _igst.VendorClientGSTExport(userId);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("ClientGST");
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
                    fileResponse.FileName = "VendorClientGST_Export_" + fileName + ".xlsx";
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
