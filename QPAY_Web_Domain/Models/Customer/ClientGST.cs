using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using QPay.UI.GlobalMaster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QPay.UI.Models.Customer
{
    public class ClientGST
    {
    }
    public class ClientGSTGrid
    {
        public string? TotalCount { get; set; }
        public int? ClientGstId { get; set; }
        public int? CompanyId { get; set; }
        public string? Company_Code { get; set; }
        public int? StateId { get; set; }
        public string? State_Name { get; set; }
        public string? GstNumber { get; set; }
        public string? PanNumber { get; set; }
        public string? TanNumber { get; set; }
        public int? InvoicingStateId { get; set; }
        public string? InvoicingState_Name { get; set; }
        public int? ClientInvoicingStateId { get; set; }
        public string? ClientInvoicingState_Name { get; set; }
        public string? CreatedBy { get; set; }
        public string? UserName { get; set; }
        public string? CreatedOn { get; set; }
        public int? Group_Detail_Id { get; set; }
        public string? Group_Name { get; set; }
        public string? Remarks { get; set; }
        public int? GstTypeId { get; set; }
        public string? GstTypeName { get; set; }
        public string? SapCustomerCode { get; set; }
        public int? InvoiceCategoryId { get; set; }
        public string? InvoiceCategory { get; set; }
        public string? StateCode { get; set; }
    }

    public class ClientGSTRequest
    {
        public string? Action { get; set; }
        public string? UserId { get; set; }
        public string? XmlData { get; set; }
        public string? ClientGstId { get; set; }
        public string? CompanyId { get; set; }
        public string? StateId { get; set; }
        public string? GstNumber { get; set; }
        public string? PanNumber { get; set; }
        public string? TanNumber { get; set; }
        public string? InvoicingStateId { get; set; }
        public string? ClientInvoicingStateId { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedOn { get; set; }
        public string? Group_Detail_Id { get; set; }
        public string? PageNo { get; set; }
        public string? PageSize { get; set; }
        public string? SortField { get; set; }
        public string? SortDirection { get; set; }
        public string? TotalCount { get; set; }
        public string? Company_Code { get; set; }
        public string? State_Name { get; set; }
        public string? InvoicingState_Name { get; set; }
        public string? ClientInvoicingState_Name { get; set; }
        public string? UserName { get; set; }
        public string? Group_Name { get; set; }
        public string? Remarks { get; set; }
        public string? GstTypeId { get; set; }
        public string? GstTypeName { get; set; }
        public string? SapCustomerCode { get; set; }
        public string? InvoiceCategoryId { get; set; }
        public string? InvoiceCategory { get; set; }
        public string? StateCode { get; set; }
    }

    public class ClientGSTResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

}
