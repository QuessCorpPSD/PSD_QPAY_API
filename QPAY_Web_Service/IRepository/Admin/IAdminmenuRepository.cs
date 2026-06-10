using QPay.UI.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Admin
{
    public interface IAdminmenuRepository
    {

        Task<DataSet> GetRoleTypes();

        Task<DataSet> GetReportingTo();

        Task<DataSet> GetAccessType();
        Task<DataSet> Search(int? UserId, String? UserName, int? RoleId, int? IsCheck);
        Task<DataSet> Create(string xml, int createdBy, string mode, string UserDetails);
    }
}
