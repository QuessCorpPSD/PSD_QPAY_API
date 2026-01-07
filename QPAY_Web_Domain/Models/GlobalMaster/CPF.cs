using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.GlobalMaster
{
    public class CPF
    {
        public int Provident_Fund_Id { get; set; }
        public int Provident_Fund_Detail_Id { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int PayCodeId { get; set; }

        public string PayCode { get; set; }

        public string Description { get; set; }

        public int From_Age { get; set; }

        public int To_Age { get; set; }

        public int IsCapType { get; set; }

        public string Category { get; set; }

        public decimal From_Value { get; set; }

        public decimal To_Value { get; set; }

        public string Criteria { get; set; }

        public int CriteriaTypeId { get; set; }

        public int Serial_No { get; set; }
        public string Error_Message { get; set; }

        public string Formula { get; set; }

        public string Criteria_Type_Name { get; set; }
    }

    public class CpfSearchParams
    {
        public int? Category { get; set; }
        public int? Paycode { get; set; }
    }

    public class CategoryUI
    {
        public int? spr_status_id { get; set; }
        public string spr_status { get; set; } = "";
        public bool? IsActive { get; set; }
    }



    public class CPFCreateParams
    {
        public string strXmlDetails { get; set; }

        public string mode { get; set; }
        public int userId { get; set; }


    }

}