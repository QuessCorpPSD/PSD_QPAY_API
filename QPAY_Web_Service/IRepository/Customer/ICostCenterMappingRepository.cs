using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface ICostCenterMappingRepository
    {
        Task<List<CostCenterMapping>> GetAllCostCentertDetails(string? costCenter);
        Task<CostCenterResponse> SaveUpdateDeleteCostCenter([FromBody] CostCenterRequest request);
        Task<string> PostCostCenterUpload(string xmlString, string userId);
        DataSet CostCenterExport(string? CostCenterMapName);
    }
}
