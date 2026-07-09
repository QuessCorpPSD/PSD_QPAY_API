using Azure.Core;
using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.DAL.Repository;
using QPay.UI.Models.SalaryReleaseInvoice;
using System.Collections;
using System.Data;
using System.Security;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace QPay.BAL.Repository.SalaryReleaseInvoice
{
    public class SalaryRequestInvoiceRepository : ISalaryRequestInvoiceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        #region Salary Request start
        public SalaryRequestInvoiceRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }     
        public async Task<List<BankAdvice>> GetBankAdviceApproveList(InvoiceCommon SRInvoiceCommon)
        {
            const string storedProcedure = "[dbo].[getInvoiceNoForSalaryreleaserequest_New]";

            var parameter = new DynamicParameters();
            parameter.Add("@CompanyID", SRInvoiceCommon.Company_Id);
            parameter.Add("@PayPeriodID", SRInvoiceCommon.Pay_Period_Id);
            parameter.Add("@mode", SRInvoiceCommon.Action);
            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BankAdvice>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<BankAdvice>>(res);
                return list?.ToList() ?? new List<BankAdvice>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<BankAdvice>();
            }

        }
        public async Task<List<ErrorMessage>> CreateRequestSalaryRelease(BankAdviceApprovalRequest approvals)
        {
            const string storedProcedure = "[dbo].[CreateRequestSalaryRelease]";

            var parameter = new DynamicParameters();

            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<BankAdviceApprovalsResponse>");

            foreach (var approval in approvals.requestdata)
            {
                xmlBuilder.Append("<BankAdviceApprovals>");
                xmlBuilder.AppendFormat("<Invoice_No>{0}</Invoice_No>", approval.Invoice_No);
                xmlBuilder.AppendFormat("<Net_Pay>{0}</Net_Pay>", approval.Net_Pay);
                xmlBuilder.Append("</BankAdviceApprovals>");
            }

            xmlBuilder.Append("</BankAdviceApprovalsResponse>");

            string resultXml = xmlBuilder.ToString();

            parameter.Add("@Company_id", approvals.Company_id);
            parameter.Add("@Pay_Period_id", approvals.Pay_Period_id);
            parameter.Add("@CreatedBy", approvals.CreatedBy);
            parameter.Add("@Mode", approvals.Mode);
            parameter.Add("@Bank_Advice_Approvals_Id", approvals.Bank_Advice_Approvals_Id);
            parameter.Add("@QZoneUserName", approvals.QZoneUserName);
            parameter.Add("@requestdata", resultXml);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<ErrorMessage>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<ErrorMessage>>(res);
                return list?.ToList() ?? new List<ErrorMessage>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<ErrorMessage>();
            }

        }

        public async Task<List<ErrorMessage>> UploadSalaryReleaseRequest(BankAdviceRequest rdata)
        {
            const string storedProcedure = "Proc_Upload_SalaryReleaseRequest";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(rdata.InvoiceList, "NewDataSet", "Table");           
            parameter.Add("@QZoneUserName", rdata.QZoneUserName);
            parameter.Add("@xml", xml);
            parameter.Add("@CreatedBy", 3);
            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<ErrorMessage>
                {
                    new ErrorMessage
                    {
                        Error_Message = "Invalid response "
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<ErrorMessage>>(res);
                return list?.ToList() ?? new List<ErrorMessage>();
            }
            catch (JsonException ex)
            {

                //return new List<HoldRequestMessage>();
                return new List<ErrorMessage>
                  {
                    new ErrorMessage
                      {
                        Error_Message = ex.Message
                      }
                 };
            }

        }    
        public DataSet SalaryReleaseTemplate(string Flag, string QZoneUserName)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = Flag,
                ["@QZoneUserName"] = QZoneUserName                
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_Get_salary_Related_Template", parameters);
        }
        #endregion Salary Request end

        #region SalaryHold Request start
        public DataSet InvoiceHoldList(SalaryHoldCommon Data)
        {
            
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Data.Company_Id,
                ["@Pay_Period_Id"] = Data.Pay_Period_Id,
                ["@QZoneUserName"] = Data.QZoneUserName
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_BankInvoiceHoldListAll", parameters);
        }
        public async Task<List<HoldRequestMessage>> HoldRequestUpload(HoldSalaryRequest payload)
        {
            const string storedProcedure = "[dbo].[Upload_UpdateInvoiceHoldEmployeeSalary]";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.requestdata, "NewDataSet", "Table" );     
         
            parameter.Add("@Createdby", payload.QZoneUserName);
            parameter.Add("@xml", xml);          

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<HoldRequestMessage> 
                {
                    new HoldRequestMessage
                    {
                        Validation = "Invalid response"
                    }
                  }; 
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<HoldRequestMessage>>(res);
                return list?.ToList() ?? new List<HoldRequestMessage>();
            }
            catch (JsonException ex)
            {
                
                //return new List<HoldRequestMessage>();
                return new List<HoldRequestMessage>
                  {
                    new HoldRequestMessage
                      {
                        Validation = ex.Message
                      }
                 };
            }

        }

        public  DataSet SingleHoldRequest(SingleHoldRequest payload)        
        {

            string xml = ConvertWithDynamicRoot(payload.HoldListData, "NewDataSet", "Table");

            var parameter = new Dictionary<string, object?>
            {
                ["@Createdby"] = payload.QZoneUserName,
                ["@xml"] = xml

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Single_UpdateInvoiceHoldEmployeeSalary", parameter,0);

        }
        #endregion SalaryHold Request end

        #region SalaryHoldReelase Request start

        public async Task<List<HoldReleaseSalary>> InvoiceHoldReleaseList(SalaryHoldReleaseCommon Data)
        {
            const string storedProcedure = "[dbo].[sp_GetAllInvoice_ReleaseEmployeeSalaryDetail]";

            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", Data.Company_Id);
            parameter.Add("@Pay_Period_Id", Data.Pay_Period_Id);
            parameter.Add("@Employee_Id", Data.Employee_Id);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<HoldReleaseSalary>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<HoldReleaseSalary>>(res);
                return list?.ToList() ?? new List<HoldReleaseSalary>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<HoldReleaseSalary>();
            }

        }

        public async Task<List<HoldReleaseSalary>> InvoiceHoldReleaseListExport(SalaryHoldReleaseCommon Data)
        {
            const string storedProcedure = "[dbo].[sp_GetAllInvoice_ReleaseEmployeeSalaryDetailExportToExcel]";

            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", Data.Company_Id);
            parameter.Add("@Pay_Period_Id", Data.Pay_Period_Id);
            parameter.Add("@Employee_Id", Data.Employee_Id);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<HoldReleaseSalary>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<HoldReleaseSalary>>(res);
                return list?.ToList() ?? new List<HoldReleaseSalary>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<HoldReleaseSalary>();
            }

        }

        public async Task<List<HoldReleaseMessage>> HoldReleaseRequest(HoldReleaseRequest payload)
        {
            const string storedProcedure = "[dbo].[USP_ReleaseInvoiceHoldEmployeeSalaryStatus]";

            var parameter = new DynamicParameters();
            string xml = ConvertWithDynamicRoot(payload.HoldReleaseList, "NewDataSet", "Table");
            //var xmlBuilder = new StringBuilder();
            //xmlBuilder.Append("<NewDataSet>");

            //foreach (var data in payload.HoldReleaseList)
            //{
            //    xmlBuilder.Append("<Table>");
            //    xmlBuilder.AppendFormat("<Company_Code>{0}</Company_Code>", data.Company_Code);
            //    xmlBuilder.AppendFormat("<Employee_Code>{0}</Employee_Code>", data.Employee_Code);
            //    xmlBuilder.AppendFormat("<PayPeriod>{0}</PayPeriod>", data.PayPeriod);
            //    xmlBuilder.AppendFormat("<InvNo>{0}</InvNo>", data.InvNo);
            //    xmlBuilder.AppendFormat("<SalaryType>{0}</SalaryType>", data.SalaryType);
            //    xmlBuilder.AppendFormat("<ProvisionalInvoiceNumber>{0}</ProvisionalInvoiceNumber>", data.ProvisionalInvoiceNumber);
            //    xmlBuilder.Append("</Table>");
            //}

            //xmlBuilder.Append("</NewDataSet>");

            //string resultXml = xmlBuilder.ToString();

            parameter.Add("@Action", "ReleaseEmployeeSalaryUpload");
            parameter.Add("@CreatedBy", 3);
            parameter.Add("@QZoneUserName", payload.QZoneUserName);
            parameter.Add("@XML_File", xml);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<HoldReleaseMessage>
                {
                    new HoldReleaseMessage
                    {
                        Error_Message = "Invalid response"
                    }
                  };
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<HoldReleaseMessage>>(res);
                return list?.ToList() ?? new List<HoldReleaseMessage>();
            }
            catch (JsonException ex)
            {
                
                return new List<HoldReleaseMessage>
                {
                    new HoldReleaseMessage
                    {
                        Error_Message = ex.Message
                    }
                  };
            }

        }

        #endregion SalaryHold Request end

        #region partila hold and release start

        public async Task<List<PartialHoldMessage>> PartialHoldRequest(PartilHoldRequest payload)
        {
            const string storedProcedure = "[dbo].[Proc_Upload_PartialHoldEmployeeSalary]";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.PartialHoldList, "NewDataSet", "Table");

            parameter.Add("@XML_File", xml);
            parameter.Add("@CreatedBy", payload.QZoneUserName);
            parameter.Add("@UploadType", "Partial Hold Salary");

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<PartialHoldMessage>
                {
                    new PartialHoldMessage
                    {
                        Error_Message = "Invalid response "
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<PartialHoldMessage>>(res);
                return list?.ToList() ?? new List<PartialHoldMessage>();
            }
            catch (JsonException ex)
            {

                 return new List<PartialHoldMessage>
                  {
                    new PartialHoldMessage
                      {
                        Error_Message = ex.Message
                      }
                 };
            }

        }

        public async Task<List<PartialHoldMessage>> PartialHoldRelease(PartialRelease payload)
        {
            const string storedProcedure = "Proc_Upload_PartialHoldEmployeeSalary";
            string xml = ConvertWithDynamicRoot(payload.PartialReleaseList, "NewDataSet", "Table");
            var parameter = new DynamicParameters();

            parameter.Add("@XML_File", xml);
            parameter.Add("@CreatedBy", payload.QZoneUserName);
            parameter.Add("@UploadType", "Partial Hold Salary Release");

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<PartialHoldMessage>
                {
                    new PartialHoldMessage
                    {
                        Error_Message = "Invalid response "
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<PartialHoldMessage>>(res);
                return list?.ToList() ?? new List<PartialHoldMessage>();
            }
            catch (JsonException ex)
            {

                return new List<PartialHoldMessage>
                  {
                    new PartialHoldMessage
                      {
                        Error_Message = ex.Message
                      }
                 };
            }

        }
    
        #endregion partila hold and release end

        #region DBT hold and release start

        public async Task<List<DBTHoldMessage>> DBTHoldRequest(DBTHoldRequest payload)
        {
            const string storedProcedure = "[dbo].[Proc_Upload_PartialHoldEmployeeSalary]";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.DBTHoldList, "NewDataSet", "Table");

            parameter.Add("@XML_File", xml);
            parameter.Add("@CreatedBy", payload.QZoneUserName);
            parameter.Add("@UploadType", "DBT Hold Salary");

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<DBTHoldMessage>
                {
                    new DBTHoldMessage
                    {
                        Error_Message = "Invalid response "
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<DBTHoldMessage>>(res);
                return list?.ToList() ?? new List<DBTHoldMessage>();
            }
            catch (JsonException ex)
            {

                return new List<DBTHoldMessage>
                  {
                    new DBTHoldMessage
                      {
                        Error_Message = ex.Message
                      }
                 };
            }

        }

        public async Task<List<DBTHoldMessage>> DBTHoldRelease(DBTRelease payload)
        {
            const string storedProcedure = "Proc_Upload_PartialHoldEmployeeSalary";

            var parameter = new DynamicParameters();
            string xml = ConvertWithDynamicRoot(payload.DBTReleaseList, "NewDataSet", "Table");         

            parameter.Add("@XML_File", xml);
            parameter.Add("@CreatedBy", payload.QZoneUserName);
            parameter.Add("@UploadType", "Partial Hold Salary Release");

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<DBTHoldMessage>
                {
                    new DBTHoldMessage
                    {
                        Error_Message = "Invalid response "
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<DBTHoldMessage>>(res);
                return list?.ToList() ?? new List<DBTHoldMessage>();
            }
            catch (JsonException ex)
            {

                return new List<DBTHoldMessage>
                  {
                    new DBTHoldMessage
                      {
                        Error_Message = ex.Message
                      }
                 };
            }

        }
        #endregion DBT hold and release end

        #region netpay summary start

        public DataSet InvoiceNetPaysummary(int Company_Id, int Pay_Period_Id, string QZoneUserName)
        {

            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_Id,
                ["@Pay_Period_Id"] = Pay_Period_Id,
                ["@QZoneUserName"] = QZoneUserName,
                ["@Action"] = "INVOICESUMMARY"

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_InvoiceNetPaySummary", parameters);
        }

        public DataSet InvoiceWiseAssociateHoldList(int Company_Id, int Pay_Period_Id, string Flag, string Invoice_No, string QZoneUserName)
        {

            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_Id,
                ["@Pay_Period_Id"] = Pay_Period_Id,
                ["@Invoice_No"] = Invoice_No,
                ["@QZoneUserName"] = QZoneUserName,
                ["@Action"] = Flag

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_InvoiceNetPaySummary", parameters);
        }
        public DataSet NetPaysummary(int Company_Id, int Pay_Period_Id, string QZoneUserName)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@PayPeriodID"] = Pay_Period_Id,
                //["@QZoneUserName"] = QZoneUserName,
                ["@CompanyId"] = Company_Id,

                 
            };
            DataSet ds=new DataSet();
            ds=_dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_NetPaySummary", parameters, 0);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }

            
        }
        #endregion netpay summary end

        #region Common drop down start
        public List<CommonDropDown> GetCommonDropDownList(string Flag, string QZoneUserName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", Flag);
            parameters.Add("@QZoneUserName", QZoneUserName);
                
            var res = this._dbRepository.GetItemsAsync("sp_Get_salary_Related_Template", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<CommonDropDown>>(res) ?? new List<CommonDropDown>();
            }

            return new List<CommonDropDown>();
        }

        #endregion Common drop down end

        #region Support methods
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

        public static string ConvertWithDynamicRoot<T>(IEnumerable<T> list, string rootName, string tableName)
        {
            var root = new XElement(rootName);

            foreach (var item in list)
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StringWriter();
                serializer.Serialize(writer, item);

                var doc = XDocument.Parse(writer.ToString());
                root.Add(new XElement(tableName, doc.Root.Elements()));
            }

            return root.ToString();
        }
        public static string ConvertWithDynamicRoot1<T>(T obj, string rootName, string tableName)
        {
            var serializer = new XmlSerializer(typeof(T));
            XDocument doc;

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                doc = XDocument.Parse(writer.ToString());
            }

            var newRoot = new XElement(rootName);

            // If list → multiple Table nodes
            if (obj is IEnumerable && !(obj is string))
            {
                foreach (var element in doc.Root.Elements())
                {
                    newRoot.Add(
                        new XElement(tableName, element.Elements())
                    );
                }
            }
            // If single object → still one Table node
            else
            {
                newRoot.Add(
                    new XElement(tableName, doc.Root.Elements())
                );
            }

            return newRoot.ToString();
        }
        public static string ConvertWithDynamicRoot2<T>( T obj, string rootName,string tableName)
        {
            // Serialize object first
            var serializer = new XmlSerializer(typeof(T));
            XDocument doc;

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                doc = XDocument.Parse(writer.ToString());
            }

            // Remove default root
            XElement dataNode = doc.Root;

            // Create dynamic structure
            var newDoc = new XDocument(
                new XElement(rootName,
                    new XElement(tableName, dataNode.Elements())
                )
            );

            return newDoc.ToString();
        }

        #endregion Support methods

        #region Bonus flush out start

        public DataSet BonusDetailsSummary(int Company_Id, string FromDate, string ToDate, string QZoneUserName)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_Id,
                ["@From_Date"] = FromDate,
                ["@To_Date"] = ToDate,
                ["@Action"] = "SUMMARY",

            };
            DataSet ds = new DataSet();
            ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Bonus_Accumated_Report", parameters, 0);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }


        }
        public DataSet BonusAccumatedReport(int Company_Id,string FromDate,string ToDate,string QZoneUserName)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_Id,
                ["@From_Date"] = FromDate,
                ["@To_Date"] = ToDate,
                //["@QZoneUserName"] = QZoneUserName,

            };
            DataSet ds = new DataSet();
            ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Bonus_Accumated_Report", parameters, 0);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }


        }

        public async Task<List<BonusErrorMessage>> BonusReleaseUpload(BonusReleaseRequest payload)
        {
            const string storedProcedure = "Proc_Upload_PartialHoldEmployeeSalary";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.BonusReleaseList, "NewDataSet", "Table");

            parameter.Add("@Createdby", payload.QZoneUserName);
            parameter.Add("@XML_File", xml);
            parameter.Add("@UploadType", "Bonus Release");
          
            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BonusErrorMessage>
                {
                    new BonusErrorMessage
                    {
                        Error_Message = "Invalid response"
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<BonusErrorMessage>>(res);
                return list?.ToList() ?? new List<BonusErrorMessage>();
            }
            catch (JsonException ex)
            {

                //return new List<HoldRequestMessage>();
                return new List<BonusErrorMessage>
                  {
                    new BonusErrorMessage
                      {
                        Error_Message = ex.Message
                      }
                 };
            }

        }


        #endregion Bonus flush out end

        #region Deduction FlasuOut start

        public DataSet DeductionFlasuOutSearch(int Company_Id, int Pay_Period_Id, string QZoneUserName)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_Id,
                ["@PayPeriod_Id"] = Pay_Period_Id,
               // ["@CreatedBy"] = QZoneUserName,
                
            };
            DataSet ds = new DataSet();
            ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_SearchDeductionFlashOut_Export", parameters, 0);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }

        }

        public async Task<List<DeductionErrorMessage>> DeductionFlasuOutUpload(DeductionReleaseRequest payload)
        {
            const string storedProcedure = "Proc_UploadDeductionFlashOut";

            var xmlBuilder = new StringBuilder();
            var fileName = string.Empty;
            xmlBuilder.Append("<NewDataSet>");
            foreach (var item in payload.DeductionReleaseList)
            {
                if (item.Attachment != null && item.Attachment.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_configuration["SalaryReleaseKey"].ToString(), "DeductionFlasuOut");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    fileName = $"{Guid.NewGuid()}_{item.Attachment.FileName}";
                    var filePath = Path.Combine(uploadsFolder , fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await item.Attachment.CopyToAsync(stream);                   
                }

                xmlBuilder.Append("<Table>");
                xmlBuilder.AppendFormat("<CompanyCode>{0}</CompanyCode>", SecurityElement.Escape(item.CompanyCode));
                xmlBuilder.AppendFormat("<Employeecode>{0}</Employeecode>", SecurityElement.Escape(item.Employeecode));
                xmlBuilder.AppendFormat("<PayPeriod>{0}</PayPeriod>", SecurityElement.Escape(item.PayPeriod));
                xmlBuilder.AppendFormat("<PayCode>{0}</PayCode>", SecurityElement.Escape(item.PayCode));
                xmlBuilder.AppendFormat("<InvoiceNumber>{0}</InvoiceNumber>", SecurityElement.Escape(item.InvoiceNumber));
                xmlBuilder.AppendFormat("<Amount>{0}</Amount>", SecurityElement.Escape(item.Amount));
                xmlBuilder.AppendFormat("<FileName>{0}</FileName>", SecurityElement.Escape(fileName));
                xmlBuilder.Append("</Table>");

                fileName = string.Empty;
            }
            xmlBuilder.Append("</NewDataSet>");
            string xml = xmlBuilder.ToString();

            var parameter = new DynamicParameters();
          
            parameter.Add("@CreatedBy", payload.QZoneUserName);
            parameter.Add("@XML_File", xml);          

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<DeductionErrorMessage>
                {
                    new DeductionErrorMessage
                    {
                        Error_Message = "Invalid response"
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<DeductionErrorMessage>>(res);
                return list?.ToList() ?? new List<DeductionErrorMessage>();
            }
            catch (JsonException ex)
            {              
                return new List<DeductionErrorMessage>
                  {
                    new DeductionErrorMessage
                      {
                        Error_Message = ex.Message
                      }
                 };
            }

        }


        #endregion Deduction FlasuOut end

        #region Salary Advance start

        public DataSet SalaryAdvanceTemplate(string Company_Code, int Pay_Period_Id, string QZoneUserName)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Code"] = Company_Code,
                ["@PayPeriodid"] = Pay_Period_Id,                
                //["@QZoneUserName"] = QZoneUserName,

            };
            DataSet ds = new DataSet();
            ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetSalaryAdvanceRequestTemplate", parameters, 0);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }


        }

        public async Task<List<ErrorMessage>> SalaryAdvanceUpload(IFormFile file, [FromForm] string QZoneUserName)
        {
            ErrorMessage ResultMessage = new ErrorMessage();
            //var ResultMessage = "";
            var result = "";

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["SalaryReleaseKey"].ToString(), "SalaryAdvanceUpload");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"SalaryAdvanceUpload_{QZoneUserName}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                ds.Tables[0].TableName = "Table";
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    result = "Excel sheet is empty or not formatted correctly.";

                    // Wrap it into a list of ErrorMessage
                    var errorList = new List<ErrorMessage>
                    {
                         new ErrorMessage { Error_Message = result }
                   };
                    return errorList;
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

                string storeProcedure = "Proc_UploadSalaryAdvanceRequest";
                var parameters = new DynamicParameters();

                
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", QZoneUserName);               


                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (string.IsNullOrWhiteSpace(res))
                {
                    return new List<ErrorMessage>(); // return empty object if no result
                }

                try
                {
                    var list = JsonConvert.DeserializeObject<List<ErrorMessage>>(res);
                    return list?.ToList() ?? new List<ErrorMessage>();
                }
                catch (JsonException ex)
                {
                    // log the error if you have logging available
                    // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                    return new List<ErrorMessage>();
                }

            }
            else
            {
                result = "Excel sheet is empty or not formatted correctly.";

                // Wrap it into a list of ErrorMessage
                var errorList1 = new List<ErrorMessage>
                    {
                         new ErrorMessage { Error_Message = result }
                   };
                return errorList1;
            }


        }

        #endregion Salary advance end

        #region Van Payment request start

        public DataSet ViewVanPaymentRequestList(VanDetailsView payload)
        {

            string xml = ConvertWithDynamicRoot(payload.CompanyCodelist, "NewDataSet", "Table");

            var parameters = new Dictionary<string, object?>
            {
                ["@XML"] = xml,
                ["@PayPeriod"] = payload.Pay_Period,
                ["@createdby"] = payload.QZoneUserName,

            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_VIEW_VAN_Payment_Request", parameters);
                    
        }

        public DataSet VanPaymentRequestUpload(VanRequest payload)
        {

            string xml = ConvertWithDynamicRoot(payload.VanRequestList, "NewDataSet", "Table");

            var parameter = new Dictionary<string, object?>
            {
                ["@CreatedBy"] = payload.QZoneUserName,
                ["@XML"] = xml

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_VAN_Payment_Request_Insert", parameter, 0);

        }     
        public async Task<List<VanCompanyCode>> VANCompanyCodeList(string QZoneUserName)
        {
            const string storedProcedure = "[dbo].[Proc_Get_All_VAN_Company_Code]";

            var parameter = new DynamicParameters();
            parameter.Add("@createdby", QZoneUserName);
            

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<VanCompanyCode>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<VanCompanyCode>>(res);
                return list?.ToList() ?? new List<VanCompanyCode>();
            }
            catch (JsonException ex)
            {
                
                return new List<VanCompanyCode>();
            }

        }

        public DataSet VANPayPeriodList(VanPayPeriod Request)
        {

            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<NewDataSet>");

            foreach (var approval in Request.requestdata)
            {
                xmlBuilder.Append("<Table>");
                xmlBuilder.AppendFormat("<CompanyCode>{0}</CompanyCode>", approval.Company_Code);
                xmlBuilder.Append("</Table>");
            }

            xmlBuilder.Append("</NewDataSet>");

            string resultXml = xmlBuilder.ToString();

            var parameters = new Dictionary<string, object?>
            {
                ["@XML"] = resultXml,
                //["@PayPeriod"] = Request.Pay_Period,
                //["@QZoneUserName"] = QZoneUserName,

            };

            DataSet ds = new DataSet();
            ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_VAN_payment_payperiod", parameters, 0);


            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }

        }

        #endregion Van Payment request end

        #region Uan hold Release request start

        public DataSet UanReleaseList(UanReleaseCommon payload)
        {
            
            var parameters = new Dictionary<string, object?>
            {
                ["@Entity_Id"] = payload.Entity_Id,
                ["@Pay_Period_Id"] = payload.Pay_Period_Id,
                ["@Employee_Id"] = payload.Employee_Id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetInovice_UAN_ReleaseEmployeeSalaryDetailExportToExcel", parameters);

        }
        public async Task<List<UanErrorMessage>> UanReleaseRequest(UanReleaseRequest payload)
        {
            const string storedProcedure = "USP_UAN_ReleaseInvoiceHoldEmployeeSalaryStatus";

            var parameter = new DynamicParameters();
            string xml = ConvertWithDynamicRoot(payload.UanReleaselist, "NewDataSet", "Table");

            parameter.Add("@XML_File", xml);
            parameter.Add("@CreatedBy", payload.QZoneUserName);
            parameter.Add("@QZoneUserName", payload.QZoneUserName);
            parameter.Add("@Action", "UanReleaseRequest");

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<UanErrorMessage>
                {
                    new UanErrorMessage
                    {
                        Error_Message = "Invalid response"
                    }
                  };
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<UanErrorMessage>>(res);
                return list?.ToList() ?? new List<UanErrorMessage>();
            }
            catch (JsonException ex)
            {

                return new List<UanErrorMessage>
                {
                    new UanErrorMessage
                    {
                        Error_Message = ex.Message
                    }
                  };
            }

        }
       

        #endregion Uan hold Release request end

        #region Reissue Request start
       
        public async Task<List<ReissueRequestMessage>> ReissueRequest(ReissueRequestData payload)
        {
            const string storedProcedure = "Proc_Reissue_request_Qzone";

            var parameter = new DynamicParameters();

            string xml = ConvertWithDynamicRoot(payload.ReissueRequestList, "NewDataSet", "Table");

            parameter.Add("@Createdby", payload.QZoneUserName);
            parameter.Add("@xml", xml);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);


            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<ReissueRequestMessage>
                {
                    new ReissueRequestMessage
                    {
                        Error_Message = "Invalid response"
                    }
                  };
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<ReissueRequestMessage>>(res);
                return list?.ToList() ?? new List<ReissueRequestMessage>();
            }
            catch (JsonException ex)
            {

                //return new List<HoldRequestMessage>();
                return new List<ReissueRequestMessage>
                  {
                    new ReissueRequestMessage
                      {
                        Error_Message = ex.Message
                      }
                 };
            }

        }
       
        #endregion Reissue Request end
    }
}
