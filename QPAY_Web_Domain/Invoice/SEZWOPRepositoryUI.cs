using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Invoice
{
    public class SEZWOPRepositoryUI
    {
        public int Serial_No { get; set; }
        public int Id { get; set; }
        public int Company_Id { get; set; }
        public int Payperiod_Id { get; set; }
        public int Invoice_Id { get; set; }
        public string Invoice_Number { get; set; } = string.Empty;
        public string Document_Name { get; set; }= string.Empty;
        public string Uploaded_Date { get; set; }= string.Empty;
        public string Document_FilePath { get; set; } = string.Empty;
        public string Obselete_Document_FilePath { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Error_Message { get; set; } = string.Empty;
        public string selectedrecord { get; set; } = string.Empty;
        public List<SelectedRecords> selectedrecordsList { get; set; } =new List<SelectedRecords>();
        public string ApprovalStatus { get; set; } = string.Empty;
        public string UploadStatus { get; set; } = string.Empty;

    }
    public class SelectedRecords
    {
        public int Serial_No { get; set; }
        public int Id { get; set; }
        public int Company_Id { get; set; }
        public int Payperiod_Id { get; set; }
        public int Invoice_Id { get; set; }  
        public string Invoice_Number { get; set; } = string.Empty;
        public string Document_Name { get; set; } = string.Empty;
        public string Uploaded_Date { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Document_FilePath { get; set; } = string.Empty;
        public int uid { get; set; }
        public string ApprovalStatus { get; set; } = string.Empty;
        public string UploadStatus { get; set; } = string.Empty;

    }
}
