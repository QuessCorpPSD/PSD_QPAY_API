using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using QPay.UI.Customer;
using QPay.UI.Models.Customer;
using System.Text;
using System.Xml.Serialization;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyPaycodeMappingController : ControllerBase
    {
        private readonly ICompanyPaycodeMappingRepository _IRepository;
        public CompanyPaycodeMappingController(ICompanyPaycodeMappingRepository IRepository)
        {
            this._IRepository = IRepository;
        }

        [HttpGet]
        [Route("Search/{companyId}")]
        public async Task<IActionResult> Search(int? companyId)
        {
            var response = await _IRepository.Search(companyId);
            if (response.Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpGet]
        [Route("ExportToExcel/{companyId}")]
        public async Task<IActionResult> ExportToExcel(int? companyId)
        {
            var response = await _IRepository.ExportToExcel(companyId);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpGet]
        [Route("GetAllCompanyPayCodePickFrom")]
        public async Task<IActionResult> GetAllCompanyPayCodePickFrom()
        {
            var response = await _IRepository.GetAllCompanyPayCodePickFrom();
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        
        [HttpGet]
        [Route("GetAllPaycodeCompanyPacode/{PayCode}/{PayTypeId}/{IsTaxable}/{PayId}")]
        public async Task<IActionResult> GetAllPaycodeCompanyPacode(string? PayCode, int? PayTypeId, int? IsTaxable, int? PayId)
        {
            var response = await _IRepository.GetAllPaycodeCompanyPacode(PayCode, PayTypeId, IsTaxable, PayId);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(PaycodeRequest req)
        {
            var companyInfo = new CompanyInfo
            {
                Company_Id = req.Company_Id,
                Company_Paycode_Mapping_Id = req.Company_Paycode_Mapping_Id,
                Pay_Structure_Id = req.Pay_Structure_Id
            };

            var paycodeDetailList = new PaycodeDetailList
            {
                Items = req.PaycodeDetail
            };

            // 2. Convert each object to XML
            string companyXml = BuildCompanyXml(companyInfo);
            string paycodeXml = BuildPaycodeXml(paycodeDetailList);

            var response = await _IRepository.Create(companyXml, paycodeXml, req.Mode, req.User_Id);
            if (response.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Data not saved" });
            }
        }

        private string BuildCompanyXml(CompanyInfo request)
        {
            var sb = new StringBuilder();
            sb.Append("<CompanyPayCodesDetails>");

            sb.Append("<CompanyPayCode>");
            sb.AppendFormat("<Company_Id>{0}</Company_Id>", request.Company_Id);
            sb.AppendFormat("<Company_Paycode_Mapping_Id>{0}</Company_Paycode_Mapping_Id>", request.Company_Paycode_Mapping_Id);
            sb.AppendFormat("<Pay_Structure_Id>{0}</Pay_Structure_Id>", request.Pay_Structure_Id);
            sb.Append("</CompanyPayCode>");

            sb.Append("</CompanyPayCodesDetails>");
            return sb.ToString();
        }
        private string BuildPaycodeXml(PaycodeDetailList request)
        {
            var sb = new StringBuilder();
            sb.Append("<CompanyPayCodeDetailResponse>");

            foreach (var row in request.Items)
            {
                sb.Append("<CompanyPayCodeDetail>");
                sb.AppendFormat("<Paycode_Id>{0}</Paycode_Id>", row.Paycode_Id);
                sb.AppendFormat("<EarnedPaycode_Code>{0}</EarnedPaycode_Code>", row.EarnedPaycode_Code);
                sb.AppendFormat("<Company_Paycode_Pick_From_Id>{0}</Company_Paycode_Pick_From_Id>", row.Company_Paycode_Pick_From_Id);
                sb.AppendFormat("<Company_Paycode_Mapping_Detail_Id>{0}</Company_Paycode_Mapping_Detail_Id>", row.Company_Paycode_Mapping_Detail_Id);
                sb.AppendFormat("<Pay_Structure_Detail_Id>{0}</Pay_Structure_Detail_Id>", row.Pay_Structure_Detail_Id);
                sb.AppendFormat("<SNo>{0}</SNo>", row.SNo);
                sb.Append("</CompanyPayCodeDetail>");
            }

            sb.Append("</CompanyPayCodeDetailResponse>");
            return sb.ToString();
        }

    }
}
