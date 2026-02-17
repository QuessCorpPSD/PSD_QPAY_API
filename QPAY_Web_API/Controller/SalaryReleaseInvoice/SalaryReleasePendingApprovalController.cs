using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.BAL.Repository.SalaryReleaseInvoice;
using QPay.UI.Models.SalaryReleaseInvoice;

namespace QPay.API.Controller.SalaryReleaseInvoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryReleasePendingApprovalController : ControllerBase
    {
        private readonly ISalaryReleasePendingApprovalRepository _SalaryReleasePendingApprovalRepository;
        private readonly IConfiguration _configuration;

        public SalaryReleasePendingApprovalController(IConfiguration configuration, ISalaryReleasePendingApprovalRepository Repository)
        {
            _SalaryReleasePendingApprovalRepository = Repository;
            _configuration = configuration;
        }

        [HttpGet, Route("BankAdviceList/{BatchType}/{CollectionStatus}/{UserId}")]
        public IActionResult BankAdviceList(string BatchType, string CollectionStatus, string UserId)
        {

            var ds = _SalaryReleasePendingApprovalRepository.BankAdviceList(BatchType, CollectionStatus, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet, Route("BankAdviceListExport/{BatchType}/{CollectionStatus}/{UserId}")]
        public IActionResult BankAdviceListExport(string BatchType, string CollectionStatus, string UserId)
        {

            var ds = _SalaryReleasePendingApprovalRepository.BankAdviceListExport(BatchType, CollectionStatus, UserId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("BankAdviceApprove")]
        public async Task<IActionResult> BankAdviceApprove([FromBody] ApproveBankAdvice payload)
        {
            var catgory = await _SalaryReleasePendingApprovalRepository.BankAdviceApprove(payload);
            return Ok(catgory);

        }

    }
}
