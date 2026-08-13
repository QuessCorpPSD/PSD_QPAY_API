using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.UI.Models.Invoice
{
    public class POCulture
    {
        [Serializable]
        [XmlType(AnonymousType = true)]
        [XmlRoot("PoCultureResponse", Namespace = "", IsNullable = false)]
        public class PoCultureResponse
        {
            [XmlElement("PoCulture")]
            public PoCulture[] PoCultureResponseDetails { get; set; }

        }

        [Table("tbl_POCulture")]
        public class PoCulture
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int? POCulture_id { get; set; }

            public int? Company_Id { get; set; }
            public string? Company_Code { get; set; }
            public string? Company_Name { get; set; }

            public int? Cost_Center_Mapping_Id { get; set; }

            public string? Map_Name { get; set; }
            // New Fields
            public bool? IsMapnameWiseInvoice { get; set; }

            public bool? HasCustomInvoicingState { get; set; }

            public int? InvoicingStateId { get; set; }
            public string? Error_Message { get; set; }
        }
        public class PurchaseOrder
        {
            public int Purchase_Order_Id { get; set; }

            public string Purchase_Request_No { get; set; }

            public decimal PO_Amount { get; set; }

            public decimal Utilized_Amount { get; set; }

            public decimal Balance_Amount { get; set; }

            public DateTime? PO_Date { get; set; }

            public DateTime? PO_Valid_From { get; set; }

            public DateTime? PO_Valid_To { get; set; }

            public string Remarks { get; set; }
        }

        public class POCultureGrid
        {
            public int? Serial_No { get; set; }
            public int? POCulture_id { get; set; }

            public int? Company_Id { get; set; }
            public string Company_Code { get; set; } = "";
            public string Company_Name { get; set; } = "";

            public int? Cost_Center_Mapping_Id { get; set; }
            public string Map_Name { get; set; } = "";

            public bool? IsMapnameWiseInvoice { get; set; }

            public bool? HasCustomInvoicingState { get; set; }

            public int? InvoicingStateId { get; set; }

            public string? InvoicingStateName { get; set; }

            public string Error_Message { get; set; } = "";
        }

        [XmlRoot("PoCultureResponse")]
        public class POCultureRequest
        {
            [XmlIgnore]
            public int createdBy { get; set; }

            [XmlIgnore]
            public string? mode { get; set; }

            [XmlElement("PoCulture")]
            public PoCulture? parentDetail { get; set; }

       
        }
    }
}