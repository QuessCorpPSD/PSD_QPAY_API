using Newtonsoft.Json;
using QPay.BAL.IRepository.Common;
using QPay.DAL.Repository;
using QPay.UI.Common;


namespace QPay.BAL.Repository.Common
{
    public class ProcessCategoryRepository : IProcessCategoryRepository
    {
        private readonly DbRepository _dbRepository;
        public ProcessCategoryRepository(DbRepository dbRepository) { 
        this._dbRepository = dbRepository;
        }

        public async Task<List<ProcessCategoryUI>> GetAllProcessCategory()
        {
            try
            {
                string sql = "select * from (SELECT  Process_Category FROM   tbl_Process_Category group by Process_Category union all select 'B1') as t";
                var test = await this._dbRepository.QueryMultiAsync(sql);
                return JsonConvert.DeserializeObject<List<ProcessCategoryUI>>(test)
                                                        ?? new List<ProcessCategoryUI>();
            }
            catch (Exception ex)
            {
                return new List<ProcessCategoryUI>();
            }
           
        }
    }
}
