using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface IEmployeeRepository
    {
        Task<DataSet> SearchDetails(int? Company, string? Employee_code);

        Task<DataSet> ExportToExcel(int? CompanyId, string EmployeeId, int? EActive);

        Task<List<CategoryUI>> GetCategory();

        Task<DataSet> Department(int? companyId);
        Task<DataSet> Designation(int? companyId);
        Task<DataSet> BillingDesignation(int? companyId);
        Task<DataSet> Costcentermapping(int? companyId);
        Task<DataSet> Band(int? companyId);
        Task<DataSet> GetCostCenter(int? companyId);
        Task<DataSet> GroupMater(int? companyId);
        Task<DataSet> GetAllPayPeriodByCompanyID(int? companyId);

        Task<DataSet> SearchBank();
        Task<DataSet> GetRole();
        Task<DataSet> GetEmploymentType();
        Task<EmployeeApiResponse> PostEmployeeUpload(string xmlString, string userId);
        Task<EmployeeApiResponse> PostEmployeeSalaryUpload(string xmlString, string userId);

        Task<DataSet> Create(EmployeeRequest request);
        Task<DataSet> BankCreate(EmployeeBankRequest request);
        Task<DataSet> InformationCreate(EmployeeInformationRequest request);
        Task<DataSet> ContactCreate(EmployeeContactRequest request);
        Task<DataSet> PersonalCreate(EmployeePersonalRequest request);
        Task<DataSet> PreviousCreate(EmployeePreviousRequest request);

        Task<DataSet> SearchSalary(int employeeId);
        Task<List<LegalEntityUI>> GetLegalEntity();

    }
}
