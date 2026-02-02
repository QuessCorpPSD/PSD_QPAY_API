using System.Xml.Serialization;

namespace QPay.UI.Customer
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "PayFrequencyDetail")]
    [System.Serializable()]
    public class PayFrequencyResponseNew
    {
        [System.Xml.Serialization.XmlElementAttribute("PayFrequency")]
        public PayFrequencyNew[] PayFrequencys { get; set; }
    }


    [XmlRoot("PayFrequency")]
    public class PayFrequencyNew
    {
        public int Pay_Frequency_Id { get; set; }
        public int Company_Id { get; set; }
        public int Group_Id { get; set; }
        //public int City_Id { get; set; }
        //public string City_Name { get; set; }
        public string Starting_Date { get; set; }
        public string Ending_Date { get; set; }
    }

    public class PayFrequencyDetailNew
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
        //  public double Pay_Frequency_Detail_New1_Id { get; set; }
    }

    public class PayFrequencyWithDetailNew
    {
        public int Pay_Frequency_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public int Group_Id { get; set; }
        public string Group { get; set; }
        public string Starting_Date { get; set; }
        public string Ending_Date { get; set; }
        public int Pay_Frequency_Detail_Id { get; set; }
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
        public string Error_Message { get; set; }
        public int SNo { get; set; }
        public double Pay_Frequency_Detail_New1_Id { get; set; }
        public int CountWithGroupIDZero { get; set; }
        public int CountWithGroupIDGreater { get; set; }
        public int Count { get; set; }
    }

    public class PayFrequencyRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public PayFrequencyNew parentDetail { get; set; }

        public List<PayFrequencyDetailNew> ChildDetail { get; set; }

    }


    [XmlRoot("PayFrequencyDetail")]
    public class PayFrequencyNewWrapper
    {
        public PayFrequencyNew PayFrequency { get; set; }
    }

    [XmlRoot("PayFrequencyDetailResponseNew")]
    public class PayFrequencyDetailNewWrapper
    {
        [XmlElement("PayFrequencyDetail")]
        public List<PayFrequencyDetailNew> PayFrequencyDetail { get; set; }
    }
}