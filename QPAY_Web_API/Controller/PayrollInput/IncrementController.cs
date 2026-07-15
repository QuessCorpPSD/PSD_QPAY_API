using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository.Common;
using QPay.DTo.Models.PayrollInput;
using QPay.IRepository.iRepository.PayrollInput;
using QPay.UI.Common;
using System.Data;

namespace Qzone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class IncrementController : ControllerBase
    {
        private readonly IIncrementRepository _iincrement;
        private readonly ICommonRepository _icommon;
        public IncrementController(IIncrementRepository iincrement, ICommonRepository icommon)
        {
            this._iincrement = iincrement;
            this._icommon = icommon;
        }

        private string GetSheetName(int i)
        {
            string sheetName = string.Empty;
            switch (i)
            {
                case 0:
                    sheetName = "Increment";
                    break;
                //case 1:
                //    sheetName = "ADHOC";
                //    break;

                default:
                    sheetName = "";
                    return sheetName;
            }
            return sheetName;

        }

        [HttpGet, Route("GetEmployeeIncrement/{companyId}/{InputType}/{MapNameId}")]

        public IActionResult GetEmployeeIncrement(int companyId, int InputType, int MapNameId)
        {

            List<PayperiodDD> payperiod = new List<PayperiodDD>();

            payperiod = _icommon.GetCurrentPayperiod(companyId);

            int payPeriodId = 0;

            if (payperiod != null && payperiod.Any())
            {

                payPeriodId = payperiod[0].Payfrequencyid;

            }

            else

            {

                return BadRequest("No pay period found for the given company ID.");

            }

            DataSet ds = _iincrement.GetEmployeeIncrement(companyId, payPeriodId, InputType, MapNameId);

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


                        fileResponse.FileName = "Increment" + fileName;



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
        [Route("UploadIncrementData")]
        public async Task<IActionResult> UploadIncrementData(IFormFile file, [FromForm] string User,
           [FromForm] string companyCode, [FromForm] int companyId, [FromForm] int InputType)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _iincrement.UploadIncrementData(file, User, companyCode, companyId, InputType);
            return Ok(result);
        }        
    }
}
