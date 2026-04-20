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
        public class TdsSlabSaveRequest
        {
            public string TdsDetails { get; set; }
            public string action { get; set; }
            public int userId { get; set; }
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