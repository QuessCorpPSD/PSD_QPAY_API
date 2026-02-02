using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;

namespace QPay.API.Models
{
    public class InvoiceInitiateRequestModel
    {
        public List<InitiationRequestUI> invoiceInitiations { get; set; }
        public int TaxTypeId { get; set; }

        public int CreatedBy { get; set; }
    }
}
