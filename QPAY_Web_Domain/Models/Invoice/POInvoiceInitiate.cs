using QPay.UI.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Invoice
{
    public class POInvoiceInitiate
    {
        public int? Serial_No { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_Code { get; set; }
        public int? InvoiceType_Id { get; set; }
        public decimal? CTC { get; set; }
        public int? Head_Count { get; set; }
        public int? Map_Name_Id { get; set; }
        public string? Map_Name { get; set; }
        public string? PO_Number { get; set; }
        public int? State_Id { get; set; }
        public string? State_Name { get; set; }
        public int? Input_Number { get; set; }
        public int? Group_Detail_Id { get; set; }
        public string? Group_Name { get; set; }
        public int? Pay_Period_Id { get; set; }
        public string? Pay_Period { get; set; }
        public int? Service_Charge_Type_Id { get; set; }
        public string? Service_Charge_Type { get; set; }
        public int? InvoiceCulture_id { get; set; }
        public string? InvoiceCul_Ref_No { get; set; }
        public string? Error_Message { get; set; }
        public int? Category_Id { get; set; }
        public string? Address_Code { get; set; }
        public decimal? MSP_Amount { get; set; }
    }
    [XmlRoot("InitiateDetails")]
    public class POInvoiceInitiateRequest
    {
        [XmlElement("Initiate")]
        public List<POInvoiceInitiate> POInvoiceInitiateMaster { get; set; }
        [XmlIgnore]
        public int CreatedBy { get; set; }
    }

    public class PoIntiateResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();

    }

}
