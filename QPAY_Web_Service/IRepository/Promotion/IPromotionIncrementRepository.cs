using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Common;
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
    public interface IPromotionIncrementRepository
    {       
        
        
        Task<DataSet> GetAllPayPeriodByCompanyID(int? companyId);
        Task<DataSet> GetEmployeeDetailsByCompanyID(int? companyId);
        Task<DataSet> GetAllIncrementDetails(int? companyId, int? employeeId, int? payPeriodId);

        Task<DataSet> GetAllIncrementDetailsByIncrementID(int? incrementId);

        Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy);


        //Task<DataSet> GetEligibleChildren(string Effective_Date, int Number_Of_Children);
        //Task<DataSet> Create(ChildrenEducationAllowanceRequest request);
        //Task<DataSet> Search(int? companyId, int? financialYearId, int? EmployeeId);
    }
}
