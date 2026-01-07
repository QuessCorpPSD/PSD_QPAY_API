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
    public interface ISDLRepository
    {
        Task<DataSet> Search();
        Task<DataSet> Create(string strXmlDetails, string mode, int userId);
        Task<DataSet> GetCriteria(int? CriteriaTypeId);
        Task<DataSet> GetPaycode();
    }
}
