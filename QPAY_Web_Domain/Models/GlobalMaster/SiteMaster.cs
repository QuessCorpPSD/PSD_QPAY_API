using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.GlobalMaster
{
    public class SiteMaster
    {
    }

    public class SiteMasterResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

    public class PortalPayslipFormatUI
    {
        public string? Portal_Payslip_Format { get; set; }
        public string? Portal_Payslip_Format_Name { get; set; }
    }

    public class CreateUpdateSitemasterRequest
    {
        public string? Action { get; set; }
        public string? UserId { get; set; }
        public int Company_Id { get; set; }
        public string? Group_Name { get; set; }
        public int? Group_Id { get; set; }
        public int? Group_Detail_Id { get; set; }
        public int Client_Id { get; set; }
        public string? CostCenter_Id { get; set; }
        public string? Establishment_Name { get; set; }
        public string? Establishment_Adress1 { get; set; }
        public string? Principal_Employer_Name { get; set; }
        public string? Principal_Employe_Address1 { get; set; }
        public string? Contractor_Name { get; set; }
        public string? Contractor_Address1 { get; set; }
        public int? PAYSLIP_FORMAT_Id { get; set; }
        public int? PAYSLIP_FORMAT { get; set; }
        public int? IsBonusPayThroughFF { get; set; }
        public int? LeaveApplicable { get; set; }
        public string? SAP_Cust_Code { get; set; }
        public string? SAP_Cust_Name { get; set; }
        public string? StartDate { get; set; }
        public string? WBS2 { get; set; }
        public string? WBS_Name { get; set; }
        public string? SalaryDate { get; set; }
        public string? Portal_Payslip_Format { get; set; }
        public int? Value { get; set; }
    }
}
