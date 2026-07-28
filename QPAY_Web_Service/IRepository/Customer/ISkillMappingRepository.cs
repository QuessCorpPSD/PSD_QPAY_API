using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface ISkillMappingRepository
    {
        Task<DataSet> Search(int? companyId, int? siteId);
        Task<string> CreateUpdateSkillMapping(SkillMappingRequest request);
        Task<string> DeleteSkillMapping(int companyId, int siteId, int userId);
    }
}
