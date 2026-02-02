using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Common;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.TaxAndSaving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IInputAggregatorRepository
    {
        Task<DataSet> QuessAttributeMaster();
        Task<DataSet> ClientAttributes(int? companyId);
        Task<DataSet> Search(int? companyId);

        Task<RequestResponse> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy);

        Task<RequestResponse> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy);

        Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy);

    }
}
