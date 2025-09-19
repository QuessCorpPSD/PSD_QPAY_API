using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public class InvoiceInitiationUI
    {
        public int Serial_No { get; set; }

        public int Company_Id { get; set; }
        public int TaxTypeId { get; set; }
        public int CreatedBy { get; set; }
        public string Company_Code { get; set; }=string.Empty;
        public int Employee_Head_Count { get; set; }
        public decimal Service_Charge { get; set; }
        public int Service_Tax { get; set; }
        public int Service_Tax_Id { get; set; }
        public string Pay_Period { get; set; } = string.Empty;
        public int Pay_Period_Id { get; set; }
        public string Service_Charge_Master { get; set; } = string.Empty;
        public string Service_Charge_Type { get; set; } = string.Empty;
        public decimal Net_CTC { get; set; }
        public string Krushi_Kalyan_CESS { get; set; } = string.Empty;
        public string Swatch_Bharat { get; set; } = string.Empty;
        public int Map_Id { get; set; }
        public string Map_Name { get; set; } = string.Empty;
        public string Effective_Date { get; set; } = string.Empty;
        public string Error_Message { get; set; } = string.Empty;
        public int InvoiceType_Id { get; set; }
        public int InvoiceCulture_id { get; set; } 
        public string InvoiceCul_Ref_No { get; set; } = string.Empty;
        public int EBASIC { get; set; }
        public int Input_No { get; set; }
        public int GEN_iID { get; set; }
        public string GEN_vDescription { get; set; } = string.Empty;
        public decimal ServiceChargeAmount { get; set; }
        public int Invoice_Category_Id { get; set; }
        public decimal INCTC { get; set; } 
        public decimal INSCG { get; set; } 
        public decimal NetPay { get; set; } 
        public string PO_Number { get; set; } = string.Empty;
        public decimal BGVBL { get; set; }
        public decimal ASTFEE { get; set; }
        public decimal DISCT1 { get; set; }
        public decimal DISCT2 { get; set; }
        public decimal IDCARD { get; set; }
        public decimal EMAIL { get; set; }
        public decimal REGFEE { get; set; }
        public decimal TRNFEE { get; set; }
        public decimal GGDBT { get; set; }
        public decimal PPEKIT { get; set; }
        public decimal VMSFEE { get; set; }

        public decimal EDUFEE { get; set; }
        public decimal NTPRY { get; set; }
        public decimal RENMAC { get; set; }
        public decimal DRADED { get; set; }
        public decimal OTHDD { get; set; }
        public decimal MBAPP { get; set; }
        public decimal CALCRG { get; set; }
        public decimal CALRT { get; set; }

        public string Narration { get; set; } = string.Empty;
    }
}
