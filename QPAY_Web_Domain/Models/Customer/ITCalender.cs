using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Customer
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "ITCalenderDetails")]
    [System.Serializable()]
    public class ITCalenderResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("ITCalender")]
        public ITCalenderDetails[] ITCalender { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_IT_Calender")]
    public class ITCalenderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IT_Calender_Id { get; set; }

        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public int Financial_Year_Id { get; set; }
        public string Financial_Year_Name { get; set; }
        public string Declaration_CutOff_Date { get; set; }
        public string Submission_CutOff_Date { get; set; }
        public bool IsActive { get; set; }
        public string Error_Message { get; set; }
        public int Serial_No { get; set; }
    }

    public class ITCalenderRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public ITCalenderDetails parentDetail { get; set; }

    }

}