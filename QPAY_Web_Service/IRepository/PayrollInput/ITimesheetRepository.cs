using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static QPay.DTo.Models.PayrollInput.Timesheet;

namespace QPay.IRepository.iRepository.PayrollInput
{
    public interface ITimesheetRepository
    {
        Task<DataSet> GetEmployeeTimesheetDaywise(string CompanyCode, int GroupName, string Empid,
            int PayPriod_Id, int City_Id);
        Task<DataSet> GetEmployeeTimesheetDaywiseDownload(string CompanyCode, int GroupName, string Empid,
            int PayPriod_Id, int City_Id);
        Task<TimesheetResponse> UploadDailyTimesheet(IFormFile file, [FromForm] string User,
          [FromForm] string CompanyCode, [FromForm] int SiteID, [FromForm] int Payperiod);
        Task<TimesheetResponse> UploadDocumentSingleMulitiple(IFormFile file, [FromForm] string User,
 [FromForm] string Employeeid, [FromForm] string CompanyCode, [FromForm] int Site_ID, [FromForm] int Payperiod_ID,
 [FromForm] string Payperiod, string fullUrl);
        Task<List<TimesheetAttachment>> GetTimesheetAttachment(string CompanyCode, int Site_ID,
    string Employee_Code, string Payperiod);
        Task<TimesheetResponse> SaveTimesheet([FromBody] TimesheetRequestDto request);
        Task<string> PostAttendanceData(string xmlString, int companyId, int payPeriodId, string filePath, string xmlString2
            , string userId, string ISFANDF);
        Task<string> VerifyAttendanceHeaders(string xmlString, int companyId, int payPeriodId, string filePath);
        //Task<List<Unseize>> GetUnseizeData(string CompanyCode, int PayPriod_Id, int GroupName, int City_Id, string Empid);
        Task<string> PostUnseize(string xmlString, int companyId, int payPeriodId, int siteCode, string userId);
        //Task<List<Attachment>> GetUnseizeAttachment(string companyCode, int siteId, string empCode, string payPeriod);
        Task<DataSet> GetTimesheetClientTemplate(string Company_Id, string PayPeriod_Id);
        Task<List<TimesheetAttachment>> DeleteTimesheetAttachment(DeleteAttachmentRequest request);
        Task<TimesheetResponse> SaveTimesheetPreviousMonth([FromBody] TimesheetRequestDto request);

    }
}
