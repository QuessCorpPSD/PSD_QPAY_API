using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Customer;
using static QPay.UI.Customer.Company;

namespace QPay.API.Controller.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _IRepository;
        public CompanyController(ICompanyRepository IRepository)
        {
            this._IRepository = IRepository;
        }


        [HttpGet]
        [Route("Search/{clientCode}/{clientName}")]
        public async Task<IActionResult> Search(string? clientCode, string? clientName)
        {
            string xml = string.Empty;

            xml = "<Search><Client_code>" + clientCode + "</Client_code><Client_Name>" + clientName + "</Client_Name>{0}</Search>";
            xml = string.Format(xml, "<CompanyType>0</CompanyType>");
            //Search
            var response = await _IRepository.Search("Search", 0, xml);
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
        [Route("View/{CompanyId}")]
        public async Task<IActionResult> View(int? CompanyId)
        {
            string xml = string.Empty;           
            //Edit
            var response = await _IRepository.View("Edit", CompanyId, xml);
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
        [Route("ExportToExcel/{clientCode}/{clientName}")]
        public async Task<IActionResult> ExportToExcel(string? clientCode, string? clientName)
        {
            string xml = string.Empty;

            xml = "<Search><Client_code>" + clientCode + "</Client_code><Client_Name>" + clientName + "</Client_Name></Search>";
            //Search
            var response = await _IRepository.ExportToExcel("Search", 0, xml);
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
        [Route("masters")]
        public async Task<IActionResult> masters()
        {
            var response = await _IRepository.masters();
            if (response != null)
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
        [Route("GetBussinessunitLocation/{BusinessUnitId}")]
        public async Task<IActionResult> GetBussinessunitLocation(int? BusinessUnitId)
        {

            var response = await _IRepository.GetBussinessunitLocation(BusinessUnitId);
            if (response != null)
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
        [Route("GetCityBasedonState/{Stateid}")]
        public async Task<IActionResult> GetCityBasedonState(int? Stateid)
        {

            var response = await _IRepository.GetCityBasedonState(Stateid);
            if (response != null)
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
        [Route("GetStatebasedoncity/{cityid}")]
        public async Task<IActionResult> GetStatebasedoncity(int? cityid)
        {

            var response = await _IRepository.GetStatebasedoncity(cityid);
            if (response != null)
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
        [Route("GetInvoiceFormat")]
        public async Task<IActionResult> GetInvoiceFormat()
        {

            var response = await _IRepository.GetInvoiceFormat();
            if (response != null)
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
        [Route("GetReimbInvoiceFormat")]
        public async Task<IActionResult> GetReimbInvoiceFormat()
        {

            var response = await _IRepository.GetReimbInvoiceFormat();
            if (response != null)
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
        [Route("GetPortalPayslipFormat")]
        public async Task<IActionResult> GetPortalPayslipFormat()
        {
            var response = await _IRepository.GetPortalPayslipFormat();
            if (response != null)
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
        public async Task<IActionResult> Create(CompanyCreateRequest request)
        {
            var response = await _IRepository.Create(request);
            if (response.Tables[0].Rows.Count > 0)
            {
                int Company_Id = Convert.ToInt32(response.Tables[0].Rows[0]["Company_Id"]);
                string message = response.Tables[0].Rows[0]["Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Paycode Created Failed" });
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(CompanyUpdateRequestPayload request)
        {
            var response = await _IRepository.Update(request);
            if (response.Tables[0].Rows.Count > 0)
            {
                
                string message = response.Tables[0].Rows[0]["Message"].ToString();
                if (!(message.Contains("Successfully")))
                {
                    return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Paycode Created Failed" });
            }
        }

        [HttpPost]
        [Route("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(CompanyDeleteRequest request)
        {
            var response = await _IRepository.DeleteCompany(request);
            if (response.Tables[0].Rows.Count > 0)
            {

                string message = response.Tables[0].Rows[0]["Message"].ToString();
                if (!(message.Contains("Successfully")) && !(message.Contains("successfully")))
                {
                    return Ok(new { StatusCode = "400", Message = response.Tables[0].Rows[0]["Message"].ToString() });
                }
                else
                {
                    var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
                    return Ok(_outputResponse);
                }
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "Paycode Created Failed" });
            }
        }

        //[HttpGet]
        //[Route("GetCriteria")]
        //public async Task<IActionResult> GetCriteria()
        //{
        //    var response = await _IRepository.GetCriteria(0);
        //    if (response != null)
        //    {
        //        var _outputResponse = ResponseWrapManager.ResponseWrapper(response, HttpContext);
        //        return Ok(_outputResponse);
        //    }
        //    else
        //    {
        //        return Ok(new { StatusCode = "400", Message = "No records found" });
        //    }
        //}

    }
}
