using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    /// <summary>
    /// User DB Object
    /// </summary>
    [Table("tbl_User")]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_Id { get; set; }
        //public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Error_Message { get; set; }
        public string Role_Name { get; set; }
        public int Role_Id { get; set; }
        // public string Page_Name { get; set; }
        public int PasswordExpire { get; set; }
        public int isFirstlogin { get; set; }
        public int EmployeeID { get; set; }

        public int TeamLeadUserId { get; set; }
        public string TeamLeadEmailId { get; set; }
        public int ManagerId { get; set; }
        public string ManagerEmailId { get; set; }

        public string Fun_Manager_User_Id { get; set; }

        public string Fun_Manager_Email_Id { get; set; }

        //public string Contact_Number { get; set; }
        // public string MacAddress { get; set; }
    }

    [Table("tbl_LoginHistory")]
    public class LoginHistoryModel
    {
        public int UserId { get; set; }
        public string MACAddess { get; set; }
        public string HostName { get; set; }
        public string PrimaryDNS { get; set; }
        public string LoginIP { get; set; }
        public string LoginTime { get; set; }
        public string LogOutIP { get; set; }
        public string LogOutTime { get; set; }
        public bool UserActive { get; set; }
        public int AuditAction { get; set; }
        public int status { get; set; }
        public LoginHistoryModel() { }
        public LoginHistoryModel(int UserId, Enum_AuditAction AuditAction, string MACAddess = null, string HostName = null, string PrimaryDNS = null, string LoginIP = null, string LoginTime = null, string LogOutIP = null, string LogOutTime = null, bool UserActive = false, Enum_AuditActionStatus status = Enum_AuditActionStatus.Unknown)
        {
            this.UserId = UserId;
            this.MACAddess = MACAddess;
            this.HostName = HostName;
            this.PrimaryDNS = PrimaryDNS;
            this.LoginIP = LoginIP;
            this.LoginTime = LoginTime;
            this.LogOutIP = LogOutIP;
            this.LogOutTime = LogOutTime;
            this.UserActive = UserActive;
            this.AuditAction = (int)AuditAction;
            this.status = (int)status;
        }

    }

    [Table("Tbl_UserAudit")]
    public class LoginAuditModel
    {
        public int UserId { get; set; }
        public string MACAddess { get; set; }
        public string IP_Address { get; set; }
        public string HostName { get; set; }
        public string PrimaryDNS { get; set; }
        public int AuditAction { get; set; }
        public int status { get; set; }
        public string CreatedOn { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }

        public LoginAuditModel() { }

        public LoginAuditModel(int UserId, Enum_AuditAction AuditAction, string MACAddess = null, string IP_Address = null, string HostName = null, string PrimaryDNS = null, string CreatedOn = null, Enum_AuditActionStatus status = Enum_AuditActionStatus.Unknown, string NewPassword = null, string OldPassword = null)
        {
            this.UserId = UserId;
            this.IP_Address = IP_Address;
            this.MACAddess = MACAddess;
            this.HostName = HostName;
            this.PrimaryDNS = PrimaryDNS;
            this.CreatedOn = CreatedOn;
            this.AuditAction = (int)AuditAction;
            this.status = (int)status;
            this.NewPassword = NewPassword;
            this.OldPassword = OldPassword;

        }

    }
    public class UsersLoginHistoryViewModel : LoginHistoryModel
    {
        public string UserName { get; set; }
        public int EmployeeID { get; set; }
        public string Password { get; set; }
        public int PasswordExpire { get; set; }
        public int PasswordAttempt { get; set; }
        public int isFirstlogin { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string ErrorMessage { get; set; }

    }
}
