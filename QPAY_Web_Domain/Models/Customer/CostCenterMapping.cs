using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Customer
{
    public class CostCenterMapping
    {
        public int? Cost_Center_Mapping_Id { get; set; }
        public string? Map_Name { get; set; }
        public int? Business_Unit_Id { get; set; }
        public string? Business_Unit_Name { get; set; }
        public int? Company_Id { get; set; }
        public string? City_Name { get; set; }
        public string? Company_Code { get; set; }
        public string? Company_Name { get; set; }
        public int? Group_Detail_Id { get; set; }
        public string? Group_Name { get; set; }
        public int? Serial_No { get; set; }
        public string? Error_Message { get; set; }
        public bool IsActive { get; set; }  
        public string? SPOC_Name { get; set; }
        public string? Cost_Center_Name { get; set; }
        public string? GRN_Number { get; set; }



    }
    public class CostCenterResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }
    public class CostCenterRequest
    {
        public string Created_By { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public List<CostCenterMapping> CostCentermaster { get; set; }
    }
}
