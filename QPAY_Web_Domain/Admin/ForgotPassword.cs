using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Admin
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_User")]
    public class ForgotPassword : ForgotPasswordFormat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Subject { get; set; } = string.Empty;
        public string Mail_Id { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


        public string User_Name { get; set; } = string.Empty;

        //public string Secret_Answer3 { get; set; }
        public string Error_Message { get; set; } = string.Empty;
        public bool IsLinkExpired { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    public class GetSecretQuestion
    {
        public string Secret_Question_Name { get; set; } = string.Empty;
        public int? Secret_Question_Id { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "forgotPasswordResponse")]
    [System.Serializable()]
    public class forgotPasswordResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("ForgotPassword")]
        public ForgotPassword[] ForgotPasswordDetails { get; set; } = new ForgotPassword[0];
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "forgotPasswordResponse")]
    [System.Serializable()]
    public class ForgotPasswordMailformat
    {

        public int Flag { get; set; }
        public string User_Name { get; set; } = string.Empty;
        public string Error_Message { get; set; } = string.Empty;
    }
    public class ForgotPasswordFormat
    {
        public int Flag { get; set; }
        public string Forgot_Password { get; set; } = string.Empty;
        public string Email_Id { get; set; } = string.Empty;
    }
}
