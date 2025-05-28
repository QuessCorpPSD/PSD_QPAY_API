using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System.Data;



namespace QPay.BAL.Repository
{
   public class AssignmentRepository : IAssignmentRepository
    {
        private readonly DbRepository _dbRepository;

        public AssignmentRepository(DbRepository dbRepository)
        {
            _dbRepository=dbRepository;
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
            string storeProcedure = string.Format("InputAutomation_Custom_Report");
            //var parameters = new DynamicParameters();
            //parameters.Add("@Company_Id", companyCode);
            //parameters.Add("@Pay_Period_Id", pay_period_id);
            //parameters.Add("@InputLotNumber", lot);
            //parameters.Add("@InputType", inputType);
            dataTable = this._dbRepository.GetDataSetAsync(companyCode, pay_period_id, lot, inputType);
            //if (res!=null)
            //{
            //    dataTable =(DataSet)JsonConvert.DeserializeObject<DataSet>(res);
           // }
            return dataTable;
        }
        public AssignmentLots GetAssignmentLotByDate(int userId)
        {
            AssignmentLots assignment = new AssignmentLots();
            string date = System.DateTime.Now.ToString("yyyy-MM-dd");
            List<AssignmentUI> assignments = new List<AssignmentUI>();
            string storeProcedure = string.Format("sp_AutoGetLotDetails");
            var parameters = new DynamicParameters();
            parameters.Add("@Date", date);
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


       public  AllotmentLotStatusUI GetLotStatus(AllotmentLotStatusRequest statusRequest)
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
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
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
            string storeProcedure = string.Format("SP_Auto_Allotment_Lot_process");
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);
           
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                allottmentUI = JsonConvert.DeserializeObject<List<AutoAllottmentUI>>(res).FirstOrDefault();
            }

            return allottmentUI;
        }
    }
}
