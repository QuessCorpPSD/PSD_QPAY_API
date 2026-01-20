using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Billing
{
    public class GenericUpload
    {
        public class GenericUploadResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class GenericUploadResponseModel
        {
            public string Result { get; set; }
            public string Error_Message { get; set; }
        }

    }
}
