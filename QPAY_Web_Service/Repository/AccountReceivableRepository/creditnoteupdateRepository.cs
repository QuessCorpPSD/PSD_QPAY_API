using Dapper;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.AccountReceivableMod.creditnoteupdatemodel;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class creditnoteupdateRepository : IcreditnoteupdateRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public creditnoteupdateRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> CreditNoteSearch(int CompanyId, string fromdate, string todate)
        {
            DateTime from;
            DateTime to;

            // ✅ Safe conversion
            if (!DateTime.TryParse(fromdate, out from))
                throw new Exception("Invalid From Date");

            if (!DateTime.TryParse(todate, out to))
                throw new Exception("Invalid To Date");

            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = CompanyId,
                ["@fromdate"] = from,
                ["@todate"] = to
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_SearchCreditNoteUpdate",
                parameters,
                1500
            );
        }

        public async Task<DataSet> CreditNoteExportToExcel(CreditNoteExport payload)
        {
            DateTime from = DateTime.Parse(payload.fromDate);
            DateTime to = DateTime.Parse(payload.toDate);

            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = Convert.ToInt32(payload.companyId),
                ["@fromdate"] = from,
                ["@todate"] = to
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_SearchCreditNoteUpdateDetail_ExportToExcel",
                parameters,
                1500
            );
        }



        public DataSet GetInvoiceDetail(int companyId, int invoiceId, int creditNoteId, string invoiceNumber, string pdfType)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@InvoiceId"] = invoiceId,
                ["@CreditNoteId"] = creditNoteId,
                ["@InvoiceNum"] = invoiceNumber,
                ["@PdfType"] = pdfType
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_GetCreditNotePdf",
                parameters,
                1500
            );

           
        }



        public async Task<CreditNoteUploadResponse> CreditNoteCancelUpload(IFormFile file, string User)
        {
            CreditNoteUploadResponse response = new CreditNoteUploadResponse();

            try
            {
          
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

           
                var basePath = _configuration["ClaimDocPath"];

                if (string.IsNullOrEmpty(basePath))
                {
                    response.response = "File path not configured.";
                    return response;
                }

                var dirPath = Path.Combine(basePath, "CreditNoteCancel");

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

             
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(dirPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                
                DataTable dt = ReadExcelToDataTable(filePath);

                if (dt.Rows.Count == 0)
                {
                    response.response = "Excel sheet is empty.";
                    return response;
                }

             
                dt.TableName = "Table";
                DataSet ds = new DataSet("NewDataSet");
                ds.Tables.Add(dt);

                string xmlInput;
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

             
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);  
                parameters.Add("@CreatedBy", User);

                string spName = "Proc_Upload_BulkCreditNoteCancel";

                var res = await _dbRepository.GetItemsAsync(spName, parameters);

        
                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success"))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response = "Upload completed with errors";
                        response.errors = new List<string> { res };
                    }
                }
                else
                {
                    response.response = "Failed";
                }
            }
            catch (Exception ex)
            {
                response.response = ex.Message;
            }

            return response;
        }
        private DataTable ReadExcelToDataTable(string filePath)
        {
            DataTable dt = new DataTable();

            using (var workbook = new ClosedXML.Excel.XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                            dt.Columns.Add(cell.ToString()?.Trim() ?? "");

                        firstRow = false;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;

                        foreach (var cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] =
                                cell.ToString()?.Trim() ?? "";
                            i++;
                        }
                    }
                }
            }

            return dt;
        }

        private string BuildCreditNoteXml(CreditNoteEditRequest request)
        {
            var sb = new StringBuilder();

            sb.Append("<CreditNoteDetails>");
            sb.Append("<CreditNote>");

            sb.AppendFormat("<CreditNote_No>{0}</CreditNote_No>", request.CreditNote.CreditNote_No ?? "");
            sb.AppendFormat("<Credit_Note_Type_Text>{0}</Credit_Note_Type_Text>", request.CreditNote.Credit_Note_Type_Text ?? "");
            sb.AppendFormat("<Invoice_Number>{0}</Invoice_Number>", request.CreditNote.Invoice_Number ?? "");

            // ✅ FIXED TAG NAME (VERY IMPORTANT)
            sb.AppendFormat("<Sap_Reference_Number>{0}</Sap_Reference_Number>", request.CreditNote.Sap_Reference_Number ?? "");

            sb.AppendFormat("<Credit_Note_Status>{0}</Credit_Note_Status>", request.CreditNote.Credit_Note_Status ?? "");

            sb.Append("</CreditNote>");
            sb.Append("</CreditNoteDetails>");

            return sb.ToString();
        }

        private string BuildEmployeeXml(List<CreditNoteEmployee> employees)
        {
            var sb = new StringBuilder();

            sb.Append("<CreditNoteDetails>");

            if (employees != null)
            {
                foreach (var item in employees)
                {
                    sb.Append("<CreditNote>");

                    sb.AppendFormat("<CreditNote_Id>{0}</CreditNote_Id>", item.CreditNote_Id);
                    sb.AppendFormat("<Employee_Code>{0}</Employee_Code>", item.Employee_Code ?? "");
                    sb.AppendFormat("<Ref_Id>{0}</Ref_Id>", item.Ref_Id ?? "");
                    sb.AppendFormat("<Credit_Note_Amount>{0}</Credit_Note_Amount>", item.Credit_Note_Amount);

                    // ✅ FIXED DATE FORMAT (VERY IMPORTANT)
                    sb.AppendFormat("<Credit_Note_Dates>{0}</Credit_Note_Dates>",
                        item.Credit_Note_Dates?.ToString("yyyy-MM-dd") ?? "");

                    sb.Append("</CreditNote>");
                }
            }

            sb.Append("</CreditNoteDetails>");

            return sb.ToString();
        }
        public async Task<string> EditCreditNote(CreditNoteEditRequest request)
        {
            if (request == null || request.CreditNote == null)
                return "Invalid request";

            var headerXml = BuildCreditNoteXml(request);
            var employeeXml = BuildEmployeeXml(request.CreditNoteDetails);

            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", headerXml);
            parameters.Add("@xmlEmployeeInput", employeeXml);
            parameters.Add("@Createdby", request.Created_By);
            parameters.Add("@mode", request.Mode);


            var result = await _dbRepository.GetItemsAsync(
                "sp_CreditNoteUpdateSaveAndCancel",
                parameters
            );

            return result;
        }

        public async Task<DataSet> CreditnoteEmployeeSearch(string creditNoteNo)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CreditNoteNo"] = creditNoteNo
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_SearchEmployeeCreditNoteUpdate",
                parameters,
                1500
            );
        }


    }
}