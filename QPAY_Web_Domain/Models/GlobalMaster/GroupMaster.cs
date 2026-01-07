using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using QPay.UI.Common;

namespace QPay.UI.GlobalMaster
{
    public class GroupMasterResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("GroupMaster")]
        public GroupMaster[] groupMaster { get; set; }
    }
    public class GroupMasterDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("GroupMasterDetails")]
        public GroupMasterDetail[] groupMasterResponseDetails { get; set; }
    }
    public class GroupMasterDetail
    {
        public int Group_Detail_Id { get; set;}
        public int Group_Id { get; set; }
        public int Company_Id { get; set; }
        public string CostCenter_Id { get; set; }
        public string SAP_Cust_Code { get; set; }
        public string SAP_Cust_Name { get; set; }

        public string WBS2 { get; set; }
        public string WBS_Name { get; set; }
        public string WBS_Invoice { get; set; }
        
        public string Group_Name { get; set;}
        public int Client_Id { get; set; }
        public string Client_Name { get; set; }
       
        public string Establishment_Name { get; set; }
        public string Establishment_Adress1 { get; set; }
        public string Principal_Employer_Name { get; set; }
        public string Principal_Employe_Address1 { get; set; }
        public string Contractor_Name { get; set; }
        public string Contractor_Address1 { get; set; }
        public string Error_Message { get; set; }
        public string PAYSLIP_FORMAT { get; set; }
        public int PAYSLIP_FORMAT_Id { get; set; }

        public bool? IsBonusPayThroughFF { get; set; }
        public string IsBonusPayThroughFFDisplay { get; set; }
        public bool? IsExtraWorkingDaysServiceFee { get; set; }
        public string IsExtraWorkingDaysServiceFeeDisplay { get; set; }

        public string IsLeaveApplicable { get; set; }
        public bool LeaveApplicable { get; set; }

        public string Casual_Leave { get; set; }
        public string Sick_Leave { get; set; }

        public string StartDate { get; set; }

        public string MainCustomerCode { get; set; }

        public string Is_NonInvoice { get; set; }

        public string SalaryDate { get; set; }
        public string Portal_Payslip_Format { get; set; }
        public string Portal_Payslip_Format_Name { get; set; }
        public  string PF_Code_Location { get; set; }
        public int PF_ID { get; set; }
        public string PF_CATEGORY { get; set; }
        public int LEAVE_ID { get; set; }
        public string LEAVE_CATEGORY { get; set; }
        public int LEAVE_TYPE_ID { get; set; }
        public string LEAVE_TYPE { get; set; }
        //added by Karuna
        public string PLE_Formula { get; set; }
        //added ends here

        public string GRTCT { get; set; }

    }

    public class GroupMaster
    {
        public int Group_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
        
    }

    

    public class GroupMasterRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public GroupMaster parentDetail { get; set; }

        public GroupMasterDetail childDetail { get; set; }

    }

}
