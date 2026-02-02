using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Invoice
{
    public class CompanyInvoiceFormat
    {
            public int? Id { get; set; }
            public int? CompanyId { get; set; }
            public string? Company_Code { get; set; }
            public int? GroupDetailId { get; set; }
            public string? Group_Name { get; set; }
            public int? InvoiceType_Id { get; set; }
            public string? InvoiceType { get; set; }
            public int? InvoiceFormatId { get; set; }
            public string? Format_Name { get; set; }

    }

    public class InvoiceTypeModel
    {
        public int? invoiceType_Id { get; set; }
        public string? invoiceType { get; set; }
    }
    public class InvoiceFormat
    {
        public int? invoiceFormatId { get; set; }
        public string? format_Name { get; set; }
    }

    public class InvoiceFormatAdd
    {
        public int userId { get; set; }
        public string? mode { get; set; }
        public int? Id { get; set; }
        public int? CompanyId { get; set; }
        public int? GroupDetailId { get; set; }
        public int? InvoiceType_Id { get; set; }
        public int? InvoiceFormatId { get; set; }

    }

}
