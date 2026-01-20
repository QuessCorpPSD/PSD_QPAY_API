using QPay.UI.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.GlobalMaster
{

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "EntityDetails")]
    [System.Serializable()]
    public class EntityResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("Entity")]
        public Entity[] EntityDetails { get; set; }
    }

    public class EntityProfitCenterResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EntityProfitCenter")]
        public EntityProfitCenter[] EntityProfitCenterDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    

    [Table("tbl_Entity")]
    public class Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Entity_Id { get; set; }
        public string Entity_Name { get; set; }
        public string Function_Code { get; set; }
        public string WBS { get; set; } 
        public string Profit_Center { get; set; } 
        public string Account_Number { get; set; }
        public int QuessLegalEntityId { get; set; }
        public string QuessLegalEntityName { get; set; }
        public string EstablishmentCode { get; set; } 
    }

    [Table("tbl_Entity_Profit_Center")]
    public class EntityProfitCenter
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Entity_Profit_Center_Id { get; set; }

        public int Entity_Id { get; set; }
        public string Entity_Name { get; set; }
        public int City_Id { get; set; }
        public string City_Name { get; set; }
        public string Location { get; set; }
        public string Error_Message { get; set; }
        public int Serial_No { get; set; }
    }

    public class EntityRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public Entity parentDetail { get; set; }
        public EntityProfitCenter ChildDetail { get; set; }

    }


    public class EntityWithProfitCenter
    {
        public int Entity_Profit_Center_Id { get; set; }
        public int Entity_Id { get; set; }
        public string Entity_Name { get; set; }
        public int City_Id { get; set; }
        public string City_Name { get; set; }
        public string Location { get; set; }
        public string Error_Message { get; set; }
        public int Serial_No { get; set; }
        public string Function_Code { get; set; }
        public string WBS { get; set; } 
        public string Profit_Center { get; set; } 
        public string Account_Number { get; set; }
        public int QuessLegalEntityId { get; set; }
        public string QuessLegalEntityName { get; set; }
        public string EstablishmentCode { get; set; } 

    }

    
}
