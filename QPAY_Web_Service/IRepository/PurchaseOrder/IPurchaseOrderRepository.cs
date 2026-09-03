using QPay.UI.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.PurchaseOrder
{
    public interface IPurchaseOrderRepository
    {
        DataSet POSearch(PurchaseOrderRequest purchaseOrderRequest);
        PurchaseOrderResponse CreateUpdateDelete(PurchaseOrderRequest purchaseOrderRequest);
    }
}
