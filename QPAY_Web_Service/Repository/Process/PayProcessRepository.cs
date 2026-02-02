using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Process;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.BAL.Repository.Process.ArrearAttendanceProcessRepository;
using static QPay.UI.Models.Process.Process;


namespace QPay.BAL.Repository.Process
{
    public class PayProcessRepository : IPayProcessRepository
    {
        private readonly DbRepository _dbRepository;

        public PayProcessRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<PayFrequency>> CheckPayPeriod(int Company_Id, string payperiod_Id)
        {
            const string storedProcedure = "[dbo].[sp_CheckPayPeriod]";

            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", Company_Id);
            parameter.Add("@Pay_Period_Sequence_Number", payperiod_Id);
            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);
            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<PayFrequency>(); // return empty object if no result
            }
            try
            {
                var list = JsonConvert.DeserializeObject<List<PayFrequency>>(res);
                return list?.ToList() ?? new List<PayFrequency>();
            }
            catch (Exception ex)
            {
                return new List<PayFrequency> { new PayFrequency() };
            }
        }

        public async Task<ProcessUI> GetITCalenderCompany(int Company_Id, string End_At)
        {
            var ProcessUIDetails = new ProcessUI();
            const string storedProcedure = "[dbo].[sp_GetITCalenderCompany]";

            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", Company_Id);
            parameter.Add("@End_At", End_At);
            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);
            if (string.IsNullOrWhiteSpace(res))
            {
                return new ProcessUI(); // return empty object if no result
            }
            try
            {
                var processlist = JsonConvert.DeserializeObject<List<ProcessUI>>(res);
                ProcessUIDetails = processlist?.FirstOrDefault() ?? new ProcessUI();
                return ProcessUIDetails;
            }
            catch (Exception ex)
            {
                return new ProcessUI();
            }
        }

        public async Task<ProcessUIDate> GetProcessDate(string PayPeriod)
        {
            try
            {
                var ProcessUIDetails = new ProcessUIDate();
                string input = PayPeriod;
                DateTime date = DateTime.ParseExact(
                    input,
                    "MMMM yyyy",
                    CultureInfo.InvariantCulture
                );

                DateTime lastDate = new DateTime(
                    date.Year,
                    date.Month,
                    DateTime.DaysInMonth(date.Year, date.Month)
                );

                ProcessUIDetails.Date =Convert.ToDateTime(lastDate).ToString("dd-MM-yyyy");
                return ProcessUIDetails;
            }
            catch (Exception ex)
            {
                return new ProcessUIDate();
            }
        }

        public async Task<PayProcessResponse> ReProcess(ReprocessRequest request)
        {
            PayProcessResponse timesheetDetails = new PayProcessResponse();

            if (request == null)
            {
                timesheetDetails.response = "Invalid request.";
                return timesheetDetails;
            }

            string storeProcedure = "spReProcess";
            var parameters = new DynamicParameters();

            parameters.Add("@Company_Id", request.Company_Id);
            parameters.Add("@Pay_Period_Id", request.Pay_Period_Id);
            parameters.Add("@Declaration_type", request.Declaration_type);
            parameters.Add("@CreatedBy", request.CreatedBy);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("ReProcessed Successfully."))
                    {
                        timesheetDetails.response = message;
                    }
                    else
                    {
                        timesheetDetails.response = "Failed.";
                        timesheetDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    timesheetDetails.response = "Error while processing response.";
                }
            }
            else
            {
                timesheetDetails.response = "Failed";
            }


            return timesheetDetails;
        }

        public async Task<PayProcessResponse> FandFReProcess(ReprocessRequest request)
        {
            PayProcessResponse timesheetDetails = new PayProcessResponse();

            if (request == null)
            {
                timesheetDetails.response = "Invalid request.";
                return timesheetDetails;
            }

            string storeProcedure = "sp_FReProcess_new";
            var parameters = new DynamicParameters();

            parameters.Add("@Company_Id", request.Company_Id);
            parameters.Add("@Pay_Period_Id", request.Pay_Period_Id);
            parameters.Add("@Declaration_type", request.Declaration_type);
            parameters.Add("@CreatedBy", request.CreatedBy);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {

                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("Processed Successfully."))
                    {
                        timesheetDetails.response = message;
                    }
                    else
                    {
                        timesheetDetails.response = "Failed.";
                        timesheetDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    timesheetDetails.response = "Error while processing response.";
                }
            }
            else
            {
                timesheetDetails.response = "Failed";
            }


            return timesheetDetails;
        }

    }
}
