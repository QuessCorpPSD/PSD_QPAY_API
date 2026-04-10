using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
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
        private readonly string[] _companyCode;
           

        public ReportsController(
            IPoReportRepository iporeport, IConfiguration configuration)
        {
            this._poReport = iporeport;
            this._configuration = configuration;
            this._companyCode = _configuration.GetSection("GetAccrualsTemplateColumn:CoulumnName").Get<string[]>() ?? Array.Empty<string>();
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
        [HttpPost]
        [Route("GetGrossMarginReport")]
        public async Task<IActionResult> GetGrossMarginReport(GrossMarginRequestModel request)
        {
            var fileResponse = new FileResponse
            {
                File = "N",
                FileName = "GrossMarginReport.xlsx"
            };


            DataSet result = request.ReportType switch
            {
                "GM" => await _poReport.GetGrossMarginReport(
                    request.Pay_Period,
                    Convert.ToInt32(request.Submit)
                ) ,
                "UGM" => await _poReport.GetUnProcessedGrossMarginReport(
                    request.Pay_Period),
                 _=> CreateEmptyDataSet()

            };

            

                if (result?.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    var dt = result.Tables[0];

                    using var workbook = new XLWorkbook();
                    var ws = workbook.Worksheets.Add(dt, "InvoiceSummary");

                    ws.Tables.First().ShowAutoFilter = false;
                    ws.Tables.First().Theme = XLTableTheme.None;

                    using var stream = new MemoryStream();
                    workbook.SaveAs(stream);

                    stream.Position = 0;

                    fileResponse.File = Convert.ToBase64String(stream.ToArray());
                if(request.ReportType=="GM")
                    fileResponse.FileName = "GrossMarginReport.xlsx";
                else
                    fileResponse.FileName = "UnprossedGrossMarginReport.xlsx";

            }
            

            return Ok(fileResponse);
        }
        private DataSet CreateEmptyDataSet()
        {
            var ds = new DataSet();
            ds.Tables.Add(new DataTable());
            return ds;
        }
        [HttpGet,Route("GetAccrualsTemplate")]
        public IActionResult GetAccrualsTemplate()
        {
            FileResponse files = new FileResponse();
            
        
            return Ok(files);
        }

        [HttpPost, Route("AccrualsTemplateUpload")]
        public IActionResult AccrualsTemplateUpload()
        {
            FileResponse files = new FileResponse();


            return Ok(files);
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
