using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.DTo.Models.Common
{
    public class APIResponse<T>
    {
            public int statuscode { get; set; }
            public string message { get; set; } = string.Empty;
            public T? data { get; set; }
            public string error { get; set; } = string.Empty;
    }
}
