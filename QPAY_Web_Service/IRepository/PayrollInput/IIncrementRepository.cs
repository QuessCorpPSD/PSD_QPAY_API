using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static QPay.DTo.Models.PayrollInput.Increment;

namespace QPay.IRepository.iRepository.PayrollInput
{
    public interface IIncrementRepository
    {
        DataSet GetEmployeeIncrement(int companyId, int payPeriodId, int InputType, int MapNameId);
        Task<IncrementResponse> UploadIncrementData(IFormFile file, [FromForm] string User,
           [FromForm] string companyCode, [FromForm] int companyId, [FromForm] int InputType);
    }
}
