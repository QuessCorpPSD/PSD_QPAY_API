using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Common;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.TaxAndSaving;
using QPay.UI.Reimbursements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IReimbursementRepository
    {
        Task<DataSet> Search(int? companyId, int? financialYearId, int? employeeId);


        Task<DataSet> GetAllFrequency(int? companyId,int? financialYearId);
        Task<DataSet> GetAllRembPaycodes(int? companyId);
        Task<DataSet> GetReimbursementDetail(int? reimbursementId);
        Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy);

        Task<DataSet> Create(ReimbursementRequest request);
    }
}
