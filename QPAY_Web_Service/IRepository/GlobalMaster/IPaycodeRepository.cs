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
        Task<PTStateExclude> CreateFlexidetails(string mode, string xml, Int32 userid);
        Task<List<PTStateExclude>> GetSearchdata(int? Company_Id, int? Band_Id, int? Flexi_Rule_Id, string? Mode);
        //Task<List<PTStateExclude>> GetEditdata(Int32 Flexi_Rule_Id, string mode);
        Task<List<CompanyPayCodeDetail>> Companypaycodes(int? company_Id);
        //Task<ActivationResponse> UploadEmployeeActivation(IFormFile file, [FromForm] string User,
        //    [FromForm] string COMPANY_CODE, [FromForm] string FLAG);            
        //Task<ActivationResponse> UploadEmployeeLWD(IFormFile file, [FromForm] string User,
        //    [FromForm] string COMPANY_CODE, [FromForm] string FLAG);

    }
}
