using System;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.Reports;
using SelectPdf;
using static QPay.UI.Models.Reports.Payslip;

namespace QPay.API.Controller.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayslipReportController : ControllerBase
    {
        private readonly IPayslipReportRepository _ipayslipReportRepository;
        private readonly IConfiguration _configuration;

        public PayslipReportController(
         IPayslipReportRepository ipayslipReportRepository)
        {
            this._ipayslipReportRepository = ipayslipReportRepository;
        }

        [HttpGet]
        [Route("GetEmployee/{companyId}/{payperiodId}")]
        public async Task<IActionResult> GetEmployee(int companyId, int payperiodId)
        {
            var ds = await _ipayslipReportRepository.GetEmployee(companyId, payperiodId);
            var payload = ResponseWrapManager.ResponseWrapper(ds, HttpContext);
            return Ok(payload);
        }

        [HttpGet]
        [Route("DownloadPayslip/{EmployeeId}/{payperiod}")]
        public async Task<PayslipDownloadResponse> DownloadPayslip(int EmployeeId, string payperiod)
        {
            PayslipDownloadResponse payslipdownloadDetails = new PayslipDownloadResponse();
            DataSet ds = await _ipayslipReportRepository.DownloadPayslip(EmployeeId, payperiod);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string html = ds.Tables[0].Rows[0]["HTML"].ToString();

                HtmlToPdf converter = new HtmlToPdf();
                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html);

                using (MemoryStream ms = new MemoryStream())
                {
                    doc.Save(ms);
                    doc.Close();

                    byte[] pdfBytes = ms.ToArray();
                    string base64Pdf = Convert.ToBase64String(pdfBytes);

                    payslipdownloadDetails.response = "Success";
                    payslipdownloadDetails.base64string = base64Pdf;
                    return payslipdownloadDetails;

                }
            }
            else
            {
                payslipdownloadDetails.response = "No Data found";
                return payslipdownloadDetails;
            }
        }

        [HttpGet]
        [Route("DownloadITSheet/{EmployeeId}/{payperiod}")]
        public async Task<PayslipDownloadResponse> DownloadITSheet(int EmployeeId, string payperiod)
        {
            PayslipDownloadResponse payslipdownloadDetails = new PayslipDownloadResponse();
            DataSet ds = await _ipayslipReportRepository.DownloadITSheet(EmployeeId, payperiod);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string html = ds.Tables[0].Rows[0]["HTML"].ToString();

                HtmlToPdf converter = new HtmlToPdf();
                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html);

                using (MemoryStream ms = new MemoryStream())
                {
                    doc.Save(ms);
                    doc.Close();

                    byte[] pdfBytes = ms.ToArray();
                    string base64Pdf = Convert.ToBase64String(pdfBytes);

                    payslipdownloadDetails.response = "Success";
                    payslipdownloadDetails.base64string = base64Pdf;
                    return payslipdownloadDetails;

                }
            }
            else
            {
                payslipdownloadDetails.response = "No Data found";
                return payslipdownloadDetails;
            }
        }

    }
}
