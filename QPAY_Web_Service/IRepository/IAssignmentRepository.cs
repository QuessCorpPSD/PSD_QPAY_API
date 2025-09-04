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
        AssignmentLots GetAssignmentLotByDate(int userId, string filter);
        DataTable GetInputLots(int companyCode, int pay_period_id, int lot, int inputType);
        AutoAllottmentUI AutoAllocationLots(int userId);

        Task<AutoAllottmentUI> AutoAllocationByUser(int userId);

        List<AllotmentUI> GetAllotmentByCompanyCodeLot(string companyCode, string payPeriod, int lot);
        Task<AllotmentLotStatusUI> GetLotStatus(AllotmentLotStatusRequest statusRequest);
        Task<object> QCQueryRaising(QCVerifyModelRequest userLotValidationRequest);
        DataSet GetInputLot(int companyCode, int pay_period_id, int lot, int inputType);
        QCVerifyModelResponse QCVerfyOrModification(QCVerifyModelRequest request);
        Task<UserLotValidationUI> UserLotValidation(UserLotValidationRequest userLotValidationRequest);
        Task<LotValidationResponse> UserEstimateLotValidationLog(LotValidationRequest lotValidationRequest);
        Task<UserEstimateLotValidationUI> UserEstimateLotValidation(LotValidationRequest lotValidationRequest);



    }
}
