using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.BAL.IRepository.PurchaseOrder;
using QPay.UI.Models.PurchaseOrder;
using System.Data;

namespace QPay.API.Controller.PurchaseOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        public PurchaseOrderController(IPurchaseOrderRepository purchaseOrderRepository) { 
        this._purchaseOrderRepository = purchaseOrderRepository;
        }

        [HttpPost]
        [Route("POSearch")]
        public IActionResult POSearch(PurchaseOrderRequest purchaseOrderRequest)
        {
            var res=_purchaseOrderRepository.POSearch(purchaseOrderRequest);
            var payload = ResponseWrapManager.ResponseWrapper(res, HttpContext);
            return Ok(payload);
        }
        [HttpPost]
        [Route("ExportExcel")]
        public IActionResult ExportExcel(PurchaseOrderRequest purchaseOrderRequest)
        {
            var res = _purchaseOrderRepository.POSearch(purchaseOrderRequest);
            DataTable item = res.Tables[0];

            List<PurchaseOrderViewModel> purchaseOrderViewModels = item.AsEnumerable()
                .Select(reader => new PurchaseOrderViewModel
                {
                    TotalCount = reader.Field<int>("TotalCount"),
                    SLNo = reader.Field<int>("SLNo"),

                    Purchase_Order_Id = reader.Field<int>(
                        Column_Constants.Column_Name_Purchase_Order_Id),

                    Company_Id = reader.Field<int?>(
                        Column_Constants.Column_Name_Company_Id),

                    Company_Code = reader.Field<string>(
                        Column_Constants.Column_Name_Company_Code),

                    Client_Id = reader.Field<int?>(
                        Column_Constants.Column_Name_Client_Id),

                    Company_Name = reader.Field<string>(
                        Column_Constants.Column_Name_Company_Name),

                    Client_PO_Ref_No = reader.Field<string>(
                        Column_Constants.Column_Name_Client_PO_Ref_No),

                    PO_Date = reader.Field<DateTime>(
                        Column_Constants.Column_Name_PO_Date),

                    Purchase_Request_No = reader.Field<string>(
                        Column_Constants.Column_Name_Purchase_Request_No),

                    PO_Amount = reader.Field<decimal>(
                        Column_Constants.Column_Name_PO_Amount),

                    PO_Valid_From = reader.Field<DateTime>(
                        Column_Constants.Column_Name_PO_Valid_From),

                    PO_Valid_To = reader.Field<DateTime>(
                        Column_Constants.Column_Name_PO_Valid_To),

                    Invoiced_Amount = reader.Field<decimal>(
                        Column_Constants.Column_Name_Invoiced_Amount),

                    Transfered_Amount = reader.Field<decimal>(
                        Column_Constants.Column_Name_Transfered_Amount),

                    PO_Based_On = reader.Field<int?>(
                        Column_Constants.Column_Name_PO_Based_On),

                    Remarks = reader.Field<string>(
                        Column_Constants.Column_Name_Remarks),

                    City_Name = reader.Field<string>(
                        Column_Constants.Column_Name_City_Name),

                    IsActive = reader.Field<bool>(
                        Column_Constants.TableConstant_IsActive),

                    CompanyGroup_Id = reader.Field<int>("CompanyGroupId"),

                    CompanyGroupCode = reader.Field<string>("CompanyGroupCode"),

                    IsCompanyGroupId = reader.Field<int>("IsCompanyGroupId")
                })
                .ToList();

            return Ok(purchaseOrderViewModels);
        }


[HttpPost("CreateUpdateDelete")]
public IActionResult CreateUpdateDelete([FromBody] PurchaseOrderRequest request)
        {
            try
            {
                var response = _purchaseOrderRepository.CreateUpdateDelete(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new PurchaseOrderResponse
                {
                    Success = false,
                    Error_Message = ex.Message
                });
            }
        }


      
    }
}
