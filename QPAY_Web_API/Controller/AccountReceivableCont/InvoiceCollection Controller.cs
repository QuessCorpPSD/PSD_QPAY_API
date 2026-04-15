using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.BAL.IRepository.IAccountReceivable;
using System.Data;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.API.Controller.AccountReceivableCont
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceCollectionController : ControllerBase
    {
        private readonly IInvoiceCollectionRepository _repo;
        public InvoiceCollectionController(IInvoiceCollectionRepository repo)
        {
           _repo = repo;
        }
        [HttpGet]
        [Route("GetMapName/{companyId}")]
        public async Task<IActionResult> GetMapName(int companyId)
        {
            var ds = await _repo.GetMapName(companyId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpGet("GetModeOfCollections/{action}")]
        public async Task<IActionResult> GetModeOfCollections(string action)
        {
            var ds = await _repo.GetModeOfCollections(action);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpGet("SearchEditInvoiceCollection")]
        public async Task<IActionResult> SearchEditInvoiceCollection(int companyId,int payPeriodId,int invoiceCollectionId, string mode)
        {
            var ds = await _repo.SearchEditInvoiceCollection(companyId, payPeriodId, invoiceCollectionId, mode);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpGet]
        [Route("ValidateInvoiceCollection/{collection}/{collectiondetail}/{createdby}/{mode}")]
        public async Task<IActionResult> ValidateInvoiceCollection(string collection, string collectiondetail, int createdby, string mode)
        {
            var ds = await _repo.ValidateInvoiceCollection(collection, collectiondetail, createdby, mode);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpGet("CreateInvoiceCollection/{collection}/{collectiondetail}/{createdby}/{mode}")]
        public async Task<IActionResult> CreateInvoiceCollection(string collection, string collectiondetail, int createdby, string mode)
        {
            var ds = await _repo.CreateInvoiceCollection(collection, collectiondetail, createdby, mode);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpGet]
        [Route("GetTDSPercentage/{companyId}")]
        public async Task<IActionResult> GetTDSPercentage(int? companyId)
        {
            var ds = await _repo.GetTDSPercentage(companyId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpGet]
        [Route("GetOnAccount/{companyId}")]
        public async Task<IActionResult> GetOnAccount(int? companyId)
        {
            var ds = await _repo.GetOnAccount(companyId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpGet]
        [Route("GetCollectionInvoiceNo/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> GetCollectionInvoiceNo(int? companyId, int payPeriodId)
        {
            var ds = await _repo.GetCollectionInvoiceNo(companyId, payPeriodId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpPost]
        [Route("InvoiceCollectionBulkUpload")]
        public async Task<IActionResult> InvoiceCollectionBulkUpload(IFormFile file, [FromForm] string fileType, [FromForm] string user)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _repo.InvoiceCollectionBulkUpload(file, fileType, user);

            return Ok(result);
        }
        [HttpPost]
        [Route("ExportInvoiceCollectionToExcel")]
        public async Task<IActionResult> ExportInvoiceCollectionToExcel([FromBody] CommonExport1 payload)
        {
            var ds = await _repo.ExportInvoiceCollectionToExcel(
                payload.companyId,
                payload.payPeriodId
            );

            var res = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(res);
        }
        [HttpGet]
        [Route("GetReceivableAmount/{PayPeriodId}/{InvoiceNumber}/{TdsPercentage}")]
        public async Task<IActionResult> GetReceivableAmount(int PayPeriodId, string InvoiceNumber, decimal TdsPercentage)
        {
            var ds = await _repo.GetReceivableAmount(PayPeriodId, InvoiceNumber, TdsPercentage);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpGet]
        [Route("GetInvoiceCollectionData/{CompanyId}/{PayPeriodId}/{MapNameId}/{RefId}")]
        public async Task<IActionResult> GetInvoiceCollectionData(int CompanyId, int PayPeriodId, int MapNameId, int RefId)
        {
            var ds = await _repo.GetInvoiceCollectionData(CompanyId, PayPeriodId, MapNameId, RefId);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpGet("GetCompanyNameByCode/{companyCode}")]
        public async Task<IActionResult> GetCompanyNameByCode(string companyCode)
        {
            var ds = await _repo.GetCompanyNameByCode(companyCode);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
        [HttpGet("GetOnAccountReference/{referenceNumber}")]
        public async Task<IActionResult> GetOnAccountReference(string referenceNumber)
        {
            var ds = await _repo.GetOnAccountReference(referenceNumber);

            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);

            return Ok(payload);
        }
    }
}
