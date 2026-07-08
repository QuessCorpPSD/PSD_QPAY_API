using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.BAL.IRepository.BankNonInvoice
{
    public interface IEmployeeSalaryReleaseRepository
    {
        Task<DataSet> SearchEmployeeSalaryRelease(int CompanyId, int PayPeriodId);

        Task<DataSet> ExportToExcel(CommonExport payload);

        Task<EmployeeSalaryReleaseResponse> UploadEmployeeSalaryRelease(IFormFile file, string User);
    }
}
