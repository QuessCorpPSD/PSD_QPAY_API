using System;
using System.Collections.Generic;

namespace QPay.UI.Models.AccountReceivableMod
{
    public class TDSSlabModels
    {
        public class UploadResponse
        {
            public string response { get; set; }
            public List<string> errors { get; set; }

            public UploadResponse()
            {
                response = string.Empty;
                errors = new List<string>();
            }
        }

        public class ClientTdsSlabMaster
        {
            public int TdsSlabMaster_Id { get; set; }
            public int Company_Id { get; set; }
            public string? Company_Name { get; set; }
            public string? Company_Code { get; set; }
            public int TypeOfSelection { get; set; }
            public decimal Value { get; set; }
            public decimal Percentage { get; set; }
            public string? FromDate { get; set; }
            public string? ToDate { get; set; }
            public string? NatureOfBusiness { get; set; }
            public string? TAN { get; set; }
            public string? PAN { get; set; }
            public int Client_Id { get; set; }
            public int Serial_No { get; set; }
            public int Financial_Year_Id { get; set; }
            public string? Financial_Year_Name { get; set; }
        }

        public class TdsSlabSaveRequest
        {
            public List<ClientTdsSlabMaster> TdsDetails { get; set; } = new List<ClientTdsSlabMaster>();
            public string action { get; set; } = string.Empty;
            public int userId { get; set; }
        }

        public class TdsSlabSaveResponse
        {
            public string response { get; set; }
            public List<string> errors { get; set; }

            public TdsSlabSaveResponse()
            {
                response = string.Empty;
                errors = new List<string>();
            }
        }

        public class TdsSlabResult
        {
            public string? Error_Message { get; set; }
        }

        public class CompanyNameByCodeResult
        {
            public int Client_Id { get; set; }
            public string? Client_Code { get; set; }
            public string? Company_Name { get; set; }
        }
    }
}