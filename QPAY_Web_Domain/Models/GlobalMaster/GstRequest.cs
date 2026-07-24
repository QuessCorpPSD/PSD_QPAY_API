using QPay.UI.CreditNoteMatrix;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.GlobalMaster
{
    public class GstRequest
    {
        public Int32 GstMasterId { get; set; }
        public DateTime EffectiveDate { get; set; }   

        public Int32 EntityId { get; set; }     

        public Int32 GstTypeId { get; set; }              

        public Int32 StateId { get; set; }

        public String GstNumber { get; set; }

        public String PanNumber { get; set; }

        public String TanNumber { get; set; }

        public String CompanyName { get; set; }

        public String CompanyAddress { get; set; }

        public Boolean CGST_Applicable { get; set; }
        public Decimal CGST_Percentage { get; set; }

        public Boolean SGST_Applicable { get; set; }  

        public Decimal SGST_Percentage { get; set; }
    
        public Boolean UTGST_Applicable { get; set; }

        public Decimal UTGST_Percentage { get; set; }
        public Boolean IGST_Applicable { get; set; }

        public Decimal IGST_Percentage { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public Decimal Cess_Percentage { get; set; }
        public DateTime? CessEffectiveFromDate { get; set; }
        
        public DateTime? CessEffectiveToDate { get; set; } 

        public Int32 LocationId { get; set; }


        public string Pincode { get; set; }
        public Int32 UserId { get; set; }


    }
}
