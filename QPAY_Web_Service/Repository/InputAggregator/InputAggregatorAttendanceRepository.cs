using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Aggregator;
using QPay.UI.Models.TaxAndSaving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository
{
    public class InputAggregatorAttendanceRepository : IInputAggregatorAttendanceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public InputAggregatorAttendanceRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> QuessLeaveMaster()
        {
            var parameters = new Dictionary<string, object?>
            {
                
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Quess_Leave_Master", parameters, 1500);
        }

        public async Task<DataSet> leaveTypeMaster()
        {
            var parameters = new Dictionary<string, object?>
            {
                
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Leave_Type_Master", parameters, 1500);
        }

        public async Task<DataSet> Createleavemapping(AttendanceAggregatorRequest items)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Leave_mapping_detail_Id"] = items.parentDetail.LEAVE_MAPPING_DETAIL_ID,
                ["@Company_Id"] = items.parentDetail.COMPANY_ID ,
                ["@Leave_Type_Id"] = items.parentDetail.LEAVE_TYPE_ID,
                ["@Leave_Treat_Id"] = items.parentDetail.LEAVE_TREAT_ID,
                ["@IsActive"] = items.parentDetail.ISACTIVE,
                ["@Header_Count"] = items.parentDetail.ATTENDANCE_TYPE,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_LEAVE_TYPE_MAPPING_DETAIL", parameters);
        }

        public async Task<DataSet> SearchLeaveTypeMapping(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@companyId"] = companyId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Leave_Type_Master_mapping_detail", parameters, 1500);
        }

        public async Task<DataSet> Createleavetype(leaveTypeMasterRequest items)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Leave_Type_Id"] = items.parentDetail.LEAVE_TYPE_ID,
                ["@Leave_Type_Name"] = items.parentDetail.LEAVE_TYPE_NAME,
                ["@IsActive"] = items.parentDetail.ISACTIVE,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_LEAVE_TYPE_DETAIL", parameters);
        }
       

        public async Task<DataSet> QuessAttendanceAttributeMaster()
        {
            var parameters = new Dictionary<string, object?>
            {
               // ["@Company_Id"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Quess_Attendance_Attributes_Master", parameters, 1500);
        } 

        public async Task<RequestResponse> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "ClientAttributes";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Upload_Client_Attendance_Attributes";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }

        public async Task<RequestResponse> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "AttributesMapping";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Upload_Quess_Client_Attendance_Attributes_mapping";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
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

        public async Task<DataSet> Search(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Quess_Client_Attendance_Attributes_Mapping_detail", parameters, 1500);
        }

        public async Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "InputAggregator";
                DirName += Code.ToString();
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                string fileExtention = Path.GetExtension(file.FileName.ToUpper());
                string FileName = Path.GetFileNameWithoutExtension(file.FileName.ToUpper());
                FileName += DateTime.Now.ToString("_yyyyMMddhhmmssffff") + fileExtention;
                //string serverpath = ConfigurationManager.AppSettings["ClaimDocPath"] + FileName;
                string serverpath = DirName + "/" + FileName;

                using (var stream = new FileStream(serverpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(serverpath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                var parameters = new Dictionary<string, object?>
                {
                    ["@companyId"] = CompanyId
                };

                DataSet ListDs = this._dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Get_Client_Attendance_Attributes", parameters, 1000);
                DataTable dt=new DataTable ();
                dt= ListDs.Tables[0];
                List<string> selectedColumns = dt.AsEnumerable().Select(r => r.Field<string>("Template_Field_Name")).ToList();

                DataTable result = new DataTable();
                foreach (string col in selectedColumns)
                {
                    for(int i=0;i< dtToSerilize.Columns.Count;i++)
                    {
                        if (dtToSerilize.Columns[i].ColumnName.Contains(col))
                        {
                            result.Columns.Add(col, dtToSerilize.Columns[col].DataType);
                        }
                    }
                }

                foreach (DataRow row in dtToSerilize.Rows)
                {
                    DataRow newRow = result.NewRow();
                    foreach (string col in selectedColumns)
                    {
                        newRow[col] = row[col];
                    }
                    result.Rows.Add(newRow);
                }
                DataSet dscolumns = new DataSet();
                dscolumns.Tables.Add(result);
                for(int i = 0; i < dscolumns.Tables[0].Columns.Count; i++)
                {
                    dscolumns.Tables[0].Columns[i].ColumnName = "A_" + dscolumns.Tables[0].Columns[i].ColumnName.Replace("/", "-");
                }

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();

                dscolumns.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                // string xmlInput = xmlWriter.ToString();
                string xmlInput = xmlWriter.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "/").Replace("_x0029_", "").
                    Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "");
                //    .Replace("_x0031_", "1").Replace("_x0032_", "2").Replace("_x0030_", "0").Replace("_x0033_", "3");
                //xmlInput = xmlInput.Replace("/", "-");


                string storeProcedure = @"Proc_Upload_Attendance_InputAggregator";
                var parameter = new DynamicParameters();
                parameter.Add("@XML_File", xmlInput);
                parameter.Add("@CreatedBy", CreatedBy);
                parameter.Add("@Company_Id", CompanyId);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Successfully"))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }
            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }

        public async Task<DataSet> billableReport(int? companyId, int? payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_Search_InputAttendance_Aggregator_detail", parameters, 1500);
        }

       

    }
}
