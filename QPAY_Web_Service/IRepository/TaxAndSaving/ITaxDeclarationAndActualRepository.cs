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
    public interface ITaxDeclarationAndActualRepository
    {
        Task<DataSet> GetAllTaxCodes();
        Task<DataSet> GetTaxCodes(string TaxCode);

        Task<DataSet> GetEligibleAmtByEmpIDTaxCode(int Employee_Id, int Financial_Year_Id, int Computation_Rule_Id);
        Task<DataSet> Search(int? companyId, int? EmployeeId);

        Task<DataSet> Create(TaxDeclarationAndActualRequest request);
        Task<RequestResponse> Upload(IFormFile file, [FromForm] int CreatedBy);
    }
}
