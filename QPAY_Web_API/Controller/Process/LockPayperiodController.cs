using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Process;
using QPay.BAL.Repository.Process;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.API.Controller.Process
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockPayperiodController : ControllerBase
    {
        private readonly ILockPayperiodRepository _processRepository;
        public LockPayperiodController(ILockPayperiodRepository processRepository)
        {
            this._processRepository = processRepository;
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(SearchLockPayperiodRequest searchRequest)
        {
            var ds = await _processRepository.SearchDetails(searchRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost, Route("ExporttoExcel")]
        public async Task<IActionResult> ExporttoExcel(SearchLockPayperiodRequest exporttoExcelRequest)
        {
            var ds = await _processRepository.ExporttoExcel(exporttoExcelRequest);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return Ok(new { StatusCode = 400, Message = "No records found." });
            }
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpPost]
        [Route("ImportLockpayperiod")]
        public async Task<IActionResult> ImportLockpayperiod(IFormFile file, [FromForm] string User)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _processRepository.ImportLockpayperiod(file, User);
            return Ok(result);
        }

        [HttpPost]
        [Route("Lock")]
        public async Task<IActionResult> Lock(LockPayperiodRequest request)
        {
            var wrapper = new LockPayPeriodDetailsWrapper
            {
                LockPayPeriod = new LockPayperiodRequest
                {
                    Company_Id = request.Company_Id,
                    Pay_Frequency_Detail_Id = request.Pay_Frequency_Detail_Id,
                    Pay_Period = request.Pay_Period,
                    CreatedBy = request.CreatedBy
                }
            };

            string xml = ToXml(wrapper);

            var result = await _processRepository.Lock(xml, request.CreatedBy);
            return Ok(result);
        }

        public string ToXml(LockPayPeriodDetailsWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(LockPayPeriodDetailsWrapper));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // remove xmlns

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,   // 🚀 remove the XML header
                Indent = true
            };

            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(writer, wrapper, ns);
                return sw.ToString();
            }
        }
    }
    [XmlRoot("LockPayPeriod")]
    public class LockPayperiodRequest
    {
        public string Company_Id { get; set; } = "";

        public string Pay_Frequency_Detail_Id { get; set; } = "";
        public string Pay_Period { get; set; } = "";

        [XmlIgnore]   // Do NOT include in XML
        public string CreatedBy { get; set; } = "";
    }

    [XmlRoot("LockPayPeriodDetails")]
    public class LockPayPeriodDetailsWrapper
    {
        public LockPayperiodRequest LockPayPeriod { get; set; }
    }
}

