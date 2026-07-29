using QPay.UI.Models.Admin;

namespace QPay.BAL.IRepository.Admin
{
    public interface ICategoryChange
    {
        Task<string> SearchCategoryChange(CategoryChangeModel model);
        Task<string> ImportCategoryChange(CategoryChangeModel model);
    }
}