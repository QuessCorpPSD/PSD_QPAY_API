using QPay.UI.Common;

namespace QPay.BAL.IRepository.Common
{
    public interface IProcessCategoryRepository
    {
        Task<List<ProcessCategoryUI>> GetAllProcessCategory();
    }
}
