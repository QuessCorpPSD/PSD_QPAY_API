using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface ICompanyPaycodeMappingRepository
    {

        Task<DataTable> Search(int? companyId);
        Task<DataSet> ExportToExcel(int? companyId);
        Task<DataSet> GetAllCompanyPayCodePickFrom();
        Task<DataSet> GetAllPaycodeCompanyPacode(string? PayCode, int? PayTypeId, int? IsTaxable, int? PayId);
        Task<DataSet> Create(string companyXml,string paycodeXml, string mode, int? User_Id);

    }
}
