using System;
using System.Collections.Generic;

namespace QPay.UI.Models.AccountReceivableMod
{
    public class ForecastRequest
    {
        public int Created_By { get; set; }
        public string Mode { get; set; }
        public List<ForecastDetails> forecast { get; set; }
    }

    public class ForecastDetails
    {
        public int Fore_Cast_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public int Pay_Period_Id { get; set; }
        public int Region_Id { get; set; }
        public int Sbu_Id { get; set; }
        public decimal Projection_Amount { get; set; }
        public decimal Collected_Amount { get; set; }
        public decimal Balance_Amount { get; set; }
        public decimal Final_Projection { get; set; }
        public int Invoice_Id { get; set; }
    }

    public class ForecastResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

    public class ForecastExport
    {
        public int CompanyId { get; set; }
        public string PayPeriod { get; set; }
      
    }
}