namespace QPay.API.Models
{
    public class GrossMarginRequestModel
    {
        public string Pay_Period { get; set; }= string.Empty;
        public string ReportType { get; set; } = string.Empty;
        public int? Submit { get; set; }
    }
}
