using System;
using System.Collections.Generic;

namespace QPay.UI.Models.BankNonInvoice
{
    public class BankAdviceSplitCultureUploadResponse
    {
        public string response { get; set; } = string.Empty;

        public List<string> errors { get; set; }
            = new List<string>();
    }

    public class BankCultureResponse
    {
        public int Status { get; set; }

        public string Error_Message { get; set; }
            = string.Empty;
    }

    public class CreateBankCultureRequest
    {
        public int CreatedBy { get; set; }

        public string Mode { get; set; }
            = string.Empty;

        public List<CreateBankCulture> Data { get; set; }
            = new List<CreateBankCulture>();
    }

    public class CreateBankCulture
    {
        public Int32 Bank_Culture_Detail_id { get; set; }

        public Int32 Bank_Culture_id { get; set; }

        public Int32 Vendor_Id { get; set; }

        public string Vendor_Name { get; set; }
            = string.Empty;

        public Int32 CreatedBy { get; set; }

        public Int32 Company_Id { get; set; }

        public string Company_Code { get; set; }
            = string.Empty;

        public Int32 Group_Detail_Id { get; set; }

        public string Group_Name { get; set; }
            = string.Empty;

        public bool available { get; set; }

        public string Culture_Type_Text { get; set; }
            = string.Empty;

        public int Culture_Type { get; set; }

        public string Error_Message { get; set; }
            = string.Empty;

        public Int64 SNo { get; set; }
    }
}