using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Admin
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "ChangePasswordDetails")]
    [System.Serializable()]
    public class ChangePwdResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("ChangePassword")]
        public ChangePassword[] ChangePasswordUser { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ChangePassword
    {
        public int User_Id { get; set; }
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConformNewPassword { get; set; }
        public string Salt { get; set; }
        public int employeeId { get; set; }
        public string Error_Message { get; set; }
    }
}
