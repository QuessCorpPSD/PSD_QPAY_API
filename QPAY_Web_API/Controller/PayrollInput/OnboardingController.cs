using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Common;
using QPay.DTo.Models.PayrollInput;
using QPay.IRepository.iRepository.PayrollInput;
using QPay.UI.Common;
using System.Data;
using System.Xml;

namespace Qzone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardingController : ControllerBase
    {
        private readonly IOnboardingRepository _ionboarding;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public OnboardingController(
            IOnboardingRepository iOnboarding, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._ionboarding = iOnboarding;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllOnboardingDetails/{companyCode}/{payPeriod}")]
        public async Task<IActionResult> GetAllOnboardingDetails(string companyCode, string? payPeriod)
        {
            var response = await _ionboarding.GetAllOnboardingDetails(companyCode, payPeriod);

            return Ok(response);
        }

        [HttpPost, Route("GetNewJoineeTemplate")]
        public IActionResult GetNewJoineeTemplate([FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] int flag, [FromForm] int mapNameId)
        {

            DataSet ds = _ionboarding.GetNewJoineeTemplate(companyId, payPeriodId, flag, mapNameId);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], GetSheetName(i, flag));
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        if (flag == 1)
                        {
                            fileResponse.FileName = "NewJoinee_Template" + fileName;
                        }
                        else if (flag == 2)
                        {
                            fileResponse.FileName = "Attendance_Template_Regular" + fileName;
                        }
                        else if (flag == 4)
                        {
                            fileResponse.FileName = "Attendance_Template_F&F" + fileName;
                        }
                        else if (flag == 5)
                        {
                            fileResponse.FileName = "OneTimeInput_Template" + fileName;
                        }
                        else if (flag == 8)
                        {
                            fileResponse.FileName = "MovetoQpay_ReviewData" + fileName;
                        }
                        else if (flag == 9)
                        {
                            fileResponse.FileName = "Attendance_Template_Maternity" + fileName;
                        }
                        fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                    }
                }
            }
            else
            {
                var response = new QPay.UI.Common.APIResponse<object>
                {
                    statuscode = 400,
                    message = "Failure",
                    data = "",
                    error = ""
                };
                return Ok(response);
            }

        }
        private string GetSheetName(int i, int flag)
        {
            string sheetName = string.Empty;
            if (flag == 1)
            {
                switch (i)
                {
                    case 0:
                        sheetName = "EMPLOYEE";
                        break;
                    case 1:
                        sheetName = "ADHOC";
                        break;

                    default:
                        sheetName = "";
                        return sheetName;
                }
                return sheetName;
            }
            else if (flag == 2)
            {
                switch (i)
                {
                    case 0:
                        sheetName = "EMPLOYEE";
                        break;
                    case 1:
                        sheetName = "PreviousLOP_LOPR";
                        break;

                    default:
                        sheetName = "";
                        return sheetName;
                }
                return sheetName;
            }
            else if (flag == 4)
            {
                switch (i)
                {
                    case 0:
                        sheetName = "EMPLOYEE";
                        break;
                    case 1:
                        sheetName = "PreviousLOP_LOPR";
                        break;

                    default:
                        sheetName = "";
                        return sheetName;
                }
                return sheetName;
            }
            else if (flag == 9)
            {
                switch (i)
                {
                    case 0:
                        sheetName = "EMPLOYEE";
                        break;
                    case 1:
                        sheetName = "PreviousLOP_LOPR";
                        break;

                    default:
                        sheetName = "";
                        return sheetName;
                }
                return sheetName;
            }
            else if (flag == 8)
            {
                switch (i)
                {
                    case 0:
                        sheetName = "Employee Detai";
                        break;
                    case 1:
                        sheetName = "Salary Detail";
                        break;

                    default:
                        sheetName = "";
                        return sheetName;
                }
                return sheetName;
            }
            else
            {
                switch (i)
                {
                    case 0:
                        sheetName = "EMPLOYEE";
                        break;
                    case 1:
                        sheetName = "ADHOC";
                        break;

                    default:
                        sheetName = "";
                        return sheetName;
                }
                return sheetName;
            }

        }

        [HttpPost, Route("MoveToQpay")]
        public async Task<IActionResult> MoveToQpay([FromBody] MoveOffer moveOffer)
        {

        string[] harbourIds = moveOffer.offerIds;

        // Create XML structure
        XmlDocument xmlDoc = new XmlDocument();
            XmlElement newDataSet = xmlDoc.CreateElement("NewDataSet");

            foreach (var id in harbourIds)
            {
                XmlElement offerDetails = xmlDoc.CreateElement("OFFER_DETAILS");
                XmlElement harbourId = xmlDoc.CreateElement("HARBOUR_ID");
                harbourId.InnerText = id;
                offerDetails.AppendChild(harbourId);
                newDataSet.AppendChild(offerDetails);
            }

            xmlDoc.AppendChild(newDataSet);

            string xmlString = xmlDoc.OuterXml;
            var response = await _ionboarding.MoveToQpay(xmlString, moveOffer.companyId, moveOffer.payPeriod, moveOffer.payPeriodId, moveOffer.userId);

            return Ok(response);
        }

        [HttpPost, Route("PostValidateOfferId")]
        public async Task<IActionResult> PostValidateOfferId([FromBody] OfferIds offerIds)
        {
            string[] harbourIds = offerIds.offerIds;

            // Create XML structure
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement newDataSet = xmlDoc.CreateElement("XmlDS");

            foreach (var id in harbourIds)
            {
                XmlElement offerDetails = xmlDoc.CreateElement("Table");
                XmlElement harbourId = xmlDoc.CreateElement("OfferID");
                harbourId.InnerText = id;
                offerDetails.AppendChild(harbourId);
                newDataSet.AppendChild(offerDetails);
            }
            
            xmlDoc.AppendChild(newDataSet);

            string xmlString = xmlDoc.OuterXml;
            var response = await _ionboarding.PostValidateOfferId(xmlString);
            return Ok(response);
        }

        [HttpPost, Route("PostRollbackOfferId")]
        public async Task<IActionResult> PostRollbackOfferId([FromBody] RollbackofferIds rollbackoffer)
        {
            string[] harbourIds = rollbackoffer.offerIds;
            string userId = rollbackoffer.userId;

            // Create XML structure
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement newDataSet = xmlDoc.CreateElement("XmlDS");

            foreach (var id in harbourIds)
            {
                XmlElement offerDetails = xmlDoc.CreateElement("Table");
                XmlElement harbourId = xmlDoc.CreateElement("OfferID");
                harbourId.InnerText = id;
                offerDetails.AppendChild(harbourId);
                newDataSet.AppendChild(offerDetails);
            }

            xmlDoc.AppendChild(newDataSet);

            string xmlString = xmlDoc.OuterXml;
            var response = await _ionboarding.PostRollbackOfferId(xmlString, userId);

            return Ok(response);
        }
        [HttpPost]
        [Route("PostNewJoineeData")]
        public async Task<IActionResult> PostNewJoineeData(IFormFile file, [FromForm] string companyCode,
            [FromForm] int companyId, [FromForm] string userId)
        {
            List<PayperiodDD> payperiod = new List<PayperiodDD>();
            payperiod = _ionboarding.GetCurrentPayperiod(companyId);
            int payPeriodId;
            string payPeriod = string.Empty;
            if (payperiod != null && payperiod.Any())
            {
                payPeriodId = payperiod[0].Payfrequencyid;
            }
            else
            {
                return BadRequest("No pay period found for the given company ID.");
            }

            if (file == null || file.Length == 0)
                return BadRequest ("File is missing.");

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
            DataSet ds =new DataSet("NewDataSet");
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return BadRequest ("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var response = await _ionboarding.PostNewJoineeData(xmlInput, companyCode, companyId, payPeriodId, serverpath, userId);

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

        [HttpPost]
        [Route("PostOneTimeInputData")]
        public async Task<IActionResult> PostOneTimeInputData(IFormFile file, [FromForm] string companyCode,
           [FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] string payPeriod, [FromForm] string userId)
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
            DataSet ds = new DataSet();
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

            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();

            var response = await _ionboarding.PostOneTimeInputData(xmlInput, companyId, payPeriodId, serverpath, userId);

            return Ok(response);
        }

        [HttpGet, Route("GetAllFinalSubmitDetails/{companyId}/{payPeriodId}/{inputId}/{userId}")]
        public async Task<IActionResult> GetAllFinalSubmitDetails(int companyId, int payPeriodId, int inputId, string userId)
        {
            
            string Action = string.Empty;
            switch (inputId)
            {
                case 1:
                    Action = "GetSalary";
                    break;
                
                case 2:
                    Action = "GetOI";
                    break;
                
                //case 3:
                //    Action = "GetRevised";
                //    break;

                default:
                    Action = "GetSalary";
                    break;
            }
            var response = await _ionboarding.GetAllFinalSubmitDetails(companyId, payPeriodId, Action, userId);

            return Ok(response);
        }

        [HttpPost]
        [Route("PostFinalSubmission")]
        public async Task<IActionResult> PostFinalSubmission([FromForm] string companyCode, [FromForm] int companyId, [FromForm] int payPeriodId,
            [FromForm] string payPeriod, [FromForm] string lotNumber, [FromForm] string userId, [FromForm] string remarks)
        {
            if (string.IsNullOrEmpty(companyCode) || string.IsNullOrEmpty(payPeriod) || string.IsNullOrEmpty(lotNumber))
            {
                return BadRequest("Company code, pay period, and lot number are required.");
            }
            var response = await _ionboarding.PostFinalSubmission(companyId, payPeriodId, lotNumber, userId, remarks);
            return Ok(response);
        }
        [HttpPost]
        [Route("AttributeTemplate")]
        public async Task<IActionResult> AttributeTemplate(AttributeRequestModel attributeRequestModel)
        {
            if (attributeRequestModel.FlagId == 1)
            {
                var attribute = await _ionboarding.AttributeTemplate(attributeRequestModel.FlagId, attributeRequestModel.CompanyId, attributeRequestModel.payPeriodId, attributeRequestModel.LotNo, attributeRequestModel.userId,"");
                return Ok(attribute);
            }
            else if (attributeRequestModel.FlagId == 2)
            {
                var bytes = Convert.FromBase64String(attributeRequestModel.uploadedFile);
                //var filePath = Path.Combine(_configuration["ClaimDocPath"].ToString(), "MapNameChanges");
                //if (!Directory.Exists(filePath))
                //{
                //    Directory.CreateDirectory(filePath);
                //}
                //string fileName = "FinalSubmittionMap" + System.DateTime.Now.ToString("ddMMyyyyhhss") + ".xlsx";
                //filePath = filePath + "\\" + fileName;
                //System.IO.File.WriteAllBytes(filePath, bytes);
                using var stream = new MemoryStream(bytes);
                DataTable dt = new DataTable();

                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);

                    bool firstRow = true;

                    foreach (var row in worksheet.RowsUsed())
                    {
                        if (firstRow)
                        {
                            foreach (var cell in row.CellsUsed())
                            {
                                dt.Columns.Add(cell.GetValue<string>());
                            }

                            firstRow = false;
                        }
                        else
                        {
                            DataRow dataRow = dt.NewRow();

                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                dataRow[i] = row.Cell(i + 1).GetValue<string>();
                            }

                            dt.Rows.Add(dataRow);
                        }
                    }
                }
                string xml = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    using StringWriter sw = new StringWriter();

                    dt.TableName = "Employee"; // Root node name
                    dt.WriteXml(sw, XmlWriteMode.WriteSchema);
                     xml = sw.ToString();

                }
                var attribute = await _ionboarding.AttributeTemplate(attributeRequestModel.FlagId, attributeRequestModel.CompanyId, attributeRequestModel.payPeriodId, attributeRequestModel.LotNo, attributeRequestModel.userId, xml);
                return Ok(attribute);

               

            }
            else
            {

                FileResponse fileResponse = new FileResponse();
                fileResponse.File = "N";
                fileResponse.FileName = "Attributeupload_" + System.DateTime.Now.ToString("ddMMyyyyssmm") + ".xlsx";
                return Ok(fileResponse);
            }


        }

        [HttpPost, Route("GetPayRegister")]
        public IActionResult DownloadPayRegister([FromForm] int companyId, [FromForm] string companyCode, [FromForm] int payPeriodId, [FromForm] string payPeriod, [FromForm] int lotNumber
            , [FromForm] int inputId, [FromForm] int flag)
        {
                try
                {
                    string Action = inputId switch
                    {
                        1 => "GetSalary",
                        2 => "GetOI",
                        _ => "GetSalary"
                    };
                    // Access config value
                    string baseDir = Path.Combine(_configuration["ClaimDocPath"].ToString());

                // Extract company code
                string companyCodeText = companyCode.Split('(')[0].Trim();

                // Build directory path
                string dirPath = Path.Combine(baseDir, companyCodeText, payPeriod, Convert.ToString(lotNumber));

                var filejson = _ionboarding.GetRegisterFilename(companyId, payPeriodId, lotNumber, Action, flag);
                var fileList = JsonConvert.DeserializeObject<List<FileJson>>(filejson);
                if (fileList == null || fileList.Count == 0 || string.IsNullOrEmpty(fileList[0]?.FileName))
                {
                    return BadRequest(new { message = "Filename not found." });
                }
                string? fileName = fileList?[0].FileName;
                string fullPath = Path.Combine(dirPath, fileName);

                if (!System.IO.File.Exists(fullPath))
                    return BadRequest(new { message = "File not found." });

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

        [HttpPost, Route("GetNewJoineeEmployeeId")]
        public IActionResult GetNewJoineeEmployeeId([FromForm] int companyId, [FromForm] string payPeriod, [FromForm] int lotNumber)
        {

            DataSet ds = _ionboarding.GetNewJoineeEmployeeId(companyId, payPeriod, lotNumber);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], GetSheetName2(i));
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "EmployeeId_Report" + fileName;
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
        private string GetSheetName2(int i)
        {
            string sheetName = string.Empty;
            switch (i)
            {
                case 0:
                    sheetName = "Employee Id Creation Report";
                    break;
                case 1:
                    sheetName = "New Joiner Salary";
                    break;

                default:
                    sheetName = "";
                    return sheetName;
            }
            return sheetName;

        }

        [HttpPost, Route("GetConsolidatePayRegister")]
        public async Task<IActionResult> GetConsolidatePayRegister([FromForm] int companyId, [FromForm] string companyCode, [FromForm] string payPeriod, [FromForm] int payPeriodId, [FromForm] string lotNumber)
        {
            FileResponse fileResponse = new FileResponse();
            fileResponse = _ionboarding.GetConsolidatePayRegister(companyId, companyCode, payPeriod, payPeriodId, lotNumber);
            return Ok(fileResponse);
        }

        [HttpPost, Route("GetConsolidatePayRegisterOT")]
        public async Task<IActionResult> GetConsolidatePayRegisterOT([FromForm] int companyId, [FromForm] string companyCode, [FromForm] string payPeriod, [FromForm] int payPeriodId, [FromForm] string lotNumber)
        {

            FileResponse fileResponse = new FileResponse();
            fileResponse = _ionboarding.GetConsolidatePayRegisterOT(companyId, companyCode, payPeriod, payPeriodId, lotNumber);
            return Ok(fileResponse);

        }
        //[HttpPost, Route("EmployeeTemplateImport")]

        //public IActionResult EmployeeTemplateImport([FromForm] IFormFile file, [FromForm] string userId, [FromForm] int companyId,
        //    [FromForm] int payPeriodId, [FromForm] string inputType, [FromForm] int lotNo)
        //{

        //    if (file == null || file.Length == 0)
        //        return BadRequest("File is missing.");

        //    string DirName = "";
        //    int inputId = 0;

        //    if (inputType == "Salary")
        //    {
        //        inputId = 1;
        //    }
        //    else if (inputType == "Other Input")
        //    {
        //        inputId = 2;
        //    }

        //    string basePath = _configuration["ClaimDocPath"].ToString();
        //    string dirPath = Path.Combine(basePath, "RevisedInput");
        //    if (!Directory.Exists(dirPath))
        //    {
        //        Directory.CreateDirectory(dirPath);
        //    }
        //    string fileExtention = Path.GetExtension(file.FileName.ToUpper());
        //    string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
        //    FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;

        //    string serverpath = Path.Combine(dirPath, FileName);

        //    using (var stream = new FileStream(serverpath, FileMode.Create))
        //    {
        //        file.CopyToAsync(stream);
        //    }
        //    DataSet ds = new DataSet("NewDataSet");
        //    ds = ExcelToDataSet(serverpath);
        //    //Convert dt to XML
        //    if (ds.Tables.Count == 0)

        //        return BadRequest("Excel sheet is empty or not formatted correctly.");

        //    // Convert DataTable to XML
        //    using var xmlWriter = new StringWriter();
        //    ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
        //    string xmlInput = xmlWriter.ToString();

        //    DataSet ds1 = _ionboarding.EmployeeTemplateImport(xmlInput, userId, companyId, payPeriodId, inputId, lotNo);
        //    if (ds1.Tables[0].Rows[0]["Result"].ToString() == "0")
        //    {
        //        using var workbook = new XLWorkbook();
        //        {
        //            if (ds1.Tables.Count > 1)
        //            {
        //                var ws = workbook.AddWorksheet(ds1.Tables[1], "Validations");
        //                ws.Table(0).ShowAutoFilter = false;
        //                ws.Table(0).Theme = XLTableTheme.None;
        //            }

        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                workbook.SaveAs(stream);
        //                var bytes = Convert.ToBase64String(stream.ToArray());
        //                FileResponse fileResponse = new FileResponse();
        //                string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
        //                fileResponse.FileName = "Revised_Validations" + fileName;
        //                fileResponse.File = bytes;

        //                return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
        //            }
        //        }
        //    }
        //    else if (ds1.Tables[0].Rows[0]["Result"].ToString() == "1")
        //    {
        //        var response = new APIResponse<object>
        //        {
        //            statuscode = 200,
        //            message = ds1.Tables[0].Rows[0]["Result"].ToString(),
        //            data = "",
        //            error = ""
        //        };
        //        return Ok(response);
        //    }
        //    else
        //    {
        //        var response = new APIResponse<object>
        //        {
        //            statuscode = 400,
        //            message = "Failure",
        //            data = "",
        //            error = ""
        //        };
        //        return Ok(response);
        //    }

        //}

        [HttpPost, Route("EmployeeTemplateImport")]
        public async Task<IActionResult> EmployeeTemplateImport(
    [FromForm] IFormFile file,
    [FromForm] string userId,
    [FromForm] int companyId,
    [FromForm] int payPeriodId,
    [FromForm] string inputType,
    [FromForm] int lotNo)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            int inputId = inputType switch
            {
                "Salary" => 1,
                "Other Input" => 2,
                _ => 0
            };

            string basePath = _configuration["ClaimDocPath"].ToString();
            string dirPath = Path.Combine(basePath, "RevisedInput");
            Directory.CreateDirectory(dirPath);

            string fileExt = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(file.FileName)
                              + DateTime.Now.ToString("_yyyyMMddHHmmssffff")
                              + fileExt;

            string serverPath = Path.Combine(dirPath, fileName);

            // ✅ Await async file copy to ensure it’s fully written
            await using (var stream = new FileStream(serverPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await file.CopyToAsync(stream);
            }

            // ✅ Ensure file is completely saved before reading
            DataSet ds = ExcelToDataSet(serverPath);

            if (ds.Tables.Count == 0)
                return BadRequest("Excel sheet is empty or not formatted correctly.");

            // Convert to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();

            DataSet ds1 = await _ionboarding.EmployeeTemplateImport(xmlInput, userId, companyId, payPeriodId, inputId, lotNo);

            if (ds1.Tables[0].Rows[0]["Result"].ToString() == "0")
            {
                using var workbook = new XLWorkbook();
                if (ds1.Tables.Count > 1)
                {
                    var ws = workbook.AddWorksheet(ds1.Tables[1], "Validations");
                    ws.Table(0).ShowAutoFilter = false;
                    ws.Table(0).Theme = XLTableTheme.None;
                }

                using var ms = new MemoryStream();
                workbook.SaveAs(ms);
                string bytes = Convert.ToBase64String(ms.ToArray());

                return Ok(new FileResponse
                {
                    FileName = "Revised_Validations" + DateTime.Now.ToString("_yyyyMMddHHmmssffff"),
                    File = bytes
                });
            }

            if (ds1.Tables[0].Rows[0]["Result"].ToString() == "1")
            {
                return Ok(new APIResponse<object>
                {
                    statuscode = 200,
                    message = "1",
                    data = "",
                    error = ""
                });
            }

            return Ok(new APIResponse<object>
            {
                statuscode = 400,
                message = "Failure",
                data = "",
                error = ""
            });
        }


        [HttpPost, Route("GetRevisedTemplate")]
        public async Task<IActionResult> GetRevisedTemplate([FromForm] int companyId, [FromForm] int payPeriodId,
            [FromForm] int mapNameId, [FromForm] string inputType, [FromForm] int lotNo)
        {
            int inputId = 0;

            if (inputType == "Salary")
            {
                inputId = 1;
            }
            else if (inputType == "Other Input")
            {
                inputId = 2;
            }

            DataSet ds = await _ionboarding.GetRevisedTemplate(companyId, payPeriodId, mapNameId, inputId, lotNo);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                if (inputId == 1)
                {
                    ds.Tables[0].TableName = "Attendance";
                    ds.Tables[1].TableName = "PreviousLOP_LOPR";
                    ds.Tables[2].TableName = "IncrementBreakup";
                    ds.Tables[3].TableName = "NewJoinee";
                    ds.Tables[4].TableName = "NewJoineeBreakup";
                    ds.Tables[5].TableName = "Table5";
                }
                else
                {
                    ds.Tables[0].TableName = "Attendance";
                }

                if (ds.Tables.Contains("Table5"))
                    ds.Tables.Remove("Table5");

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
                        fileResponse.FileName = "RevisedInput_Template" + fileName;
                        fileResponse.File = bytes;

                        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
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
        [Route("PostRevisedInput")]
        public async Task<IActionResult> PostRevisedInput(IFormFile file, [FromForm] string companyCode,
            [FromForm] int companyId, [FromForm] string payPeriod, [FromForm] int payPeriodId, [FromForm] string userId, [FromForm] string inputType, 
            [FromForm] int lotNo)
        {
            int inputId = 0;

            if (inputType == "Salary")
            {
                inputId = 1;
            }
            else if (inputType == "Other Input")
            {
                inputId = 2;
            }

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
            DataSet ds = new DataSet("NewDataSet");
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return BadRequest("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var response = await _ionboarding.PostRevisedInput(xmlInput, userId, companyCode, companyId, payPeriodId, inputId, lotNo, serverpath);

            return Ok(response);
        }

        [HttpPost, Route("GetInputautomationReport")]
        public async Task<IActionResult> GetInputautomationReport([FromForm] int companyId, [FromForm] int payPeriodId,
        [FromForm] int inputId, [FromForm] int lotNumber)
        {
            //int inputId = 0;

            //if (inputType == "Salary")
            //{
            //    inputId = 1;
            //}
            //else if (inputType == "Other Input")
            //{
            //    inputId = 2;
            //}

            DataSet ds = await _ionboarding.GetInputautomationReport(companyId, payPeriodId, inputId, lotNumber);
            if (ds != null && ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                ds.Tables[0].TableName = "New Joinee Employee id Creation";
                ds.Tables[1].TableName = "New Joinee Breakup";
                ds.Tables[2].TableName = "Attendance";
                ds.Tables[3].TableName = "Adhoc Or Pay Transaction";
                ds.Tables[4].TableName = "Increment Break up";
                ds.Tables[5].TableName = "LOP Details";
                ds.Tables[6].TableName = "New Joinee";
                ds.Tables[7].TableName = "Maternity status";
                ds.Tables[8].TableName = "Leave Availed Details";

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
                    fileResponse.FileName = "InputAutomationReport" + fileName;
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
        [Route("PostCustomerConfirmation")]
        public async Task<IActionResult> PostCustomerConfirmation([FromForm] string companyCode, [FromForm] int companyId, [FromForm] int payPeriodId,
        [FromForm] string payPeriod, [FromForm] string lotNumber, [FromForm] string userId)
        {
            if (string.IsNullOrEmpty(companyCode) || string.IsNullOrEmpty(payPeriod) || string.IsNullOrEmpty(lotNumber))
            {
                return BadRequest("Company code, pay period, and lot number are required.");
            }
            var response = await _ionboarding.PostCustomerConfirmation(companyId, payPeriodId, lotNumber, userId);
            return Ok(response);
        }

        [HttpPost]
        [Route("PostFinalSubmissionLotMerge")]
        public async Task<IActionResult> PostFinalSubmissionLotMerge(FinalSubmitMerge request)
        {

            var draftInformation = await _ionboarding.PostFinalSubmissionLotMerge(request);

            return Ok(draftInformation);
        }
    }
}
