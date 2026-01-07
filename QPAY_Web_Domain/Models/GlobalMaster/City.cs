using System.Xml.Serialization;

namespace QPay.UI.Models.GlobalMaster
{
    public class City
    {
        public int? Serial_No { get; set; }
        public int? CityID { get; set; }
        public string? CityName { get; set; }
        public string? CityCode { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public string? Country { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public string? SAP_Code { get; set; }
        public bool IsActive { get; set; }
        public string? Pin_Code { get; set; }
        public string? Taluk { get; set; }
        public string? District { get; set; }
        public string? circle { get; set; }
        public string? Ikya_Location { get; set; }
        public string? ESI_SUB_CODE { get; set; }
        public string? ESI_SUB_CODE_NAME { get; set; }
        public string? Tier { get; set; }
        public string? ESIC_Implementation { get; set; }
        public string? Zone { get; set; }

    }

    public class CityAdd 
    {
        public int City_Id { get; set; }
        public string City_Code { get; set; }
        public string City_Name { get; set; }
        public int State_Id { get; set; }
        public string State_Name { get; set; }
        public string Country { get; set; }
        public int Region_Id { get; set; }
        public string SAP_Code { get; set; }
        public string circle { get; set; }
        public string Region_Name { get; set; }
        public string Error_Message { get; set; }
        public int Serial_No { get; set; }
        public string Pin_Code { get; set; }
        public string Taluk { get; set; }
        public string District { get; set; }
        public int TotalNoofRows { get; set; }
        public string Country_Name { get; set; }
        public string Country_Id { get; set; }
        public string Ikya_Location { get; set; }
        public int ESI_SubCode { get; set; }
        public string ESI_SubCode_Name { get; set; }
        public string Tier { get; set; }
        public string ESIC_Implementation { get; set; }
        public string Zone { get; set; }
    }

    [XmlRoot("CityDetails")]
    public class CityAddRequest
    {
        [XmlIgnore]
        public string mode { get; set; }
        [XmlIgnore]
        public int createdBy { get; set; }
        [XmlElement("City")]
        public CityAdd details { get; set; }
    }
    public class Circle
    {
        public int? PTCircle_Id { get; set; }
        public string? PTCircle_Name { get; set; }
    }
}
