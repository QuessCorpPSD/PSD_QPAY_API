using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.Repository.Common;
using QPay.UI.Models;
using QPay.UI.Models.Customer;
using System.Data;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAddressController : ControllerBase
    {
        private readonly IClientAddressRespository _iaddress;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public ClientAddressController(
            IClientAddressRespository iaddress, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._iaddress = iaddress;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllClientAddressDetails/{userId}")]
        public async Task<IActionResult> GetAllClientAddressDetails(int userId)
        {
            var response = await _iaddress.GetAllClientAddressDetails(userId);

            return Ok(response);
        }

        [HttpPost("PostAddClientAddress")]
        public async Task<IActionResult> PostAddClientAddress([FromBody] AddressRequest addressRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _iaddress.PostAddClientAddress(addressRequest);
            return Ok(response);
        }

        [HttpGet("PostDeleteClientAddress/{ClientAddressId}/{UserId}")]
        public async Task<IActionResult> PostDeleteClientAddress(int ClientAddressId,int UserId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _iaddress.PostDeleteClientAddress(ClientAddressId, UserId);
            return Ok(response);
        }


        [HttpPost]
        [Route("PostClientAddressUpload")]
        public async Task<IActionResult> PostClientAddressUpload(IFormFile file, [FromForm] string userId)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "ClientAddress";
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


            var response = await _iaddress.PostClientAddressUpload(xmlInput, userId);

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
        [Route("ClientAddressExport/{userId}")]
        public IActionResult ClientAddressExport(int userId)
        {

            DataSet ds = _iaddress.ClientAddressExport(userId);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("Department");
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
                    fileResponse.FileName = "ClientAddress_Export_" + fileName + ".xlsx";
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
