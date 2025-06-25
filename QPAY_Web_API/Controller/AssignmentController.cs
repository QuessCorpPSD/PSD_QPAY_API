using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.Repository;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System.Data;
using System.Text;
using System.Text.Json;

namespace QPay.API.Controller
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
   
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentRepository _assignment;
        private readonly IPayRegisterRepository _payRegisterRepository;
        private readonly HttpClient _client;
        private IConfiguration _config;

        public AssignmentController(HttpClient client, IConfiguration config, IAssignmentRepository assignment, IPayRegisterRepository payRegisterRepository)
        {
            _client = client;
            this._assignment=assignment;
            this._payRegisterRepository=payRegisterRepository;
            this._config=config;
        }


        [HttpGet,Route("GetAssignmentLot/{userId}")]
        public IActionResult GetAssignmentLot(int userId)
        {
            var lots = this._assignment.GetAssignmentLotByDate(userId);
            return Ok(lots);

        }

        [HttpPost("GetAllotment")]
        public IActionResult GetAllotment(AllotmentRequest allotment)
        {
            var allot = this._assignment.GetAllotmentByCompanyCodeLot(allotment.companyCode, allotment.payPeriod, allotment.lot);
            return Ok(allot);
        }
        [HttpPost,Route("LotStatus")]
        public async Task<IActionResult> LotStatus(AllotmentLotStatusRequest lotStatusrequestModel)
        {
            var status =await this._assignment.GetLotStatus(lotStatusrequestModel);
            return Ok(status);
        }
        [HttpPost,Route("UserLotStatusValidation")]
        public async Task<IActionResult> UserLotStatusValidation(UserLotValidationRequest userLotValidationRequest)
        {
            var status = await this._assignment.UserLotValidation(userLotValidationRequest);
            return Ok(status);
        }
        [HttpPost,Route("QCQueryRaise")]        
        public IActionResult QCQueryRaise(QCApprovedRequest lotStatusrequestModel)
        {
            AllotmentLotStatusUI allotmentLotStatus = new AllotmentLotStatusUI();
            AllotmentLotStatusRequest lotStatusUI = new AllotmentLotStatusRequest()
            {
                Company_Id = lotStatusrequestModel.Company_Id,
                pay_period_id = lotStatusrequestModel.pay_period_id,
                lotnumber = lotStatusrequestModel.lotnumber,
                UpdateStatus = lotStatusrequestModel.UpdateStatus,
                Payroll_Input_Type = lotStatusrequestModel.Payroll_Input_Type,
                createdon = lotStatusrequestModel.createdon,
                QC_RaiseQuery = lotStatusrequestModel.QC_RaiseQuery

            };
            QCVerifyModelRequest modelRequest = new QCVerifyModelRequest()
            {
                InputLot_Id = 0,
                Company_Id = lotStatusrequestModel.Company_Id,
                pay_period_id = lotStatusrequestModel.pay_period_id,
                lotnumber = lotStatusrequestModel.lotnumber,
                UpdateStatus = lotStatusrequestModel.UpdateStatus,
                Payroll_Input_Type = lotStatusrequestModel.Payroll_Input_Type,
                createdon = lotStatusrequestModel.createdon,
                Remarks ="",
                RequestForModification = lotStatusrequestModel.UpdateStatus == "Q" ? false : true,
                QC_RaiseQuery = lotStatusrequestModel.QC_RaiseQuery


            };


            var status = this._assignment.QCQueryRaising(modelRequest).Result;

            allotmentLotStatus = this._assignment.GetLotStatus(lotStatusUI).Result;



            return Ok(allotmentLotStatus);
        }
        public async Task<IActionResult> QCLotVerify(QCApprovedRequest lotStatusrequestModel)
        {
            AllotmentLotStatusUI allotmentLotStatus = new AllotmentLotStatusUI();
            AllotmentLotStatusRequest lotStatusUI = new AllotmentLotStatusRequest()
            {
                Company_Id =lotStatusrequestModel.Company_Id,
                pay_period_id =lotStatusrequestModel.pay_period_id,
                lotnumber =lotStatusrequestModel.lotnumber,
                UpdateStatus =lotStatusrequestModel.UpdateStatus,
                Payroll_Input_Type =lotStatusrequestModel.Payroll_Input_Type,
                createdon =lotStatusrequestModel.createdon,
                QC_RaiseQuery=lotStatusrequestModel.QC_RaiseQuery
               
            };




            if (lotStatusrequestModel.UpdateStatus=="Q")
            {
                FileResponse fileResponse = new FileResponse();
                PayRegisterRequest payRegisterRequest = new PayRegisterRequest() { companycode=lotStatusrequestModel.Company_Id, pay_period_Id=lotStatusrequestModel.pay_period_id, lotNumber=lotStatusrequestModel.lotnumber };
                var comayName = _payRegisterRepository.CompanyNameByCode(lotStatusrequestModel.Company_Id);
                var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                if(lotStatusrequestModel.Payroll_Input_Type=="Other Input")
                {
                    fileResponse = this._payRegisterRepository.GetOtherIncomePayRegister(lotStatusrequestModel.Company_Id, lotStatusrequestModel.pay_period_id, lotStatusrequestModel.lotnumber);
                }
                else
                {
                    fileResponse = this._payRegisterRepository.PayRegisterDownload(lotStatusrequestModel.Company_Id, lotStatusrequestModel.pay_period_id, lotStatusrequestModel.lotnumber);
                }
                    

                if (fileResponse.File!="No")
                {
                    foreach (var item in lotStatusrequestModel.allotments)
                    {
                        QCVerifyModelRequest modelRequest = new QCVerifyModelRequest()
                        {
                            InputLot_Id=item.InputLot_Id,
                            Company_Id =lotStatusrequestModel.Company_Id,
                            pay_period_id =lotStatusrequestModel.pay_period_id,
                            lotnumber =lotStatusrequestModel.lotnumber,
                            UpdateStatus =lotStatusrequestModel.UpdateStatus,
                            Payroll_Input_Type =lotStatusrequestModel.Payroll_Input_Type,
                            createdon =lotStatusrequestModel.createdon,
                            Remarks=item.Remarks,
                            RequestForModification=lotStatusrequestModel.UpdateStatus=="Q" ? false : true,
                            QC_RaiseQuery=lotStatusrequestModel.QC_RaiseQuery
                            

                        };
                        var QC_Status = this._assignment.QCVerfyOrModification(modelRequest);
                    }
                    allotmentLotStatus = this._assignment.GetLotStatus(lotStatusUI).Result;
                    allotmentLotStatus.fileResponse=fileResponse;
                    PayRegisterUploadModel payRegisterUploadModel = new PayRegisterUploadModel()
                    {
                        CompanyId= lotStatusrequestModel.Company_Id,
                        CompanyCode= lotStatusrequestModel.CompanyCode,
                        Pay_Period_id= lotStatusrequestModel.pay_period_id,
                        Pay_Period= lotStatusrequestModel.Pay_Period,
                        LotNumber= lotStatusrequestModel.lotnumber,
                        FilePath= "",
                        FileName= fileResponse.FileName,
                        FileType= ".xlsx",
                        LoginUser= lotStatusrequestModel.userId.ToString(),
                        Input_type= lotStatusrequestModel.Payroll_Input_Type,
                        Docs= fileResponse.File
                    };
                    var requestJsonContent = System.Text.Json.JsonSerializer.Serialize(payRegisterUploadModel);


                    var requestStringContents = new StringContent(requestJsonContent, Encoding.UTF8, "application/json");
                    var uri = this._config["ApiURL"]+"PayRegister/PayRegisterAutoUpload";
                    using (var httpResponse = await _client.PostAsync(uri, requestStringContents))
                    {
                        httpResponse.EnsureSuccessStatusCode();
                        if (!httpResponse.IsSuccessStatusCode)
                        {
                            throw new Exception("Cannot retrieve tasks");
                        }
                        var content = await httpResponse.Content.ReadAsStringAsync();
                        var orderItem = System.Text.Json.JsonSerializer.Deserialize<PayRegisterResponse>(content);
                        // return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                    }

                    return Ok(allotmentLotStatus);
                }
                else
                {
                    allotmentLotStatus.fileResponse=fileResponse;
                }

            }
            else
            {
                foreach (var item in lotStatusrequestModel.allotments)
                {
                    QCVerifyModelRequest modelRequest = new QCVerifyModelRequest()
                    {
                        InputLot_Id=item.InputLot_Id,
                        Company_Id =lotStatusrequestModel.Company_Id,
                        pay_period_id =lotStatusrequestModel.pay_period_id,
                        lotnumber =lotStatusrequestModel.lotnumber,
                        UpdateStatus =lotStatusrequestModel.UpdateStatus,
                        Payroll_Input_Type =lotStatusrequestModel.Payroll_Input_Type,
                        createdon =lotStatusrequestModel.createdon,
                        Remarks=item.Remarks,
                        RequestForModification=lotStatusrequestModel.UpdateStatus=="Q" ? false : true

                    };
                    var QC_Status = this._assignment.QCVerfyOrModification(modelRequest);
                    allotmentLotStatus = this._assignment.GetLotStatus(lotStatusUI).Result;
                }
            }

                return Ok(allotmentLotStatus);
        }

        [HttpPost,Route("SendFeedBackMail")]
        public async Task<IActionResult> SendFeedBackMail(FeedBackMailRequest feedBackMailRequest)
        {
            FeedBackMailResponse feedBackMailResponse = new FeedBackMailResponse();
            var requestJsonContent = System.Text.Json.JsonSerializer.Serialize(feedBackMailRequest);


            var requestStringContents = new StringContent(requestJsonContent, Encoding.UTF8, "application/json");
            var uri = this._config["EmailSetting:EmailAPI_Url"] + "api/AutoMailer/sendautomail";
            using (var httpResponse = await _client.PostAsync(uri, requestStringContents))
            {
                httpResponse.EnsureSuccessStatusCode();
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Cannot retrieve tasks");
                }
                var content = await httpResponse.Content.ReadAsStringAsync();
                feedBackMailResponse = System.Text.Json.JsonSerializer.Deserialize<FeedBackMailResponse>(content) ?? new FeedBackMailResponse();
               return Ok(feedBackMailResponse);
            }

            
        }
        [HttpPost("RequestForModification")]
        public IActionResult RequestForModification(RequestForModificationModel requestForModification)
        {
            return Ok();
        }
        [HttpPost("InputLotAllDownload")]
        public IActionResult InputLotAllDownload(InputLotDownloadModel inputLotDownloadModel)
        {
            int inputType = 1;
            if(inputLotDownloadModel.InputType=="Salary")
            {
                inputType=1;
            }
            else if (inputLotDownloadModel.InputType=="OtherInput")
            {
                inputType=2;
            }
            else if (inputLotDownloadModel.InputType=="RevisedInput")
            {
                inputType=3;
            }
            else if (inputLotDownloadModel.InputType=="RevisedOtherInput")
            {
                inputType=4;
            }
            DataTable input = _assignment.GetInputLots(inputLotDownloadModel.companycode, inputLotDownloadModel.pay_period_id, inputLotDownloadModel.lotNumber, inputType);

            using var workbook = new XLWorkbook();
            {
                var ws = workbook.AddWorksheet(input, "Sheet1");
                //ws.Table(0).ShowAutoFilter = false;
               // ws.Table(0).Theme = XLTableTheme.None;
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());
                    FileResponse fileResponse = new FileResponse();
                    fileResponse.FileName="InputLot";
                    fileResponse.File=bytes;

                    return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                }
            }
        }

        [HttpPost("InputLotDownload")]
        public IActionResult InputLotDownload(InputLotDownloadModel inputLotDownloadModel)
        {
            int inputType = 1;
            if (inputLotDownloadModel.InputType=="Salary")
            {
                inputType=1;
            }
            else if (inputLotDownloadModel.InputType=="OtherInput")
            {
                inputType=2;
            }
            else if (inputLotDownloadModel.InputType=="RevisedInput")
            {
                inputType=3;
            }
            else if (inputLotDownloadModel.InputType=="RevisedOtherInput")
            {
                inputType=4;
            }
            DataSet input = _assignment.GetInputLot(inputLotDownloadModel.companycode, inputLotDownloadModel.pay_period_id, inputLotDownloadModel.lotNumber, inputType);

            using var workbook = new XLWorkbook();
            {
                for (int i = 0; i < input.Tables.Count; i++)
                {                   
                    var ws = workbook.AddWorksheet(input.Tables[i], GetSheetName(i));
                    ws.Table(0).ShowAutoFilter = false;
                    ws.Table(0).Theme = XLTableTheme.None;
                }
              
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = Convert.ToBase64String(stream.ToArray());
                    FileResponse fileResponse = new FileResponse();
                    fileResponse.FileName="InputLot";
                    fileResponse.File=bytes;

                    return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                }
            }
        }
        public string GetSheetName(int i)
        {
            string sheetName = string.Empty;
            switch(i)
            {
                case 0:
                    sheetName= "New Joinee Employee id Creation";
                    break;
                case 1:
                    sheetName= "New Joinee Breakup";
                    break;

                case 2:
                    sheetName= "Attendance";
                    break;
                case 3:
                    sheetName= "Adhoc Or Pay Transaction";
                    break;

                case 4:
                    sheetName= "Increment Break up";
                    break;
                case 5:
                    sheetName= "LOP Details";
                    break;
                case 6:
                    sheetName= "New Joinee LOP Details";
                    break;
                case 7:
                    sheetName= "ds7";
                    break;
                case 8:
                    sheetName= "ds8";
                    break;

                default: sheetName= "";
                    return sheetName;
            }
            return sheetName;

        }
    }
}
