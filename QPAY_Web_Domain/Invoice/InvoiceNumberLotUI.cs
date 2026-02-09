using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Invoice
{
    public class InvoiceNumberLotUI
    {
        public int SLNO { get; set; }
        public int Company_Id { get; set; }
        public int Pay_Period_id { get; set; }
        public string Invoice_Number { get; set; } = "";
        public int LotNo { get; set; }
        public string Data_from { get; set; } = "";
        public int Invoice_Id { get; set; }
        public int? IsGenerated_IRN { get; set; }
        public int? Regenerate { get; set; }

    }
}
