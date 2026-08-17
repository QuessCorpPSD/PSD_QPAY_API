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
                ["@Action"] = "GET",
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
    }
}
