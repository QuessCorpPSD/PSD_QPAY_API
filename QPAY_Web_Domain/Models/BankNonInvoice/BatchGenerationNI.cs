using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.BankNonInvoice
{
    public class BatchGenerationNI
    {
        public long Salary_Process_Initiate_detail_Id { get; set; }
        public long Salary_Process_Initiate_Id { get; set; }
        public int Bank_Culture_Id { get; set; }
        public int Group_Count { get; set; }
        public string? BatchId { get; set; }
        public string Group_Name { get; set; }
        public string? WBS_Code { get; set; }
        public int Pay_Frequency_Detail_Id { get; set; }
        public string Pay_Period { get; set; }
        public string Vendor_Name { get; set; }
        public string Purpose { get; set; }
        public string? Input_No { get; set; }
    }

    public class EntityMasterNI
    {
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }

    }

    public class NIBatchGenerate    
    {
       
        public string BatchType { get; set; }
        public int Entity_id { get; set; }
        public int batchCreationTypes { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }

        public List<BatchGenerationNI> BatchList { get; set; }
    }

    public class IntitiateBatch
    {
        public string BatchType { get; set; }
        public string BatchId { get; set; }
        public int UserId { get; set; }

    }

    public class CommonGenModel
    {
        public int GEN_iID { get; set; }
        public string GEN_vDescription { get; set; }
    }
    public class BatchList
    {
        public string BATCHID { get; set; }

    }

    public class BulkUploadErrorMessage
    {
        // public int Status { get; set; }
        public string Validation { get; set; }
    }

    public class SatausErrorMessage
    {
        public string Error_Message { get; set; }
    }

    public class CommonDropDown
    {
        public string value { get; set; }
        public string name { get; set; }
    }
}
