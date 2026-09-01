using QPay.BAL.IRepository.PurchaseOrder;
using QPay.DAL.Repository;
using QPay.UI.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.PurchaseOrder
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {

        private readonly DbRepository _dbRepository;
        public PurchaseOrderRepository(DbRepository dbRepository) {
        this._dbRepository = dbRepository;
        }    

        public DataSet POSearch(PurchaseOrderRequest purchaseOrderRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = purchaseOrderRequest.Action,
                ["@CompanyId"] = purchaseOrderRequest.Company_Id,
                ["@Purchase_Request_No"] = purchaseOrderRequest.Purchase_Request_No,
                ["@Purchase_Order_Id"] = purchaseOrderRequest.Purchase_Order_Id,
                ["@PODateFrom"] = purchaseOrderRequest.PODateFrom,
                ["@PODateTo"] = purchaseOrderRequest.PODateTo,
                ["@PageNo"] = purchaseOrderRequest.PageNo,
                ["@PageSize"] = purchaseOrderRequest.PageSize,
                ["@SortField"] = purchaseOrderRequest.SortField,
                ["@SortDirection"] = purchaseOrderRequest.SortDirection,
                ["@TotalCount"] = purchaseOrderRequest.TotalCount

            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPurchaseOrder_NewUI", parameters, 1500);

        }
        public PurchaseOrderResponse CreateUpdateDelete(
      PurchaseOrderRequest purchaseOrderRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = purchaseOrderRequest.Action,
                ["@PurchaseOrderID"] = purchaseOrderRequest.Purchase_Order_Id,
                ["@Company_Id"] = purchaseOrderRequest.Company_Id,
                ["@PO_Date"] = purchaseOrderRequest.PO_Date,
                ["@PO_Based_On"] = purchaseOrderRequest.PO_Based_On,
                ["@Purchase_Request_No"] = purchaseOrderRequest.Purchase_Request_No,
                ["@PO_Amount"] = purchaseOrderRequest.PO_Amount,
                ["@PO_Valid_From"] = purchaseOrderRequest.PO_Valid_From,
                ["@PO_Valid_To"] = purchaseOrderRequest.PO_Valid_To,
                ["@Remarks"] = purchaseOrderRequest.Remarks,
                ["@IsActive"] = purchaseOrderRequest.IsActive,
                ["@CreatedBy"] = purchaseOrderRequest.CreatedBy,
                ["@ModifiedBy"] = purchaseOrderRequest.ModifiedBy
            };

            DataSet result = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_PurchaseOrderCreateUpdateDelete",
                parameters,
                1500);

            string errorMessage = "";

            if (result != null &&
                result.Tables.Count > 0 &&
                result.Tables[0].Rows.Count > 0)
            {
                errorMessage = result.Tables[0]
                    .Rows[0]["Error_Message"]?
                    .ToString() ?? "";
            }

            return new PurchaseOrderResponse
            {
                Success = true,
                Error_Message = errorMessage
            };
        }


    }
}
