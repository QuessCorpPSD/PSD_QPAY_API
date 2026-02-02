namespace QPay.API.Models
{
    public class BillableDaysModelRequest
    {
        // public string? ActionType { get; set; } = string.Empty;
        public string CreatedBy { get; set; }
        public FileList? File { get; set; }
        public int? importType { get; set; }


    }
    public class ExportToExcelModelRequest
    {
        public int Param { get; set; }
        public int Company_Id { get; set; }
        public int Pay_Period_Id { get; set; }
        public string Employee_Code { get; set; } = string.Empty;


    }
    public class FileList
    {
        public string? name { get; set; } = string.Empty;
        public string? type { get; set; } = string.Empty;
        public int? size { get; set; }
        public string? content { get; set; } = string.Empty;
    }
    public class BillableDaysSearchRequestModel
    {
        public int Company_Id { get; set; }
        public int Pay_Period_Id { get; set; }
        public string Employee_Code { get; set; } = string.Empty;
    }
}
