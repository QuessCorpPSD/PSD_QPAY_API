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
        public int? Company_Id {get; set;}
        public string? Company_code {get; set;} = "";
        public int? SiteId {get; set;}
        public string? SiteName {get; set;} = "";
        public string? SkillType {get; set;} = "";
        public decimal? Amount {get; set;}
        public string? EffectiveDate {get; set;} = "";
        public string? PO_Number { get; set; } = "";
        public string? Action { get; set; } = "";
        public int? UserId { get; set; }

    }
}
