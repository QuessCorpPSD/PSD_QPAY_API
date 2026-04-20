using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.AccountReceivableMod
{
    public class LTDSReportModel
    {
        public int ReportTypeId { get; set; }
        public string ReportTypeValue { get; set; }
    }
    public class LTDSExportRequest
    {
        public int ReportTypeId { get; set; }
        public int FinancialYearId { get; set; }
        public string TanNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public int BusinessUnitId { get; set; } 
    }
    public class BusinessUnitModel
    {
        public int BusinessUnitId { get; set; }
        public string? BusinessUnitName { get; set; }
    }

}
