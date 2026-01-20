using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Common
{
    public class RoleUI
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;
        public bool IsSysAdmin { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedUserName { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string ModifiedByUserName { get; set; } = string.Empty;
    }

    public class RequestResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

}
