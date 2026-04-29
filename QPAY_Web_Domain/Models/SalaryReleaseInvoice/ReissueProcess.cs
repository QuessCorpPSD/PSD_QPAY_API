using System;
using System.Collections.Generic;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class ReissueProcessDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
    }

    // Use this for Export Payload
    public class CommonExport
    {
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string status { get; set; }
    }

    public class ReissueProcessReportResponse
    {
        public string response { get; set; }
        public List<ResponseModel> data { get; set; }
        public List<string> errors { get; set; }
    }

    public class ResponseModel
    {
        public string Error_Message { get; set; }
    }
}