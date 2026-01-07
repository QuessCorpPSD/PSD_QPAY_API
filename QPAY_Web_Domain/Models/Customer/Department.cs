using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Customer
{
    public class Department
    {
        public int? Department_Id { get; set; }
        public string? Department_Code { get; set; }
        public string? Department_Name { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_Code { get; set; }
        public int? Serial_No { get; set; }
        public string? Error_Message { get; set; }
    }
    public class DepartmentResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }
    public class DepartmentRequest
    {
        public string Created_By { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public List<Department> Departmentmaster { get; set; }
    }
}
