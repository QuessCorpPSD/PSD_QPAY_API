using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.GlobalMaster
{
    public class SHG
    {
        public int LWF_Slab_Id { get; set; }
        public int Financial_Year_Id { get; set; }
        public int LWF_Slab_Detail_Id { get; set; }
        public int Pay_Frequency_Id { get; set; }

        public string Effective_Date { get; set; }
        public string Category { get; set; }
        public string Financial_Year { get; set; }
        public string From_Value { get; set; }
        public string To_Value { get; set; }
        public string EmployerContributionPerc { get; set; }
        public int Serial_No { get; set; }
        public string Error_Message { get; set; }

    }

    public class SHGCreateParams
    {
        public string strXmlDetails { get; set; }

        public string mode { get; set; }
        public int userId { get; set; }


    }

}
