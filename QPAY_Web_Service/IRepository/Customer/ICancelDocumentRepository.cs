using QPay.UI.Models.Customer;

namespace QPay.BAL.IRepository.Customer
{
    public interface ICancelDocumentRepository
    {
        Task<List<CancelDocument>> Search(int companyId, int payPeriodId);
        Task<string> UploadDocument(string xml, int userId);
    }
}
