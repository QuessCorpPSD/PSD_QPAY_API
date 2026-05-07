using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Invoice
{
    public  class InvoiceDetail
    {
        public string Invoice_Number { get; set; }
        public int Company_Id { get; set; }
        public int Pay_Period_Id { get; set; }
        public int IsGenerated_IRN { get; set; }
        public string Data_from { get; set; }
        public int? Regenerate { get; set; }
    }
}
