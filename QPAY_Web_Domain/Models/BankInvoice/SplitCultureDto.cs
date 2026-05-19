using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace QZone.DTo.SplitCulture
{
    public class SplitCultureSearchDto
    {
        public int Company_Id { get; set; }
        public int? Bank_Culture_Id { get; set; }
        public string Mode { get; set; } = "";
    }

    public class SplitCultureResponseDto
    {
        public int Bank_Culture_Id { get; set; }
        public string Culture_Name { get; set; } = string.Empty;
        public string Bank_Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class BankCultureRequestDto
    {
        public int? Company_id { get; set; }
        public int? Vendor_id { get; set; }
        public int? Culture_Type { get; set; }
        public int? CreatedBy { get; set; } 
        public int? Bank_Culture_id { get; set; }
        public int? Bank_Culture_Detail_id { get; set; }
        public string Mode { get; set; } = string.Empty;

        public BankCultureDetailsResponseDto? BankCultureDetailsResponse { get; set; }
    }

    public class BankCultureDetailsResponseDto
    {
        public List<BankCultureDto> BankCulture { get; set; } = new();
    }

    public class BankCultureDto
    {
        public bool available { get; set; }
        public int Bank_Culture_id { get; set; }
        public int Bank_Culture_Detail_id { get; set; }
        public int Company_Id { get; set; }
        public int Map_Name_Id { get; set; }
        public string Map_Name { get; set; }
        public int CreatedBy { get; set; }
        public int Culture_Type { get; set; }
        public int Group_Detail_Id { get; set; }
        public int SNo { get; set; }
        public int Vendor_Id { get; set; }
    }

    public class SplitCultureResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

    public class UploadResponse
    {
        public string Validation { get; set; }
    }
}