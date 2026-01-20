using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using System.Data;
using System.Text;

namespace QPay.BAL.Repository.Customer
{
    public class ServiceChargeRepository : IServiceChargeRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public ServiceChargeRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
       
        public async Task<DataSet> serviceChargeMaster()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Value"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("GetServiceChargeMaster", parameters);
        }
        public async Task<DataSet> serviceChargeMasterNew(int companyId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@CompanyId"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetServiceChargebyCompany", parameters);
        }

        public async Task<DataSet> GetUnitType()
        {
            var parameters = new Dictionary<string, object>
            {
                
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spUnitTypedroprown", parameters);
        }


        public async Task<DataSet> serviceChargeType(int? serviceChargeId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Value"] = serviceChargeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("GetServiceChargeMaster", parameters);
        }


        //public async Task<DataSet> Create(ServiceChargeRequest items)
        //{

        //    var data = GenericSerializer<CompanyServiceCharge1>.Serialize(items.serviceChargeDetails);

        //    string ServiceChargeSerialize = string.Empty;
        //    CompanyServiceCharge1[] NewCompanyServiceChargeDetails = null;


        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        NewCompanyServiceChargeDetails = JsonConvert.DeserializeObject<CompanyServiceCharge1[]>(data);
        //        ServiceChargeResponse objServiceChargeDetails = new ServiceChargeResponse();
        //        objServiceChargeDetails.CompanyServiceChargeDetail = NewCompanyServiceChargeDetails;
        //        ServiceChargeSerialize = GenericSerializer<ServiceChargeResponse>.Serialize(objServiceChargeDetails);
        //    }
        //    string Xml = "<Main>" + ServiceChargeSerialize + "</Main>";

        //    var parameters = new Dictionary<string, object>
        //    {
        //        ["@xmlInput"] = Xml,
        //        ["@mode"] = items.Mode,
        //        ["@CreatedBy"] = items.Created_By,
        //    };
        //    return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdate_Company", parameters);
        //}

        public async Task<ServiceChargeResponse> Create(ServiceChargeRequest request)
        {
            ServiceChargeResponse serviceresponse = new ServiceChargeResponse();

            if (request == null || request.ServiceChargemaster == null || !request.ServiceChargemaster.Any())
            {
                serviceresponse.response = "Invalid request.";
            }

            var xmlInput = BuildServiceChargeXml(request);

            string storeProcedure = "sp_CreateUpdate_ServiceCharge_NewUI";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@CreatedBy", request.Created_By);
            parameters.Add("@Company_ID", request.CompanyId);
            parameters.Add("@mode", request.Mode);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            string? msg = null;

            if (!string.IsNullOrWhiteSpace(res))
            {
                var arr = JArray.Parse(res);
                msg = arr[0]?["Error_Message"]?.ToString();
            }
            if (!string.IsNullOrWhiteSpace(msg))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(msg) && (msg.Contains("Service Charge Created Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Service Charge Updated Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Service Charge Deleted Successfully", StringComparison.OrdinalIgnoreCase)))
                    {
                        serviceresponse.response = msg;
                    }
                    else
                    {
                        serviceresponse.response = "Failed to " + request.Mode + ".";
                        serviceresponse.errors = msg
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    serviceresponse.response = "Error while processing response.";
                }
            }
            else
            {
                serviceresponse.response = "Failed";
            }

            return serviceresponse;
        }
        private string BuildServiceChargeXml(ServiceChargeRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<ServiceChargeDetail>");

            foreach (var row in request.ServiceChargemaster)
            {
                sb.Append("<ServiceCharge>");
                sb.AppendFormat("<Company_Service_Charge_Master_Id>{0}</Company_Service_Charge_Master_Id>", row.Company_Service_Charge_Master_Id);
                sb.AppendFormat("<Company_Service_Charge_Type_Id>{0}</Company_Service_Charge_Type_Id>", row.Company_Service_Charge_Type_Id);
                sb.AppendFormat("<Service_Charge_Slab_Item_Id>{0}</Service_Charge_Slab_Item_Id>", row.Service_Charge_Slab_Item_Id);
                sb.AppendFormat("<Service_Charge_Slab_Inner_Item_Id>{0}</Service_Charge_Slab_Inner_Item_Id>", row.Service_Charge_Slab_Inner_Item_Id);
                sb.AppendFormat("<Slab_Id>{0}</Slab_Id>", row.Slab_Id);
                sb.AppendFormat("<Cost_Center_Mapping_Id>{0}</Cost_Center_Mapping_Id>", row.Cost_Center_Mapping_Id);
                sb.AppendFormat("<Map_Name>{0}</Map_Name>", row.Map_Name);
                sb.AppendFormat("<Invoicing_Type>{0}</Invoicing_Type>", row.Invoicing_Type);
                sb.AppendFormat("<Service_Charge_Name>{0}</Service_Charge_Name>", row.Service_Charge_Name);
                sb.AppendFormat("<PayCode_Code>{0}</PayCode_Code>", row.PayCode_Code);
                sb.AppendFormat("<MaxAmount>{0}</MaxAmount>", row.MaxAmount);
                sb.AppendFormat("<Type>{0}</Type>", row.Type);
                sb.AppendFormat("<Value>{0}</Value>", row.Value);
                sb.AppendFormat("<Effective_Date>{0}</Effective_Date>", row.Effective_Date);
                sb.AppendFormat("<IsBillToRate>{0}</IsBillToRate>", row.IsBillToRate);
                sb.AppendFormat("<IsCTC>{0}</IsCTC>", row.IsCTC);
                sb.AppendFormat("<IsHeadCount>{0}</IsHeadCount>", row.IsHeadCount);
                sb.AppendFormat("<IsAttendanceProrated>{0}</IsAttendanceProrated>", row.IsAttendanceProrated);
                sb.AppendFormat("<IsCriteriaApplicable>{0}</IsCriteriaApplicable>", row.IsCriteriaApplicable);
                sb.AppendFormat("<Criteria>{0}</Criteria>", row.Criteria);
                sb.AppendFormat("<IsReplacementClauseApplicable>{0}</IsReplacementClauseApplicable>", row.IsReplacementClauseApplicable);
                sb.AppendFormat("<Replacement>{0}</Replacement>", row.Replacement);
                sb.AppendFormat("<IsSourcingWaitingPeriod_Id>{0}</IsSourcingWaitingPeriod_Id>", row.IsSourcingWaitingPeriod_Id);
                sb.AppendFormat("<SourcingValue>{0}</SourcingValue>", row.SourcingValue);
                sb.AppendFormat("<TATDays>{0}</TATDays>", row.TATDays);
                sb.AppendFormat("<IsMapNameRequired>{0}</IsMapNameRequired>", row.IsMapNameRequired);
                sb.AppendFormat("<Category_Id>{0}</Category_Id>", row.Category_Id);
                sb.AppendFormat("<Invoice_Map_Name_Id>{0}</Invoice_Map_Name_Id>", row.Invoice_Map_Name_Id);
                sb.AppendFormat("<Compliance_Fee>{0}</Compliance_Fee>", row.Compliance_Fee);
                sb.AppendFormat("<RandStad_Fee>{0}</RandStad_Fee>", row.RandStad_Fee);
                sb.AppendFormat("<UnitType_Id>{0}</UnitType_Id>", row.UnitType_Id);
                sb.AppendFormat("<Discount_Type_Id>{0}</Discount_Type_Id>", row.Discount_Type_Id);
                sb.AppendFormat("<Discount_Amount>{0}</Discount_Amount>", row.Discount_Amount);
                sb.AppendFormat("<Type_Id>{0}</Type_Id>", row.Type_Id);
                sb.AppendFormat("<Pay_Code_Id>{0}</Pay_Code_Id>", row.Pay_Code_Id);
                sb.AppendFormat("<From>{0}</From>", row.From);
                sb.AppendFormat("<To>{0}</To>", row.To);
                sb.AppendFormat("<Slab_Calculation_Type_Id>{0}</Slab_Calculation_Type_Id>", row.Slab_Calculation_Type_Id);
                sb.AppendFormat("<Cap_Value>{0}</Cap_Value>", row.Cap_Value);
                sb.AppendFormat("<Upfront_Charge>{0}</Upfront_Charge>", row.Upfront_Charge);
                sb.AppendFormat("<Upfront_PayCode>{0}</Upfront_PayCode>", row.Upfront_PayCode);
                sb.AppendFormat("<Upfront_Type_Id>{0}</Upfront_Type_Id>", row.Upfront_Type_Id);
                sb.AppendFormat("<Insurance_Amount>{0}</Insurance_Amount>", row.Insurance_Amount);
                sb.AppendFormat("<MarginalPayCodeId>{0}</MarginalPayCodeId>", row.MarginalPayCodeId);
                sb.AppendFormat("<QDemyFee>{0}</QDemyFee>", row.QDemyFee);
                sb.AppendFormat("<InEdgeFee>{0}</InEdgeFee>", row.InEdgeFee);
                sb.AppendFormat("<IsNewjoineeProrate>{0}</IsNewjoineeProrate>", row.IsNewjoineeProrate);
                sb.AppendFormat("<IsFAndFProrate>{0}</IsFAndFProrate>", row.IsFAndFProrate);
                sb.AppendFormat("<IsFAndFArrearProrate>{0}</IsFAndFArrearProrate>", row.IsFAndFArrearProrate);
                sb.AppendFormat("<IsNewJoineeArrearProrate>{0}</IsNewJoineeArrearProrate>", row.IsNewJoineeArrearProrate);
                sb.AppendFormat("<QDemyFee_Type_Id>{0}</QDemyFee_Type_Id>", row.QDemyFee_Type_Id);
                sb.AppendFormat("<InEdgeFee_Type_Id>{0}</InEdgeFee_Type_Id>", row.InEdgeFee_Type_Id);

                sb.Append("</ServiceCharge>");
            }

            sb.Append("</ServiceChargeDetail>");
            return sb.ToString();
        }

        public async Task<ServiceChargeResponse> FileUpload(IFormFile file, [FromForm] int ServiceChargeMaster, [FromForm] int ServiceChargeType,
              [FromForm] int SlabType, [FromForm] int SlabInnerType, [FromForm] int CreatedBy)
        {
            //string ServiceChargeMaster,string ServiceChargeType,string SlabType, string SlabInnerType, int CreatedBy
            ServiceChargeResponse poDetails = new ServiceChargeResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "ServiceCharge");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                
                var filePath = Path.Combine(uploadsFolder, originalFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
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
                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();

                string storeProcedure = @"spImportServiceCharge";
                var parameters = new DynamicParameters();

                parameters.Add("@xmlInput", xmlInput);
                parameters.Add("@Service_Charge_Master_Id", ServiceChargeMaster);
                parameters.Add("@Service_Charge_Type_Id", ServiceChargeType);
                parameters.Add("@SlabType", SlabType);
                parameters.Add("@SlabInnerType", SlabInnerType);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Record(s) Inserted Successfully!", StringComparison.OrdinalIgnoreCase))
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

        public async Task<DataSet> GetAllServiceCharge(int companyId)
        {
            var parameters = new Dictionary<string, object>
            {
              ["@CompanyId"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetAllServiceCharge", parameters);
        }



    }
}
