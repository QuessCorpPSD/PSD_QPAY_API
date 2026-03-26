using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.GlobalMaster
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "FormulaDetails")]

    [System.Serializable()]
    public class FormulaResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("Formulas")]
        public Formulas[] FormulaDetails { get; set; }
    }

    public class MCFormulaResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("Formulas")]
        public MCFormulas[] FormulaDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Formula")]
    public class Formulas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Formula_Id { get; set; }

        public Int64 Paycode_Id { get; set; }
        public string Paycode_Code { get; set; }
        public string Formula_Name { get; set; }
        public string Formula { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public int PayCategory_Id { get; set; }
        public string Paycateory { get; set; }
        public string Error_Message { get; set; }
        public int SNo { get; set; }
    }

    public class FormulasRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public Formulas detail { get; set; }

    }

    public class MCFormulasRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public MCFormulas detail { get; set; }

    }

    public class MCFormulas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Formula_Id { get; set; }
        public int PayrollType { get; set; }
        public string PayrollTypeName { get; set; }
        public Int64 Paycode_Id { get; set; }
        public string Paycode_Code { get; set; }
        public string Formula_Name { get; set; }
        public string Formula { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public int PayCategory_Id { get; set; }
        public string Paycateory { get; set; }
        public string Error_Message { get; set; }
        public int SNo { get; set; }
    }

}