namespace QPay.UI.Customer
{
    public class ServiceCharge
    {
        public int? Company_Service_Charge_Master_Id { get; set; }
        public int? Company_Service_Charge_Type_Id { get; set; }
        public int? Service_Charge_Slab_Item_Id { get; set; }
        public int? Service_Charge_Slab_Inner_Item_Id { get; set; }
        public int? Slab_Id { get; set; }
        public int? Cost_Center_Mapping_Id { get; set; }
        public string? Map_Name { get; set; }
        public Boolean Invoicing_Type { get; set; }
        public string? Service_Charge_Name { get; set; }
        public string? PayCode_Code { get; set; }
        public decimal MaxAmount { get; set; }
        public int? Type { get; set; }
        public string? Value { get; set; }
        public string? Effective_Date { get; set; }
        public int? IsBillToRate { get; set; }
        public int? IsCTC { get; set; }
        public int? IsHeadCount { get; set; }
        public int? IsAttendanceProrated { get; set; }
        public int? IsCriteriaApplicable { get; set; }
        public string? Criteria { get; set; }
        public int? IsReplacementClauseApplicable { get; set; }
        public decimal Replacement { get; set; }
        public int? IsSourcingWaitingPeriod_Id { get; set; }
        public decimal SourcingValue { get; set; }
        public int? TATDays { get; set; }
        public int? IsMapNameRequired { get; set; }
        public int? Category_Id { get; set; }
        public int? Invoice_Map_Name_Id { get; set; }
        public decimal Compliance_Fee { get; set; }
        public decimal RandStad_Fee { get; set; }
        public int? UnitType_Id { get; set; }
        public int? Discount_Type_Id { get; set; }
        public decimal Discount_Amount { get; set; }
        public int? Type_Id { get; set; }
        public int? Pay_Code_Id { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
        public int? Slab_Calculation_Type_Id { get; set; }
        public decimal Cap_Value { get; set; }
        public decimal Upfront_Charge { get; set; }
        public string? Upfront_PayCode { get; set; }
        public int? Upfront_Type_Id { get; set; }
        public string? Insurance_Amount { get; set; }
        public int? MarginalPayCodeId { get; set; }
        public decimal QDemyFee { get; set; }
        public decimal InEdgeFee { get; set; }
        public int? IsNewjoineeProrate { get; set; }
        public int? IsFAndFProrate { get; set; }
        public int? IsFAndFArrearProrate { get; set; }
        public int? IsNewJoineeArrearProrate { get; set; }
        public int? QDemyFee_Type_Id { get; set; }
        public int? InEdgeFee_Type_Id { get; set; }

    }
    public class ServiceChargeResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

    public class ResponseModel
    {
        public string? Result { get; set; }
        public string? Error_Message { get; set; }
        public string? Validation { get; set; }
    }

    public class ServiceChargeRequest
    {
        public int? CompanyId { get; set; }
        public string Created_By { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public List<ServiceCharge> ServiceChargemaster { get; set; }
    }
}