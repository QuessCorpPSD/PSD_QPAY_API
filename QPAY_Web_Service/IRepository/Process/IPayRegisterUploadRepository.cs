using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.IRepository.Process
{
    public interface IPayRegisterUploadRepository
    {
        Task<DataSet> DownloadTemplate(SearchPayRegisterRequest searchRequest);
        Task<DataSet> ExporttoExcel(SearchPayRegisterRequest exporttoExcelRequest);
        Task<ProcessResponse> ImportPayRegister(IFormFile file, [FromForm] string User);
    }
}
