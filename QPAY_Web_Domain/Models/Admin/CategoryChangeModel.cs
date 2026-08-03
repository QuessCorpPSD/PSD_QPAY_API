namespace QPay.UI.Models.Admin
{
    public class CategoryChangeModel
    {
        public int CompanyID { get; set; }
        public int PayPeriod { get; set; }
        public int LotNumber { get; set; }
        public int Revised { get; set; }
        public string Flag { get; set; } = "";
        public string? XML_File { get; set; }
        public int CreatedBy { get; set; }
    }
}