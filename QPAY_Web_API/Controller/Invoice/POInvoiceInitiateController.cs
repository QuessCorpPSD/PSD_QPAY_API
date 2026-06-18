using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.API.Models;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.Repository.Invoice;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using QPay.UI_Domain.Models.PurchaseOrder;
using System.Data;
using System.Xml.Serialization;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class POInvoiceInitiateController : ControllerBase
    {
        private readonly IPOInvoiceInitiateRepository _porepository;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;
        private readonly HttpClient _client;
        public POInvoiceInitiateController(ILoggerManager logger, HttpClient client, IPOInvoiceInitiateRepository porepository, IConfiguration configuration)
        {
            this._porepository = porepository;
            _configuration = configuration;
            this._logger = logger;
            this._client = client;
        }

        [HttpGet, Route("Search/{companyId}/{payPeriodId}")]
        public async Task<IActionResult> Search(int companyId, int payPeriodId)
        {
            var search = await this._porepository.Search(companyId, payPeriodId);
            return Ok(search);
        }

        [HttpGet, Route("POInvoiceRequest/{companyId}/{payPeriodId}/{flag}")]
        public async Task<IActionResult> POInvoiceRequest(int companyId, int payPeriodId, string flag)
        {
            var search = await this._porepository.POInvoiceRequest(companyId, payPeriodId, flag);
            if (search.Tables[0].Rows.Count > 0)
            {
                var _outputResponse = ResponseWrapManager.ResponseWrapper(search, HttpContext);
                return Ok(_outputResponse);
            }
            else
            {
                return Ok(new { StatusCode = "400", Message = "No records found" });
            }
        }

        [HttpPost, Route("POInvoiceInitiate")]
        public async Task<IActionResult> POInvoiceInitiate(POInvoiceInitiateRequest request)
        {
            string xml = XmlHelper2.SerializeObjectToXml(request);
            var result = await _porepository.POInvoiceInitiate(xml, request.CreatedBy);

            return Ok(result);
        }

        [HttpGet]
        [Route("POInvoiceInitiateExport/{companyId}/{payPeriodId}")]
        public IActionResult POInvoiceInitiateExport(int companyId, int payPeriodId)
        {

            DataSet ds = _porepository.POInvoiceInitiateExport(companyId, payPeriodId);
            if (ds.Tables.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("POInitiate");
                ws.Cell(1, 1).InsertTable(ds.Tables[0], "NewDataSet", true);

                var table = ws.Table("NewDataSet");
                table.ShowAutoFilter = false;
                table.Theme = XLTableTheme.None;

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());

                    FileResponse fileResponse = new FileResponse();
                    string fileName = DateTime.Now.ToString();
                    fileResponse.FileName = "POInitiate_Export_" + fileName + ".xlsx";
                    fileResponse.File = bytes;
                    return Ok(fileResponse);
                }
            }
            else
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "Failure",
                    Data = "",
                    Error = ""
                };
                return Ok(response);
            }

        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string userId, [FromForm] int importType)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            var result = await _porepository.Upload(file, userId);
            return Ok(result);
        }


        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet("NewDataSet");

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable("Table");
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
                                        .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }
        public static string ConvertToXml(List<BDE> bdeList)
        {
            var serializer = new XmlSerializer(typeof(List<DOJ>), new XmlRootAttribute("BillableDaysDocumentElement"));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, bdeList);
                return stringWriter.ToString();
            }
        }

    }
}
