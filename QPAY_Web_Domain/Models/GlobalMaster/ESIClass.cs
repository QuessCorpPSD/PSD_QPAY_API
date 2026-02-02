using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.GlobalMaster
{
    public class ESIClass
    {
        public class EsiBlockUI
        {
            public string? Block_Type_Id { get; set; }
            public string? Block_Type_Name { get; set; }
        }
        public class EsiMonthsUI
        {
            public string? Month_Id { get; set; }
            public string? Month_Name { get; set; }
        }

        [XmlRoot(ElementName = "ESIBlockDetails")]
        public class ESIBlockDetails
        {

            [XmlElement(ElementName = "ESI_Block_Details_Id")]
            public string? ESIBlockDetailsId { get; set; }

            //[XmlElement(ElementName = "ESI_Block_Id")]
            //public string? ESIBlockId { get; set; }

            [XmlElement(ElementName = "Block_Type_Id")]
            public string? BlockTypeId { get; set; }

            [XmlElement(ElementName = "Frequency_Id")]
            public string? FrequencyId { get; set; }
        }

        [XmlRoot(ElementName = "ESIBlockDetailsResponse")]
        public class ESIBlockDetailsResponse
        {

            [XmlElement(ElementName = "ESIBlockDetails")]
            public List<ESIBlockDetails> ESIBlockDetails { get; set; } = new List<ESIBlockDetails>();
        }

        [XmlRoot(ElementName = "main")]
        public class EsiblockMain
        {

            [XmlElement(ElementName = "financialyear_id")]
            public string? Effectivedate { get; set; }

            [XmlElement(ElementName = "ESI_Block_Id")]
            public string? ESIBlockId { get; set; }

            [XmlElement(ElementName = "ESIBlockDetailsResponse")]
            public ESIBlockDetailsResponse ESIBlockDetailsResponse { get; set; }
        }

        public class EsiblockRequest
        {
            public EsiblockMain main { get; set; } = new EsiblockMain();
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
        }

        public class EsiResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class PaycodeUI
        {
            public string? Paycode_Id { get; set; }
            public string? Paycode_Code { get; set; }
            public string? Description { get; set; }
            public string? Print_As { get; set; }
            public string? Page_Type_Value { get; set; }
        }

        public class EsiStateUI
        {
            public string? StateId { get; set; }
            public string? StateName { get; set; }
            public string? StateCode { get; set; }

        }

        public class EsiCityUI
        {
            public string? CityID { get; set; }
            public string? CityName { get; set; }
            public string? CityCode { get; set; }

        }

        public class EsiCriteriaTypeUI
        {
            public string? Criteria_Type_Id { get; set; }
            public string? Criteria_Type_Name { get; set; }
        }

        public class EsiLocationSlabSearchRequest
        {
            public string? FromDate { get; set; }
            public string? ToDate { get; set; }

        }

        public class EsiLocationSlabRequest
        {
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
            public EsiLocationSlab ESILocationSlab { get; set; } = new EsiLocationSlab();
            public List<EsiLocationSlabDetails> ESILocationSlabDetails { get; set; } = new List<EsiLocationSlabDetails>();
        }

        public class EsiLocationSlab
        {
            public int ESI_Location_Slab_Id { get; set; }
            public string? From_Date { get; set; }
            public string? To_Date { get; set; }
            public int Paycode_Id { get; set; }
            public int City_ID { get; set; }
            public int State_ID { get; set; }

        }

        public class EsiLocationSlabDetails
        {
            public string? From_Value { get; set; }
            public string? To_Value { get; set; }
            public string? Criteria { get; set; }
            public int Criteria_Type_Id { get; set; }
            public int ESI_Location_Slab_Detail_id { get; set; }
        }

        [XmlRoot("ESILocationSlabsData")]
        public class ESILocationSlabsData
        {
            [XmlElement("ESILocationSlab")]
            public EsiLocationSlab ESILocationSlab { get; set; } = new EsiLocationSlab();
        }

        // Wrapper for XML2
        [XmlRoot("ESILocationSlabDetailResponse")]
        public class ESILocationSlabDetailResponse
        {
            [XmlElement("ESILocationSlabDetail")]
            public List<EsiLocationSlabDetails> ESILocationSlabDetails { get; set; } = new List<EsiLocationSlabDetails>();
        }

        public class EsiSlabSearchRequest
        {
            public string? FromDate { get; set; }
            public string? ToDate { get; set; }

        }

        public class EsiSlabRequest
        {
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
            public EsiSlab ESISlab { get; set; } = new EsiSlab();
            public List<ESISlabDetail> ESISlabDetail { get; set; } = new List<ESISlabDetail>();
        }

        public class EsiSlab
        {
            public int ESI_Slab_Id { get; set; }
            public int Paycode_Id { get; set; }
            public string? Effective_Date { get; set; }

        }

        public class ESISlabDetail
        {
            public string? From_Value { get; set; }
            public string? To_Value { get; set; }
            public string? Criteria { get; set; }
            public int Criteria_Type_Id { get; set; }
            public int ESI_Slab_Detail_Id { get; set; }
        }

        [XmlRoot("ESISlabsDetails")]
        public class ESISlabsDetails
        {
            [XmlElement("ESISlab")]
            public EsiSlab EsiSlab { get; set; } = new EsiSlab();
        }

        // Wrapper for XML2
        [XmlRoot("ESISlabDetailResponse")]
        public class ESISlabDetailResponse
        {
            [XmlElement("ESISlabDetail")]
            public List<ESISlabDetail> ESISlabDetail { get; set; } = new List<ESISlabDetail>();
        }
    }
}
