using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
   public interface IAssignmentRepository
    {
        AssignmentLots GetAssignmentLotByDate(int userId);
        DataTable GetInputLots(int companyCode, int pay_period_id, int lot, int inputType);
        AutoAllottmentUI AutoAllocationLots(int userId);
        List<AllotmentUI> GetAllotmentByCompanyCodeLot(string companyCode, string payPeriod, int lot);
        AllotmentLotStatusUI GetLotStatus(AllotmentLotStatusRequest statusRequest);
        DataSet GetInputLot(int companyCode, int pay_period_id, int lot, int inputType);
        QCVerifyModelResponse QCVerfyOrModification(QCVerifyModelRequest request);




    }
}
