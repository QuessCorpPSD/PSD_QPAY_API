using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public  class UserLotValidationRequest
    {
        public int userId { get; set; }
        public int companycode { get; set; }
        public int pay_period_Id { get; set; }
        public int lot_number { get; set; }


    }
}
