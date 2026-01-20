using QPay.UI.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Invoice
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "PayFrequencyDetail")]
    [System.Serializable()]
    public class BillingPayFrequencyResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("PayFrequency")]
        public BillingPayFrequency[] PayFrequencys { get; set; }
    }

    [XmlRoot("PayFrequency")]
    public class BillingPayFrequency
    {
        public int Pay_Frequency_Id { get; set; }

        public int Company_Id { get; set; }
        public int Group_Id { get; set; }
        public string Starting_Date { get; set; }
        public string Ending_Date { get; set; }
    }

    public class BillingPayFrequencyDetail
    {
        public int Pay_Frequency_Detail_Id { get; set; }
        public int Pay_Frequency_Id { get; set; }
        public string Pay_Sequence_Number { get; set; }
        public string Pay_Period { get; set; }
        public string Start_At { get; set; }
        public string End_At { get; set; }
        public string Salary_Date { get; set; }
        public int Pay_Period_Days { get; set; }
        public int Weekly_Holidays { get; set; }
        public int Monthly_Holidays { get; set; }
        public int Other_Holidays { get; set; }
        public int Working_Days { get; set; }
    }

    public class BillingPayFrequencyWithDetail
    {
        public int Pay_Frequency_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public int Group_Id { get; set; }
        public string Group { get; set; }
        public DateTime Starting_Date { get; set; }
        public DateTime Ending_Date { get; set; }
        public int Pay_Frequency_Detail_Id { get; set; }
        public string Pay_Sequence_Number { get; set; }
        public string Pay_Period { get; set; }
        public DateTime Start_At { get; set; }
        public DateTime End_At { get; set; }
        public DateTime Salary_Date { get; set; }
        public int Pay_Period_Days { get; set; }
        public int Weekly_Holidays { get; set; }
        public int Monthly_Holidays { get; set; }
        public int Other_Holidays { get; set; }
        public int Working_Days { get; set; }
        public string Error_Message { get; set; }
        public int SNo { get; set; }
        public int CountWithGroupIDZero { get; set; }
        public int CountWithGroupIDGreater { get; set; }
        public int Count { get; set; }
    }

    public class BillingPayFrequencyRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public BillingPayFrequency parentDetail { get; set; }
        public List<BillingPayFrequencyDetail> ChildDetail { get; set; }

    }

    [XmlRoot("PayFrequencyDetail")]
    public class PayFrequencyNewWrapper
    {
        public BillingPayFrequency PayFrequency { get; set; }
    }

    [XmlRoot("BillingPayFrequencyDetailResponse")]
    public class PayFrequencyDetailNewWrapper
    {
        [XmlElement("PayFrequencyDetail")]
        public List<BillingPayFrequencyDetail> PayFrequencyDetail { get; set; }
    }
}
