using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Customer;
using System.Data;

namespace QPay.BAL.IRepository.Customer
{
    public interface IServiceChargeRepository
    {
        //Task<DataTable> Search(string action, int? companyId, string xml);
        //Task<DataSet> ExportToExcel(string action, int? companyId, string xml);
        Task<DataSet> serviceChargeMaster();
        Task<DataSet> serviceChargeMasterNew(int companyId);
        Task<DataSet> GetUnitType();
        Task<DataSet> serviceChargeType(int? serviceChargeId);
        Task<ServiceChargeResponse> Create(ServiceChargeRequest request);

        Task<ServiceChargeResponse> FileUpload(IFormFile file, [FromForm] int ServiceChargeMaster, [FromForm] int ServiceChargeType,
              [FromForm] int SlabType, [FromForm] int SlabInnerType, [FromForm] int CreatedBy);
        Task<DataSet> GetAllServiceCharge(int companyId);


    }
}
