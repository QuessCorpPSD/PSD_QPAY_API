using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.Models.GlobalMaster;
using System.Data;
using System.Xml.Linq;

namespace QPay.BAL.Repository.GlobalMaster
{

    public class SiteMasterRepository : ISiteMasterRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public SiteMasterRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? companyId, int? groupId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Companycode"] = companyId,
                ["@Group_Details_Id"] = groupId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetGroupMasterDetails", parameters); ;
        }


        public async Task<DataSet> GetQuessLegalEntity()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = "GetQuessLegalEntity",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_CommonDropDowns", parameters);

        }
        //public async Task<DataSet> Create(EntityRequest items)
        //{

        //    var parentdata = GenericSerializer<Entity>.Serialize(items.parentDetail);
        //    var childdata = GenericSerializer<EntityProfitCenter>.Serialize(items.ChildDetail);

        //    //Entity entity = JsonConvert.DeserializeObject<Entity>(parentdata);
        //    //var entityResponse = new EntityResponse();
        //    //entityResponse.EntityDetails = new Entity[1];
        //    //entityResponse.EntityDetails[0] = entity;

        //    //EntityProfitCenter[] entityProfitCenter = JsonConvert.DeserializeObject<EntityProfitCenter[]>(childdata);
        //    //var entityProfitCenterResponse = new EntityProfitCenterResponse();
        //    //entityProfitCenterResponse.EntityProfitCenterDetails = entityProfitCenter;

        //    //string entityResponseSerialize = GenericSerializer<EntityResponse>.Serialize(entityResponse);
        //    //string entityProfitCenterResponseSerialize = GenericSerializer<EntityProfitCenterResponse>.Serialize(entityProfitCenterResponse);
        //    //entityProfitCenterResponseSerialize = entityProfitCenterResponseSerialize == "<EntityProfitCenterResponse />" ? "<EntityProfitCenterResponse></EntityProfitCenterResponse>" : entityProfitCenterResponseSerialize;

        //    var parameters = new Dictionary<string, object>
        //    {
        //        ["@xmlInput"] = "",
        //        ["@xmlInputDetail"] = "",
        //        ["@mode"] = items.mode,
        //        ["@CreatedBy"] = items.createdBy,
        //    };
        //    return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateEntity", parameters);
        //}

        public async Task<DataSet> ExporttoExcel(int? companyId, int? groupId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Companycode"] = companyId,
                ["@Group_Details_Id"] = groupId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetGroupMasterDetailsExportToExcel", parameters); ;
        }

        public async Task<List<PortalPayslipFormatUI>> GetPortalPayslipFormat()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@pageName", "SiteMaster");

            var res = await this._dbRepository.GetItemsAsync("GetPortalPayslipFormat", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PortalPayslipFormatUI>>(res) ?? new List<PortalPayslipFormatUI>();
            }

            return new List<PortalPayslipFormatUI>();
        }

        public async Task<SiteMasterResponse> CreateUpdateSiteMaster(CreateUpdateSitemasterRequest request)
        {
            SiteMasterResponse responseDetails = new SiteMasterResponse();

            string xmlData = GenerateXmlSitemaster(request);

            string storeProcedure = "sp_CreateUpdateGroupMaster";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlData);
            parameters.Add("@mode", request.Action);
            parameters.Add("@CreatedBy", request.UserId);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    // Parse JSON array
                    var items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(res);

                    if (items != null && items.Count > 0)
                    {
                        var first = items[0];

                        if (first.ContainsKey("Error_Message"))
                        {
                            string msg = first["Error_Message"]?.ToString();

                            if (msg.Contains("Successfully") || msg.Contains("successfully"))
                            {
                                responseDetails.response = msg;   // success
                            }
                            else
                            {
                                responseDetails.response = msg;  // business error
                            }
                        }
                        // CASE 2: SQL/System Error (ErrorMessage)
                        else if (first.ContainsKey("ErrorMessage"))
                        {
                            string sqlError = first["ErrorMessage"]?.ToString();

                            responseDetails.response = "Failed to import due to system error.";
                            responseDetails.errors = new List<string> { sqlError };
                        }
                        else
                        {
                            responseDetails.response = "Failed. Unknown response format.";
                        }
                    }
                    else
                    {
                        responseDetails.response = "Failed. Empty result.";
                    }
                }
                catch
                {
                    responseDetails.response = "Error while processing response.";
                }
            }
            else
            {
                responseDetails.response = "Failed";
            }
            return responseDetails;
        }

        public string GenerateXmlSitemaster(CreateUpdateSitemasterRequest emp)
        {
            XElement xml = new XElement("main",

                // ---------- GroupMasterResponse ----------
                new XElement("GroupMasterResponse",
                    new XElement("GroupMaster",
                        new XElement("Company_Id", emp.Company_Id),
                        new XElement("Group_Detail_Id", emp.Group_Detail_Id),
                        new XElement("Group_Name", emp.Group_Name),
                        new XElement("Client_Id", emp.Client_Id),
                        new XElement("CostCenter_Id", emp.CostCenter_Id),
                        //new XElement("Branch_Name", ""),
                        //new XElement("LWW_Formula", ""),
                        //new XElement("ATB_Formula", ""),
                        //new XElement("Auth_OT_Formula", ""),
                        //new XElement("NFH_Formula", ""),
                        //new XElement("Unauthorized_OT", ""),
                        //new XElement("Additional_Formula_1", ""),
                        //new XElement("Additional_Formula_2", ""),
                        //new XElement("Additional_Formula_3", ""),
                        //new XElement("AROTHRS", ""),
                        //new XElement("ROTHRS", ""),
                        //new XElement("GRAT", ""),
                        //new XElement("City_Id", ""),
                        new XElement("Establishment_Name", emp.Establishment_Name),
                        new XElement("Establishment_Adress1", emp.Establishment_Adress1),
                        //new XElement("Establishment_Adress2", ""),
                        //new XElement("Establishment_Adress3", ""),
                        new XElement("Principal_Employer_Name", emp.Principal_Employer_Name),
                        new XElement("Principal_Employe_Address1", emp.Principal_Employe_Address1),
                        //new XElement("Principal_Employe_Address2", ""),
                        //new XElement("Principal_Employe_Address3", ""),
                        new XElement("Contractor_Name", emp.Contractor_Name),
                        new XElement("Contractor_Address1", emp.Contractor_Address1),
                        //new XElement("Contractor_Address2", ""),
                        //new XElement("Contractor_Address3", ""),
                        //new XElement("OPS_Manager", ""),
                        //new XElement("Site_Incharge", ""),
                        new XElement("PAYSLIP_FORMAT_Id", emp.PAYSLIP_FORMAT_Id),
                        new XElement("PAYSLIP_FORMAT", emp.PAYSLIP_FORMAT),
                        //new XElement("PROVISION_BONUS", ""),
                        //new XElement("Leave_Credit", ""),
                        //new XElement("Region", ""),
                        //new XElement("Po_HeadCount", ""),
                        new XElement("IsBonusPayThroughFF", emp.IsBonusPayThroughFF),
                        //new XElement("IsExtraWorkingDaysServiceFee", ""),
                        new XElement("LeaveApplicable", emp.LeaveApplicable),
                        //new XElement("CasualLeave", ""),
                        //new XElement("SickLeave", ""),
                        //new XElement("MainCustomerCode", ""),
                        new XElement("StartDate", emp.StartDate),
                        new XElement("SAP_Cust_Code", emp.SAP_Cust_Code),
                        new XElement("SAP_Cust_Name", emp.SAP_Cust_Name),
                        new XElement("WBS2", emp.WBS2),
                        new XElement("WBS_Name", emp.WBS_Name),
                        //new XElement("Flex1", ""),
                        //new XElement("Flex2", ""),
                        new XElement("SalaryDate", emp.SalaryDate),
                        new XElement("Portal_Payslip_Format", emp.Portal_Payslip_Format),
                        new XElement("Value", emp.Value),
                    //new XElement("PF_Code_Location", ""),
                    //new XElement("PF_ID", ""),
                    //new XElement("LEAVE_ID", ""),
                    //new XElement("LEAVE_TYPE_ID", ""),
                    //new XElement("PLE_Formula", ""),
                    //new XElement("GRTCT", "")
                    new XElement("Value", emp.Po_Salary),
                    new XElement("Value", emp.Po_OtherIncome)
                    )
                ),

                // ---------- GroupMasterDetailResponse ----------b
                new XElement("GroupMasterDetailResponse",
                    new XElement("GroupMasterDetails",
                        new XElement("Company_Id", emp.Company_Id),
                        new XElement("Group_Id", emp.Group_Id)
                    )
                )
            );

            return xml.ToString();
        }

        public async Task<SiteMasterResponse> UploadSiteMaster(IFormFile file, [FromForm] string User)
        {
            SiteMasterResponse sitemasterDetails = new SiteMasterResponse();
            string sFileName = "";
            string fileNameToDelete = "";

            if (file != null && file.Length != 0)
            {
                var DirName = Path.Combine(_configuration["ClaimDocPath"].ToString(), "SiteMaster");
                if (!Directory.Exists(DirName))
                {
                    Directory.CreateDirectory(DirName);
                }

                sFileName = file.FileName;
                sFileName = sFileName.Replace(" ", "");
                string Extension = Path.GetExtension(sFileName);

                string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(sFileName);
                string sfilewithExtension = FileNameWithoutExtension + Extension;
                fileNameToDelete = Guid.NewGuid().ToString() + sfilewithExtension;
                string excelPath = DirName + "\\" + fileNameToDelete;

                using (var stream = new FileStream(excelPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(excelPath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    sitemasterDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return sitemasterDetails;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    sitemasterDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return sitemasterDetails;
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

                string storeProcedure = "Proc_Upload_SiteMaster";
                var parameters = new DynamicParameters();

                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", User);
                parameters.Add("@Action", "SiteMasterUpload");

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) && message.Contains("Row(s) Uploaded Successfully."))
                        {
                            sitemasterDetails.response = message;
                        }
                        else
                        {
                            sitemasterDetails.response = "Failed to Import.";
                            sitemasterDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        sitemasterDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    sitemasterDetails.response = "Failed";
                }

            }
            else
            {
                sitemasterDetails.response = "File not found";
            }
            return sitemasterDetails;
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
