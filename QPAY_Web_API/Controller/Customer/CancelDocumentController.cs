using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.Repository.Common;
using QPay.UI.Models.Customer;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancelDocumentController : ControllerBase
    {
        private readonly ICancelDocumentRepository _icancel;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public CancelDocumentController(
            ICancelDocumentRepository icancel, ICommonRepository iCommon, IConfiguration configuration)
        {
            this._icancel = icancel;
            this._icommon = iCommon;
            this._configuration = configuration;
        }

        [HttpGet, Route("Search/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> Search(int companyId, int payPeriodId)
        {
            var response = await _icancel.Search(companyId, payPeriodId);

            return Ok(response);
        }

        [HttpPost]
        [Route("UploadDocument")]
        public async Task<IActionResult> UploadDocument(IFormFile file, [FromForm] string cancelDocument, [FromForm] int userId)
        {
                if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["UploadRepositoryDocument"].ToString());
            string fileExtention = Path.GetExtension(file.FileName.ToUpper());
            string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
            FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
            //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
            string serverpath = DirName + FileName;

            using (var stream = new FileStream(serverpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var cancelclass = JsonConvert.DeserializeObject<CancelDocument>(cancelDocument);
            cancelclass.Document_FilePath = DirName.ToString();
            var request = new CancelledInvoiceRepositoryResponse
            {
                CancelledInvoiceRepository = cancelclass
            };

            string xml = XmlHelper2.SerializeObjectToXml(request);

            var response = await _icancel.UploadDocument(xml, userId);

            return Ok(response);
        }

    }
}
