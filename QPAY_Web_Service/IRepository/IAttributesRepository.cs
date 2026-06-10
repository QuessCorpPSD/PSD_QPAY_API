using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.AttributesClass;

namespace QPay.BAL.IRepository
{
    public interface IAttributesRepository
    {

        Task<DataSet> GetAttributes();
        Task<List<AttributeUI>> GetAllAttribute(AttributeUI attributeUI);
        Task<AttributesResponse> UploadAttributesData(IFormFile file, [FromForm] string User,
          [FromForm] string companyCode, [FromForm] string OfferId);

        Task<DataSet> GetAttributeTemplate(string xml);

    }
}
