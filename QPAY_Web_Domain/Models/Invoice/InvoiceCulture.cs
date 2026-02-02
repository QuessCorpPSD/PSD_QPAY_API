using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Invoice
{
    public class InvoiceCulture
    {
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "InvoiceStructureResponse")]
        [System.Serializable()]

        public class InvoiceStructureResponse
        {
            [System.Xml.Serialization.XmlElementAttribute("InvoiceStructure")]
            public InvoiceStructure[] InvoiceStructureResponseDetails { get; set; }

            [System.Xml.Serialization.XmlElementAttribute("InvoiceStructureDetails")]
            public TypeOfInvoiceForInvoiceStructure[] TypeOfInvoiceDetails { get; set; }


        }
        [Table("tbl_InvoiceCulture")]
        public class InvoiceStructure
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int? InvoiceCulture_id { get; set; }
            
            public int? Company_Id { get; set; }
            public string? Company_Code { get; set; }
            public string? Company_Name { get; set; }
            public string? InvoiceCul_Ref_No { get; set; }
            public string? InvoiceType { get; set;}
            public int? InvoiceType_Id { get; set; }
            public int? Cost_Center_Mapping_Id { get; set; }
            public int? Service_Charge_Master_Id { get; set; }
            public int? Service_Charge_Type_Id { get; set; }
            public int? Service_Charge_Slab_Item_Id { get; set; }
            public int? Service_Charge_Slab_Inner_Item_Id { get; set; }
            public int? Map_Name_Id { get; set; }
            public string? Map_Name { get; set; }
            public string? Error_Message { get; set; }
            public int? Invoice_Category_Id { get; set; }
            public int? State_Id { get; set; }
            public int? Spilt_Type_Id { get; set; }
        }

        public class TypeOfInvoiceForInvoiceStructure
        {
            public int? InvoiceCulture_id { get; set; }
            public int? Company_Id { get; set; }
            public int? Paycode_Id { get; set; }
            public string? Paycode_Code { get; set; }
            public bool? HasAccess { get; set; }
        }

        public class ServiceChargeMastereDD
        {
            public int? Service_Charge_Master_Id { get; set; }
            public string Service_Charge_Master_Name { get; set; } = "";
        }

        public class InvoiceTypeforCultureDD
        {
            public int? InvoiceType_Id { get; set; }
            public string InvoiceType { get; set; } = "";
        }
        public class GenDD
        {
            public int? GEN_iID { get; set; }
            public string GEN_vDescription { get; set; } = "";
        }

        public class InvoiceCultureGrid
        {
            public int? Serial_No { get; set; }
            public int? InvoiceCulture_id { get; set; }
            public int? Company_Id { get; set; }
            public string Company_Code { get; set; } = "";
            public string Company_Name { get; set; } = "";
            public string InvoiceCul_Ref_No { get; set; } = "";
            public string InvoiceType { get; set; } = "";
            public string Type_Of_Invoice_Name { get; set; } = "";
            public int? Service_Charge_Type_Id { get; set; }
            public int? Service_Charge_Master_Id { get; set; }
            public int? Service_Charge_Slab_Item_Id { get; set; }
            public int? Service_Charge_Slab_Inner_Item_Id { get; set; }
            public int? Map_Name_Id { get; set; }
            public string Map_Name { get; set; } = "";
            public int? InvoiceType_Id { get; set; }
            public int? Cost_Center_Mapping_Id { get; set; }
            public int? Type_Of_Invoice { get; set; }
            public string InvoiceType_Id_Name { get; set; } = "";
            public string City_Name { get; set; } = "";
            public string Error_Message { get; set; } = "";
            public int? Invoice_Category_Id { get; set; }
            public string Invoice_Category_Name { get; set; } = "";
            public int? State_Id { get; set; }
            public string State_Name { get; set; } = "";
        }


        [XmlRoot("InvoiceStructureResponse")]
        public class InvoiceStructureRequest
        {
            [XmlIgnore]
            public int createdBy { get; set; }
            [XmlIgnore]
            public string mode { get; set; }
            [XmlElement("InvoiceStructure")]
            public InvoiceStructure parentDetail { get; set; }
            [XmlElement("InvoiceStructureDetails")]
            public List<TypeOfInvoiceForInvoiceStructure> childDetail { get; set; }

        }

    }
}
