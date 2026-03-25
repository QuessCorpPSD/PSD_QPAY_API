using System.Collections.Generic;

namespace QPay.UI.Models.Invoice
{
    public class SEZWOPRepositoryResponse
    {
        public SEZWOPRepository[] SEZWOPRepositoryDetails { get; set; }
    }

    public class SEZWOPRepository
    {
        public int Serial_No { get; set; }
        public int Id { get; set; }
        public int Company_Id { get; set; }
        public int Payperiod_Id { get; set; }
        public int Invoice_Id { get; set; }
        public string Invoice_Number { get; set; }
        public string Document_Name { get; set; }
        public string Uploaded_Date { get; set; }
        public string Document_FilePath { get; set; }
        public string Obselete_Document_FilePath { get; set; }
        public string Remark { get; set; }
        public string Error_Message { get; set; }
        public string selectedrecord { get; set; }
        public List<SelectedRecords> selectedrecordsList { get; set; }
        public string ApprovalStatus { get; set; }
        public string UploadStatus { get; set; }
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

    public class DocumentTypeMaster
    {
        public int Document_Type_Id { get; set; }
        public string Document_Type { get; set; }
    }

    public class DocumentUploadsFiles
    {
        public string Document_Name { get; set; }
        public string Document_Remarks { get; set; }
        public string EmpployeeID { get; set; }
    }

    public class SearchRequestDto
    {
        public int CompanyId { get; set; }
        public int PayPeriodId { get; set; }
        public string InvoiceNumbers { get; set; }
        public int Year { get; set; }
    }

    public class DeleteRequestDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
    }

    public class ExportToExcelRequestDto
    {
        public int? CompanyId { get; set; }
        public string StatusId { get; set; }
        public string InvoiceNumbers { get; set; }
        public int? Year { get; set; }
    }

    public class FilesDetailsRequestDto
    {
        public string Document_Name { get; set; }
        public string Document_Remarks { get; set; }
        public string Empid { get; set; }
    }

    public class UploadStatusRequestDto
    {
        public string ApprovalStatus { get; set; }
        public string selectedrecord { get; set; }
    }
}
