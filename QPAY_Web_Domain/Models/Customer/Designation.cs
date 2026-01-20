using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Customer
{
    public class Designation
    {
        public int? Designation_Id { get; set; }
        public string? Designation_Code { get; set; }
        public string? Designation_Name { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_Code { get; set; }
        public int? Serial_No { get; set; }
        public string? Error_Message { get; set; }
        public string? Standard_Designation { get; set; }
        public string? Amount { get; set; }
        public string? Skill_Category { get; set; }
        public int? NpDays { get; set; }
    }
    public class DesignationResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }
    public class DesignationRequest
    {
        public string Created_By { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public List<Designation> Designationmaster { get; set; }
    }
}
