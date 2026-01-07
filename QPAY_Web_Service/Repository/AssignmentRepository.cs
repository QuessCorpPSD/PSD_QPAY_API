using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Admin;
using QPay.UI.Models;
using System.Data;



namespace QPay.BAL.Repository
{
   public class AssignmentRepository : IAssignmentRepository
    {
        private readonly DbRepository _dbRepository;

        public AssignmentRepository(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }       

        public DataTable GetInputLots(int companyCode, int pay_period_id, int lot, int inputType)
        {
            DataTable dataTable = new DataTable();
            string storeProcedure = string.Format("InputAutomation_Custom_Report_psd");
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyCode);
            parameters.Add("@Pay_Period_Id", pay_period_id);
            parameters.Add("@InputLotNumber", lot);
            parameters.Add("@InputType", inputType);
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                dataTable =(DataTable)JsonConvert.DeserializeObject<DataTable>(res);
            }
            return dataTable;
        }

        public DataSet GetInputLot(int companyCode, int pay_period_id, int lot, int inputType)
        {
            DataSet dataTable = new DataSet();
            string storeProcedure = string.Empty;
            if (inputType==5)
            {
                storeProcedure = string.Format("sp_PayregisteruploadexporttoExcel");
                var parameter=new   DynamicParameters();
                parameter.Add("@Company_Id", companyCode);
                parameter.Add("@Pay_Period_Id", pay_period_id);
               // var res = this._dbRepository.GetItemsAsync(storeProcedure, parameter).Result;                
                //dataTable = JsonConvert.DeserializeObject<DataSet>(res) ?? new DataSet();

                storeProcedure = string.Format("sp_PayregisteruploadexporttoExcel");
                dataTable = this._dbRepository.GetDataSetsSecondaryAsync(companyCode, pay_period_id);

            }
            else
            {
                 storeProcedure = string.Format("InputAutomation_Custom_Report");
                dataTable = this._dbRepository.GetDataSetsSecondaryAsync(companyCode, pay_period_id, lot, inputType);
            }           
          
           
          
            return dataTable;
        }

        //public async Task<AutoAllottmentUI> AssignmentRevok(AllotmentRevok revok)
        //{
        //    AutoAllottmentUI autoAllottmentUI=new AutoAllottmentUI();
        //    const string storedProcedure = "SP_Auto_Allotment_Lot_process_Revised";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Company_Id", revok.Company_Id);
        //    parameters.Add("@Pay_Period_Id", revok.Pay_Period_Id);
        //    parameters.Add("@Lot_Number", revok.Lot_Number);
        //    parameters.Add("@CreatedOn", revok.CreatedOn);
        //    parameters.Add("@userId", revok.userId);
        //    var res = await _dbRepository.GetItemsAsync(storedProcedure, parameters);
        //    try
        //    {
        //        var allotments = JsonConvert.DeserializeObject<List<AutoAllottmentUI>>(res);
        //        return allotments?.FirstOrDefault();
        //    }
        //    catch (JsonException ex)
        //    {
        //        autoAllottmentUI.StatusCode = 201;
        //        autoAllottmentUI.Messages = ex.Message;
        //        // Optional: log the error for debugging
        //        Console.Error.WriteLine($"JSON Deserialization error: {ex.Message}");
        //        return autoAllottmentUI;
        //    }
        //}
        public AssignmentLots GetAssignmentLotByDate(int userId,string filter)
        {
            AssignmentLots assignment = new AssignmentLots();
            string date = System.DateTime.Now.ToString("yyyy-MM-dd");
            List<AssignmentUI> assignments = new List<AssignmentUI>();
            string storeProcedure = string.Format("sp_AutoGetLotDetails");
            var parameters = new DynamicParameters();
            parameters.Add("@Date", date);
            parameters.Add("@filter", filter);
            parameters.Add("@user_id", userId);
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                assignments = JsonConvert.DeserializeObject<List<AssignmentUI>>(res).ToList();

                var pendinglots = assignments.Where(p => p.Assignment=="P");

                if (pendinglots.Count()>0)
                {
                    List<AssignmentUI> pending = pendinglots.Select(p=>new AssignmentUI
                    {
                        InputLot_Id=p.InputLot_Id,
                        Assignment=p.Assignment,
                        Company_code=p.Company_code,
                        Pay_period=p.Pay_period,
                        Company_name=p.Company_name,
                        Lot_Number=p.Lot_Number,
                        Payroll_Input_Type=p.Payroll_Input_Type,
                        Revisedtime=p.Revisedtime,
                        Process_Category=p.Process_Category,
                        Estimate_time=p.Estimate_time,
                        HeadCount=p.HeadCount,
                        Company_Id=p.Company_Id,
                        pay_period_id=p.pay_period_id,
                        CreatedOn=p.CreatedOn,
                        P1_HeadCount=p.P1_HeadCount,
                        P2_HeadCount=p.P2_HeadCount,
                        P3_HeadCount=p.P3_HeadCount,
                        P4_HeadCount=p.P4_HeadCount

                    }).ToList();
                    assignment.PendingLots=pending;
                }
                var todaylots = assignments.Where(p => p.Assignment=="T");
                if (todaylots.Count()>0)
                {
                    List<AssignmentUI> pending = todaylots.Select(p => new AssignmentUI
                    {
                        InputLot_Id=p.InputLot_Id,
                        Assignment=p.Assignment,
                        Company_code=p.Company_code,
                        Pay_period=p.Pay_period,
                        Company_name=p.Company_name,
                        Lot_Number=p.Lot_Number,
                        Payroll_Input_Type=p.Payroll_Input_Type,
                        Revisedtime=p.Revisedtime,
                        Process_Category=p.Process_Category,
                        Estimate_time=p.Estimate_time,
                        HeadCount=p.HeadCount,
                        Company_Id=p.Company_Id,
                        pay_period_id=p.pay_period_id,
                        CreatedOn=p.CreatedOn,
                        P1_HeadCount=p.P1_HeadCount,
                        P2_HeadCount=p.P2_HeadCount,
                        P3_HeadCount=p.P3_HeadCount,
                        P4_HeadCount=p.P4_HeadCount

                    }).ToList();
                    assignment.TodayLots=pending;
                }
                return assignment;
            }

            return assignment;
        }

        public List<AllotmentUI> GetAllotmentByCompanyCodeLot(string companyCode, string payPeriod, int lot)
        {
            List<AllotmentUI> allotments = new List<AllotmentUI>();
            string storeProcedure = string.Format("sp_AutoGetLotwiseDetails");
            var parameters = new DynamicParameters();
            parameters.Add("@Company_code", companyCode);
            parameters.Add("@Pay_Period", payPeriod);
            parameters.Add("@Lot_No", lot);
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                allotments = JsonConvert.DeserializeObject<List<AllotmentUI>>(res).ToList();
            }
            return allotments;
        }

        
        public async Task<List<AllottmentRevokRequest>> AllottmentRevokDetail(int userId)
        {
            List<AllottmentRevokRequest> allotments = new List<AllottmentRevokRequest>();
            string storeProcedure = string.Format("SP_User_Allotted_Detail");
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);
            
            
            var res =await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (res != "")
            {
                allotments = JsonConvert.DeserializeObject<List<AllottmentRevokRequest>>(res).ToList();
            }
            return allotments;
        }
        public async Task<UserEstimateLotValidationUI> UserEstimateLotValidation(LotValidationRequest lotValidationRequest)
        {
            UserEstimateLotValidationUI validation = new UserEstimateLotValidationUI();
            string storeProcedure = string.Format("SP_Estimate_Time_Validation_By_User");
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", lotValidationRequest.company_Id);
            parameters.Add("@PayperiodId", lotValidationRequest.payperiodId);
            parameters.Add("@LotNumber", lotValidationRequest.lotnumber);
            parameters.Add("@Payroll_Input_Type", lotValidationRequest.Payroll_Input_Type);
            parameters.Add("@CreatedOn", lotValidationRequest.CreatedOn);
            parameters.Add("@userId", lotValidationRequest.userId);
            var res =await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (res != "")
            {
                validation = JsonConvert.DeserializeObject<List<UserEstimateLotValidationUI>>(res).FirstOrDefault() ?? new UserEstimateLotValidationUI();
            }
            return validation;

        }

        public async Task<LotValidationResponse> UserEstimateLotValidationLog(LotValidationRequest lotValidationRequest)
        {
            LotValidationResponse validation = new LotValidationResponse();
            string storeProcedure = string.Format("SP_Estimate_Time_Validation_By_User_Insert");
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", lotValidationRequest.company_Id);
            parameters.Add("@PayperiodId", lotValidationRequest.payperiodId);
            parameters.Add("@LotNumber", lotValidationRequest.lotnumber);
            parameters.Add("@Payroll_Input_Type", lotValidationRequest.Payroll_Input_Type);
            parameters.Add("@CreatedOn", lotValidationRequest.CreatedOn);
            parameters.Add("@userId", lotValidationRequest.userId);
            parameters.Add("@ActionType", lotValidationRequest.ActionType);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (res != "")
            {
                validation = JsonConvert.DeserializeObject<List<LotValidationResponse>>(res).FirstOrDefault() ?? new LotValidationResponse();
            }
            return validation;

        }
        public async Task<UserLotValidationUI> UserLotValidation(UserLotValidationRequest userLotValidationRequest)
        {
            UserLotValidationUI userLotValidationUI = new UserLotValidationUI();
            string storeProcedure = string.Format("SP_AllottedTime_Eatimate_Time_validation");
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userLotValidationRequest.userId);
            parameters.Add("@companycode", userLotValidationRequest.companycode);
            parameters.Add("@pay_period_Id", userLotValidationRequest.pay_period_Id);
            parameters.Add("@lot_number", userLotValidationRequest.lot_number);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (res != "")
            {
                userLotValidationUI = JsonConvert.DeserializeObject<List<UserLotValidationUI>>(res).FirstOrDefault();
            }
            return userLotValidationUI;
        }

        public async Task<object> QCQueryRaising(QCVerifyModelRequest userLotValidationRequest)
        {
            
            var res = await this._dbRepository.QueryAsync(@"update tbl_InputLot_Details set QC_RaiseQuery=@QC_RaiseQuery Where 
	        inputLot_Id='"+ userLotValidationRequest .InputLot_Id+ "' and Company_Id='"+ userLotValidationRequest.Company_Id+ "' and Pay_Period_Id ='"+ userLotValidationRequest .pay_period_id+ "' and Lot_Number='"+ userLotValidationRequest.lotnumber + "' and Payroll_Input_Type='"+ userLotValidationRequest .Payroll_Input_Type+ "'  and cast(createdon as date)=cast('"+ userLotValidationRequest .createdon+ "' as date) and QC_Verified_Status is null");
            //if (res != "")
            //{
            //    userLotValidationUI = JsonConvert.DeserializeObject<List<string>>(res).FirstOrDefault();
            //}
            return "";
        }

        public async  Task<AllotmentLotStatusUI> GetLotStatus(AllotmentLotStatusRequest statusRequest)
        {
            AllotmentLotStatusUI allotment = new AllotmentLotStatusUI();
            string storeProcedure = string.Format("SP_AllotmentWiseQCUpdate");
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", statusRequest.Company_Id);
            parameters.Add("@pay_period_id", statusRequest.pay_period_id);
            parameters.Add("@lotnumber", statusRequest.lotnumber);
            parameters.Add("@UpdateType", statusRequest.UpdateStatus);
            parameters.Add("@Payroll_Input_Type", statusRequest.Payroll_Input_Type);
            parameters.Add("@createdon", statusRequest.createdon);
            var res =await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (res!="")
            {
                allotment = JsonConvert.DeserializeObject<List<AllotmentLotStatusUI>>(res).FirstOrDefault();
                if (allotment.QC_Verified_Status)
                {
                    AutoAllocationLots(statusRequest.userId);
                }
            }
            return allotment;

        }
        public QCVerifyModelResponse QCVerfyOrModification(QCVerifyModelRequest request)
        {
            QCVerifyModelResponse verifyModelResponse = new QCVerifyModelResponse();
            string storeProcedure = string.Format("SP_QCVerify_OR_RequestForModification");
            var parameters = new DynamicParameters();
            
                 parameters.Add("@inputLot_Id", request.InputLot_Id);
            parameters.Add("@Company_Id", request.Company_Id);
            parameters.Add("@pay_period_id", request.pay_period_id);
            parameters.Add("@lotnumber", request.lotnumber);
            parameters.Add("@UpdateType", request.UpdateStatus);
            parameters.Add("@Payroll_Input_Type", request.Payroll_Input_Type);
            parameters.Add("@createdon", request.createdon);
            parameters.Add("@Remarks", request.Remarks);
            parameters.Add("@RequestForModification", request.RequestForModification);
            parameters.Add("@QC_RaiseQuery", request.QC_RaiseQuery);
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                verifyModelResponse = JsonConvert.DeserializeObject<List<QCVerifyModelResponse>>(res).FirstOrDefault();
            }
            return verifyModelResponse;
        }
        public AutoAllottmentUI AutoAllocationLots(int userId)
        {
            AutoAllottmentUI allottmentUI = new AutoAllottmentUI();
            // string storeProcedure = string.Format("SP_Auto_Allotment_Lot_process");
            const string storeProcedure = "SP_Auto_Allotment_Lot_process_Revised";
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);
           
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                allottmentUI = JsonConvert.DeserializeObject<List<AutoAllottmentUI>>(res).FirstOrDefault();
            }

            return allottmentUI;
        }
        public async Task<AutoAllottmentUI> AssignmentRevok(AllotmentRevok allotmentRevok)
        {
            AutoAllottmentUI allottmentUI = new AutoAllottmentUI();
            // string storeProcedure = string.Format("SP_Auto_Allotment_Lot_process");
            const string storeProcedure = "SP_RevokDetail";
            var parameters = new DynamicParameters();
            parameters.Add("@company_id", allotmentRevok.Company_Id);
            parameters.Add("@pay_period_Id", allotmentRevok.Pay_Period_Id);
            parameters.Add("@lot_number", allotmentRevok.Lot_Number);
            parameters.Add("@User_Id", allotmentRevok.userId);
            parameters.Add("@CreatedBy", allotmentRevok.CreatedBy);
            parameters.Add("@InputLot_Id", allotmentRevok.InputLot_Id);

            var res =await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (res != "")
            {
                allottmentUI = JsonConvert.DeserializeObject<List<AutoAllottmentUI>>(res).FirstOrDefault();
            }
            //const string  storeProcedure_audit = "sp_InputLotDetails_Audit";
            //parameters = new DynamicParameters();
            //parameters.Add("@CreatedBy", allotmentRevok.CreatedBy);
            //parameters.Add("@InputLot_Id", allotmentRevok.InputLot_Id);
            //res = await this._dbRepository.GetItemsAsync(storeProcedure_audit, parameters);
            
            return allottmentUI;
        }

        public async Task<AutoAllottmentUI> AutoAllocationByUser(int userId)
        {
            const string storedProcedure = "SP_Auto_Allotment_Lot_process_Revised";
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameters);
            if (string.IsNullOrWhiteSpace(res))
                return null;

            try
            {
                var allotments = JsonConvert.DeserializeObject<List<AutoAllottmentUI>>(res);
                return allotments?.FirstOrDefault();
            }
            catch (JsonException ex)
            {
                // Optional: log the error for debugging
                Console.Error.WriteLine($"JSON Deserialization error: {ex.Message}");
                return null;
            }
        }

    }
}
