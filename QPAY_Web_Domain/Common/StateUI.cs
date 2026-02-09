using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Common
{
    public class StateUI
    {
        public int? State_Id { get; set; }

        public string State_Name { get; set; }= "";
        public string Country { get; set; }= "";

        public int? Region_Id { get; set; }
        public string SAP_Code { get; set; } = "";
        public string State_Code { get; set; }= "";

        public string Region_Name { get; set; } = "";

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        

    }
    public class StateResponse
    {
        public int? StateId { get; set; }

        public string State_Name { get; set; } = "";
    }

    }
