using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.Process.AttendanceProcess;

namespace QPay.BAL.IRepository.Process
{
    public interface IArrearAttendanceProcessRepository
    {
        Task<DataSet> SearchDetails(SearchArrearRequest searchRequest);
        Task<AttendanceProcessResponse> ImportArrearAttendnace(IFormFile file, [FromForm] string User);
    }
}
