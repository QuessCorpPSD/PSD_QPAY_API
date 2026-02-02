using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.PurchaseOrder
{
    public class PoRequest
    {
        public class CreateMainPoRequest
        {
            public int Company_ID { get; set; }
            public int Po_ID { get; set; }
            public string PODate { get; set; } = string.Empty;
            public string StartDate { get; set; } = string.Empty;
            public string ExpDate { get; set; } = string.Empty;
            public string PoNumber { get; set; } = string.Empty;
            public int PricingType { get; set; }
            public string POValue { get; set; } = string.Empty;
            public int CurrencyType { get; set; }
            public int POQuantutyType { get; set; }
            public string POQuantuty { get; set; } = string.Empty;
            public int InteExternal { get; set; }
            public string DocPath { get; set; } = string.Empty;
            public int Extention { get; set; }
            public string ExtendedStartDate { get; set; } = string.Empty;
            public string ExtendedEndDate { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public string BillingType { get; set; } = string.Empty;
            public int ACTION { get; set; }
            public int Po_CategoryID { get; set; }

        }

        public class PoResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class EmployeePoNewCreationModel
        {
            public string POID { get; set; } = "";
            public string Company_ID { get; set; } = "";
            public string Group_Detail_Id { get; set; } = "";
            public string PoNumber { get; set; } = "";
            public string EmployeeListID { get; set; } = "";
            public string ResourceType { get; set; } = "";
            public string StartDate { get; set; } = "";
            public string ExpDate { get; set; } = "";
            public string Duration { get; set; } = "";
            public string ItemType { get; set; } = "";
            public string Balanceamount { get; set; } = "";
            public string EmployeeID { get; set; } = "";
            public string EmployeeName { get; set; } = "";
            public string ClientEmpNo { get; set; } = "";
            public string OfferPOValue { get; set; } = "";
            public string QuantityType { get; set; } = "";
            public string Quantity { get; set; } = "";
            public string CTC { get; set; } = "";
            public string ServiceCharge { get; set; } = "";
            public string FixedRate { get; set; } = "";
            public string UnitPrice { get; set; } = "";
            public string MonthlyRate { get; set; } = "";
            public string Totalresourcevalue { get; set; } = "";
            public string EmployeePOAttachment { get; set; } = "";
            public string CreatedBy { get; set; } = "";
            public string Flag { get; set; } = "";
            public string PricingType { get; set; } = "";
            public string ChkFlag { get; set; } = "";
            public string ExtStartDate { get; set; } = "";
            public string ExtEndDate { get; set; } = "";
            public string ExtDuration { get; set; } = "";

        }

        public class EmployeePoEditModel
        {
            public string StartDate { get; set; } = "";
            public string ExpDate { get; set; } = "";
            public string Duration { get; set; } = "";
            public string ItemType { get; set; } = "";
            public string Balanceamount { get; set; } = "";
            public string QuantityType { get; set; } = "";
            public string Quantity { get; set; } = "";
            public string CTC { get; set; } = "";
            public string ServiceCharge { get; set; } = "";
            public string FixedRate { get; set; } = "";
            public string MonthlyRate { get; set; } = "";
            public string Totalresourcevalue { get; set; } = "";
            public int EmployeeListID { get; set; }
            public string CreatedBy { get; set; } = "";
            public int Flag { get; set; }
        }

        public class EmployeePoInsertModel
        {
            public int POID { get; set; }
            public int Company_ID { get; set; }
            public int Group_Detail_Id { get; set; }
            public string PoNumber { get; set; } = "";
            public int EmployeeListID { get; set; }
            public int ResourceType { get; set; }
            public string StartDate { get; set; } = "";
            public string ExpDate { get; set; } = "";
            public string Duration { get; set; } = "";
            public int ItemType { get; set; }
            public string Balanceamount { get; set; } = "";
            public string EmployeeID { get; set; } = "";
            public string EmployeeName { get; set; } = "";
            public string ClientEmpNo { get; set; } = "";
            public string OfferPOValue { get; set; } = "";
            public int QuantityType { get; set; }
            public int Quantity { get; set; }
            public int CTC { get; set; }
            public string ServiceCharge { get; set; } = "";
            public string FixedRate { get; set; } = "";
            public string UnitPrice { get; set; } = "";
            public string MonthlyRate { get; set; } = "";
            public string Totalresourcevalue { get; set; } = "";
            public string EmployeePOAttachment { get; set; } = "";
            public string CreatedBy { get; set; } = "";
            public int Flag { get; set; }
            public string PricingType { get; set; } = "";
            public int ChkFlag { get; set; }
            public string ExtStartDate { get; set; } = "";
            public string ExtEndDate { get; set; } = "";
            public string ExtDuration { get; set; } = "";

        }
    }
}
