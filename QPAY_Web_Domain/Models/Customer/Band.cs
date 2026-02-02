using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QPay.UI.Models.Customer
{
    public class Band
    {
        public int? Band_Id { get; set; }
        public string? Band_Code { get; set; }
        public string? Band_Name { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_Code { get; set; }
        public int? Serial_No { get; set; }
        public string? Error_Message { get; set; }
    }
    public class BandResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }
    public class BandRequest
    {
        public string Created_By { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public List<Band> Bandmaster { get; set; }
    }
}
