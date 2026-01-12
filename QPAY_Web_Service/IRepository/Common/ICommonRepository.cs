using DocumentFormat.OpenXml.Bibliography;
using QPay.UI.Common;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Common
{
    public interface ICommonRepository
    {
        Task<List<CompanyPicker>> GetallCompanyCodes(int userId);
        Task<List<PayperiodDD>> GetAllPayperiod(int companyId);
        List<PayperiodDD> GetCurrentPayperiod(int companyId);
        Task<List<MapnameDD>> GetMapNamebyCompany(int companyId);
        Task<List<InputTypeDD>> GetAllInputType();
        Task<List<PSDStatus>> GetLotwisePSDStatus(int companyId, int payPeriodId, int lotNumber, string Payroll_Input_Type);
        Task<List<Site>> GetSitesByCompanyId(int companyId);
        Task<List<UI.Common.City>> GetCityByCompanyCode(string CompanyCode, int Group_Id);
        Task<List<AllPayperiod>> GetPayPeriod();
        Task<List<Paycodes>> GetPaycodes();
        Task<List<StateUI>> GetAllState();
        Task<List<CityUI>> GetCityByStateId(int stateId);

    }
}
