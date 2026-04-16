using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.GrossMargin
{
    public class AccuralsModelRequest
    {
        public int CompanyId {  get; set; }
        public int PayPeriodId { get; set; }
        public int CreatedBy {  get; set; }
        public string File { get; set; } = string.Empty;
        public string FileName { get; set; }= string.Empty;
    }

    public class AccuralsModelResponse
    {
        public int StatusCode {  get; set; }
        public string StatusMessage { get; set; } = string.Empty;
    }
}
