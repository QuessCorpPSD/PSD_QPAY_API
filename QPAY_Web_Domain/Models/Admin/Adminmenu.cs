using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.UI.Models.Admin
{
    public class Adminmenu
    {
 
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? RoleId { get; set; }
        public int? IsCheck { get; set; }


    }
    [XmlRoot("UserResponse")]
    public class adminmenurequest
    {
        [XmlIgnore]
        public int createdBy { get; set; }
        [XmlIgnore]
        public string mode { get; set; }
        [XmlElement("UserDetails")]
        public UserDetails UserDetails { get; set; }

    }

    public class UserDetails
    {
        public int? User_Id { get; set; }

        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public string? Mail_Id { get; set; }
        public int? Reporting_To { get; set; }
        public int? Role_Id { get; set; }
        public int? Access_Type_Id { get; set; }
        public int? EmployeeID { get; set; }
        public int? IsActive { get; set; }
        
    }

    public class Roles
    {
        public string? Action { get; set; }

        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public int? IsSysAdmin { get; set; }
        public int? IsActive { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }

    }

    public class CompanyPermission
    {

        public int? Userid { get; set; }
        public int? Businessunitnameid { get; set; }
        public int? CompanyPermissionId { get; set; }


    }
    public class EditPermissiondetails
    {

        public int? Userid { get; set; }
        public int? Businessunitnameid { get; set; }
        public int? CompanyPermissionId { get; set; }


    }

    public class LoadCompany
    {

        public int? BusinessUnitNameId { get; set; }
        public int? BusinessZonenName { get; set; }


    }

    [XmlRoot("CompanyPermissionResponse")]
    public class CompanyPermissionRequest
    {
        [XmlIgnore]
        public int createdBy { get; set; }

        [XmlIgnore]
        public string mode { get; set; }

        [XmlElement("CompanyPermissionModelResponse")]
        public CompanyPermissionModel CompanyPermissionModel { get; set; }

        [XmlElement("CompanyPermissionModelDetailsResponse")]
        public List<CompanyPermissionDetails> CompanyPermissionDetails { get; set; }
    }

    public class CompanyPermissionModel
    {
        [XmlElement("User_Id")]
        public int? User_Id { get; set; }

        [XmlElement("Business_Unit_Name_id")]
        public int? Business_Unit_Name_id { get; set; }
        [XmlElement("Company_Permission_Id")]
        public int? Company_Permission_Id { get; set; }
    }

    public class CompanyPermissionDetails
    {
        [XmlElement("Company_Permission_Details_Id")]
        public int? Company_Permission_Details_Id { get; set; }

        [XmlElement("Company_Permission_Id")]
        public int? Company_Permission_Id { get; set; }

        [XmlElement("Is_Permission")]
        public bool? Is_Permission { get; set; }

        [XmlElement("Company_Id")]
        public int? Company_Id { get; set; }

        [XmlElement("Company_Code")]
        public string? Company_Code { get; set; }
    }
}
