using Dapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Common;
using QPay.DAL.Repository;
using QPay.UI.Common;

namespace QPay.IRepository.Repository.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly DbRepository _dbRepository;

        public CommonRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<CompanyPicker>> GetallCompanyCodes(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@USER_ID", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Get_AllCompanyCodebyUserId", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CompanyPicker>>(res) ?? new List<CompanyPicker>();
            }

            return new List<CompanyPicker>();
        }

        public async Task<List<PayperiodDD>> GetAllPayperiod(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllPayperiodByCompanyId", parameters);
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }
        public List<PayperiodDD> GetCurrentPayperiod(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = this._dbRepository.GetItemsAsync("Proc_GetCurrentPayperiod", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }
        public async Task<List<MapnameDD>> GetMapNamebyCompany(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = await this._dbRepository.GetItemsAsync("sp_GetAllMapNameByCompanyId_NewUI", parameters);
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<MapnameDD>>(res) ?? new List<MapnameDD>();
            }

            return new List<MapnameDD>();
        }
        public async Task<List<InputTypeDD>> GetAllInputType()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllInputTypes", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InputTypeDD>>(res) ?? new List<InputTypeDD>();
            }

            return new List<InputTypeDD>();
        }
        public async Task<List<PSDStatus>> GetLotwisePSDStatus(int companyId, int payPeriodId, int lotNumber, string Payroll_Input_Type)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@pay_period_id", payPeriodId);
            parameters.Add("@lotnumber", lotNumber);
            parameters.Add("@Payroll_Input_Type", Payroll_Input_Type);


            var res = await this._dbRepository.GetItemsAsync("Proc_InputLotStatus", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PSDStatus>>(res) ?? new List<PSDStatus>();
            }

            return new List<PSDStatus>();
        }

        public async Task<List<Site>> GetSitesByCompanyId(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);

            var res = await this._dbRepository.GetItemsAsync("Proc_view_SitesByCompanyId", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Site>>(res) ?? new List<Site>();
            }

            return new List<Site>();
        }

        public async Task<List<City>> GetCityByCompanyCode(string CompanyCode, int Group_Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyCode", CompanyCode);
            parameters.Add("@Group_Detail_Id", Group_Id);

            var res = await this._dbRepository.GetItemsAsync("Proc_GetCityByCompanyCode", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<City>>(res) ?? new List<City>();
            }

            return new List<City>();
        }

        public async Task<List<AllPayperiod>> GetPayPeriod()
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllPayPeriod", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<AllPayperiod>>(res) ?? new List<AllPayperiod>();
            }
            return new List<AllPayperiod>();
        }

        public async Task<List<StateUI>> GetAllState()
        {
            string storeProcedure = "[dbo].[sp_GetAllStates]" ?? "";
            var parameter = new DynamicParameters();
            //parameter.Add("@CompanyId", companyid);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<StateUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<StateUI>>(res);
                return list?.ToList() ?? new List<StateUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<StateUI>();
            }

        }
        public async Task<List<StateResponse>> GetClientGstStateList(int companyid)
        {
            string storeProcedure = "[dbo].[Proc_ManageClientGst]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@CompanyId", companyid);
            parameter.Add("@Action", "Get");
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<StateResponse>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<StateResponse>>(res);
                return list?.ToList() ?? new List<StateResponse>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<StateResponse>();
            }

        }
        public async Task<List<CityUI>> GetCityByStateId(int stateId)
        {
            string storeProcedure = "[dbo].[sp_GetAllCityByStateId]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@StateId", stateId);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<CityUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<CityUI>>(res);
                return list?.ToList() ?? new List<CityUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<CityUI>();
            }

        }

        public async Task<List<CityName>> GetAutoEntityLocation(int CompanyId)
        {
            string storeProcedure = "[dbo].[USP_CommonDropDowns]" ?? "";
            var parameter = new DynamicParameters();
           parameter.Add("@CompanyId", CompanyId);
            parameter.Add("@Description", "");
           parameter.Add("@Action", "GetAutoEntityLocation");
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<CityName>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<CityName>>(res);
                return list?.ToList() ?? new List<CityName>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<CityName>();
            }

        }

        public async Task<List<Paycodes>> GetPaycodes()
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync("sp_GetAllPaycode", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Paycodes>>(res) ?? new List<Paycodes>();
            }
            return new List<Paycodes>();
        }

        public async Task<List<Paycodes>> GetMultiCommercialPaycodes()
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync("sp_GetAllMultiCommercialPaycode", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Paycodes>>(res) ?? new List<Paycodes>();
            }
            return new List<Paycodes>();
        }

        public async Task<List<GSTType>> GetGSTTypes(int stateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StateId", stateId);
            var res = await this._dbRepository.GetItemsAsync("Proc_GetGstTypebyStateId", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<GSTType>>(res) ?? new List<GSTType>();
            }
            return new List<GSTType>();
        }
        public async Task<List<InvoiceCategories>> GetInvoiceCategory()
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllInvoiceCategories", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InvoiceCategories>>(res) ?? new List<InvoiceCategories>();
            }
            return new List<InvoiceCategories>();
        }

    }
}
