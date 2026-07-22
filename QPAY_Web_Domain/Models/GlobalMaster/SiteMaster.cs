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
        public string? Branch_Name { get; set; }
        public string? LWW_Formula { get; set; }
        public string? ATB_Formula { get; set; }
        public string? Auth_OT_Formula { get; set; }
        public string? NFH_Formula { get; set; }
        public string? Unauthorized_OT { get; set; }
        public string? Additional_Formula_1 { get; set; }
        public string? Additional_Formula_2 { get; set; }
        public string? Additional_Formula_3 { get; set; }
        public string? AROTHRS { get; set; }
        public string? ROTHRS { get; set; }
        public string? GRAT { get; set; }
        public int? City_Id { get; set; }
        public string? Establishment_Name { get; set; }
        public string? Establishment_Adress1 { get; set; }
        public string? Establishment_Adress2 { get; set; }
        public string? Establishment_Adress3 { get; set; }
        public string? Principal_Employer_Name { get; set; }
        public string? Principal_Employe_Address1 { get; set; }
        public string? Principal_Employe_Address2 { get; set; }
        public string? Principal_Employe_Address3 { get; set; }
        public string? Contractor_Name { get; set; }
        public string? Contractor_Address1 { get; set; }
        public string? Contractor_Address2 { get; set; }
        public string? Contractor_Address3 { get; set; }
        public string? OPS_Manager { get; set; }
        public string? Site_Incharge { get; set; }
        public int? PAYSLIP_FORMAT_Id { get; set; }
        public string? PAYSLIP_FORMAT { get; set; }
        public string? PROVISION_BONUS { get; set; }
        public string? Leave_Credit { get; set; }
        public string? Region { get; set; }
        public string? Po_HeadCount { get; set; }
        public int? IsBonusPayThroughFF { get; set; }
        public int? IsExtraWorkingDaysServiceFee { get; set; }
        public int? LeaveApplicable { get; set; }
        public string? CasualLeave { get; set; }
        public string? SickLeave { get; set; }
        public string? MainCustomerCode { get; set; }
        public string? StartDate { get; set; }
        public string? SAP_Cust_Code { get; set; }
        public string? SAP_Cust_Name { get; set; }
        public string? WBS2 { get; set; }
        public string? WBS_Name { get; set; }
        public string? Flex1 { get; set; }
        public string? Flex2 { get; set; }
        public string? SalaryDate { get; set; }
        public string? Portal_Payslip_Format { get; set; }
        public string? PF_Code_Location { get; set; }
        public int? PF_ID { get; set; }
        public int? LEAVE_ID { get; set; }
        public int? LEAVE_TYPE_ID { get; set; }
        public string? PLE_Formula { get; set; }
        public string? GRTCT { get; set; }
        public string? WorkingHours { get; set; }
        public int? FurloughLeaveApplicability { get; set; }
        public int? FurloughBillingApplicability { get; set; }
        public int? Po_Salary { get; set; }
        public int? Po_OtherIncome { get; set; }
        public string? Discount_Type { get; set; }
        public float? Discount_Value { get; set; }
    }

    public class SiteInchargeUI
    {
        public string Site_Incharge_Id { get; set; } = "";
        public string Site_Incharge { get; set; } = "";
    }

    public class SMCityUI
    {
        public string CITY_ID { get; set; } = "";
        public string City_Name { get; set; } = "";
        public string State_Name { get; set; } = "";
    }

    public class PFCategoryUI
    {
        public string PF_ID { get; set; } = "";
        public string PF_CATEGORY { get; set; } = "";
    }
    public class LeaveCategoryUI
    {
        public string LEAVE_ID { get; set; } = "";
        public string LEAVE_CATEGORY { get; set; } = "";
    }

    public class LeaveTypeUI
    {
        public string LEAVE_TYPE_ID { get; set; } = "";
        public string LEAVE_TYPE { get; set; } = "";
    }

}
