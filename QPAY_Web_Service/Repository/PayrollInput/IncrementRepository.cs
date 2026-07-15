using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Common;
using QPay.DAL.Repository;
using QPay.IRepository.iRepository.PayrollInput;
using QPay.UI.Common;
using System.Data;
using static QPay.DTo.Models.PayrollInput.Increment;
//using static QZone.DTo.Models.Common.ActivationLwd;

namespace QPay.IRepository.Repository.PayrollInput
{
    public class IncrementRepository : IIncrementRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public IncrementRepository(DbRepository dbRepository, ICommonRepository icommon, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._icommon = icommon;
            _configuration = configuration;
        }


        public DataSet GetEmployeeIncrement(int companyId, int payPeriodId, int InputType, int MapNameId )
        {
            DataSet ds = this._dbRepository.GetEmployeeIncrementDataSet(companyId, payPeriodId,  InputType, MapNameId);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }

        }
        public async Task<IncrementResponse> UploadIncrementData(IFormFile file, [FromForm] string User,
           [FromForm] string companyCode, [FromForm] int companyId, [FromForm] int InputType)
        {
            IncrementResponse incrementDetails = new IncrementResponse();

            if (file != null && file.Length != 0)
            {
                List<PayperiodDD> payperiod = new List<PayperiodDD>();
                payperiod = _icommon.GetCurrentPayperiod(companyId);
                int payPeriodId = 0;
                string payPeriod = string.Empty;

                if (payperiod != null && payperiod.Any())
                {
                    payPeriodId = payperiod[0].Payfrequencyid;
                    payPeriod = payperiod[0].PayPeriod;
                }
                else
                {
                    incrementDetails.response = "No pay period found for the given company ID.";
                    return incrementDetails;
                }

                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Invoice", "Increment");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Increment_{companyCode}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    incrementDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return incrementDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "PROC_QZONE_EXCEL_UPLOAD_INCREMENT";
                var parameters = new DynamicParameters();

                parameters.Add("@Company_Id", companyId);
                parameters.Add("@PayPeriod_Id", payPeriodId);
                parameters.Add("@User", User);
                parameters.Add("@xml", xmlInput);
                parameters.Add("@InputType", InputType);                

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res) && res.Contains("Rows Inserted Successfully", StringComparison.OrdinalIgnoreCase))
                        {
                            incrementDetails.response = res;
                        }
                        else
                        {
                            incrementDetails.response = "Failed to import.";
                            incrementDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        incrementDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    incrementDetails.response = "Failed";
                }

            }
            else
            {
                incrementDetails.response = "File not found";
            }
            return incrementDetails;
        }

        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable(worksheet.Name);
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


    }




}
