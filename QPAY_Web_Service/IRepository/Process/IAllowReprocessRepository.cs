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
    public interface IAllowReprocessRepository
    {
        Task<DataSet> SearchDetails(SearchAllowReprocessRequest searchRequest);
        Task<DataSet> ExporttoExcel(SearchAllowReprocessRequest exporttoExcelRequest);
        Task<ProcessResponse> Create(AllowReprocessCreateRequest createRequest);
    }
}
