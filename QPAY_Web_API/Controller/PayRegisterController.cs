using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.UI.Models;
using System.Buffers.Text;
using System.Collections;
using System.Data;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PayRegisterController : ControllerBase
    {
        private readonly IPayRegisterRepository _payRegisterRepository;
        private readonly IConfiguration _configuration;

        public PayRegisterController(IPayRegisterRepository payRegisterRepository, IConfiguration configuration)
        {
            this._payRegisterRepository=payRegisterRepository;
            _configuration = configuration;
        }

        [HttpPost("PayRegisterUpload")]
        public IActionResult PayRegisterUpload(PayRegisterUploadModel payRegisterUpload)
        {
            PayRegisterResponse payRegisterResponse = new PayRegisterResponse();
            if (payRegisterUpload!=null)
            {
                var bytes = Convert.FromBase64String(payRegisterUpload.Docs);

                string fileExtention = Path.GetExtension(payRegisterUpload.FileName.ToUpper());
                string fileName = string.Format("{0}{1}{2}", Path.GetFileNameWithoutExtension(payRegisterUpload.FileName.ToUpper()), DateTime.Now.ToString("_yyyyMMddhhmmssffff"), fileExtention);

                var companyPath = Path.Combine(_configuration["FilePath"].ToString(), payRegisterUpload.CompanyCode);
                var payperiodPath = Path.Combine(companyPath, payRegisterUpload.Pay_Period);
                var filePath = Path.Combine(payperiodPath, payRegisterUpload.LotNumber.ToString());
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                //Directory.CreateDirectory(filePath);
                filePath =filePath+"\\"+fileName;
                
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
                payRegisterUpload.FilePath=filePath;

                PayRegisterUI payRegisterUI = new PayRegisterUI()
                {
                    CompanyCode=payRegisterUpload.CompanyId,
                    Pay_Period_id=payRegisterUpload.Pay_Period_id,
                    LotNumber=payRegisterUpload.LotNumber,
                    FilePath=filePath,
                    LoginUser=payRegisterUpload.LoginUser,
                    Input_type=payRegisterUpload.Input_type,
                    FileName=fileName

                };

                var staus = this._payRegisterRepository.PayRegisterUpload(payRegisterUI);
            }
            return Ok(payRegisterResponse);
        }

        [HttpPost, Route("OutFileDownload")]
        public async Task<IActionResult> OutFileDownload(PayRegisterQzoneRequest registerRequest)
        {

            if (registerRequest.payroll_input_type == "Q")
            {
                var filename = this._payRegisterRepository.GetFileNameFromQzone(registerRequest.companyId, registerRequest.pay_period_Id, registerRequest.lotNumber);
                if(filename.FileName=="")
                {
                    var fileResponse = new FileResponse
                    {
                        FileName =string.Format("Lot Number {0} not available in QZone", registerRequest.lotNumber),
                        File = "No"
                    };
                }
                string path = string.Format("{0}/{1}/{2}/{3}/{4}",
                _configuration["FilePath"],
                registerRequest.companycode,
                registerRequest.pay_period,
                registerRequest.lotNumber,
                filename.FileName);
                

                    if (System.IO.File.Exists(path))
                    {
                        byte[] fileBytes =await  System.IO.File.ReadAllBytesAsync(path);
                        string base64 = Convert.ToBase64String(fileBytes);

                        var fileResponse = new FileResponse
                        {
                            FileName = "PayRegister.xlsx",
                            File = base64
                        };

                        return Ok(fileResponse); // or return it in your response
                    }
                    else
                    {
                        var fileResponse = new FileResponse
                        {
                            File = "No",
                            FileName = "File Not Existing from Qzone Application"
                        };
                        return Ok(fileResponse);
                    }




                    //Directory.CreateDirectory(path);
              
               


            }
            else
            {
                var fileResponse = new FileResponse
                {
                    File = "No",
                    FileName = "Directory not exists"
                };
                return Ok(fileResponse);
            }

            




        }

        [HttpPost,Route("GetPayRegister")]
        public IActionResult PayRegister(PayRegisterRequest registerRequest)
        {

            var comayName = _payRegisterRepository.CompanyNameByCode(registerRequest.companycode);
            var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
            var register = this._payRegisterRepository.PayRegisterDownload(registerRequest.companycode, registerRequest.pay_period_Id,registerRequest.lotNumber, registerRequest.pay_period);
            return Ok(register);
            //using var workbook = new XLWorkbook();
            //{               
            //    var ws = workbook.AddWorksheet(register,"PayRegister");
            //    ws.Table(0).ShowAutoFilter = false;
            //    ws.Table(0).Theme = XLTableTheme.None;
            //    if (register.Columns.Count>1)
            //    {

            //        ws.Row(1).InsertRowsAbove(1);
            //        ws.Range("A1:B1").Merge();
            //        ws.Cell(1, 1).Value = comapny.Client_Name;
            //        ws.Cell(1, 1).Style.Font.Bold=true;

            //        var ctc = register.AsEnumerable().Sum(row => row.Field<double?>("TOTAL COST TO COMPANY"));
            //        var service = register.AsEnumerable().Sum(row => row.Field<double?>("Service_charge"));
            //        if (ctc!=null && service!=null)
            //        {
            //            var Total = ctc+service;
            //            var toal_GST = Total*(18.0/100.0);

            //            int lastRow = ws.LastRowUsed().RowNumber();
            //            ws.Cell(lastRow + 3, 4).Value = comapny.Client_Name;

            //            ws.Cell(lastRow, 1).Value = "Grand Total";

            //            var clinet_cell = ws.Cell(lastRow + 3, 4);
            //            clinet_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //            clinet_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //            clinet_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //            clinet_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;



            //            ws.Cell(lastRow + 3, 5).Value = ctc;
            //            var ctc_cell = ws.Cell(lastRow + 3, 5);
            //            ctc_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //            ctc_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //            ctc_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //            ctc_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //            ws.Cell(lastRow + 4, 4).Value = "Service Charge:";
            //            var Service_cell = ws.Cell(lastRow + 4, 4);
            //            Service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //            Service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //            Service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //            Service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //            ws.Cell(lastRow + 4, 5).Value = service;
            //            var service_cell = ws.Cell(lastRow + 4, 5);
            //            service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //            service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //            service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //            service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //            ws.Cell(lastRow + 5, 4).Value = "";
            //            var empty_cell = ws.Cell(lastRow + 5, 4);
            //            empty_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //            empty_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //            empty_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //            empty_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //            ws.Cell(lastRow + 5, 5).Value = Total;
            //            var Total_cell = ws.Cell(lastRow + 5, 5);
            //            Total_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //            Total_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //            Total_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //            Total_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //            ws.Cell(lastRow + 6, 4).Value = "Total";
            //            var Total1_cell = ws.Cell(lastRow + 6, 4);
            //            Total1_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //            Total1_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //            Total1_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //            Total1_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

            //            ws.Cell(lastRow + 6, 5).Value = toal_GST;
            //            var toal_GST_cell = ws.Cell(lastRow + 6, 5);
            //            toal_GST_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            //            toal_GST_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            //            toal_GST_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            //            toal_GST_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;


            //        }

            //    }
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        workbook.SaveAs(stream);
            //        var bytes =Convert.ToBase64String(stream.ToArray());
            //        FileResponse fileResponse = new FileResponse();
            //        fileResponse.FileName="PayRegister";
            //        fileResponse.File=bytes;

            //        return Ok(fileResponse);//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
            //    }
            //}




        }

        [HttpPost, Route("GetReconPayRegister")]
        public IActionResult GetReconPayRegister(PayRegisterRequest registerRequest)
        {

            //var comayName = _payRegisterRepository.CompanyNameByCode(registerRequest.companycode);
            //var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
            if(registerRequest.payroll_input_type=="Other Input")
            {
                var register = this._payRegisterRepository.GetQCOtherIncomePayRegister(registerRequest.companycode, registerRequest.pay_period_Id, registerRequest.lotNumber, registerRequest.pay_period) ;
                return Ok(register);
            }
            else
            {
                var register = this._payRegisterRepository.ReconPayRegister(registerRequest.companycode, registerRequest.pay_period_Id, registerRequest.lotNumber);
                return Ok(register);
            }
            
          



        }
    }
}
