using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using System.Data;

namespace QPay.BAL.Repository.Customer
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public EmployeeRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(int? Company, string? Employee_code)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company,
                ["@Employee_code"] = Employee_code,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllEmployeeByPaging_New_api", parameters, 1500);
        }

        public async Task<DataSet> ExportToExcel(int? CompanyId, string EmployeeId, int? EActive)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = CompanyId,
                ["@Employee_Id"] = EmployeeId,
                ["@EActive"] = EActive
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllEmployeeDetailsExportToExcel", parameters, 1500);
        }

        public async Task<List<CategoryUI>> GetCategory()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("Proc_GetCategory_CPF", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CategoryUI>>(res) ?? new List<CategoryUI>();
            }

            return new List<CategoryUI>();
        }

        public async Task<DataSet> Department(int? Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id,
                ["@Department_Id"] = 0
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetDepartmentDetails", parameters, 1500);
        }

        public async Task<DataSet> Designation(int? Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetDesignationDetails", parameters, 1500);
        }

        public async Task<DataSet> BillingDesignation(int? Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetBillingDesignationDetails", parameters, 1500);
        }

        public async Task<DataSet> Costcentermapping(int? Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetCostCenterMappingForEmployee", parameters, 1500);
        }

        public async Task<DataSet> Band(int? Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetBandDetails", parameters, 1500);
        }

        public async Task<DataSet> GetCostCenter(int? Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetCostCenter", parameters, 1500);
        }

        public async Task<DataSet> GroupMater(int? Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Companycode"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetGroupMasterDetails", parameters, 1500);
        }

        public async Task<DataSet> GetAllPayPeriodByCompanyID(int? Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = Company_id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPaySequenceByCompanyID", parameters, 1500);
        }


        public async Task<DataSet> SearchBank()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@BankName"] = "",
                ["@BranchName"] = "",
                ["@Bank_Id"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetBankDetails", parameters, 1500);
        }


        public async Task<DataSet> GetRole()
        {
            var parameters = new Dictionary<string, object?>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetRole", parameters, 1500);
        }


        public async Task<DataSet> GetEmploymentType()
        {
            var parameters = new Dictionary<string, object?>
            {

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spEmploymentTypedroprown", parameters, 1500);
        }

        public async Task<EmployeeApiResponse> PostEmployeeUpload(string xmlString, string userId)
        {
            EmployeeApiResponse employeeDetails = new EmployeeApiResponse();

            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("SpImportEmployee", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Successfully"))
                    {
                        employeeDetails.response = message;
                    }
                    else
                    {
                        employeeDetails.response = "Failed to import.";
                        employeeDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    employeeDetails.response = "Error while processing response.";
                }
            }
            else
            {
                employeeDetails.response = "Failed";
            }

            return employeeDetails;

        }

        public async Task<EmployeeApiResponse> PostEmployeeSalaryUpload(string xmlString, string userId)
        {
            EmployeeApiResponse employeeDetails = new EmployeeApiResponse();
            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Upload_NewJoineeSalary", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Successfully"))
                    {
                        employeeDetails.response = message;
                    }
                    else
                    {
                        employeeDetails.response = "Failed to import.";
                        employeeDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    employeeDetails.response = "Error while processing response.";
                }
            }
            else
            {
                employeeDetails.response = "Failed";
            }

            return employeeDetails;

        }


        public async Task<DataSet> Create(EmployeeRequest items)
        {
            var EmpResponse = new EmployeeResponse();
            EmpResponse.EmployeeDetails = new EmployeeWithDetails[1];
            EmpResponse.EmployeeDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeeResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Employee",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateEmployee", parameters);
        }


        public async Task<DataSet> BankCreate(EmployeeBankRequest items)
        {
            var EmpResponse = new EmployeeBankDetailResponse();
            EmpResponse.EmployeeBankDetails = new EmployeeBankDetail[1];
            EmpResponse.EmployeeBankDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeeBankDetailResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Bank",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> InformationCreate(EmployeeInformationRequest items)
        {
            var EmpResponse = new EmployeeInformationResponse();
            EmpResponse.EmployeeInformationDetails = new EmployeeInformation[1];
            EmpResponse.EmployeeInformationDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeeInformationResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Information",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> ContactCreate(EmployeeContactRequest items)
        {
            var EmpResponse = new EmployeeContactResponse();
            EmpResponse.EmployeeContactDetails = new EmployeeContactDetail[1];
            EmpResponse.EmployeeContactDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeeContactResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Contact",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> PersonalCreate(EmployeePersonalRequest items)
        {
            var EmpResponse = new EmployeePersonalResponse();
            EmpResponse.EmployeePersonalDetails = new EmployeePersonalDetail[1];
            EmpResponse.EmployeePersonalDetails[0] = items.detail;
            string empResponseSerialize = GenericSerializer<EmployeePersonalResponse>.Serialize(EmpResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = empResponseSerialize,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Personal",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> PreviousCreate(EmployeePreviousRequest items)
        {
            var parentdata = GenericSerializer<EmployeePreviousEmployment>.Serialize(items.detail);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@mode"] = "Edit",
                ["@CreatedBy"] = items.createdBy,
                ["@Type"] = "Previous",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateEmployee", parameters);
        }

        public async Task<DataSet> SearchSalary(int employeeId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Employee_Id"] = employeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetEmployeeSalaryInformation", parameters);
        }

        public async Task<List<LegalEntityUI>> GetLegalEntity()
        {
            var parameters = new DynamicParameters();
            var res = await this._dbRepository.GetItemsAsync("SP_Get_LegalEntity", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<LegalEntityUI>>(res) ?? new List<LegalEntityUI>();
            }
            return new List<LegalEntityUI>();
        }

    }
}
