using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface ICPFRepository
    {
        Task<DataSet> Search(int? PayCode, int? Category);
        Task<DataSet> Create(string strXmlDetails, string mode, int userId);
        Task<DataSet> GetPaycode();
        Task<DataSet> GetCriteria(int? CriteriaTypeId);
        Task<List<CategoryUI>> GetCategory();
    }
}
