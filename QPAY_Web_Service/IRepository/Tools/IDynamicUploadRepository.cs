using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Tools
{
    public interface IDynamicUploadRepository
    {
        Task<DataSet> GetUploadType(int? Upload_Type, int? UserId);
        Task<DataSet> GetAllColumns(int? Upload_Type, int? UserId);

        Task<ServiceChargeResponse> FileUpload(IFormFile file, [FromForm] int UploadTypeId,[FromForm] int CreatedBy);

    }
}
