using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.DAL.Repository;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.SalaryReleaseInvoice
{
    public class ReissueProcessRepository : IReissueProcessRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public ReissueProcessRepository(
            DbRepository dbRepository,
            IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }

        // REPOSITORY

        public async Task<ReissueProcessReportResponse> ImportReissueProcess(
            IFormFile file,
            string createdBy)
        {
            ReissueProcessReportResponse response =
                new ReissueProcessReportResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

                DataTable dt = ReadExcelToDataTable(file);

                if (dt.Rows.Count == 0)
                {
                    response.response = "Excel sheet is empty.";
                    return response;
                }

                // XML FORMAT SAME AS MVC
                dt.TableName = "Table";

                DataSet ds = new DataSet("NewDataSet");
                ds.Tables.Add(dt);

                string xmlInput = "";

                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", createdBy);

                var res = await _dbRepository.GetItemsAsync(
                    "Upload_DeleteBankRejection",
                    parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success"))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response = "Failed to Import.";

                        response.errors = res?
                            .Split(
                                new[] { '\r', '\n' },
                                StringSplitOptions.RemoveEmptyEntries
                            )
                            .ToList()
                            ?? new List<string> { "Unknown error." };
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

        private DataTable ReadExcelToDataTable(IFormFile file)
        {
            DataTable dt = new DataTable();

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheet(1);

            bool firstRow = true;

            foreach (var row in worksheet.RowsUsed())
            {
                if (firstRow)
                {
                    foreach (var cell in row.Cells())
                    {
                        dt.Columns.Add(
                            cell.Value.ToString().Trim()
                        );
                    }

                    firstRow = false;
                }
                else
                {
                    dt.Rows.Add();

                    int i = 0;

                    foreach (var cell in row.Cells())
                    {
                        dt.Rows[dt.Rows.Count - 1][i] =
                            cell.Value.ToString().Trim();

                        i++;
                    }
                }
            }

            return dt;
        }


        public DataSet ReissueProcessReportExportToExcel(
            CommonExport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@fromdate"] = payload.fromdate,
                ["@todate"] = payload.todate,
                ["@Status"] = payload.status
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Sp_ReissueProcessReport_ExportToExcel",
                parameters,
                1500);
        }

        public DataSet ReissueProcessSearch(string fromdate, string todate, string status)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@fromdate"] = fromdate,
                ["@todate"] = todate,
                ["@Status"] = status
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_ReissueProcessReport", parameters );
        }
    }
}
