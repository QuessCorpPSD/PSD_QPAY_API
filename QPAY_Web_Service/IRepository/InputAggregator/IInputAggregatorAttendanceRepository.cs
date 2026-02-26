using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Common;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Aggregator;
using QPay.UI.Models.TaxAndSaving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IInputAggregatorAttendanceRepository
    {
        Task<DataSet> QuessLeaveMaster();
        Task<DataSet> leaveTypeMaster();
        Task<DataSet> Createleavemapping(AttendanceAggregatorRequest request);

        Task<DataSet> Createleavetype(leaveTypeMasterRequest request);

        Task<DataSet> SearchLeaveTypeMapping(int? companyId);

        Task<DataSet> QuessAttendanceAttributeMaster();

        Task<DataSet> Search(int? companyId);

        Task<RequestResponse> ClientAttributesUpload(IFormFile file, [FromForm] string CreatedBy);

        Task<RequestResponse> AttributesMappingUpload(IFormFile file, [FromForm] string CreatedBy);

        Task<RequestResponse> Upload(IFormFile file, [FromForm] string CreatedBy, [FromForm] string CompanyId);

        Task<DataSet> billableReport(int? companyId, int? payPeriodId);
    }
}
