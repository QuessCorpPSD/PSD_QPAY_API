using QPay.UI.Customer;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Models.TaxAndSaving
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "ChildrenEducationAllowance")]
    [System.Serializable()]
    public class ChildrenEducationAllowanceResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("ChildrenEducation")]
        public ChildrenEducationAllowance[] ChildrenEducationAllowance { get; set; }
    }

    public class ChildrenEducationAllowanceDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("ChildrenEducationDetail")]
        public ChildrenEducationAllowanceDetail[] ChildrenEducationAllownceDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Children_Education_Allowance")]
    public class ChildrenEducationAllowance
    {
        public int Children_Education_Allowance_Id { get; set; }
        public string Declaration_Date { get; set; }
        public int Employee_Id { get; set; }
        public int Financial_Year_Id { get; set; }
        public int Number_Of_Children { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public decimal Claim_Amount { get; set; }
        public decimal Eligible_Amount { get; set; }
        public bool Is_Tuition_Eligible { get; set; }
        public bool Is_Hostel_Eligible { get; set; }

    }

    [Table("tbl_Children_Education_Allowance_Detail")]
    public class ChildrenEducationAllowanceDetail
    {
        public int Children_Education_Allowance_Detail_Id { get; set; }
        public int Children_Education_Allowance_Id { get; set; }
        public string Student_Name { get; set; }
        public string School_Name { get; set; }
        public string Hostel_Name { get; set; }
        public string Phone_Number { get; set; }
        public decimal Exemption_Amount { get; set; }
    }

    public class ChildrenEducationAllowanceRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public ChildrenEducationAllowance parentDetail { get; set; }
        public List<ChildrenEducationAllowanceDetail> childDetail { get; set; }

    }

}