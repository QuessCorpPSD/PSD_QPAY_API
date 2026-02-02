using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.GlobalMaster
{
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false, ElementName = "BankDetails")]
    [Serializable()]
    public class BankResponse
    {
        [System.Xml.Serialization.XmlElement("Bank")]
        public Bank[] BankDetails { get; set; }
    }

    public class IFSCResponse
    {
        [System.Xml.Serialization.XmlElement("IFSC")]
        public IFSC[] IFSCDetails { get; set; }
    }

    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [Table("tbl_Bank")]
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Bank_Id { get; set; }
        public string Bank_Name { get; set; }
        public int Serial_No { get; set; }
        public int Bank_Account_Number_Digits { get; set; }
        public string Error_Message { get; set; }
        public string Digit_Length_Condition { get; set; }

    }

    [Table("tbl_IFSC")]
    public class IFSC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IFSC_Id { get; set; }

        public string IFSC_Code { get; set; }
        public int Bank_Id { get; set; }
        public string Bank_Name { get; set; }
        public int Serial_No { get; set; }
        public string Error_Message { get; set; }
    }


    public class BankRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public Bank detail { get; set; }

    }
}