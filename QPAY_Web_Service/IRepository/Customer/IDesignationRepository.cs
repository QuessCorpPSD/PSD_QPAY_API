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
    public interface IDesignationRepository
    {
        Task<List<Designation>> GetAllDesignationDetails(string companyId);
        Task<DesignationResponse> SaveUpdateDeleteDesignation([FromBody] DesignationRequest request);
        Task<string> PostDesignationUpload(string xmlString, string userId);
        DataSet DesignationExport(int companyId);
    }
}
