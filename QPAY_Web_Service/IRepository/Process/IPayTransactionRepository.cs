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
    public interface IPayTransactionRepository
    {
        Task<DataSet> GetEmployeeDetailsByCompanyID(SearchEmployeeRequest searchRequest);
        Task<DataSet> GetAllPayCodeByCompanyID(SearchEmployeeRequest searchRequest);
        Task<DataSet> SearchDetails(SearchPayTransactionRequest searchRequest);
        Task<DataSet> Exporttoexcel(SearchPayTransactionRequest searchRequest);
        Task<ProcessResponse> ImportPayTransaction(IFormFile file, [FromForm] string User);
        Task<DataSet> DeletePayTransaction(string Pay_Transaction_Id, string CreatedBy);
    }
}
