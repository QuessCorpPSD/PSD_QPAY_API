using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Reports;
using QPay.DTo.Models.Common;
using QPay.UI.Models;
using System.Data;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputReportConsolidatedController : ControllerBase
    {
        private readonly IInputReportConsolidatedRepository _iRepository;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public InputReportConsolidatedController(
            IInputReportConsolidatedRepository iRepository, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._iRepository = iRepository;
            this._icommon = iCommon;
            this._configuration = configuration;
        }
        [HttpGet, Route("GetInputReportConsolidated/{companyId}/{payPeriodId}/{inputId}")]
        public IActionResult GetInputReportConsolidated(int companyId, int payPeriodId, int inputId)
        {
            DataSet ds = _iRepository.GetInputReportConsolidated(companyId, payPeriodId, inputId);
            if (ds != null && ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                if (inputId == 2)
                {
                    ds.Tables[0].TableName = "OI Data";
                }
                else if (inputId == 1)
                {
                    ds.Tables[0].TableName = "New Joinee Employee id Creation";
                    ds.Tables[1].TableName = "New Joinee Breakup";
                    ds.Tables[2].TableName = "Attendance";
                    ds.Tables[3].TableName = "Adhoc Or Pay Transaction";
                    ds.Tables[4].TableName = "Increment Break up";
                    ds.Tables[5].TableName = "LOP Details";
                    ds.Tables[6].TableName = "New Joinee";
                    ds.Tables[7].TableName = "Maternity status";
                    ds.Tables[8].TableName = "Leave Availed Details";
                }
                else if (inputId == 3)
                {
                    ds.Tables[0].TableName = "Sheet1";
                    ds.Tables[1].TableName = "Sheet2";
                    ds.Tables[2].TableName = "Sheet3";
                    ds.Tables[3].TableName = "Sheet4";
                    ds.Tables[4].TableName = "Sheet5";
                    ds.Tables[5].TableName = "Sheet6";
                    //ds.Tables[6].TableName = "New Joinee";
                    //ds.Tables[7].TableName = "Maternity status";
                    //ds.Tables[8].TableName = "Leave Availed Details";
                }
                else if (inputId == 4)
                {
                    ds.Tables[0].TableName = "Sheet1";
                    //ds.Tables[1].TableName = "New Joinee Breakup";
                    //ds.Tables[2].TableName = "Attendance";
                    //ds.Tables[3].TableName = "Adhoc Or Pay Transaction";
                    //ds.Tables[4].TableName = "Increment Break up";
                    //ds.Tables[5].TableName = "LOP Details";
                    ////ds.Tables[6].TableName = "New Joinee";
                    ////ds.Tables[7].TableName = "Maternity status";
                    ////ds.Tables[8].TableName = "Leave Availed Details";
                }

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
                    fileResponse.FileName = "InputReportConsolidated" + fileName;
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
    }
}
