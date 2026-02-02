using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.IRepository.Process
{
    public interface IOnetimeReplacementRepository
    {
        Task<DataSet> SearchDetails(SearchOnetimeReplacementRequest searchRequest);
        Task<DataSet> ExporttoExcel(SearchOnetimeReplacementRequest exporttoExcelRequest);
        Task<ProcessResponse> ImportOnetimeReplacement(IFormFile file, [FromForm] string User);
        Task<DataSet> DeleteOnetimeReplacement(string One_Time_Replacement_Id, string CreatedBy);
    }
}
