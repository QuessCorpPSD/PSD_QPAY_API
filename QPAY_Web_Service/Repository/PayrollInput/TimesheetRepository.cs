using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.DAL.Repository;
using QPay.IRepository.iRepository.PayrollInput;
using System.Data;
using System.Globalization;
using System.Text;
using static QPay.BAL.Repository.EInvoiceRepository;
using static QPay.DTo.Models.PayrollInput.Timesheet;

namespace QPay.IRepository.Repository.PayrollInput
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public TimesheetRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetEmployeeTimesheetDaywise(string CompanyCode, int GroupName, string Empid,
            int PayPriod_Id, int City_Id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyCode"] = CompanyCode,
                ["@GroupName"] = GroupName,
                ["@Empid"] = Empid,
                ["@PayPriod_Id"] = PayPriod_Id,
                ["@City_Id"] = City_Id,
                ["@EmployeeCode"] = null,
                ["@EmployeeName"] = null
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_ViewAttendance_QzoneNewUI", parameters, 1500);
        }

        public async Task<DataSet> GetEmployeeTimesheetDaywiseDownload(string CompanyCode, int GroupName, string Empid,
   int PayPriod_Id, int City_Id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyCode"] = CompanyCode,
                ["@GroupName"] = GroupName,
                ["@Empid"] = Empid,
                ["@PayPriod_Id"] = PayPriod_Id,
                ["@City_Id"] = City_Id
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_ViewAttendancefordownload_QzoneNewUI", parameters, 1500);
        }

        public async Task<TimesheetResponse> UploadDailyTimesheet(IFormFile file, [FromForm] string User,
          [FromForm] string CompanyCode, [FromForm] int SiteID, [FromForm] int Payperiod)
        {
            TimesheetResponse timesheetDetails = new TimesheetResponse();

            if (file != null && file.Length != 0)
            {

                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Daily_Timesheet");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Daily_Timesheet_{CompanyCode}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    timesheetDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return timesheetDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName.Equals("OTWKD") ||
            col.ColumnName.Equals("OTWND") ||
            col.ColumnName.Equals("NSOTH") ||
            col.ColumnName.Equals("POTRS"))
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[col] != DBNull.Value && !string.IsNullOrWhiteSpace(row[col].ToString()))
                                {
                                    // Keep integers clean, decimals with up to 2 digits
                                    row[col] = Convert.ToDecimal(row[col]).ToString("0.##");
                                }
                            }

                            // Change column type to string so XML writes it as text
                            col.DataType = typeof(string);
                        }

                        if (char.IsDigit(col.ColumnName[0]))
                        {
                            col.ColumnName = "A" + col.ColumnName;
                        }
                    }

                    DataTable newTable = dt.Clone();

                    foreach (DataColumn col in newTable.Columns)
                    {
                        if (char.IsDigit(col.ColumnName[0]))
                        {
                            col.ColumnName = "A" + col.ColumnName;
                        }
                    }

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                string FromDate = dtToSerilize.Columns[9].ToString();
                int count = dtToSerilize.Columns.Count;
                var lastColumn = dtToSerilize.Columns[count - 1];
                string Todate = lastColumn.ColumnName;

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Upload_Attendancetimesheet_QzoneNewUI";
                var parameters = new DynamicParameters();

                parameters.Add("@CreatedBy", User);
                parameters.Add("@Xml_file", xmlInput);
                parameters.Add("@FromDate", FromDate.Replace("A", ""));
                parameters.Add("@ToDate", Todate.Replace("A", ""));
                //parameters.Add("@FromDate", FromDate);
                //parameters.Add("@ToDate", Todate);
                parameters.Add("@CompanyCode", CompanyCode);
                parameters.Add("@SiteID", SiteID);
                parameters.Add("@Payperiod", Payperiod);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message == "2")
                        {
                            timesheetDetails.response = "Import Successfully Done.";
                        }
                        else
                        {
                            timesheetDetails.response = "Failed to import.";
                            timesheetDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        timesheetDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    timesheetDetails.response = "Failed";
                }

            }
            else
            {
                timesheetDetails.response = "File not found";
            }
            return timesheetDetails;
        }

        public async Task<TimesheetResponse> UploadDocumentSingleMulitiple(IFormFile file, [FromForm] string User,
 [FromForm] string Employeeid, [FromForm] string CompanyCode, [FromForm] int Site_ID, [FromForm] int Payperiod_ID,
 [FromForm] string Payperiod, [FromForm] string fullUrl)
        {
            TimesheetResponse timesheetDetails = new TimesheetResponse();

            string[] payperiodsplit = Payperiod.ToString().Split(' ');
            string month = payperiodsplit[0];
            string year = payperiodsplit[1];
            int Selectedyear = Convert.ToInt32(year);    //Convert.ToInt32(ddlYear.SelectedValue);
            int selectedMonth = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;
            string sFileName = "";
            string fileNameToDelete = "";

            string SelectedMonth = "";
            if (selectedMonth < 10)
            {
                SelectedMonth = "0" + Convert.ToString(selectedMonth);
            }
            else
            {
                SelectedMonth = Convert.ToString(selectedMonth);
            }

            if (file != null && file.Length != 0)
            {
                var DirName = Path.Combine(_configuration["ClaimDocPath"].ToString(),"Timesheet", year, SelectedMonth);
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                sFileName = file.FileName;
                sFileName = sFileName.Replace(" ", "");
                string Extension = Path.GetExtension(sFileName);

                string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(sFileName);
                string sfilewithExtension = FileNameWithoutExtension + Extension;
                fileNameToDelete = Guid.NewGuid().ToString() + sfilewithExtension;
                string excelPath = DirName + "\\" + fileNameToDelete;

                using (var stream = new FileStream(excelPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                string QzoneUrl = _configuration["QZoneURL"].ToString();
                string ReplaceUrl = _configuration["ReplaceURL"].ToString();

                string excelPath1 = excelPath.Replace("\\", "/");
                string excelPathFinal = excelPath1.Replace(ReplaceUrl, QzoneUrl);

                string storeProcedure = "";

                if (Employeeid.Contains(","))
                {
                    storeProcedure = "Proc_Upload_document";//Multiple file attachment
                }
                else
                {
                    storeProcedure = "Proc_Upload_document_Single";//Single file attachment
                }
                var parameters = new DynamicParameters();

                parameters.Add("@createdby", User);
                parameters.Add("@Employeeid", Employeeid);
                parameters.Add("@Filepath", excelPathFinal);
                parameters.Add("@Month", SelectedMonth);
                parameters.Add("@filename", excelPathFinal);
                parameters.Add("@year", year);
                parameters.Add("@CompanyCode", CompanyCode);
                parameters.Add("@Site_ID", Site_ID);
                parameters.Add("@Payperiod_ID", Payperiod_ID);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (res.Contains("RESULT"))
                {
                    timesheetDetails.response = "File Uploaded successfully";
                }
                else
                {
                    timesheetDetails.response = "File not Uploaded";
                }

            }
            else
            {
                timesheetDetails.response = "File not found";
            }
            return timesheetDetails;
        }

        public async Task<List<TimesheetAttachment>> GetTimesheetAttachment(string CompanyCode, int Site_ID,
    string Employee_Code, string Payperiod)
        {

            string[] payperiodsplit = Payperiod.ToString().Split(' ');
            string month = payperiodsplit[0];
            string year = payperiodsplit[1];
            int Selectedyear = Convert.ToInt32(year);    //Convert.ToInt32(ddlYear.SelectedValue);
            int selectedMonth = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;

            string SelectedMonth = "";
            if (selectedMonth < 10)
            {
                SelectedMonth = "0" + Convert.ToString(selectedMonth);
            }
            else
            {
                SelectedMonth = Convert.ToString(selectedMonth);
            }

            var AnswerDetails = new List<TimesheetAttachment>();
            string storeProcedure = "USP_ATTENDANCE_EMPLOYEE_ATTACHMENTS";
            var parameters = new DynamicParameters();
            parameters.Add("@Client_ID", CompanyCode);
            parameters.Add("@Site_ID", Site_ID);
            parameters.Add("@Employee_Code", Employee_Code);
            parameters.Add("@Month", SelectedMonth);
            parameters.Add("@Year", Selectedyear);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<TimesheetAttachment>>(res)
                                                      ?? new List<TimesheetAttachment>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<TimesheetAttachment>();
                }
            }


            return AnswerDetails;
        }

        public async Task<TimesheetResponse> SaveTimesheet([FromBody] TimesheetRequestDto request)
        {
            TimesheetResponse timesheetDetails = new TimesheetResponse();

            if (request == null || request.Rows == null || !request.Rows.Any())
            {
                timesheetDetails.response = "Invalid timesheet request.";
            }

            string[] payperiodsplit = request.PayPeriod.ToString().Split(' ');
            string month = payperiodsplit[0];
            string year = payperiodsplit[1];
            int Selectedyear = Convert.ToInt32(year);    //Convert.ToInt32(ddlYear.SelectedValue);
            int selectedMonth = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;

            string SelectedMonth = "";
            if (selectedMonth < 10)
            {
                SelectedMonth = "0" + Convert.ToString(selectedMonth);
            }
            else
            {
                SelectedMonth = Convert.ToString(selectedMonth);
            }

            var xmlInput = BuildTimesheetXml(request);

            string storeProcedure = "Proc_UpdateEmployeeTimeSheet_QzoneNewUI";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@Month", SelectedMonth);
            parameters.Add("@Year", Selectedyear);
            parameters.Add("@SubmitStatus", request.Status);
            parameters.Add("@Site_Id", request.SiteId);
            parameters.Add("@Company_Code", request.CompanyCode);
            parameters.Add("@PayPeriod_Id", request.PayPeriodId);
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Data saved successfully"))
                    {
                        timesheetDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) && message.Contains("Data submitted successfully."))
                    {
                        timesheetDetails.response = message;
                    }
                    else
                    {
                        timesheetDetails.response = "Failed to import.";
                        timesheetDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    timesheetDetails.response = "Error while processing response.";
                }
            }
            else
            {
                timesheetDetails.response = "Failed";
            }


            return timesheetDetails;
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


        private string GetSheetName(int i)
        {
            string sheetName = string.Empty;
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

        public static int? ToNullableInt(string? input)
        {
            return int.TryParse(input, out int val) ? val : (int?)null;
        }

        public async Task<string> PostAttendanceData(string xmlString, int companyId, int payPeriodId,
      string filePath, string xmlString2, string userId, string ISFANDF)
        {
            string res = "";

            if (ISFANDF == "9") 
            {
                var maternityParameters = new DynamicParameters();
                maternityParameters.Add("@FLAG", 1);
                maternityParameters.Add("@COMPANY_ID", companyId);
                maternityParameters.Add("@PAYPERIOD_ID", payPeriodId);
                maternityParameters.Add("@CREADTEDBY", userId);
                maternityParameters.Add("@XML", xmlString);

                res = await this._dbRepository.GetItemsAsync(
                    "USP_UPLOAD_EMPLOYEE_MATERNITYMASTER",
                    maternityParameters);
            }
            else 
            {
                var parameters = new DynamicParameters();
                parameters.Add("@payperiod_Id", payPeriodId);
                parameters.Add("@Company_Id", companyId);
                parameters.Add("@User", userId);
                parameters.Add("@ISFANDF", ISFANDF);
                parameters.Add("@xml", xmlString);
                parameters.Add("@InputType", "1");
                parameters.Add("@LotNumber", "0");
                parameters.Add("@XML1", xmlString2);
                parameters.Add("@FilePath", filePath);

                res = await this._dbRepository.GetItemsAsync(
                    "PROC_BULK_UPLOAD_ATTENDANCE_NEWUI",
                    parameters);
            }

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public async Task<string> VerifyAttendanceHeaders(string xmlString, int companyId, int payPeriodId, string filePath)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@xML", xmlString);

            var res = await this._dbRepository.GetItemsAsync("USP_Attendance_Upload_IsValidHeaders_New", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        //public async Task<List<Unseize>> GetUnseizeData(string CompanyCode, int PayPriod_Id, int GroupName, int City_Id, string Empid)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Company_Code", CompanyCode);
        //    parameters.Add("@Site_Id", GroupName);
        //    parameters.Add("@PayPriod_Id", PayPriod_Id);
        //    parameters.Add("@City_Id", City_Id);

        //    var res = await this._dbRepository.GetItemsAsync("Proc_UnSeizeEmployee_NewUI", parameters);

        //    if (!string.IsNullOrEmpty(res))
        //    {
        //        return JsonConvert.DeserializeObject<List<Unseize>>(res) ?? new List<Unseize>();
        //    }

        //    return new List<Unseize>();
        //}

        public async Task<string> PostUnseize(string xmlString, int companyId, int payPeriodId, int siteCode, string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@XMLInput", xmlString);
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@Payperiod_ID", payPeriodId);
            parameters.Add("@Site_ID", siteCode);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_DeleteUnSeizeEmployee_bulk", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        //public async Task<List<Attachment>> GetUnseizeAttachment(string companyCode, int siteId, string empCode, string payPeriod)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Client_ID", companyCode);
        //    parameters.Add("@Site_ID",siteId);
        //    parameters.Add("@EMPLOYEE_CODE", empCode);
        //    parameters.Add("@PAYPERIOD", payPeriod);

        //    var res = await this._dbRepository.GetItemsAsync("USP_ATTENDANCE_EMPLOYEE_ATTACHMENTS_NEWUI", parameters);

        //    if (!string.IsNullOrEmpty(res))
        //    {
        //        return JsonConvert.DeserializeObject<List<Attachment>>(res) ?? new List<Attachment>();
        //    }

        //    return new List<Attachment>();
        //}

        public async Task<DataSet> GetTimesheetClientTemplate(string Company_Id, string PayPeriod_Id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Employee_Id"] = "0",
                ["@Company_Id"] = Company_Id,
                ["@PayPeriod_Id"] = PayPeriod_Id,
                ["@MapNameId"] = "0",
                ["@InputType"] = "1",
                ["@LotNumber"] = "0"
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_view_Client_Template", parameters, 1500);
        }

        public async Task<List<TimesheetAttachment>> DeleteTimesheetAttachment(DeleteAttachmentRequest request)
        {
            var AnswerDetails = new List<TimesheetAttachment>();
            string storeProcedure = "USP_DELETE_EMPLOYEE_ATTACHMENTS";
            var parameters = new Dictionary<string, object?>
            {
                ["@Client_ID"] = request.Client_ID,
                ["@Site_ID"] = request.Site_ID,
                ["@FileID"] = request.FileID,
                ["@Month"] = request.Month,
                ["@Year"] = request.Year
            };

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<TimesheetAttachment>>(res)
                                                      ?? new List<TimesheetAttachment>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<TimesheetAttachment>();
                }
            }

            return AnswerDetails;
            
        }

        public async Task<TimesheetResponse> SaveTimesheetPreviousMonth([FromBody] TimesheetRequestDto request)
        {
            TimesheetResponse timesheetDetails = new TimesheetResponse();

            if (request == null || request.Rows == null || !request.Rows.Any())
            {
                timesheetDetails.response = "Invalid timesheet request.";
            }

            string[] payperiodsplit = request.PayPeriod.ToString().Split(' ');
            string month = payperiodsplit[0];
            string year = payperiodsplit[1];
            int Selectedyear = Convert.ToInt32(year);    //Convert.ToInt32(ddlYear.SelectedValue);
            int selectedMonth = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;

            string SelectedMonth = "";
            if (selectedMonth < 10)
            {
                SelectedMonth = "0" + Convert.ToString(selectedMonth);
            }
            else
            {
                SelectedMonth = Convert.ToString(selectedMonth);
            }

            var xmlInput = BuildTimesheetXml(request);

            string storeProcedure = "Proc_UpdateEmployeeTimeSheet_PreviousMonth";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@Month", SelectedMonth);
            parameters.Add("@Year", Selectedyear);
            parameters.Add("@SubmitStatus", request.Status);
            parameters.Add("@Site_Id", request.SiteId);
            parameters.Add("@Company_Code", request.CompanyCode);
            parameters.Add("@PayPeriod_Id", request.PayPeriodId);
            parameters.Add("@CreatedBy", request.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Data saved successfully"))
                    {
                        timesheetDetails.response = message;
                    }
                    else if (!string.IsNullOrWhiteSpace(message) && message.Contains("Data submitted successfully."))
                    {
                        timesheetDetails.response = message;
                    }
                    else
                    {
                        timesheetDetails.response = "Failed to import.";
                        timesheetDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    timesheetDetails.response = "Error while processing response.";
                }
            }
            else
            {
                timesheetDetails.response = "Failed";
            }


            return timesheetDetails;
        }

        public class MyResult
        {
            public string result { get; set; }  // must match JSON key
        }

        private string BuildTimesheetXml(TimesheetRequestDto request)
        {
            var sb = new StringBuilder();
            sb.Append("<Attendance>");

            foreach (var row in request.Rows)
            {
                foreach (var entry in row.DayEntries)
                {
                    sb.Append("<Employee>");
                    sb.AppendFormat("<EmployeeID>{0}</EmployeeID>", row.EmpID);
                    sb.AppendFormat("<EmpAttendanceMark>{0}</EmpAttendanceMark>", entry.OT);
                    sb.AppendFormat("<EmpAttendanceMarkOT>{0}</EmpAttendanceMarkOT>", entry.HOT);
                    sb.AppendFormat("<AttendanceDate>{0:yyyy-MM-dd}</AttendanceDate>", entry.Date);
                    sb.Append("</Employee>");
                }

                // Employee Status section (optional, you can extend with real values)

                string activeStatus = row.Status switch
                {
                    "Assigned" => "1",
                    "UnAssigned" => "2",
                    "Saparated" => "3", // (assuming typo, maybe "Separated")
                    "Seized" => "4",
                    _ => "0"  // default or unknown
                };

                sb.Append("<EmployeeStatus>");
                sb.AppendFormat("<EmployeeID>{0}</EmployeeID>", row.EmpID);
                sb.AppendFormat("<ActiveStatus>{0}</ActiveStatus>", activeStatus);
                sb.AppendFormat("<Remarks>{0}</Remarks>", row.Remarks ?? "");
                sb.AppendFormat("<Approver>{0}</Approver>", row.Approver ?? "");
                sb.AppendFormat("<OT>{0}</OT>", row.OT ?? "0");
                sb.AppendFormat("<Date_Of_Join>{0}</Date_Of_Join>", row.DOJ ?? "");
                sb.AppendFormat("<Date_Of_Separation>{0}</Date_Of_Separation>", row.Seperation ?? "");
                sb.Append("</EmployeeStatus>");
            }

            sb.Append("</Attendance>");
            return sb.ToString();
        }
    }
}
