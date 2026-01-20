using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.TaxAndSaving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IChildrenEducationAllowanceRepository
    {       
        Task<DataSet> sp_GetFinancialYear();
        Task<DataSet> GetAllType();
        Task<DataSet> GetEmployeesList(int? companyId, int? financialYearId);
        Task<DataSet> GetEligibleEmployee(int? financialYearId, int? EmployeeId);
        Task<DataSet> GetEligibleChildren(string Effective_Date, int Number_Of_Children);
        Task<DataSet> Create(ChildrenEducationAllowanceRequest request);
        Task<DataSet> Search(int? companyId, int? financialYearId, int? EmployeeId);
    }
}
