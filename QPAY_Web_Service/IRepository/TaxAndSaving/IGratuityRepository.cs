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
    public interface IGratuityRepository
    {       
        Task<DataSet> GetEmployeeCodeForGratuity(int? companyId,int? FinancialYrId);
        Task<DataSet> GetBasicAmountByEmployeeId(int? employeeId);
        Task<DataSet> GetDAAmountByEmployeeId(int? employeeId);

        Task<DataSet> GetGratuityEmployeeByEmpId(int? employeeId);


        Task<DataSet> Create(GratuityRequest request);
        Task<DataSet> Search(int? companyId, int? EmployeeId);
    }
}
