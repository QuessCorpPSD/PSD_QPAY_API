using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QPay.BAL.IRepository.SalaryReleaseInvoice
{
    public interface ISalaryRequestInvoiceRepository
    {
        #region Salary Request start
        Task<List<BankAdvice>> GetBankAdviceApproveList(InvoiceCommon SRInvoiceCommon);

        Task<List<ErrorMessage>> CreateRequestSalaryRelease(BankAdviceApprovalRequest Request);

        Task<List<ErrorMessage>> UploadSalaryReleaseRequest(BankAdviceRequest rdata);
        DataSet SalaryReleaseTemplate(string Flag, string QZoneUserName);

        #endregion Salary Request end

        #region SalaryHold Request start
        DataSet InvoiceHoldList(SalaryHoldCommon Data);

        Task<List<HoldRequestMessage>> HoldRequestUpload(HoldSalaryRequest payload);

        DataSet SingleHoldRequest(SingleHoldRequest payload);

        #endregion SalaryHold Request end

        #region SalaryHoldRelease Request start

        Task<List<HoldReleaseSalary>> InvoiceHoldReleaseList(SalaryHoldReleaseCommon Data);
        Task<List<HoldReleaseSalary>> InvoiceHoldReleaseListExport(SalaryHoldReleaseCommon Data);

       Task<List<HoldReleaseMessage>> HoldReleaseRequest(HoldReleaseRequest payload);

        #endregion SalaryHoldRelease Request end

        #region partila hold and release start

        Task<List<PartialHoldMessage>> PartialHoldRequest(PartilHoldRequest payload);
        Task<List<PartialHoldMessage>> PartialHoldRelease(PartialRelease payload);


        #endregion partila hold and release end

        #region DBT hold and release start

        Task<List<DBTHoldMessage>>DBTHoldRequest(DBTHoldRequest payload);
        Task<List<DBTHoldMessage>> DBTHoldRelease(DBTRelease payload);


        #endregion DBT hold and release end

        #region netpay summary start

        DataSet InvoiceNetPaysummary(int Company_Id, int Pay_Period_Id, string QZoneUserName);
        DataSet InvoiceWiseAssociateHoldList(int Company_Id, int Pay_Period_Id, string Flag, string Invoice_No, string QZoneUserName);
        DataSet NetPaysummary(int Company_Id, int Pay_Period_Id, string QZoneUserName);

        #endregion netpay summary end

        #region Common drop down start
        List<CommonDropDownBA> GetCommonDropDownList(string Flag, string QZoneUserName);

        #endregion Common drop down end

        #region Bonus flush out start

        DataSet BonusDetailsSummary(int Company_Id, string FromDate, string ToDate, string QZoneUserName);
        DataSet BonusAccumatedReport(int Company_Id, string FromDate, string ToDate, string QZoneUserName);
        Task<List<BonusErrorMessage>> BonusReleaseUpload(BonusReleaseRequest payload);
        #endregion Bonus flush out end

        #region Deduction FlasuOut start
        DataSet DeductionFlasuOutSearch(int Company_Id, int Pay_Period_Id, string QZoneUserName);
        Task<List<DeductionErrorMessage>> DeductionFlasuOutUpload(DeductionReleaseRequest payload);
        #endregion Deduction FlasuOut end

        #region Salary Advance start
        DataSet SalaryAdvanceTemplate(string Company_Code, int Pay_Period_Id, string QZoneUserName);

        Task<List<ErrorMessage>> SalaryAdvanceUpload(IFormFile file, [FromForm] string QZoneUserName);

        #endregion Salary advance end

        #region Van Payment request start
        DataSet ViewVanPaymentRequestList(VanDetailsView payload);

        DataSet VanPaymentRequestUpload(VanRequest payload);
        Task<List<VanCompanyCode>> VANCompanyCodeList(string QZoneUserName);

        DataSet VANPayPeriodList(VanPayPeriod data);
        #endregion Van Payment request end

        #region Uan hold Release request start

        DataSet UanReleaseList(UanReleaseCommon payload);
   
        Task<List<UanErrorMessage>> UanReleaseRequest(UanReleaseRequest payload);

        #endregion Uan hold Release request end

        #region Reissue Request start
       
        Task<List<ReissueRequestMessage>> ReissueRequest(ReissueRequestData payload);       

        #endregion Reissue Request end
    }
}
