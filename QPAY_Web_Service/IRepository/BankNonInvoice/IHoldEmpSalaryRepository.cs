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
    public interface IHoldEmpSalaryRepository
    {
        Task<DataSet> SearchHoldEmpSalary(int CompanyId, int PayPeriodId, string Status);

        Task<DataSet> GetSalaryHoldType();

        Task<DataSet> ExportToExcel(HoldEmpSalaryExportRequest payload);

        Task<HoldEmpSalaryResponse> UploadHoldEmpSalary(IFormFile file, string User);
    }
}
