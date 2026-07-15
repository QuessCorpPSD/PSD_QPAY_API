using QPay.DTo.Models.PayrollInput;
using QPay.UI.Common;
using System.Data;

namespace QPay.IRepository.iRepository.PayrollInput
{
    public interface IOnboardingRepository
    {
        Task<List<Onboarding>> GetAllOnboardingDetails(string companyId, string? payPeriod);
        DataSet GetNewJoineeTemplate(int companyId, int payPeriodId, int flag, int mapNameId);
        Task<string> MoveToQpay(string xmlString, int companyId, string payPeriod, int payPeriodId, string userId);
        Task<string> PostValidateOfferId(string xmlString);
        Task<string> PostRollbackOfferId(string xmlString, string userId);
        Task<string> PostNewJoineeData(string xmlString, string companyCode, int companyId, int payPeriodId, string filePath, string userId);
        List<PayperiodDD> GetCurrentPayperiod(int companyId);
        Task<string> PostOneTimeInputData(string xmlString, int companyId, int payPeriodId, string filePath, string userId);
        Task<List<FinalSubmission>> GetAllFinalSubmitDetails(int companyId, int payPeriodId, string Action, string userId);
        string GetRegisterFilename(int companyId, int payPeriodId, int lotNumber, string inputType, int flag);
        Task<string> PostFinalSubmission(int companyId, int payPeriodId, string LotNos, string userId, string remarks);
        DataSet GetNewJoineeEmployeeId(int companyId, string payPeriod, int lotNumber);
        FileResponse GetConsolidatePayRegister(int companyId, string companyCode, string payPeriod, int payPeriodId, string lotNumber);
        FileResponse GetConsolidatePayRegisterOT(int companyId, string companyCode, string payPeriod, int payPeriodId, string lotNumber);
        Task<DataSet> EmployeeTemplateImport(string xmlInput, string userId, int companyId, int payPeriodId, int inputId, int lotNo);
        Task<DataSet> GetRevisedTemplate(int companyId, int payPeriodId, int mapNameId, int inputId, int lotNo);
        Task<string> PostRevisedInput(string xmlString, string userId, string companyCode, int companyId, int payPeriodId, int inputType, int lotNo, string filePath);
        Task<DataSet> GetInputautomationReport(int companyId, int payPeriodId, int inputId, int lotNo);
        Task<string> PostCustomerConfirmation(int companyId, int payPeriodId, string LotNos, string userId);
        Task<string> PostFinalSubmissionLotMerge(FinalSubmitMerge request);
        Task<FileResponse> AttributeTemplate(int flagId, int companyId, int payperiodId, int lotno, string createdBy,string xml);
    }

}
