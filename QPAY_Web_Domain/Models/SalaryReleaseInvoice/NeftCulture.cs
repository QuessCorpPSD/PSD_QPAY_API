using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class NeftCulture
    {
        public string Bank_Id {  get; set; }
        public string Bank_Name { get; set; }
        public string Bank_Culture_id { get; set; }
        public string available { get; set; }       
    }

    public class Culturedata
    {
        public int Bank_Id { get; set; }
        public int Bank_Culture_id { get; set; } = 0;
       // public string Bank_Name { get; set; } = "";
    }

    public class Culturesave
    {
        public string Mode { get; set; }
        public int UserId { get; set; }
        public int Company_Id { get; set; }
        public List<Culturedata> culturedatas { get; set; } 
    }

    public class CultureMessage
    {
        public string Error_Message { get; set; } = "";
    }
}
