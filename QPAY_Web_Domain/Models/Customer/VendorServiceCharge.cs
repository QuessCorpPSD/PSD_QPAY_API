namespace QPay.UI.Customer
{
    public class VendorServiceCharge
    {
        public int? Cost_Center_Mapping_Id { get; set; }
        public int? Service_Charge_Type_Id { get; set; }
        public int? Billing_Type_Id { get; set; }
    
        public decimal MaxAmount { get; set; }
        public decimal? FromValue { get; set; }
        public decimal? ToValue { get; set; }
        public string? Effective_Date { get; set; }
         }
    public class VendorServiceChargeResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

    public class VendorResponseModel
    {
        public string? Result { get; set; }
        public string? Error_Message { get; set; }
        public string? Validation { get; set; }
    }

    public class VendorServiceChargeRequest
    {
        public int? CompanyId { get; set; }
        public string Created_By { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public List<VendorServiceCharge> VendorServiceChargemaster { get; set; }
    }
}