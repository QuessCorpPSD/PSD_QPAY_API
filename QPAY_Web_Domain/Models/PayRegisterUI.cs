using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
   public class PayRegisterUI
    {
        public int CompanyCode { get; set; }

        public int Pay_Period_id { get; set; }

        public int LotNumber { get; set; }

        public string? FilePath { get; set; } = "";

        public string FileName { get; set; }
        public string FileType { get; set; }

        public string LoginUser { get; set; }

        public string Input_type { get; set; }

        public string checkInSheet { get; set; }
    }
}
