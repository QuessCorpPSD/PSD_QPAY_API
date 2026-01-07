using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Process
{
    public class PayProcessRequest
    {
        public int company_Id { get; set; } 

        public string End_At { get; set; } = "";
    }

    public class PayProcessPayperiodRequest
    {
        public int company_Id { get; set; }

        public string payperiodId { get; set; } = "";
    }
    public class Message
    {
        public string Error_Message { get; set; }
    }
    public class ProcessUI
    {
        public string Is_Processed { get; set; } = "";

        public string Actual_declared { get; set; } = "";
    }

    public class PayFrequency
    {
        public int? Pay_Frequency_Id { get; set; }
        public int? Pay_Frequency_Detail_Id { get; set; }
        public string Pay_Period { get; set; }
        public string Pay_Sequence_Number { get; set; }
        public int? Pay_Period_Days { get; set; }
        public int? Working_Days { get; set; }
        public string Start_At { get; set; }
        public string Actual_declared { get; set; }
        public int? Is_Processed { get; set; }
        public int Is_Reprocessed { get; set; }
        public int Is_FProcessed { get; set; }
        public int Is_FReprocessed { get; set; }
        public string End_At { get; set; }
        public decimal? Month_Days { get; set; }
    }
}
