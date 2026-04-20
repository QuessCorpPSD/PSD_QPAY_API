using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.BAL.Models;
using QPay.UI.Models.AccountReceivableMod;

namespace QPay.API.Controller.AccountReceivableCont
{
    [Route("api/[controller]")]
    [ApiController]
    public class Clientledgercontroller : ControllerBase
    {
        private readonly IClientledger _repo;
        public Clientledgercontroller(IClientledger repo)
        {
            _repo = repo;
        }

        [HttpGet("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear(int? id)
        {
            var ds = await _repo.GetFinancialYear(id);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpPost("ClientLedgerExport")]
        public async Task<IActionResult> ClientLedgerExport([FromBody] ClientLedgerExportRequest request)
        {
            try
            {
                var ds = await _repo.ExportClientLedger(request);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Ok(ResponseWrapManager.ResponseWrapper("No Data To Export", HttpContext));

                using (XLWorkbook wb = new XLWorkbook())
                {                  
                    for (int i = 1; i < ds.Tables.Count; i++)
                    {
                        string sheetName = "Sheet" + i;

                        if (ds.Tables[0].Rows.Count > i && ds.Tables[0].Columns.Count > 1)
                        {
                            sheetName = ds.Tables[0].Rows[i][1]?.ToString() ?? sheetName;
                        }

                        wb.Worksheets.Add(ds.Tables[i], sheetName);
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var content = stream.ToArray();

                        string fileName = "ClientLedger.xlsx";

                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count > 1)
                        {
                            fileName = ds.Tables[0].Rows[0][1]?.ToString() + ".xlsx";
                        }

                        return File(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName);
                    }
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
    }
}