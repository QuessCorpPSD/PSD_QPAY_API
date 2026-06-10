using QPay.UI.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Admin
{
    public interface IRolesRepository
    {

        Task<DataSet> RolesCRUD(Roles _params);

    }
}
