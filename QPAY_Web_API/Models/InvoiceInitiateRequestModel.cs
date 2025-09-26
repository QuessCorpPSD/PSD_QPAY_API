using QPay.UI.Models;

namespace QPay.API.Models
{
    public class InvoiceInitiateRequestModel
    {
        public List<InvoiceInitiationUI> invoiceInitiations { get; set; }
        public int TaxTypeId { get; set; }

        public int CreatedBy { get; set; }
    }
}
