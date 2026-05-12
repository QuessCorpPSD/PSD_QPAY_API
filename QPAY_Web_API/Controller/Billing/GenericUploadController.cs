using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Billing;
using QPay.BAL.IRepository.Customer;
using QPay.UI.Common;
using QPay.UI.Models;
using System.Data;
using static QPay.UI.Billing.GenericUpload;

namespace QPay.API.Controller.Billing
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericUploadController : ControllerBase
    {
        private readonly IGenericUploadRepository _IRepository;
        private readonly IConfiguration _configuration;
        public GenericUploadController(IGenericUploadRepository IRepository , IConfiguration configuration)
        {
            this._IRepository = IRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("masters/{userId}")]
        public async Task<IActionResult> masters(int userId)
        {
            DataSet ds = await _IRepository.masters(userId);
            if (ds.Tables.Count == 0)
                return Ok(new List<object>());

            var result = ds.Tables[0].AsEnumerable()
                .Select(row => ds.Tables[0].Columns.Cast<DataColumn>()
                    .ToDictionary(col => col.ColumnName, col =>
                    {
                        return row[col];
                    }))
                .ToList();

            return Ok(result);
        }

        [HttpPost, Route("GetGenericTemplate")]
        public async Task<IActionResult> GetGenericTemplate([FromBody] UploadTypeClass req)
        {

            DataSet ds = await _IRepository.GetGenericTemplate(req.uploadType);
            if (ds != null && ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                ds.Tables[0].TableName = req.uploadType;

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    var ws = workbook.AddWorksheet(ds.Tables[i], ds.Tables[i].TableName);
                    ws.Table(0).ShowAutoFilter = false;
                    ws.Table(0).Theme = XLTableTheme.None;
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());
                    FileResponse fileResponse = new FileResponse();
                    string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                    fileResponse.FileName = req.uploadType+"_Template" + fileName;
                    fileResponse.File = bytes;

                    return Ok(fileResponse);
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

        [HttpPost]
        [Route("PostGenericUpload")]
        public async Task<IActionResult> PostGenericUpload(IFormFile file, [FromForm] string userId, [FromForm] string uploadType)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "GenericUpload";
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


            var response = await _IRepository.PostGenericUpload(xmlInput, userId, uploadType);

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



    }
}
