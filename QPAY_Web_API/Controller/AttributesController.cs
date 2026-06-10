using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Common;
using QPay.UI.Models;
using System.Data;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        private readonly IAttributesRepository _iAttributes;
        private readonly ICommonRepository _icommon;
        public AttributesController(IAttributesRepository iAttributes, ICommonRepository icommon)
        {
            this._iAttributes = iAttributes;
            this._icommon = icommon;
        }

        [HttpGet]
        [Route("GetAttributes")]
        public async Task<IActionResult> GetAttributes()
        {
            var ds = await _iAttributes.GetAttributes();
            if (ds.Tables[0].Rows.Count == 0)
            {
                return BadRequest(new { StatusCode = "400", Message = "No records found." });
            }            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }
        [HttpPost,Route("AttributeAddUpdate")]
        public async Task<IActionResult> AttributeAddUpdate(AttributeUI attributeUI)
        {
            var attributes = await _iAttributes.GetAllAttribute(attributeUI);
            return Ok(attributes);
        }
        [HttpPost,Route("GetAllAttribute")]
        public async Task<IActionResult> GetAllAttribute(AttributeUI attributeUI)            
        {
            var payload = await _iAttributes.GetAllAttribute(attributeUI);

            var attribute = payload.Select(x => new SelectedItems()
            {
                value=x.AttributeName,
                text=x.AttributeName
            }).ToList();

            return Ok(attribute.ToList());
        }


        [HttpPost]
        [Route("UploadAttributesData")]
        public async Task<IActionResult> UploadAttributesData(IFormFile file, [FromForm] string User,
         [FromForm] string companyCode, [FromForm] string OfferId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var result = await _iAttributes.UploadAttributesData(file, User, companyCode, OfferId);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetAttributeTemplate")]
        public async Task<IActionResult> GetAttributeTemplate([FromBody] AttributeValues request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);

            var ds = await _iAttributes.GetAttributeTemplate(xml);
            var table = ds.Tables[0];

            var result = table.AsEnumerable()
                .Select(row =>
                    table.Columns.Cast<DataColumn>()
                        .ToDictionary(
                            col => col.ColumnName,
                            col => row[col] == DBNull.Value ? null : row[col]
                        )
                ).ToList();

            return Ok(result);
        }
    }
}
