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
    public interface IOtherIncomeRepository
    {
        Task<DataSet> SearchDetails(SearchOIRequest searchRequest);
        Task<ProcessResponse> ImportOtherIncome(IFormFile file, [FromForm] string User);
        Task<DataSet> DeleteOtherIncome(string Other_Income_Id, string CreatedBy);
    }
}
