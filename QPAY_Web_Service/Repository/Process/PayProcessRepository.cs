//using Dapper;
//using Newtonsoft.Json;
//using QPay.BAL.IRepository.Process;
//using QPay.DAL.Repository;
//using QPay.UI.Process;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace QPay.BAL.Repository.Process
//{
//    public class PayProcessRepository : IPayProcessRepository
//    {
//        private readonly DbRepository _dbRepository;

//        public PayProcessRepository(DbRepository dbRepository) { 
//        this._dbRepository = dbRepository;
//        }
//        public async Task<List<PayFrequency>> CheckPayPeriod(int Company_Id, string payperiod_Id)
//        {
//            const string storedProcedure = "[dbo].[sp_CheckPayPeriod]";

//            var parameter = new DynamicParameters();
//            parameter.Add("@Company_Id", Company_Id);
//            parameter.Add("@Pay_Period_Sequence_Number", payperiod_Id);
//            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);
//            if (string.IsNullOrWhiteSpace(res))
//            {
//                return new List<PayFrequency>(); // return empty object if no result
//            }
//            try
//            {
//                var list = JsonConvert.DeserializeObject<List<PayFrequency>>(res);
//                return list?.ToList() ?? new List<PayFrequency>();
//            }
//            catch (Exception ex)
//            {
//                return new List<PayFrequency> { new PayFrequency() };
//            }
//        }
//        public async Task<List<Message>> ReProcess(int Company_Id, int Pay_Frequency_Id, string Declaration_type, int UserId)
//        {
//            const string storedProcedure = "[dbo].[sp_GetITCalenderCompany]";
//            List<Message> result = new List<Message>();
//            var parameter = new DynamicParameters();
//            parameter.Add("Company_Id", Company_Id);
//            parameter.Add("@Pay_Period_Id", Pay_Frequency_Id);
//            parameter.Add("@Declaration_type", Declaration_type);
//            parameter.Add("@CreatedBy", UserId);
//            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);
//            if (string.IsNullOrWhiteSpace(res))
//            {
//                return new List<Message>(); // return empty object if no result
//            }
//            try
//            {
//                var list = JsonConvert.DeserializeObject<List<Message>>(res);
//                return list?.ToList() ?? new List<Message>();
//            }
//            catch (Exception ex)
//            {
//                return new List<Message> { new Message() };
//            }
            
//        }

//        public async Task<List<PayFrequency>> GetITCalenderCompany(int Company_Id, string End_At)
//        {
//            const string storedProcedure = "[dbo].[sp_GetITCalenderCompany]";

//            var parameter = new DynamicParameters();
//            parameter.Add("@Company_Id", Company_Id);
//            parameter.Add("@End_At", End_At);            
//            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);
//            if (string.IsNullOrWhiteSpace(res))
//            {
//                return new List<PayFrequency>(); // return empty object if no result
//            }
//            try
//            {
//                var list = JsonConvert.DeserializeObject<List<PayFrequency>>(res);
//                return list?.ToList() ?? new List<PayFrequency>();
//            }
//            catch (Exception ex)
//            {
//                return new List<PayFrequency> { new PayFrequency() };
//            }
//        }
//    }
//}
