using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.IRepository.Process
{
    public interface IPayProcessRepository
    {
        Task<ProcessUIDate> GetProcessDate(string PayPeriod);
        Task<ProcessUI> GetITCalenderCompany(int Company_Id, string End_At);
        Task<List<PayFrequency>> CheckPayPeriod(int Company_Id, string payperiod_Id);
        Task<PayProcessResponse> ReProcess(ReprocessRequest payProcessRequest);
        Task<PayProcessResponse> FandFReProcess(ReprocessRequest payProcessRequest);
    }
}
