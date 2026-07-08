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
    public interface IReleaseholdemployeesalaryRepository
    {
        Task<DataSet> search(int Company_Id, int Pay_Period_Id, int? Employee_Id);
        Task<DataSet> ExportToExcel(CommonExports payload);
        Task<ReleaseHoldSalaryResponse> UploadReleaseholdsalary(IFormFile file, string CreatedBy, string action);
        Task<ReleaseHoldSalaryResponse> SaveReleaseHoldSalary(ReleaseHoldSalaryRequest request);
    }
}
