using QPay.UI.Models.GlobalMaster;
using System.Data;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IPaycodeRepository
    {
        Task<DataSet> Search(string strPayCode, int? intPayTypeId, int? IsTaxable, int? PayId);
        Task<DataSet> Create(string strXmlDetails, string mode, int userId);
        Task<DataSet> GetPageType();
        Task<string> GetPayType();
        // Task<List<PayCodeUI>> GetPayCodeByCompanyId(int companyId);
        Task<DataSet> GetPayCodeByCompanyId(int companyId, int invoiceCultureId, string type);
        //Task<ActivationResponse> UploadEmployeeActivation(IFormFile file, [FromForm] string User,
        //    [FromForm] string COMPANY_CODE, [FromForm] string FLAG);            
        //Task<ActivationResponse> UploadEmployeeLWD(IFormFile file, [FromForm] string User,
        //    [FromForm] string COMPANY_CODE, [FromForm] string FLAG);

    }
}
