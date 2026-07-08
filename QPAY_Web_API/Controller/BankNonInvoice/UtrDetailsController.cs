using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.BankNonInvoice;
using QPay.UI.Common;
using QPay.UI.Models;
using System.Data;

namespace QPay.API.Controller.BankNonInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtrDetailsController : ControllerBase
    {
        private readonly IUtrDetailsRepository _iutrdetail;
        public UtrDetailsController(
          IUtrDetailsRepository _iutrdetail)
        {
            this._iutrdetail = _iutrdetail;
        }

        [HttpGet]
        [Route("GetutrDetailDownload/{CompanyId}/{PayPeriodID}")]
        public async Task<IActionResult> GetutrDetailDownload(int CompanyId, int PayPeriodID)
        {
            return await _iutrdetail.GetutrDetailDownload(CompanyId, PayPeriodID);

        }

        [HttpGet]
        [Route("NetPaysummaryNI/{Company_Id}/{Pay_Period_Id}")]
        public IActionResult NetPaysummaryNI(int Company_Id, int Pay_Period_Id)
        {
            DataSet ds = _iutrdetail.NetPaysummaryNI(Company_Id, Pay_Period_Id);
            //var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            //return Ok(payload);

            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();

                ds.Tables[0].TableName = "Net Pay Summary Report";
                ds.Tables[1].TableName = "Net Pay Summary Details";
                ds.Tables[2].TableName = "Partial Hold Summary Report";
                ds.Tables[3].TableName = "Gratuity Summary Report";
                
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
                    fileResponse.FileName = "Net Pay Summary Report" + fileName;
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
    }
}
