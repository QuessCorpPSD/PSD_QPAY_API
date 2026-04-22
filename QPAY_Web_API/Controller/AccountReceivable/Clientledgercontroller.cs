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




        [HttpPost]
        [Route("ClientLedgerExport")]
        public async Task<IActionResult> ClientLedgerExport([FromBody] ClientLedgerExportRequest request)
        {
            var ds = await _repo.ClientLedgerExportToExcel(request);

            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }
    }
}