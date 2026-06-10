using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.MailApprovalProcess;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.BAL.Repository.SalaryReleaseInvoice;
using QPay.UI.Models.MailApprovalProcess;
using QPay.UI.Models.SalaryReleaseInvoice;

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
    }
}
