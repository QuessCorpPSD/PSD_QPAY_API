using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Customer
{
    public class CancelDocument
    {
        public int? Serial_No { get; set; }
        public int? Id { get; set; }
        public int? Company_Id { get; set; }
        public int? Payperiod_Id { get; set; }
        public int? Invoice_Id { get; set; }
        public string? Invoice_Number { get; set; }
        public string? Document_Name { get; set; }
        public string? Uploaded_Date { get; set; }
        public string? Document_Remarks { get; set; }
        public string? Document_FilePath { get; set; }
        public string? Remark { get; set; }
        public string? Error_Message { get; set; }
    }

    [XmlRoot("CancelledInvoiceRepositoryResponse")]
    public class CancelledInvoiceRepositoryResponse
    {
        [XmlElement("CancelledInvoiceRepository")]
        public CancelDocument CancelledInvoiceRepository { get; set; }
    }
}
