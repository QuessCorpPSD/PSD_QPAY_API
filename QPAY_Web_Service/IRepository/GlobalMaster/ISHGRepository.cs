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
    public interface ISHGRepository
    {
        Task<DataSet> Search(string? effectiveDate);
        Task<DataSet> Create(string strXmlDetails, string mode, int userId);
        Task<List<CategoryUI>> GetCategory();
    }
}
