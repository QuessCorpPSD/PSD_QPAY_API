using QPay.UI.Customer;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Customer.Company;

namespace QPay.BAL.IRepository.Customer
{
    public interface ICompanyRepository
    {
        Task<DataTable> Search(string action, int? companyId, string xml);
        Task<DataSet> View(string action, int? companyId, string xml);
        Task<DataSet> ExportToExcel(string action, int? companyId, string xml);
        Task<CompanyDetails> masters();

        Task<DataSet> Create(CompanyCreateRequest request);
        Task<DataSet> Update(CompanyUpdateRequestPayload request);
        Task<DataSet> DeleteCompany(CompanyDeleteRequest request);
        Task<DataSet> GetBussinessunitLocation(int? BusinessUnitId);
        Task<DataSet> GetCityBasedonState(int? Stateid);

        Task<DataSet> GetStatebasedoncity(int? cityid);

        Task<DataSet> GetInvoiceFormat();
        Task<DataSet> GetReimbInvoiceFormat();
        Task<DataSet> GetPortalPayslipFormat();

    }
}
