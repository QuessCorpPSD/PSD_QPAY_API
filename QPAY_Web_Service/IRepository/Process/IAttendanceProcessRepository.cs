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
    public interface IAttendanceProcessRepository
    {
        Task<DataSet> SearchDetails(SearchRequest searchRequest);
        Task<DataSet> ExporttoExcel(ExporttoExcelRequest exporttoExcelRequest);
        Task<AttendanceProcessResponse> ImportAttendnace(IFormFile file, [FromForm] string User);
    }
}
