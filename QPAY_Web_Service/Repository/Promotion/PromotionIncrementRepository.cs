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
using QPay.UI.Models.TaxAndSaving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository
{
    public class PromotionIncrementRepository : IPromotionIncrementRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public PromotionIncrementRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

       

        public async Task<DataSet> GetAllPayPeriodByCompanyID(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPaySequenceByCompanyID", parameters, 1500);
        }

        public async Task<DataSet> GetEmployeeDetailsByCompanyID(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = companyId,
                ["@EmployeeID"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllEmployeeByCompanyID", parameters, 1500);
        }

        public async Task<DataSet> GetAllIncrementDetails(int? companyId, int? employeeId, int? payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = companyId,
                ["@EmployeeID"] = employeeId,
                ["@Pay_Frequency_Detail_Id"] = payPeriodId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllIncrementDetailsByCIDandEID", parameters, 1500);
        }

        public async Task<DataSet> GetAllIncrementDetailsByIncrementID(int? incrementId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@IncrementID"] = incrementId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllIncrementDetailsById", parameters, 1500);
        }

        public async Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy)
        {
            RequestResponse poDetails = new RequestResponse();

            if (file != null && file.Length != 0)
            {
                string DirName = "";

                DirName = Path.Combine(_configuration["ClaimDocPath"].ToString());
                string Code = "Increment";
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

                string storeProcedure = "Proc_Upload_Increment1";
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


        //public async Task<DataSet> GetEligibleEmployee(int? financialYearId, int? EmployeeId)
        //{
        //    var parameters = new Dictionary<string, object?>
        //    {
        //        ["@Financial_Year_Id"] = financialYearId,
        //        ["@Employee_Id"] = EmployeeId,
        //    };
        //    return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetEligibleAmountbyEmployeeID", parameters, 1500);
        //}

        //public async Task<DataSet> GetEligibleChildren(string Effective_Date, int Number_Of_Children)
        //{
        //    var parameters = new Dictionary<string, object?>
        //    {
        //        ["@Effective_Date"] = Effective_Date,
        //        ["@Number_Of_Children"] = Number_Of_Children,
        //    };
        //    return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetEligibleAmountbyFinancialYearNoOfChildren", parameters, 1500);
        //}

        //public async Task<DataSet> Search(int? companyId, int? financialYearId, int? EmployeeId)
        //{
        //    var parameters = new Dictionary<string, object?>
        //    {
        //        ["@CompanyId"] = companyId,
        //        ["@FinancialYearId"] = financialYearId,
        //        ["@EmployeeId"] = EmployeeId,
        //    };
        //    return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spGetAllChildrenEducationAllowance", parameters, 1500);
        //}

        //public async Task<DataSet> Create(ChildrenEducationAllowanceRequest items)
        //{
        //    string parentdata = JsonConvert.SerializeObject(items.parentDetail);
        //    string childdata = JsonConvert.SerializeObject(items.childDetail);

        //    ChildrenEducationAllowance objChildrenEducationAllowance = JsonConvert.DeserializeObject<ChildrenEducationAllowance>(parentdata);
        //    ChildrenEducationAllowanceDetail[] objChildrenEducationAllowanceDetail = JsonConvert.DeserializeObject<ChildrenEducationAllowanceDetail[]>(childdata);
        //    var ChildrenEducationAllowanceDetailResponse = new ChildrenEducationAllowanceResponse();
        //    ChildrenEducationAllowanceDetailResponse.ChildrenEducationAllowance = new ChildrenEducationAllowance[1];
        //    ChildrenEducationAllowanceDetailResponse.ChildrenEducationAllowance[0] = objChildrenEducationAllowance;
        //    string resultMessage = string.Empty;
        //    var objChildrenEducationAllowanceResponse2 = new ChildrenEducationAllowanceDetailResponse();
        //    objChildrenEducationAllowanceResponse2.ChildrenEducationAllownceDetails = objChildrenEducationAllowanceDetail;
        //    string ChildrenEducationAllowanceResponseSerialize = GenericSerializer<ChildrenEducationAllowanceResponse>.Serialize(ChildrenEducationAllowanceDetailResponse);
        //    string ChildrenEducationAllowanceResponseDetailSerialize = GenericSerializer<ChildrenEducationAllowanceDetailResponse>.Serialize(objChildrenEducationAllowanceResponse2);
        //    ChildrenEducationAllowanceResponseSerialize = ChildrenEducationAllowanceResponseSerialize == "<ChildrenEducationAllowanceDetailResponse/>" ? "<ChildrenEducationAllowanceDetailResponse></ChildrenEducationAllowanceDetailResponse>" : ChildrenEducationAllowanceResponseSerialize;


        //    var parameters = new Dictionary<string, object>
        //    {
        //        ["@xmlInput"] = ChildrenEducationAllowanceResponseSerialize,
        //        ["@xmlInputDetail"] = ChildrenEducationAllowanceResponseDetailSerialize,
        //        ["@mode"] = items.mode,
        //        ["@CreatedBy"] = items.createdBy,
        //    };
        //    return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateChildrenEducationAllowance", parameters);
        //}

    }
}
