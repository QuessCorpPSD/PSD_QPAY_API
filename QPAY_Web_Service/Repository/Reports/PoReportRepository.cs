using ClosedXML.Excel;
using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using QPay.UI.GrossMargin;
using QPay.UI.Models;
using QPay.UI.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Reports
{
    public class PoReportRepository : IPoReportRepository
    {
        private readonly DbRepository _dbRepository;

        public PoReportRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<string> GetAllPOEmployeeReportNew(string employeeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EMP_ID", employeeId);

            var res = await this._dbRepository.GetItemsAsync("USP_PO_EMPLOYEEWISE_REPORT_NEW", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        //public async Task<>
        public async Task<string> GetAllPOEmployeeReportOld(string employeeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EMP_ID", employeeId);

            var res = await this._dbRepository.GetItemsAsync("USP_PO_EMPLOYEEWISE_REPORT_NEW", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> GetPOYears()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("USP_POACTIVE_GETYEARS", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public async Task<DataSet> GetGrossMarginReport(string pay_Period,int submit)
        {
            var parameters = new Dictionary<string, object?>
            {
                
                ["@Pay_Period"] = pay_Period,
                ["@Submit"] = submit
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GrossMarginReport", parameters, 1500);
        }
        public async Task<AccuralsModelResponse> AccuralFileupload(AccuralsModelRequest accuralsModelRequest)
        {
            AccuralsModelResponse accuralsModelResponse = new AccuralsModelResponse();
            var files = Convert.FromBase64String(accuralsModelRequest.File);
            using (var stream = new MemoryStream(files))
            using (var workbook = new XLWorkbook(stream))
            {
                var ws = workbook.Worksheet(1);
                var dt = new DataTable();

                // Columns
                foreach (var cell in ws.Row(1).Cells())
                {
                    dt.Columns.Add(cell.Value.ToString());
                }

                // Rows
                foreach (var row in ws.RowsUsed().Skip(1))
                {
                    var dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dr[i] = row.Cell(i + 1).Value.ToString();
                    }
                    dt.Rows.Add(dr);
                }
                
                using (StringWriter sw = new StringWriter())
                {
                    dt.TableName = "Rows";
                    dt.WriteXml(sw);
                    sw.ToString();
                    var parameter = new DynamicParameters();
                    parameter.Add("@CompanyId", accuralsModelRequest.CompanyId);
                    parameter.Add("@PayPeriod", accuralsModelRequest.PayPeriodId);
                    parameter.Add("@CreatedBy", accuralsModelRequest.CreatedBy);
                    parameter.Add("@XmlInput", sw.ToString());
                    var res =await _dbRepository.GetItemsAsync("SP_PROC_Accural_Upload_CompanyWise", parameter);
                    if(res.Any())
                    {
                        accuralsModelResponse = JsonConvert.DeserializeObject<List<AccuralsModelResponse>>(res).FirstOrDefault();
                    }
                    else
                    {
                        accuralsModelResponse.StatusCode = 201;
                        accuralsModelResponse.StatusMessage = "Accural upload failed";
                    }
                }

                
            }
            return accuralsModelResponse;
        }
        public async Task<FileResponse> AccuralFileFormat()
        {
            DataTable dataTable = new DataTable();
            FileResponse fileResponse = new FileResponse();
            var parameter = new DynamicParameters();
            var res = await _dbRepository.GetItemsAsync("SP_Accrual_Format_download", parameter);
            if (res.Any())
            {
                dataTable = JsonConvert.DeserializeObject<DataTable>(res);
            }
            if (dataTable.Rows.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add(dataTable, "InvoiceSummary");

                ws.Tables.First().ShowAutoFilter = false;
                ws.Tables.First().Theme = XLTableTheme.None;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);

                stream.Position = 0;
                fileResponse.File = Convert.ToBase64String(stream.ToArray());
                fileResponse.FileName = "Accurals_" + System.DateTime.Now.ToString("ddMMyyyyhh:ss") + ".xlsx";
            }
            else
            {
                fileResponse.File = "N";
                fileResponse.FileName = "Accurals_" + System.DateTime.Now.ToString("ddMMyyyyhh:ss") + ".xlsx";
            }
            return fileResponse;
        }

        public async Task<DataSet> GetUnProcessedGrossMarginReport(string pay_Period)
        {
            var parameters = new Dictionary<string, object?>
            {

                ["@Pay_Period"] = pay_Period
               
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("UnProcessed_GrossMargin_Payregister", parameters, 1500);
        }

        public async Task<string> GetVerticals(string userId, string poType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LOGGEDIN_USER", userId);
            parameters.Add("@PO_TYPE", poType);

            var res = await this._dbRepository.GetItemsAsync("USP_PO_GET_VERTICALS", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> POActiveReportGrid(POActiveInactive pOActiveInactive)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CLIENT_ID", pOActiveInactive.CompanyId);
            parameters.Add("@SITE_ID", pOActiveInactive.SiteId);
            parameters.Add("@ISACTIVE", pOActiveInactive.Isactive);
            parameters.Add("@Access_Company_Code", pOActiveInactive.CompanyCode);
            parameters.Add("@PO_TYPE", pOActiveInactive.PoType);
            parameters.Add("@YEAR", pOActiveInactive.PoYear);
            parameters.Add("@VERTICAL", pOActiveInactive.Vertical);
            parameters.Add("@LOGGEDIN_USER", pOActiveInactive.UserId);

            var res = await this._dbRepository.GetItemsAsync("USP_PO_GET_NEW_EMPACTIVE_REPORTDEATILS_EXPORT", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }


        public DataSet GetAllMonthWisePOReport(string txtFromDate, string txtToDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@STARTDT"] = txtFromDate,
                ["@ENDDT"] = txtToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_PO_EmployeeMonth_Report_1", parameters);
        }
    }
}
