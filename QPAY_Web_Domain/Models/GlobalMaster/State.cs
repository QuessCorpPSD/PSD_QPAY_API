using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.GlobalMaster
{
    public class State
    {
        public int? SERIAL_NO {get; set;}
        public int? StateId {get; set;}
        public string? StateName {get; set;}
        public string? StateCode {get; set;}
        public string? Country {get; set;}
        public string? RegionName {get; set;}
        public string? SapCode {get; set;}
        public bool? IsActive {get; set;}
        public int? RegionId {get; set;}
        public int? Min_Labor_Count {get; set;}
        public decimal? Security_Deposit { get; set; }
    }

    public class StateAdd
    {
        public int State_Id { get; set; }
        public string State_Code { get; set; }
        public string State_Name { get; set; }
        public string Country { get; set; }
        public int Region_Id { get; set; }
        public string SAP_Code { get; set; }
        public int Min_Labor_Count { get; set; }
        public decimal Security_Deposit { get; set; }
    }

    [XmlRoot("StateDetails")]
    public class StateAddRequest
    {
        [XmlIgnore]
        public string mode { get; set; }
        [XmlIgnore]
        public int createdBy { get; set; }
        [XmlElement("State")]
        public StateAdd details { get; set; }
    }

    public class Region
    {
        public int Region_Id { get; set; }
        public string Region_Name { get; set; }
    }
}
