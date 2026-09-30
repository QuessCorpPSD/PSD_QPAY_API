using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Customer
{
    public class SkillMapping
    {
    }

    public class SkillMappingRequest
    {
        public int? Manual_Invoice_Id { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_code { get; set; } = "";
        public int? SiteId { get; set; }
        public string? SiteName { get; set; } = "";
        public string? SkillType { get; set; } = "";
        public decimal? Amount { get; set; }
        public decimal? OTRate { get; set; }
        public string? EffectiveDate { get; set; } = "";
        public string? PO_Number { get; set; } = "";
        public string? Action { get; set; } = "";
        public int? UserId { get; set; }
        public int? DesignationId { get; set; }
        public string? DesignationName { get; set; }
        public int? MaterialCodeId { get; set; }
        public string? MaterialCodeName { get; set; }
        public int? BillingTypeId { get; set; }
        public string? BillingTypeName { get; set; }

    }
}


