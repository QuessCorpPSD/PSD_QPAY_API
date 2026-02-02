using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.TaxAndSaving;
using QPay.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IHRARepository
    {       
        Task<DataSet> Create(HRARequest request);
        Task<DataSet> Search(int? companyID, int? employeeID, int? FinYearId);
        Task<DataSet> GetEmployeeListAdd(int? companyID, int? FinYearId,int? employeeID);
        Task<DataSet> GetDeclarationType();

        Task<RequestResponse> Upload(IFormFile file, [FromForm] int CreatedBy);
    }
}
