using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public class AllotmentRevok
    {
        public int InputLot_Id { get; set; }
        public int Company_Id {  get; set; }
        public int Pay_Period_Id { get; set; }
        public int Lot_Number { get; set; }       
        public int userId { get; set; }
        public int CreatedBy { get; set; }


    }

    public class AllottmentRevokRequest
    {
        public int InputLot_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_code { get; set; } = "";
        public string Company_Name { get; set; } = "";
        public int Pay_Period_Id { get; set; }
        public string Pay_Period { get; set; } = "";
        public int Lot_Number { get; set; }
        public int Revised { get; set; }
        public string CreatedOn { get; set; } = "";
        public DateTime  AllottedDatetime { get; set; }
        public DateTime? StartTime { get; set; }

    }
}
