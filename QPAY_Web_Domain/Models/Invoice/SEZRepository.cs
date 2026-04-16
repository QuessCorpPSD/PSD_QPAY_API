using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Invoice
{
    public class SEZRepository
    {
        public int Serial_No { get; set; }
        public int Id { get; set; }
        public int Company_Id { get; set; }
        public int Payperiod_Id { get; set; }
        public int Invoice_Id { get; set; }
        public string? Invoice_Number { get; set; }
        public string? Document_Name { get; set; }
        public string? Uploaded_Date { get; set; }
        public string? Document_FilePath { get; set; }
        public string? Obselete_Document_FilePath { get; set; }
        public string? Document_Remarks { get; set; }
        public string? Error_Message { get; set; }
        public string? selectedrecord { get; set; }
        public List<SelectedRecords> selectedrecordsList { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? UploadStatus { get; set; }
        public string? RequestedBy { get; set; }
        public string? AckNo { get; set; }
    }


    public class SelectedRecords
    {
        public int Serial_No { get; set; }
        public int Id { get; set; }
        public int Company_Id { get; set; }
        public int Payperiod_Id { get; set; }
        public int Invoice_Id { get; set; }
        public string Invoice_Number { get; set; }
        public string Document_Name { get; set; }
        public string Uploaded_Date { get; set; }
        public string Remark { get; set; }
        public string Document_FilePath { get; set; }
        public int uid { get; set; }
        public string ApprovalStatus { get; set; }
        public string UploadStatus { get; set; }

    }
    public class FileDetails
    {
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public class SEZJson
    {
        public string? FilePath { get; set; }
    }

    public class ApproveRequest
    {
      public string? Invoice_Id { get; set; }
      public string? Remarks { get; set; }
      public string? UserId { get; set; }
      public string? Action { get; set; }
    }

    public class SEZCertificate
    {
        public int Serial_No { get; set; }
        public int Id { get; set; }
        public int Company_Id { get; set; }
        public string? Company_Code { get; set; }
        public string? Document_Name { get; set; }
        public string? AckNo { get; set; }
        public string? Valid_From {get; set;}
        public string? Valid_To { get; set; }
        public string? Uploaded_Date { get; set; }
        public string? Document_FilePath { get; set; }
        public string? Document_Remarks { get; set; }
        public string? RequestedBy { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? UploadStatus { get; set; }
    }
}
