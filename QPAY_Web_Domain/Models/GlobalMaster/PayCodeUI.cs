using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.GlobalMaster
{
    public class Paycodes
    {
        public List<SelectedItems> MappedPaycode { get; set; }
        public List<SelectedItems> availablePaycode { get; set; }
    }
    public class PayCodeUI
    {
        public int? Company_Id { get; set; }
        public int? PayCode_Id {  get; set; }
        public string PayCode_code { get; set; } = "";
        public string PayCodeName { get; set; } = "";
    }
    public class PTStateExcludeDetailsResponse
    {
       // [System.Xml.Serialization.XmlElementAttribute("PTStateExclude")]
        public List<PTStateExclude> PTStateExcludeDetails { get; set; }
    }

    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    //[Table("tbl_PTState_Exclude")]
    public class PTStateExclude
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PTState_Ex_ID { get; set; }
        public int SNo { get; set; }
        public string State_Name { get; set; }
        public int State_Id { get; set; }
        public string Exclusion_Type { get; set; }
        public int Exclusion_Type_Id { get; set; }
        public int Paycode_Id { get; set; }
        public string Paycode_Code { get; set; }
        public string Error_Message { get; set; }

    }

    public class CompanyPayCodeDetail
    {
        public int Paycode_Id { get; set; }

        public string Paycode_Code { get; set; } = "";
        public string Description { get; set; } = "";

    }
}
