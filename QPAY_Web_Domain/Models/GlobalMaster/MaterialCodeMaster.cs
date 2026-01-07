using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.GlobalMaster
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "MaterialCodeMaster")]
    [System.Serializable()]
    public class MaterialCodeMasterResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("MaterialCodeMaster")]
        public MaterialCodeMaster[] LstMaterialCodeMaster { get; set; }
    }

    //public class InsuranceMasterDetailResponse
    //{
    //    [System.Xml.Serialization.XmlElementAttribute("InsuranceMasterData")]

    //}

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("TBL_SAP_MATERIAL_CODE")]
    public class MaterialCodeMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SNo { get; set; }

        public int Id { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public string IsSalary { get; set; }

    }

    public class MaterialCodeMasterRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public MaterialCodeMaster detail { get; set; }
    }

}
