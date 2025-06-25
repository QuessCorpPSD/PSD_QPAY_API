using QPay.UI.Admin;
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
        Task<Users?>  UserLogin(int userName, string password, string loginIp, string CName);
        Task<Users?> UserCreate(Users user);
        Task<List<Users>> GetAllActiveUsers();
        Task<QPay.UI.Models.Users?> ChangePasswordAsync(ChangePassword changePassword);
        Payload GetCompanies();
    }
}
