using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.IRepository.Process
{
    public interface ILockPayperiodRepository
    {
        Task<DataSet> SearchDetails(SearchLockPayperiodRequest searchRequest);
        Task<DataSet> ExporttoExcel(SearchLockPayperiodRequest exporttoExcelRequest);
        Task<ProcessResponse> ImportLockpayperiod(IFormFile file, [FromForm] string User);
        Task<ProcessResponse> Lock(string xml,string User);
    }
}
