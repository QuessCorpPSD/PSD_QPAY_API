using QPay.API.Models;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
   public interface IPayRegisterRepository
    {
        FileResponse PayRegisterDownload(int companyCode, int pay_period_Id, int lotNumber,string pay_period);
        string CompanyNameByCode(int company_Id);
        PayRegisterResponse PayRegisterUpload(PayRegisterUI payRegisterUI);
        FileResponse ReconPayRegister(int companyCode, int pay_period_Id, int lotNumber);
        FileResponse GetOtherIncomePayRegister(int companyCode, int pay_period_Id, int lotNumber);
        FileResponse GetQCOtherIncomePayRegister(int companyCode, int pay_period_Id, int lotNumber, string pay_period);
        PayRegisterQzoneResponse GetFileNameFromQzone(int companyCode, int pay_period_Id, int lotNumber);
    }
}
