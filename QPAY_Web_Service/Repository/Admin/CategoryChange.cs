using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Admin;
using QPay.DAL.Repository;
using QPay.UI.Models.Admin;

namespace QPay.BAL.Repository.Admin
{
    public class CategoryChangeRepo : ICategoryChange
    {
        private readonly DbRepository _dbRepository;

        public CategoryChangeRepo(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<string> SearchCategoryChange(CategoryChangeModel model)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@companyID", model.CompanyID);
            parameters.Add("@payPeriod", model.PayPeriod);
            parameters.Add("@lotNumber", model.LotNumber);
            parameters.Add("@Revised", model.Revised);
            parameters.Add("@flag", model.Flag);
            parameters.Add("@CreatedBy", model.CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(
                "Sp_InputLotSearchCategoryUpdate_Test",
                parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> ImportCategoryChange(CategoryChangeModel model)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@companyID", model.CompanyID);
            parameters.Add("@payPeriod", model.PayPeriod);
            parameters.Add("@lotNumber", model.LotNumber);
            parameters.Add("@Revised", model.Revised);
            parameters.Add("@flag", model.Flag);
            parameters.Add("@XML_File", model.XML_File);
            parameters.Add("@CreatedBy", model.CreatedBy);

            var result = await _dbRepository.GetItemsAsync(
                "Sp_InputLotSearchCategoryUpdate_Test",
                parameters);

            return result;
        }
    }
}