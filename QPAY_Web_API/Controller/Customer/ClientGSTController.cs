using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Customer;
using QPay.UI.Models;
using QPay.UI.Models.Customer;
using System.Data;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientGSTController : ControllerBase
    {
        private readonly IClientGSTRepository _igst;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public ClientGSTController(
            IClientGSTRepository igst, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._igst = igst;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllClientGSTDetails/{userId}")]
        public async Task<IActionResult> GetAllClientGSTDetails(int userId)
        {
            var response = await _igst.GetAllClientGSTDetails(userId);

            return Ok(response);
        }

        [HttpPost("PostAddClientGST")]
        public async Task<IActionResult> PostAddClientGST(ClientGSTRequest Request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _igst.PostAddClientGST(Request);
            return Ok(response);
        }

        [HttpGet("PostDeleteClientGST/{ClientGSTId}/{UserId}")]
        public async Task<IActionResult> PostDeleteClientGST(int ClientGSTId, int UserId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _igst.PostDeleteClientGST(ClientGSTId, UserId);
            return Ok(response);
        }


        [HttpPost]
        [Route("PostClientGSTUpload")]
        public async Task<IActionResult> PostClientGSTUpload(IFormFile file, [FromForm] string userId)
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


            var response = await _igst.PostClientGSTUpload(xmlInput, userId);

            return Ok(response);
        }
        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

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
        [HttpGet]
        [Route("ClientGSTExport/{userId}")]
        public IActionResult ClientGSTExport(int userId)
        {

            DataSet ds = _igst.ClientGSTExport(userId);
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
                    fileResponse.FileName = "ClientGST_Export_" + fileName + ".xlsx";
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
