using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.MailApprovalProcess;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.BAL.Repository.SalaryReleaseInvoice;
using QPay.UI.Models.MailApprovalProcess;
using QPay.UI.Models.SalaryReleaseInvoice;
using System.Data;

namespace QPay.API.Controller.MailApprovalProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerBlockApprovalController : ControllerBase
    {
        private readonly ICustomerBlockApprovalRepository _CustomerBlockApprovalRepository;
        private readonly IConfiguration _configuration;

        public CustomerBlockApprovalController(IConfiguration configuration, ICustomerBlockApprovalRepository Repository)
        {
            _CustomerBlockApprovalRepository = Repository;
            _configuration = configuration;
        }

        #region CustomerBlockApproval start

        [HttpGet, Route("GetApproveClientList/{UserId}")]
        public IActionResult GetApproveClientList(string UserId)
        {

            var ds = _CustomerBlockApprovalRepository.GetApproveClientList(UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("ClientApproveReject")]
        public async Task<IActionResult> ClientApproveReject([FromBody] ClientApprove payload)
        {

            var catgory = await _CustomerBlockApprovalRepository.ClientApproveReject(payload);
            return Ok(catgory);
        }

        #endregion CustomerBlockApproval end

        #region SourcingBlockApproval start

        [HttpPost, Route("SourcingApproveReject")]
        public async Task<IActionResult> SourcingApproveReject([FromBody] SourcingApprove payload)
        {
            var ds = await _CustomerBlockApprovalRepository.SourcingApproveReject(payload);

            if (ds == null || ds.Tables.Count == 0)
                return Ok(new List<object>());

            var rows = new List<Dictionary<string, object>>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                }
                rows.Add(dict);
            }

            return Ok(rows);
        }
        #endregion SourcingBlockApproval end


    }
}
