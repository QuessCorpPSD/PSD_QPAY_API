using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Extensions;
using QPay.DTo.Models.PayrollInput;
using QPay.IRepository.iRepository.PayrollInput;
using QPay.UI.Common;
using System.Data;
using System.Xml;
using static QPay.DTo.Models.PayrollInput.Timesheet;

namespace Qzone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetRepository _itimesheet;
        private readonly IConfiguration _configuration;

        public TimesheetController(
           ITimesheetRepository itimesheet, IConfiguration configuration)
        {
            this._itimesheet = itimesheet;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("GetEmployeeTimesheetDaywise/{CompanyCode}/{GroupName}/{Empid}/{PayPriod_Id}/{City_Id}")]
        public async Task<IActionResult> GetEmployeeTimesheetDaywise(string CompanyCode, int GroupName,
    string Empid, int PayPriod_Id, int City_Id)
        {
            var ds = await _itimesheet.GetEmployeeTimesheetDaywise(
                CompanyCode,
                GroupName,
                Empid,
                PayPriod_Id,
                City_Id
            );

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetEmployeeTimesheetDaywiseDownload/{CompanyCode}/{GroupName}/{Empid}/{PayPriod_Id}/{City_Id}")]
        public async Task<IActionResult> GetEmployeeTimesheetDaywiseDownload(string CompanyCode, int GroupName,
    string Empid, int PayPriod_Id, int City_Id)
        {
            var ds = await _itimesheet.GetEmployeeTimesheetDaywiseDownload(
                CompanyCode,
                GroupName,
                Empid,
                PayPriod_Id,
                City_Id
            );

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("UploadDailyTimesheet")]
        public async Task<IActionResult> UploadDailyTimesheet(IFormFile file, [FromForm] string User,
          [FromForm] string CompanyCode, [FromForm] int SiteID, [FromForm] int Payperiod)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _itimesheet.UploadDailyTimesheet(file, User, CompanyCode, SiteID, Payperiod);
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadDocumentSingleMulitiple")]
        public async Task<IActionResult> UploadDocumentSingleMulitiple(IFormFile file, [FromForm] string User,
 [FromForm] string Employeeid, [FromForm] string CompanyCode, [FromForm] int Site_ID, [FromForm] int Payperiod_ID,
 [FromForm] string Payperiod)
        {
            var request = HttpContext.Request;

            var fullUrl = $"{request.Scheme}://{request.Host}";

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _itimesheet.UploadDocumentSingleMulitiple(file, User, Employeeid, CompanyCode, Site_ID,
                Payperiod_ID, Payperiod, fullUrl);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetTimesheetAttachment/{CompanyCode}/{Site_ID}/{Employee_Code}/{Payperiod}")]
        public async Task<IActionResult> GetTimesheetAttachment(string CompanyCode, int Site_ID,
        string Employee_Code, string Payperiod)
        {

            var res = await this._itimesheet.GetTimesheetAttachment(
                CompanyCode,
                Site_ID,
                Employee_Code,
                Payperiod
            );
            return Ok(res);

        }

        [HttpPost("SaveTimesheet")]
        public async Task<IActionResult> SaveTimesheet([FromBody] TimesheetRequestDto request)
        {
            var res = await this._itimesheet.SaveTimesheet(request);
            return Ok(res);
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

        [HttpPost]
        [Route("PostAttendanceData")]
        public async Task<IActionResult> PostAttendanceData(IFormFile file, [FromForm] string companyCode,
           [FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] string payPeriod,
           [FromForm] string userId, [FromForm] string ISFANDF)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            string[] Code = companyCode.ToString().Split('(');
            DirName += Code[0].ToString();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            DirName += "\\" + payPeriod.ToString();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            string fileExtention = Path.GetExtension(file.FileName.ToUpper());
            string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
            FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
            //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
            string serverpath = DirName + FileName;

            using (var stream = new FileStream(serverpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            DataSet ds = new DataSet("DocumentElement");
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)
                return BadRequest("Excel sheet is empty or not formatted correctly.");
            DataSet dscolumns = new DataSet();
            foreach (DataTable dt in ds.Tables)
            {
                DataTable newTable = dt.Clone();

                if (dt.Rows.Count > 0)
                    newTable.ImportRow(dt.Rows[0]);

                dscolumns.Tables.Add(newTable);
            }

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            using var xmlWriter2 = new StringWriter();

            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();
            string xmlInput2 = xmlWriter2.ToString();

            var response = await _itimesheet.PostAttendanceData(xmlInput, companyId, payPeriodId, serverpath, xmlInput2, userId, ISFANDF);
            return Ok(response);
        }

        [HttpPost]
        [Route("VerifyAttendanceHeaders")]
        public async Task<IActionResult> VerifyAttendanceHeaders(IFormFile file, [FromForm] string companyCode,
           [FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] string payPeriod)
        {

            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            string[] Code = companyCode.ToString().Split('(');
            DirName += Code[0].ToString();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            DirName += "\\" + payPeriod.ToString();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            string fileExtention = Path.GetExtension(file.FileName.ToUpper());
            string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
            FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
            //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
            string serverpath = DirName + FileName;

            using (var stream = new FileStream(serverpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            DataSet ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)
                return BadRequest("Excel sheet is empty or not formatted correctly.");
            DataSet dscolumns = new DataSet();
            foreach (DataTable dt in ds.Tables)
            {
                DataTable newTable = dt.Clone();

                if (dt.Rows.Count > 0)
                    newTable.ImportRow(dt.Rows[0]);

                dscolumns.Tables.Add(newTable);
            }

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            dscolumns.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();

            var response = await _itimesheet.VerifyAttendanceHeaders(xmlInput, companyId, payPeriodId, serverpath);

            return Ok(response);
        }


        //[HttpGet, Route("GetUnseizeData/{companyCode}/{payPeriod}/{siteCode}/{city_Id}/{empid}")]
        //public async Task<IActionResult> GetUnseizeData(string companyCode, int payPeriod, int siteCode
        //   , int city_Id, string empid)
        //{
        //    var response = await _itimesheet.GetUnseizeData(companyCode, payPeriod, siteCode, city_Id, empid);

        //    return Ok(response);
        //}
        [HttpGet, Route("PostUnseize/{empIdJson}/{companyId}/{payPeriodId}/{siteCode}/{userId}")]
        public async Task<IActionResult> PostUnseize(string empIdJson, int companyId, int payPeriodId, int siteCode, string userId)
        {

            string[] empIds = JsonConvert.DeserializeObject<string[]>(empIdJson);

            // Create XML structure
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement newDataSet = xmlDoc.CreateElement("NewDataSet");

            foreach (var id in empIds)
            {
                XmlElement offerDetails = xmlDoc.CreateElement("Table");
                XmlElement harbourId = xmlDoc.CreateElement("EmployeeID");
                harbourId.InnerText = id;
                offerDetails.AppendChild(harbourId);
                newDataSet.AppendChild(offerDetails);
            }

            xmlDoc.AppendChild(newDataSet);

            string xmlString = xmlDoc.OuterXml;
            var response = await _itimesheet.PostUnseize(xmlString, companyId, payPeriodId, siteCode, userId);

            return Ok(response);
        }

        //[HttpGet, Route("GetUnseizeAttachment/{companyCode}/{siteId}/{empCode}/{payPeriod}")]
        //public async Task<IActionResult> GetUnseizeAttachment(string companyCode, int siteId, string empCode, string payPeriod)
        //{
        //    var response = await _itimesheet.GetUnseizeAttachment(companyCode, siteId, empCode, payPeriod);

        //    return Ok(response);
        //}
        [HttpPost, Route("GetUnseizeFile")]
        public IActionResult GetUnseizeFile([FromForm] string filePath, [FromForm] string fileName)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                    return BadRequest(new { message = "File not found." });

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
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

        [HttpGet, Route("GetTimesheetClientTemplate/{Company_Id}/{PayPeriod_Id}")]
        public async Task<IActionResult> GetTimesheetClientTemplate(string Company_Id, string PayPeriod_Id)
        {
            DataSet ds = await _itimesheet.GetTimesheetClientTemplate(Company_Id, PayPeriod_Id);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], GetSheetName(i));
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "Client_Template_" + fileName;
                        fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
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

        [HttpPost]
        [Route("DeleteTimesheetAttachment")]
        public async Task<IActionResult> DeleteTimesheetAttachment(DeleteAttachmentRequest request)
        {
            var res = await this._itimesheet.DeleteTimesheetAttachment(request);
            return Ok(res);
        }

        [HttpPost("SaveTimesheetPreviousMonth")]
        public async Task<IActionResult> SaveTimesheetPreviousMonth([FromBody] TimesheetRequestDto request)
        {
            var res = await this._itimesheet.SaveTimesheetPreviousMonth(request);
            return Ok(res);
        }

        private string GetSheetName(int i)
        {
            string sheetName = string.Empty;
            switch (i)
            {
                case 0:
                    sheetName = "NEW JOINEE";
                    break;
                case 1:
                    sheetName = "ATTENDANCE AND ONE TIME";
                    break;
                case 2:
                    sheetName = "EMPLOYEE MASTER CHANGES";
                    break;
                case 3:
                    sheetName = "SALARY REVISION";
                    break;
                default:
                    sheetName = "";
                    return sheetName;
            }
            return sheetName;
        }
    }
}
