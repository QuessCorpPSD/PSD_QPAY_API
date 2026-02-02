using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.UI.Invoice;
using QPay.UI.Models.Invoice;
using QPay.UI_Domain.Models.PurchaseOrder;
using System.Data;
using System.Xml.Serialization;

namespace QPay.API.Controller.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillableDaysController : ControllerBase
    {
        private readonly IBillableDaysRepository _billableDaysRepository;
        private readonly IConfiguration _configuration;
        public BillableDaysController(IConfiguration configuration, IBillableDaysRepository billableDaysRepository)
        {
            this._billableDaysRepository = billableDaysRepository;
            this._configuration = configuration;
        }
        [HttpPost, Route("BillableDaysUpload")]
        public async Task<IActionResult> BillableDaysUpload(IFormFile file, [FromForm] string userId, [FromForm] int importType)
        {
            if (file == null || file.Length == 0)
                return Ok("File is missing.");

            string DirName = "";

            DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
            DirName += "BillableDays";
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            string fileExtention = Path.GetExtension(file.FileName.ToUpper());
            string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
            FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;

            string serverpath = DirName + FileName;

            using (var stream = new FileStream(serverpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            DataSet ds = new DataSet("NewDataSet");
            ds = ExcelToDataSet(serverpath);
            //Convert dt to XML
            if (ds.Tables.Count == 0)

                return Ok("Excel sheet is empty or not formatted correctly.");

            // Convert DataTable to XML
            using var xmlWriter = new StringWriter();
            ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
            string xmlInput = xmlWriter.ToString();


            var response = await _billableDaysRepository.BillableDaysUpload(xmlInput, userId, importType);

            return Ok(response);
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
        public static DataTable ReadExcelToDataTable(string filePath, bool hasHeader = true)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // First sheet
                var dataTable = new DataTable();
                var firstRowUsed = worksheet.FirstRowUsed();
                var row = firstRowUsed.RowUsed();

                int columnCount = worksheet.LastColumnUsed().ColumnNumber();

                // Add columns
                foreach (var cell in row.Cells(1, columnCount))
                {
                    dataTable.Columns.Add(hasHeader ? cell.GetValue<string>() : $"Column {cell.Address.ColumnNumber}");
                }

                // Start reading after header if exists
                var firstDataRow = hasHeader ? row.RowBelow() : row;

                foreach (var dataRow in worksheet.Rows(firstDataRow.RowNumber(), worksheet.LastRowUsed().RowNumber()))
                {
                    var data = new object[columnCount];
                    for (int i = 0; i < columnCount; i++)
                    {
                        data[i] = dataRow.Cell(i + 1).Value;
                    }
                    dataTable.Rows.Add(data);
                }

                return dataTable;
            }
        }

        [HttpPost, Route("SearchDetails")]
        public async Task<IActionResult> SearchDetails(BillableDaysSearchRequestModel searchRequestModel)
        {

            string xml = string.Empty;
            xml = "<Main>" + "<Pay_Frequency_Id>" + searchRequestModel.Pay_Period_Id + "</Pay_Frequency_Id>" + "<Company_id>" + searchRequestModel.Company_Id + "</Company_id><Emp_Code> " + searchRequestModel.Employee_Code + " </Emp_Code></Main>";
            var search = await this._billableDaysRepository.SearchDetails("Search", xml);
            return Ok(search);
        }

        [HttpGet, Route("DownloadTemplate/{importType}")]
        public async Task<IActionResult> DownloadTemplate(int importType)
        {

            var downloadTemplate = await _billableDaysRepository.DownloadTemplate(importType);

            if (string.IsNullOrEmpty(downloadTemplate.File))
            {
                return NotFound("No data available to export.");
            }

            return Ok(downloadTemplate);
        }


        [HttpPost, Route("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(ExportToExcelModelRequest excelRequestModel)
        {
            try
            {
                if (excelRequestModel.Param == -1)
                {
                    return BadRequest("Invalid parameter.");
                }

                string xml = $@"
        <Main>
            <Pay_Frequency_Id>{excelRequestModel.Pay_Period_Id}</Pay_Frequency_Id>
            <Company_id>{excelRequestModel.Company_Id}</Company_id>
            <Emp_Code>{excelRequestModel.Employee_Code}</Emp_Code>
        </Main>";

                var BillableDaysExcel = await _billableDaysRepository.ExportToExcel(xml);

                if (string.IsNullOrEmpty(BillableDaysExcel.File))
                {
                    return NotFound("No data available to export.");
                }

                return Ok(BillableDaysExcel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while exporting the file: {ex.Message}");
            }
        }

    }
}
