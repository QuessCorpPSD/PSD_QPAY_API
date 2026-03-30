using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Common;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommonController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IProcessCategoryRepository _processCategoryRepository;
        private readonly IAccesstypeRepository _accesstypeRepository;
        private readonly IFinancialYearRepository _financialYearRepository;
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly ICommonRepository _icompanyCode;

        public CommonController(
            IRoleRepository roleRepository,
            IProcessCategoryRepository processCategoryRepository,
            IAccesstypeRepository accesstypeRepository,
            IFinancialYearRepository financialYearRepository,
            IAdminDashboardRepository adminDashboardRepository,
            ICommonRepository icompanyCode)
        {
            _roleRepository = roleRepository;
            _processCategoryRepository = processCategoryRepository;
            _accesstypeRepository = accesstypeRepository;
            _financialYearRepository = financialYearRepository;
            _adminDashboardRepository = adminDashboardRepository;
            _icompanyCode = icompanyCode;
        }

        [HttpGet, Route("GetAllPayperiod/{companyId}")]
        public async Task<IActionResult> GetAllPayperiod(int companyId)
        {
            var response = await _adminDashboardRepository.GetAllPayperiod(companyId);

            return Ok(response);
        }
        [HttpGet, Route("GetAllCompanyCode/{userId}")]
        public async Task<IActionResult> GetAllCompanyCodes(string userId)
        {
            var response = await _adminDashboardRepository.GetallCompanyCodes(userId);

            return Ok(response);
        }

        [HttpGet, Route("GetCurrentPayperiod/{companyId}")]
        public IActionResult GetCurrentPayperiod(int companyId)
        {
            var response = _adminDashboardRepository.GetCurrentPayperiod(companyId);

            return Ok(response);
        }

        [HttpGet,Route("GetAllActiveRole")]
        public async Task<IActionResult> GetAllActiveRole()=>
            Ok(await this._roleRepository.GetAllActiveRole());

        [HttpGet, Route("GetAllProcessCategory")]
        public async Task<IActionResult> GetAllProcessCategory() =>
            Ok(await this._processCategoryRepository.GetAllProcessCategory());

        [HttpGet, Route("GetAccessType")]
        public async Task<IActionResult> GetAccessType() =>
            Ok(await this._accesstypeRepository.GetAllAccessType());

        [HttpGet, Route("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear() =>
         Ok(await this._financialYearRepository.GetFinancialYears());

        //[HttpGet, Route("GetAllCompanyCode/{userId}")]
        //public async Task<IActionResult> GetAllCompanyCode(int userId)
        //{
        //    var response = await _icompanyCode.GetallCompanyCodes(userId);

        //    return Ok(response);
        //}

        [HttpGet, Route("GetMapNamebyCompany/{companyId}")]
        public async Task<IActionResult> GetMapNamebyCompany(int companyId)
        {
            var response = await _icompanyCode.GetMapNamebyCompany(companyId);

            return Ok(response);
        }

        [HttpGet, Route("GetAutoEntityLocation/{CompanyId}")]
        public async Task<IActionResult> GetAutoEntityLocation(int CompanyId)
        {
            var response = await _icompanyCode.GetAutoEntityLocation(CompanyId);

            return Ok(response);
        }
        [HttpGet, Route("GetAllInputType")]
        public async Task<IActionResult> GetAllInputType()
        {
            var result = await _icompanyCode.GetAllInputType();

            return Ok(result);
        }
        [HttpPost, Route("GetLotwisePSDStatus")]
        public async Task<IActionResult> GetLotwisePSDStatus([FromForm] int companyId, [FromForm] int payPeriodId, [FromForm] int lotNumber, [FromForm] string Payroll_Input_Type)
        {

            var response = await _icompanyCode.GetLotwisePSDStatus(companyId, payPeriodId, lotNumber, Payroll_Input_Type);
            return Ok(response);
        }

        [HttpGet, Route("GetSitesByCompanyId/{companyId}")]
        public async Task<IActionResult> GetSitesByCompanyId(int companyId)
        {
            var response = await _icompanyCode.GetSitesByCompanyId(companyId);

            return Ok(response);
        }

        [HttpGet, Route("GetCityByCompanyCode/{CompanyCode}/{Group_Id}")]
        public async Task<IActionResult> GetCityByCompanyCode(string CompanyCode, int Group_Id)
        {
            var response = await _icompanyCode.GetCityByCompanyCode(CompanyCode, Group_Id);

            return Ok(response);
        }

        [HttpGet, Route("GetCityByStateId/{stateId}")]
        public async Task<IActionResult> GetCityByStateId(int stateId)
        {
            var state = await this._icompanyCode.GetCityByStateId(stateId);
            return Ok(state);
        }

        [HttpGet, Route("GetPayPeriod")]
        public async Task<IActionResult> GetPayPeriod()
        {
            var response = await _icompanyCode.GetPayPeriod();

            return Ok(response);
        }

        [HttpGet, Route("GetPaycodes")]
        public async Task<IActionResult> GetPaycodes()
        {
            var response = await _icompanyCode.GetPaycodes();

            return Ok(response);
        }

        [HttpGet, Route("GetMultiCommercialPaycodes")]
        public async Task<IActionResult> GetMultiCommercialPaycodes()
        {
            var response = await _icompanyCode.GetMultiCommercialPaycodes();

            return Ok(response);
        }

        [HttpGet, Route("GetAllState")]
        public async Task<IActionResult> GetAllState()
        {
            var state = await this._icompanyCode.GetAllState();
            return Ok(state);
        }
        [HttpGet, Route("GetClientGstStateList/{companyId}")]
        public async Task<IActionResult> GetClientGstStateList(int companyId)
        {
            var state = await this._icompanyCode.GetClientGstStateList(companyId);
            return Ok(state);
        }

        [HttpGet, Route("GetGSTTypes/{stateId}")]
        public async Task<IActionResult> GetGSTTypes(int stateId)
        {
            var state = await this._icompanyCode.GetGSTTypes(stateId);
            return Ok(state);
        }

        [HttpGet, Route("GetInvoiceCategory")]
        public async Task<IActionResult> GetInvoiceCategory()
        {
            var state = await this._icompanyCode.GetInvoiceCategory();
            return Ok(state);
        }


    }
}
