using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Customer;
using System.Data;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.BAL.IRepository.Customer
{
    public interface IVendorServiceChargeRepository
    {
       
        Task<VendorServiceChargeResponse> Create(VendorServiceChargeRequest request);

        Task<VendorServiceChargeResponse> FileUpload(IFormFile file, [FromForm] int CreatedBy);

        Task<DataSet> GetAllVendorServiceCharge(int companyId);
        Task<List<GenDD>> GetAllVendorServiceType();
        Task<List<GenDD>> GetAllBillingTypes();

    }
}
