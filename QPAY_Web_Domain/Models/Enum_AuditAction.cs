using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public enum Enum_AuditAction
    {
        Unknown = 0,
        Login = 1,
        LogOut = 2
    }

    public enum Enum_AuditActionStatus
    {
        Unknown = 0,
        Success = 1,
        Fail = 2
    }
}
