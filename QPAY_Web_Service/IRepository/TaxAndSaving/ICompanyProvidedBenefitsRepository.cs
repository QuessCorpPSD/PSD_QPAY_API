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
    public interface ICompanyProvidedBenefitsRepository
    {       
        Task<DataSet> GetPerkCodes();
        Task<DataSet> GetEmployeesList(int? companyId);

        Task<DataSet> Create(CompanyProvidedBenefitsRequest request);
        Task<DataSet> Search(int? companyId,  int? EmployeeId);

        Task<RequestResponse> Upload(IFormFile file, [FromForm] int CreatedBy);
    }
}
