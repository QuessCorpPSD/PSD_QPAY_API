using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Reports;
using QPay.UI.Common;
using QPay.UI.Models;
using QPay.UI.Models.Reports;
using System.Data;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IPoReportRepository _poReport;
        private readonly IConfiguration _configuration;

        public ReportsController(
            IPoReportRepository iporeport, IConfiguration configuration)
        {
            this._poReport = iporeport;
            this._configuration = configuration;
        }

        [HttpGet, Route("GetAllPOEmployeeReport/{employeeId}/{emlpoyeeType}")]
        public async Task<IActionResult> GetAllPOEmployeeReport(string employeeId, string emlpoyeeType)
        {
            var response = string.Empty;

            if (emlpoyeeType == "0")
            {
                //USP_PO_EMPLOYEEWISE_REPORT
                response = await _poReport.GetAllPOEmployeeReportNew(employeeId);
            }
            else if (emlpoyeeType == "1")
            {
                //USP_PO_EMPLOYEEWISE_REPORT_NEW
                response = await _poReport.GetAllPOEmployeeReportOld(employeeId);
            }
            else
            {
                return Ok("Invalid employee type");
            }

            return Ok(response);
        }

        [HttpGet, Route("GetPOYears")]
        public async Task<IActionResult> GetPOYears()
        {
            var response = string.Empty;

            response = await _poReport.GetPOYears();

            return Ok(response);
        }

        [HttpGet, Route("GetVerticals/{userId}/{potype}")]
        public async Task<IActionResult> GetVerticals(string userId, string potype)
        {
            var response = string.Empty;

            response = await _poReport.GetVerticals(userId, potype);

            return Ok(response);
        }

        [HttpPost, Route("POActiveReportGrid")]
        public async Task<IActionResult> POActiveReportGrid(POActiveInactive pOActiveInactive)
        {
            var response = string.Empty;

            response = await _poReport.POActiveReportGrid(pOActiveInactive);

            return Ok(response);
        }

        [HttpGet, Route("GetAllMonthWisePOReport/{txtFromDate}/{txtToDate}")]
        public IActionResult GetAllMonthWisePOReport(string txtFromDate, string txtToDate)
        {
            DataSet ds = _poReport.GetAllMonthWisePOReport(txtFromDate, txtToDate);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        var ws = workbook.AddWorksheet(ds.Tables[i], "sheet" + (i));
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        FileResponse fileResponse = new FileResponse();
                        string fileName = DateTime.Now.ToString("_yyyyMMddhhmmssffff");
                        fileResponse.FileName = "PO_MonthWise_Report" + fileName;
                        fileResponse.File = bytes;

                        return Ok(fileResponse);
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


    }
}
