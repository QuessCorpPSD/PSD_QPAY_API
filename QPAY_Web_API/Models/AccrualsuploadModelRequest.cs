using QPay.UI.Models;

namespace QPay.API.Models
{
    public class AccrualsuploadModelRequest
    {
        public string CompanyCode {  get; set; }
        public string Payperiod { get; set; }
        public FileResponse FileResponse { get; set; }

    }
}
