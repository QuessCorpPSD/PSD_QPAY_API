using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository.ARKnockOff;
using QPay.DTo.Models.Masters;
using QPay.UI.Common;
using QPay.UI.Models;
using QPay.UI.Models.ARKnockOff;
using System;
using System.Data;
using System.IO;


namespace QPay.API.Controller.ARKnockOff
{
    [Route("api/[controller]")]
    [ApiController]
    public class ARKnockOffController : ControllerBase
    {
        private readonly IARKnockOffRepository _IRepository;
        public ARKnockOffController(IARKnockOffRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpPost, Route("SaveARDetails")]
        public async Task<IActionResult> SaveARDetails(ARKnockOffclass request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _IRepository.SaveARDetails(xml);
            return Ok(response);
        }

        [HttpGet]
        [Route("ARReportExport/{FromDate}")]
        public IActionResult ARReportExport(string FromDate )
        {

            DataSet ds = _IRepository.ARReportExport(FromDate);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("ArReport");
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
                    fileResponse.FileName = "ARReport_Export_" + fileName + ".xlsx";
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

        [HttpGet, Route("GetARInvoiceDetails")]
        public async Task<IActionResult> GetARInvoiceDetails()
        {
            var search = await this._IRepository.GetARInvoiceDetails();
            if (search.Tables[0].Rows.Count > 0)
            {
                string json = JsonConvert.SerializeObject(search, Formatting.Indented);
                //var _outputResponse = ResponseWrapManager.ResponseWrapper(search, HttpContext);
                return Ok(json);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }
        [HttpGet, Route("GetIgnoreSubjectLine")]
        public async Task<IActionResult> GetIgnoreSubjectLine()
        {
            var search = await this._IRepository.GetIgnoreSubjectLine();
            if (search.Tables[0].Rows.Count > 0)
            {
                string json = JsonConvert.SerializeObject(search, Formatting.Indented);
                //var _outputResponse = ResponseWrapManager.ResponseWrapper(search, HttpContext);
                return Ok(json);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

    }

}
