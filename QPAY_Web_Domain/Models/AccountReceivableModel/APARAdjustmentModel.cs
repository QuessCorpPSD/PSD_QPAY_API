using System;
using System.Collections.Generic;

namespace QPay.UI.Models.AccountReceivableMod
{
    public class APARAdjustmentExport
    {
        public string companyId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class APARAdjustmentUploadResponse
    {
        public string response { get; set; }
        public List<string> errors { get; set; }
    }


    public class APARAdjustmentEditRequest
    {
        public APARAdjustmentHeader APARAdjustment { get; set; }

  
        public string APARAdjustmentdetail { get; set; }

        public string Mode { get; set; }
        public string Created_By { get; set; }
    }

    public class APARAdjustmentHeader
    {
        public string APARAdjustment_No { get; set; }
        public string APAR_Adjustment_Type_Text { get; set; }
        public string Invoice_Number { get; set; }
        public string Sap_Reference_Number { get; set; }
        public string APAR_Adjustment_Status { get; set; }
    }

    public class APARAdjustmentEmployee
    {
        public int APARAdjustment_Id { get; set; }
        public string Employee_Code { get; set; }
        public string Ref_Id { get; set; }
        public decimal APAR_Adjustment_Amount { get; set; }
        public DateTime? APAR_Adjustment_Dates { get; set; }
    }
}