namespace QPay.API.Models
{
    public class PayRegisterUploadModel
    {
        public int CompanyId { get; set; }

        public string CompanyCode { get; set; }
        public int Pay_Period_id { get; set; }
        public string Pay_Period { get; set; }
        public int LotNumber { get; set; }

        public string? FilePath { get; set; } = "";

        public string FileName { get; set; }
        public string FileType { get; set; }

        public string LoginUser { get; set; }

        public string Input_type { get; set; }
        public string Docs { get; set; }
        // public List<FilesModel> Files { get; set; }
    }
    public class FilesModel
    {
       
    }
}
