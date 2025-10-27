using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using QPay.API.LoggerService;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.Repository;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        private readonly ILoggerManager _logger;
       // private readonly IMemoryCache _cache;
       // private readonly IMemoryCache _cache;

        public AssignmentController(HttpClient client, ILoggerManager logger, IConfiguration config, IAssignmentRepository assignment, IPayRegisterRepository payRegisterRepository)
        {
            _client = client;
            this._assignment=assignment;
            this._payRegisterRepository=payRegisterRepository;
            this._config=config;
            this._logger=logger;
            //  this._cache=cache; IMemoryCache cache,

        }
        //[HttpPost("clear-cache")]
        //public IActionResult ClearCache()
        //{
        //    _cache.Remove("my-cache-key");
        //    return Ok("Cache cleared.");
        //}

        [HttpGet,Route("GetAssignmentLot/{userId}/{filter}")]
        public IActionResult GetAssignmentLot(int userId, string filter)
        {
            //Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            //Response.Headers["Pragma"] = "no-cache";
            //Response.Headers["Expires"] = "0";
            this._assignment.AutoAllocationLots(userId);
            var lots = this._assignment.GetAssignmentLotByDate(userId, filter);
            return Ok(lots);

        }

        [HttpPost("GetAllotment")]
        public IActionResult GetAllotment(AllotmentRequest allotment)
        {
           
            var allot = this._assignment.GetAllotmentByCompanyCodeLot(allotment.companyCode, allotment.payPeriod, allotment.lot);
            return Ok(allot);
        }
        [HttpPost,Route("LotStatus")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> LotStatus(AllotmentLotStatusRequest lotStatusrequestModel)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var status =await this._assignment.GetLotStatus(lotStatusrequestModel);
            return Ok(status);
        }
        [HttpPost,Route("UserLotStatusValidation")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> UserLotStatusValidation(UserLotValidationRequest userLotValidationRequest)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
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

        [HttpGet,Route("AutoAllomentByUserId/{userId}")]
        public async Task<IActionResult> AutoAllomentByUserId(int userId)
        {
            var res =await this._assignment.AutoAllocationByUser(userId);
            return Ok(res);
        }
       

        [HttpPost]
        [Route("QCLotVerify")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> QCLotVerify(QCApprovedRequest lotStatusrequestModel)
        {
            // Prevent caching at response level
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

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
                 this._logger.LogInfo("QC Verify Request");
                FileResponse fileResponse = new FileResponse();
                PayRegisterRequest payRegisterRequest = new PayRegisterRequest() { companycode=lotStatusrequestModel.Company_Id, pay_period_Id=lotStatusrequestModel.pay_period_id, lotNumber=lotStatusrequestModel.lotnumber };
                var comayName = _payRegisterRepository.CompanyNameByCode(lotStatusrequestModel.Company_Id);
                var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                //if(lotStatusrequestModel.Payroll_Input_Type=="Other Input")
                //{
                //    fileResponse = this._payRegisterRepository.GetOtherIncomePayRegister(lotStatusrequestModel.Company_Id, lotStatusrequestModel.pay_period_id, lotStatusrequestModel.lotnumber);
                //}
                //else if (lotStatusrequestModel.Payroll_Input_Type == "External Payregister")
                //{
                //    fileResponse = this._payRegisterRepository.ExternalPayRegister(lotStatusrequestModel.Company_Id, lotStatusrequestModel.pay_period_id);
                //}
                //else
                //{
                //    fileResponse = this._payRegisterRepository.PayRegisterDownload(lotStatusrequestModel.Company_Id, lotStatusrequestModel.pay_period_id, lotStatusrequestModel.lotnumber, lotStatusrequestModel.Pay_Period);
                //}
                 fileResponse = lotStatusrequestModel.Payroll_Input_Type switch
                {
                    "Other Input" => _payRegisterRepository.GetQCOtherIncomePayRegister(
                                        lotStatusrequestModel.Company_Id,
                                        lotStatusrequestModel.pay_period_id,
                                        lotStatusrequestModel.lotnumber,lotStatusrequestModel.Pay_Period, lotStatusrequestModel.CompanyCode),
                    "Revised Other Input" => _payRegisterRepository.GetQCOtherIncomePayRegister(
                    lotStatusrequestModel.Company_Id,
                    lotStatusrequestModel.pay_period_id,
                    lotStatusrequestModel.lotnumber, lotStatusrequestModel.Pay_Period, lotStatusrequestModel.CompanyCode),

                    "External Payregister" => _payRegisterRepository.ExternalPayRegister(
                                        lotStatusrequestModel.Company_Id,
                                        lotStatusrequestModel.pay_period_id),

                    _ => _payRegisterRepository.PayRegisterDownload(
                                        lotStatusrequestModel.Company_Id,
                                        lotStatusrequestModel.pay_period_id,
                                        lotStatusrequestModel.lotnumber,
                                        lotStatusrequestModel.Pay_Period,0)
                };



                if (fileResponse.File!="No")
                {
                    this._logger.LogInfo("Pay Register Generated");
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
                    
                    this._logger.LogInfo("Qzone upload started");
                    var companyPath = Path.Combine(_config["FilePath"].ToString(), payRegisterUploadModel.CompanyCode);
                    var payperiodPath = Path.Combine(companyPath, payRegisterUploadModel.Pay_Period);
                    this._logger.LogInfo("File path get from payperiodPath " + payperiodPath);
                    var filePath = Path.Combine(payperiodPath, payRegisterUploadModel.LotNumber.ToString());
                    this._logger.LogInfo("File path get from filePath " + payperiodPath);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var bytes = Convert.FromBase64String(payRegisterUploadModel.Docs);
                    string fileExtention = Path.GetExtension(payRegisterUploadModel.FileName.ToUpper());
                    string fileName = string.Format("{0}_{1}_{2}_{3}_{4}{5}",
                      payRegisterUploadModel.CompanyCode,
                      comapny.Client_Name,
                      payRegisterUploadModel.Input_type,
                      payRegisterUploadModel.LotNumber,
                      DateTime.Now.ToString("_yyyyMMddhhmmssffff"),
                      fileExtention);
                    
                    filePath = filePath + "\\" + fileName;
                    this._logger.LogInfo("Folder Created");
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    this._logger.LogInfo("File converted base64 to byte");
                    payRegisterUploadModel.FilePath = filePath;

                    
                    PayRegisterUI payRegisterUI = new PayRegisterUI()
                    {
                        CompanyCode = payRegisterUploadModel.CompanyId,
                        Pay_Period_id = payRegisterUploadModel.Pay_Period_id,
                        LotNumber = payRegisterUploadModel.LotNumber,
                        FilePath = filePath,
                        LoginUser = payRegisterUploadModel.LoginUser,
                        Input_type = payRegisterUploadModel.Input_type,
                        FileName = fileName

                    };
                    this._logger.LogInfo("DB Updated request");
                    var  payRegisterResponse = await this._payRegisterRepository.PayRegisterUpload(payRegisterUI);
                    this._logger.LogInfo("DB Updated Completed");
                    // PayRegisterAutoUpload(payRegisterUploadModel);
                    //var requestJsonContent = System.Text.Json.JsonSerializer.Serialize(payRegisterUploadModel);
                    //var requestStringContents = new StringContent(requestJsonContent, Encoding.UTF8, "application/json");
                    //var status = await Payregisterupload(requestStringContents);
                    this._logger.LogInfo("Qzone upload completed");
                    if (allotmentLotStatus.QC_Verified_Status)
                    {
                        this._assignment.AutoAllocationLots(lotStatusrequestModel.userId);
                    }
                    this._logger.LogInfo("Next Lot Allocation Completed ");
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

        [HttpPost]
        [Route("AutoQCLotVerify")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> AutoQCLotVerify(QCApprovedRequest lotStatusrequestModel)
        {
            // Prevent caching at response level
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

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




            if (lotStatusrequestModel.UpdateStatus == "Q")
            {
                this._logger.LogInfo("QC Verify Request");
                FileResponse fileResponse = new FileResponse();
                PayRegisterRequest payRegisterRequest = new PayRegisterRequest() { companycode = lotStatusrequestModel.Company_Id, pay_period_Id = lotStatusrequestModel.pay_period_id, lotNumber = lotStatusrequestModel.lotnumber };
                var comayName = _payRegisterRepository.CompanyNameByCode(lotStatusrequestModel.Company_Id);
                var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                
                fileResponse = lotStatusrequestModel.Payroll_Input_Type switch
                {
                    "Other Input" => _payRegisterRepository.GetQCOtherIncomePayRegister(
                                        lotStatusrequestModel.Company_Id,
                                        lotStatusrequestModel.pay_period_id,
                                        lotStatusrequestModel.lotnumber, lotStatusrequestModel.Pay_Period, lotStatusrequestModel.CompanyCode),
                    "Revised Other Input" => _payRegisterRepository.GetQCOtherIncomePayRegister(
                    lotStatusrequestModel.Company_Id,
                    lotStatusrequestModel.pay_period_id,
                    lotStatusrequestModel.lotnumber, lotStatusrequestModel.Pay_Period, lotStatusrequestModel.CompanyCode),

                    "External Payregister" => _payRegisterRepository.ExternalPayRegister(
                                        lotStatusrequestModel.Company_Id,
                                        lotStatusrequestModel.pay_period_id),

                    _ => _payRegisterRepository.PayRegisterDownload(
                                        lotStatusrequestModel.Company_Id,
                                        lotStatusrequestModel.pay_period_id,
                                        lotStatusrequestModel.lotnumber,
                                        lotStatusrequestModel.Pay_Period, 0)
                };



                if (fileResponse.File != "No")
                {
                    this._logger.LogInfo("Pay Register Generated");
                    foreach (var item in lotStatusrequestModel.allotments)
                    {
                        QCVerifyModelRequest modelRequest = new QCVerifyModelRequest()
                        {
                            InputLot_Id = item.InputLot_Id,
                            Company_Id = lotStatusrequestModel.Company_Id,
                            pay_period_id = lotStatusrequestModel.pay_period_id,
                            lotnumber = lotStatusrequestModel.lotnumber,
                            UpdateStatus = lotStatusrequestModel.UpdateStatus,
                            Payroll_Input_Type = lotStatusrequestModel.Payroll_Input_Type,
                            createdon = lotStatusrequestModel.createdon,
                            Remarks = item.Remarks,
                            RequestForModification = lotStatusrequestModel.UpdateStatus == "Q" ? false : true,
                            QC_RaiseQuery = lotStatusrequestModel.QC_RaiseQuery


                        };
                        var QC_Status = this._assignment.QCVerfyOrModification(modelRequest);
                    }
                    allotmentLotStatus = this._assignment.GetLotStatus(lotStatusUI).Result;
                    allotmentLotStatus.fileResponse = fileResponse;
                    PayRegisterUploadModel payRegisterUploadModel = new PayRegisterUploadModel()
                    {
                        CompanyId = lotStatusrequestModel.Company_Id,
                        CompanyCode = lotStatusrequestModel.CompanyCode,
                        Pay_Period_id = lotStatusrequestModel.pay_period_id,
                        Pay_Period = lotStatusrequestModel.Pay_Period,
                        LotNumber = lotStatusrequestModel.lotnumber,
                        FilePath = "",
                        FileName = fileResponse.FileName,
                        FileType = ".xlsx",
                        LoginUser = lotStatusrequestModel.userId.ToString(),
                        Input_type = lotStatusrequestModel.Payroll_Input_Type,
                        Docs = fileResponse.File
                    };

                    this._logger.LogInfo("Qzone upload started");
                    var companyPath = Path.Combine(_config["FilePath"].ToString(), payRegisterUploadModel.CompanyCode);
                    var payperiodPath = Path.Combine(companyPath, payRegisterUploadModel.Pay_Period);
                    this._logger.LogInfo("File path get from payperiodPath " + payperiodPath);
                    var filePath = Path.Combine(payperiodPath, payRegisterUploadModel.LotNumber.ToString());
                    this._logger.LogInfo("File path get from filePath " + payperiodPath);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var bytes = Convert.FromBase64String(payRegisterUploadModel.Docs);
                    string fileExtention = Path.GetExtension(payRegisterUploadModel.FileName.ToUpper());
                    string fileName = string.Format("{0}_{1}_{2}_{3}_{4}{5}",
                      payRegisterUploadModel.CompanyCode,
                      comapny.Client_Name,
                      payRegisterUploadModel.Input_type,
                      payRegisterUploadModel.LotNumber,
                      DateTime.Now.ToString("_yyyyMMddhhmmssffff"),
                      fileExtention);

                    filePath = filePath + "\\" + fileName;
                    this._logger.LogInfo("Folder Created");
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    this._logger.LogInfo("File converted base64 to byte");
                    payRegisterUploadModel.FilePath = filePath;


                    PayRegisterUI payRegisterUI = new PayRegisterUI()
                    {
                        CompanyCode = payRegisterUploadModel.CompanyId,
                        Pay_Period_id = payRegisterUploadModel.Pay_Period_id,
                        LotNumber = payRegisterUploadModel.LotNumber,
                        FilePath = filePath,
                        LoginUser = payRegisterUploadModel.LoginUser,
                        Input_type = payRegisterUploadModel.Input_type,
                        FileName = fileName

                    };
                    this._logger.LogInfo("DB Updated request");
                    var payRegisterResponse = await this._payRegisterRepository.PayRegisterUpload(payRegisterUI);
                    this._logger.LogInfo("DB Updated Completed");
                    // PayRegisterAutoUpload(payRegisterUploadModel);
                    //var requestJsonContent = System.Text.Json.JsonSerializer.Serialize(payRegisterUploadModel);
                    //var requestStringContents = new StringContent(requestJsonContent, Encoding.UTF8, "application/json");
                    //var status = await Payregisterupload(requestStringContents);
                    this._logger.LogInfo("Qzone upload completed");
                    //if (allotmentLotStatus.QC_Verified_Status)
                    //{
                    //    this._assignment.AutoAllocationLots(lotStatusrequestModel.userId);
                    //}
                    this._logger.LogInfo("Next Lot Allocation Completed ");
                    return Ok(allotmentLotStatus);
                }
                else
                {
                    foreach (var item in lotStatusrequestModel.allotments)
                    {
                        QCVerifyModelRequest modelRequest = new QCVerifyModelRequest()
                        {
                            InputLot_Id = item.InputLot_Id,
                            Company_Id = lotStatusrequestModel.Company_Id,
                            pay_period_id = lotStatusrequestModel.pay_period_id,
                            lotnumber = lotStatusrequestModel.lotnumber,
                            UpdateStatus = lotStatusrequestModel.UpdateStatus,
                            Payroll_Input_Type = lotStatusrequestModel.Payroll_Input_Type,
                            createdon = lotStatusrequestModel.createdon,
                            Remarks = item.Remarks,
                            RequestForModification = lotStatusrequestModel.UpdateStatus == "Q" ? false : true,
                            QC_RaiseQuery = lotStatusrequestModel.QC_RaiseQuery


                        };
                        var QC_Status = this._assignment.QCVerfyOrModification(modelRequest);
                    }
                    allotmentLotStatus.fileResponse = fileResponse;
                }

            }
            else
            {
                foreach (var item in lotStatusrequestModel.allotments)
                {
                    QCVerifyModelRequest modelRequest = new QCVerifyModelRequest()
                    {
                        InputLot_Id = item.InputLot_Id,
                        Company_Id = lotStatusrequestModel.Company_Id,
                        pay_period_id = lotStatusrequestModel.pay_period_id,
                        lotnumber = lotStatusrequestModel.lotnumber,
                        UpdateStatus = lotStatusrequestModel.UpdateStatus,
                        Payroll_Input_Type = lotStatusrequestModel.Payroll_Input_Type,
                        createdon = lotStatusrequestModel.createdon,
                        Remarks = item.Remarks,
                        RequestForModification = lotStatusrequestModel.UpdateStatus == "Q" ? false : true

                    };
                    var QC_Status = this._assignment.QCVerfyOrModification(modelRequest);
                    allotmentLotStatus = this._assignment.GetLotStatus(lotStatusUI).Result;
                }
            }

            return Ok(allotmentLotStatus);
        }
        [HttpGet, Route("AllottmentRevokDetail/{userId}")]
        public async Task<IActionResult> AllottmentRevokDetail(int userId)
        {
            if (userId > 0)
            {
             var revok= await  this._assignment.AllottmentRevokDetail(userId);
                return Ok(revok);
            }

            return Ok();
        }

        [HttpPost,Route("AssignmentRevok")]
        public async Task<IActionResult> AssignmentRevok(AllotmentRevok allotmentRevok)
        {
            var revok = await this._assignment.AssignmentRevok(allotmentRevok);
            return Ok(revok);
        }

        public async Task<PayRegisterResponse> PayRegisterAutoUpload(PayRegisterUploadModel payRegisterUpload)
        {
            PayRegisterResponse payRegisterResponse = new PayRegisterResponse();
            try
            {
                if (payRegisterUpload != null)
                {
                    this._logger.LogInfo("From PSD Payregister Received json" + JsonConvert.SerializeObject(payRegisterUpload));
                    var bytes = Convert.FromBase64String(payRegisterUpload.Docs);
                    this._logger.LogInfo("File Name extension");
                    var comayName = _payRegisterRepository.CompanyNameByCode(payRegisterUpload.CompanyId);
                    var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                    string fileExtention = Path.GetExtension(payRegisterUpload.FileName.ToUpper());

                    string fileName = string.Format("{0}_{1}_{2}_{3}_{4}{5}",
                        payRegisterUpload.CompanyCode,
                        comapny.Client_Name,
                        payRegisterUpload.Input_type,
                        payRegisterUpload.LotNumber,
                        DateTime.Now.ToString("_yyyyMMddhhmmssffff"),
                        fileExtention);
                    this._logger.LogInfo("File Name With Extension" + fileName);
                    //  var filepaths = "\\\\stgqcpsftpstorg.file.core.windows.net\\sftpstorage\\APP_Data\\QZone\\CApplication_Documents\\Application_Documents\\ClaimDocPath\\\"";
                    //_configuration["FilePath"].ToString()

                    this._logger.LogInfo("File path get from Configfile ");
                    var companyPath = Path.Combine(_config["FilePath"].ToString(), payRegisterUpload.CompanyCode);
                    this._logger.LogInfo("File path get from Configfile " + companyPath);

                    var payperiodPath = Path.Combine(companyPath, payRegisterUpload.Pay_Period);
                    this._logger.LogInfo("File path get from payperiodPath " + payperiodPath);
                    var filePath = Path.Combine(payperiodPath, payRegisterUpload.LotNumber.ToString());
                    this._logger.LogInfo("File path get from filePath " + payperiodPath);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    //Directory.CreateDirectory(filePath);
                    filePath = filePath + "\\" + fileName;
                    this._logger.LogInfo("Folder Created");
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    this._logger.LogInfo("File converted base64 to byte");
                    payRegisterUpload.FilePath = filePath;

                    PayRegisterUI payRegisterUI = new PayRegisterUI()
                    {
                        CompanyCode = payRegisterUpload.CompanyId,
                        Pay_Period_id = payRegisterUpload.Pay_Period_id,
                        LotNumber = payRegisterUpload.LotNumber,
                        FilePath = filePath,
                        LoginUser = payRegisterUpload.LoginUser,
                        Input_type = payRegisterUpload.Input_type,
                        FileName = fileName

                    };
                    this._logger.LogInfo("DB Updated request");
                    payRegisterResponse =await this._payRegisterRepository.PayRegisterUpload(payRegisterUI);
                    this._logger.LogInfo("DB Updated Completed");

                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format("Stack Trace :{0} , InnerException : {1} , Message : {2}", ex.StackTrace, ex.InnerException, ex.Message));
            }

            return payRegisterResponse;
        }

        public async Task<bool> Payregisterupload(HttpContent requestStringContents)
        {
            try
            {
                var uri = _config["ApiURL"] + "PayRegister/PayRegisterAutoUpload";
                _logger.LogInfo("Initiating PayRegister upload to URL: {Uri} "+ uri);

                using var httpResponse = await _client.PostAsync(uri, requestStringContents);
                var content = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Upload failed. StatusCode: {StatusCode}, Response: {ResponseContent}"
                        );
                    return false;
                }

                var response = System.Text.Json.JsonSerializer.Deserialize<PayRegisterResponse>(content);

                // Optional: validate the response object
                if (response == null)
                {
                   this. _logger.LogDebug("Deserialized PayRegisterResponse is null.");
                    return false;
                }

                _logger.LogInfo("PayRegister upload succeeded.");
                return true;
            }
            catch (HttpRequestException httpEx)
            {
               this._logger.LogDebug(httpEx.ToString());
                return false;
            }
            catch (Exception ex)
            {
                this._logger.LogDebug(ex.Message);
                return false;
            }
        }


        [HttpPost,Route("UserEstimateLotValidation")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> UserEstimateLotValidation(LotValidationRequest lotValidationRequest)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            var usersvalidation = await this._assignment.UserEstimateLotValidation(lotValidationRequest);
            return Ok(usersvalidation);
        }

        [HttpPost, Route("UserEstimateLotValidationAdd")]
        public async Task<ActionResult> UserEstimateLotValidationAdd(LotValidationRequest lotValidationRequest)
        {
            var usersvalidation = await this._assignment.UserEstimateLotValidationLog(lotValidationRequest);
            return Ok(usersvalidation);
        }

        [HttpPost,Route("SendFeedBackMail")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> SendFeedBackMail(FeedBackMailRequest feedBackMailRequest)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
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
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult RequestForModification(RequestForModificationModel requestForModification)
        {
            return Ok();
        }
        [HttpPost,Route("InputLotAllDownload")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult InputLotAllDownload(InputLotDownloadModel inputLotDownloadModel)
        {
            //int inputType = 1;
            int inputType = inputLotDownloadModel.InputType switch
            {
                "Salary" => 1,
                "Other Input" => 2,
                "Revised" => 3,
                "Revised Other Input" => 4,
                _ => 0 
            };
            DataTable input = _assignment.GetInputLots(inputLotDownloadModel.companycode, inputLotDownloadModel.pay_period_id, inputLotDownloadModel.lotNumber, inputType);

            using var workbook = new XLWorkbook();
            {
                var ws = workbook.AddWorksheet(input, "Sheet1");
               
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
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult InputLotDownload(InputLotDownloadModel inputLotDownloadModel)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            int inputType = inputLotDownloadModel.InputType?.ToLower() switch
            {
                "salary" => 1,
                "other input" => 2,
                "revised" => 3,
                "revised other input" => 4,
                "external payregister"=>5,
                _ => 0
            };
            DataSet input = _assignment.GetInputLot(inputLotDownloadModel.companycode, inputLotDownloadModel.pay_period_id, inputLotDownloadModel.lotNumber, inputType);

            using var workbook = new XLWorkbook();
            {
                for (int i = 0; i < input.Tables.Count; i++)
                {

                    var ws = workbook.AddWorksheet(input.Tables[i], GetSheetName(i, inputType));
                    ws.Table(0).ShowAutoFilter = false;
                    ws.Table(0).Theme = XLTableTheme.None;
                }
                //if (inputType == 2)
                //{

                //    var ws = workbook.AddWorksheet(input.Tables[0], "Other Input");
                //    ws.Table(0).ShowAutoFilter = false;
                //    ws.Table(0).Theme = XLTableTheme.None;
                //}
                //else
                //{

                //}

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
        public string GetSheetName(int i,int inputType)
        {
            string sheetName = "Input";

            switch (inputType)
            {
                case 1:
                    sheetName = i switch
                    {
                        0 => "New Joinee Employee id Creation",
                        1 => "New Joinee Breakup",
                        2 => "Attendance",
                        3 => "Adhoc Or Pay Transaction",
                        4 => "Increment Break up",
                        5 => "LOP Details",
                        6 => "New Joinee LOP Details",
                        7 => "ds7",
                        8 => "ds8",
                        _ => ""
                    };
                    break;

                case 2:
                    sheetName = i == 0 ? "Other Input" : $"Other Input{i}";
                    break;

                case 3:
                    sheetName = i switch
                    {
                        0 => "Attendance",
                        1 => "Adhoc Or Pay Transaction",
                        2 => "IncrementDetails",
                        3 => "LOP Details",
                        4 => "NewJoinee",
                        5 => "NewJoineeBreakup",
                        _ => $"Other Input{i}"
                    };
                    break;

                case 4:
                    sheetName = i == 0 ? "Other Input" : $"Other Input{i}";
                    break;
            }
            return sheetName;

        }
    }
}
