using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.CreditNoteMatrix
{
    public class CreditNoteMatrixRequest
    {
        public int? CreatedBy { get; set; }

        public List<CreditNoteMatrixData>? requestdata { get; set; }
    }
    public class CreditNoteMatrixData
    {
        public int? SNo { get; set; }
        public string? CRNCategory { get; set; }
        public bool? PPT { get; set; }
        public int? PPTUserId { get; set; }
        public string? PPTMailId { get; set; }
        public bool? ZM { get; set; }
        public int? ZMUserId { get; set; }
        public string? ZMMailId { get; set; }
        public bool? BillingHead { get; set; }
        public int? BillingHeadUserId { get; set; }
        public string? BillingHeadMailId { get; set; }
        public bool? BF { get; set; }
        public int? BFUserId { get; set; }
        public string? BFMailId { get; set; }
        public bool? COO { get; set; }
        public int? COOUserId { get; set; }
        public string? COOMailId { get; set; }
        public bool? CEO { get; set; }
        public int? CEOUserId { get; set; }
        public string? CEOMailId { get; set; }
        public bool? WCFO { get; set; }
        public int? WCFOUserId { get; set; }
        public string? WCFOMailId { get; set; }
        public bool? President { get; set; }
        public int? PresidentUserId { get; set; }
        public string? PresidentMailId { get; set; }
    }
    public class ErrorMessage
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class CommonDropDown1
    {
        public string value { get; set; }
        public string name { get; set; }
    }
}