using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Common
{
    public  class FinancialYearUI
    {
        public int Financial_Year_Id { get; set; }
        public string Financial_Year_Name { get; set; } = string.Empty;

        public DateTime? From_Date { get; set; }
        public DateTime? To_Date { get; set; }

        public DateTime? Invoice_StartDate { get; set; }
        public string Assessment_Year { get; set; } = string.Empty;
        public Int64 Invoice_Sequence { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Process_End_Date { get; set; }

        public string TdsNote1 { get; set; }=string.Empty;
        public string TdsNote2 { get; set; } = string.Empty;
        

    }
}
