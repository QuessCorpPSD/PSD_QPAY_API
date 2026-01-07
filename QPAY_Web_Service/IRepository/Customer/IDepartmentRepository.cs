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
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllDepartmentDetails(string companyId);
        Task<DepartmentResponse> SaveUpdateDeleteDepartment([FromBody] DepartmentRequest request);
        Task<string> PostDepartmentUpload(string xmlString, string userId);
        DataSet DepartmentExport(int companyId);
    }
}
