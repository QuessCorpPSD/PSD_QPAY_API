namespace QPay.DTo.Models.PayrollInput
{
    public class AttributeRequestModel
    {
        public int FlagId { get; set; }
        public int CompanyId { get; set; }
        public int payPeriodId { get; set; }
        public int LotNo { get; set; }
        public string userId { get; set; } = "";
        public string? uploadedFile { get; set; } = "";
    }
    public class AttributeUploadRequest
    {
        public int CompanyId { get; set; }
        public int payPeriodId { get; set; }
        public string uploadedFile { get; set; } = "";
        public string userId { get; set; }
    }

}
