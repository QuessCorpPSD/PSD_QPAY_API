using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Common;
using QPay.BAL.Repository.Common;

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

        public CommonController(
            IRoleRepository roleRepository,
            IProcessCategoryRepository processCategoryRepository,
            IAccesstypeRepository accesstypeRepository,
            IFinancialYearRepository financialYearRepository,
            IAdminDashboardRepository adminDashboardRepository)
        {
            _roleRepository = roleRepository;
            _processCategoryRepository = processCategoryRepository;
            _accesstypeRepository = accesstypeRepository;
            _financialYearRepository = financialYearRepository;
            _adminDashboardRepository = adminDashboardRepository;
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
    }
}
