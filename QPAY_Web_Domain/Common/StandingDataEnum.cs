using System.ComponentModel.DataAnnotations;

namespace QPay.UI.Common
{
    public class StandingDataEnum
    {
        public enum ModeOfPayment
        {
            Upfront = 1,
            Collections = 0,
        }

        public enum Yes_No
        {
            Yes = 1,
            No = 0
        }

        public enum ReportType
        {
            Invoice_Report = 1,

            Daily_Collection = 2,

            Invoice_collection_report = 3,

            Day_wise_collection_report = 4,

            Collection_pending_report = 5,

            Invoice_raised_vs_collection_received = 6,

            MTD = 7,
        }

        public enum ModeOfCollection
        {
            RTGS = 0,
            Deposit = 1
        }

        public enum CollectionAgainst
        {
            Reference = 0,
            Deposit = 1
        }

        public enum DuesBasedOn
        {
            SD = 1,
            DR = 0
        }

        public enum Incentivetype
        {
            Quarterly = 1,
            Monthly = 2,
            Yearly = 3,
            Fortnightly = 4
        }

        public enum BillingType
        {
            Exclusive = 1,
            Inclusive = 0
        }

        public enum CustomerType
        {
            GeneralStaffing = 1,
            ITStaffing = 2,
            EDGE = 3,
            ManagedServices = 4
        }

        public enum ReimbursementType
        {
            Quarterly = 1,
            Monthly = 2,
            Yearly = 3,
            Fortnightly = 4,
            Weekly = 5
        }

        public enum SourcingFee
        {
            Percentage = 1,
            Fixed = 0
        }

        public enum POBasedOn
        {
            Request = 1,
            Direct = 0
        }

        public enum AbsorptionFee
        {
            Percentage = 1,
            Fixed = 0
        }

        public enum CompanyType
        {
            Select_Company_Type=0,
            Domestic = 1,
            Out_Of_India = 2
        }
    }

    public class EnumModel
    {
        [Key]
        public string Value { get; set; }

        public string Name { get; set; }
    }
    public enum ConnectionString
    {
      Primary=0,
      Secondary=1
    }
}