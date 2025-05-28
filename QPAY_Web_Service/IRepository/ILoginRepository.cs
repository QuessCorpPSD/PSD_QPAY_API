using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
   public interface ILoginRepository
    {
        Users UserLogin(int userName, string password, string loginIp, string CName);
        Payload GetCompanies();
    }
}
