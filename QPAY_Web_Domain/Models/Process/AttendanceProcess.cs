namespace QPay.UI.Models.Process
{
    public class AttendanceProcess
    {
        public class AttendanceProcessResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }
        public class SearchRequest
        {
            public string mode { get; set; } = "";
            public string Value1 { get; set; } = "";
            public searchxml searchxml { get; set; } = new searchxml();

        }

        public class searchxml
        {
            public string Company_id { get; set; } = "";
            public string Pay_Frequency_Id { get; set; } = "";
            public string Resign_Status { get; set; } = "";
            public string Emp_Code { get; set; } = "";
            public string Process_Type { get; set; } = "";
        }

        public class ExporttoExcelRequest
        {
            public string mode { get; set; } = "";
            public string Value1 { get; set; } = "";
            public searchxml exporttoExcelxml { get; set; } = new searchxml();

        }

        public class ExporttoExcelxml
        {
            public string Company_id { get; set; } = "";
            public string Pay_Frequency_Id { get; set; } = "";
        }

        public class SearchArrearRequest
        {
            public string Company_id { get; set; } = "";
            public string Pay_Frequency_Id { get; set; } = "";
        }
    }
}
