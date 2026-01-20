
namespace QPay.UI.Models.Reports
{
    public class POActiveInactive
    {
      public int? CompanyId { get; set; }
      public string CompanyCode { get; set; } = "";
      public int? SiteId { get; set; }
      public int? Isactive { get; set; }
      public string PoType { get; set; } = "";
      public string PoYear { get; set; } = "";
      public string Vertical { get; set; } = "";
      public string UserId { get; set; } = "";
    }
}
