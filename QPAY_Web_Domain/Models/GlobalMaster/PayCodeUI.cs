using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.GlobalMaster
{
    public class Paycodes
    {
        public List<Invoice.SelectedItems> MappedPaycode { get; set; }
        public List<Invoice.SelectedItems> availablePaycode { get; set; }
    }
    public class PayCodeUI
    {
        public int? Company_Id { get; set; }
        public int? PayCode_Id {  get; set; }
        public string PayCode_code { get; set; } = "";
        public string PayCodeName { get; set; } = "";
    }
}
