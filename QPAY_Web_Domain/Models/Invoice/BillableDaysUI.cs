using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Invoice
{
    public class BillableDaysUI
    {
        public string Sno { get; set; } = string.Empty;
        public int BillableDays_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; } = string.Empty;
        public string Company_Name { get; set; } = string.Empty;
        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; } = string.Empty;
        public string Employee_Name { get; set; } = string.Empty;
        public string Axpert_Employee_Id { get; set; } = string.Empty;
        public string Pay_Period { get; set; } = string.Empty;
        public int Billing_Pay_Period_Id { get; set; }
        public int Pay_Frequency_Id { get; set; }
        public int Map_Name_Id { get; set; }
        public int Invoice_MapName_Id { get; set; }
        public int State_Id { get; set; }
        public int Group_Detail_Id { get; set; }
        public required string Error_Message { get; set; }
        public int Input_Number { get; set; }
        public int Service_Charge_Type_Id { get; set; }
        public decimal Billable_Month_Days { get; set; }
        public decimal Billable_Days { get; set; }
        public decimal Billable_CTC { get; set; }
        public required string IQN_REF_NO { get; set; } = string.Empty;
        public string IQN_ID { get; set; } = string.Empty;
        public DateTime? REF_DATE { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public string WO_NUMBER { get; set; } = string.Empty;
        public string GRN_NUMBER { get; set; } = string.Empty;
        public string RECRUITER_NAME { get; set; } = string.Empty;
        public decimal? CLIENT_BILLING_PERCENTAGE { get; set; }
        public string LOCATION_NAME { get; set; } = string.Empty;
        public decimal? OT_AMOUNT { get; set; }
        public decimal? OTHER_ALLOWANCE { get; set; }
        public decimal? REIMB_AMOUNT { get; set; }
        public string DISCOUNT_TYPE { get; set; } = string.Empty;
        public decimal? Discount_Amount { get; set; }
    }
    public class BillableDaysResponse
    {
        public string Error_Message { get; set; } = string.Empty;
    }

    [XmlRoot("BillableDaysDocumentElement")]
    public class BillableDaysDocumentElement
    {
        [XmlElement("BDE")]
        public List<BDE> BDEList { get; set; } = new List<BDE>();
    }
    public class BDE
    {
        [XmlElement("EMPLOYEE_CODE")]
        public string Employee_Code { get; set; } = string.Empty;

        [XmlElement("PAY_PERIOD")]
        public string Pay_Period { get; set; } = string.Empty;

        [XmlElement("BILLABLE_DAYS")]
        public string Billable_days { get; set; } = string.Empty;

        [XmlElement("SERVICE_CHARGE_TYPE")]
        public string Service_Charge_Type { get; set; } = string.Empty;    // keep string if format is dd/MM/yyyy

        [XmlElement("INPUT_NUMBER")]
        public string Input_Number { get; set; } = string.Empty;

        [XmlElement("IQN_REF_NO")]
        public string IQN_Ref_No { get; set; } = string.Empty;

        [XmlElement("IQN_ID")]
        public string IQN_Id { get; set; } = string.Empty;

        [XmlElement("REF_DATE")]
        public string Ref_Date { get; set; } = string.Empty;

        [XmlElement("FROM_DATE")]
        public string From_Date { get; set; } = string.Empty;

        [XmlElement("TO_DATE")]
        public string To_date { get; set; } = string.Empty;

        [XmlElement("WO_NUMBER")]
        public string WO_Number { get; set; } = string.Empty;

        [XmlElement("GRN_NUMBER")]
        public string GRN_Number { get; set; } = string.Empty;

        [XmlElement("RECRUITER_NAME")]
        public string Recruiter_Name { get; set; } = string.Empty;

        [XmlElement("CLIENT_BILLING_PERCENTAGE")]
        public string Client_Billing_Percentage { get; set; } = string.Empty;

        [XmlElement("LOCATION_NAME")]
        public string Location_Name { get; set; } = string.Empty;

        [XmlElement("OT_AMOUNT")]
        public string OT_Amount { get; set; } = string.Empty;

        [XmlElement("OTHER_ALLOWANCE")]
        public string Other_Allowance { get; set; } = string.Empty;

        [XmlElement("REIMB_AMOUNT")]
        public string Reimb_Amount { get; set; } = string.Empty;

        [XmlElement("QUANTITY")]
        public string Quantity { get; set; } = string.Empty;

        [XmlElement("MATERIALCODE")]
        public string MaterialCode { get; set; } = string.Empty;

        [XmlElement("TS_START_DATE")]
        public string TS_Start_Date { get; set; } = string.Empty;

        [XmlElement("TS_END_DATE")]
        public string TS_End_date { get; set; } = string.Empty;

        [XmlElement("REMARK")]
        public string Remark { get; set; } = string.Empty;

        [XmlElement("SHIPPING_PARTNER_CODE")]
        public string Shipping_Partner_Code { get; set; } = string.Empty;

        [XmlElement("BILLING_PARTNER_CODE")]
        public string Billing_Partner_Code { get; set; } = string.Empty;

        [XmlElement("BILLABLE_MONTH_DAYS")]
        public string Billable_Month_days { get; set; } = string.Empty;

        [XmlElement("DISCOUNT")]
        public string Discount { get; set; } = string.Empty;

        [XmlElement("ADDRESSCODE")]
        public string AddressCode { get; set; } = string.Empty;

        [XmlElement("MSP_Fee")]
        public string MSP_Fee { get; set; } = string.Empty;

        [XmlElement("GST_GROUP_NAME")]
        public string GST_Group_Name { get; set; } = string.Empty;

        [XmlElement("DEPT_CODE")]
        public string Dept_Code { get; set; } = string.Empty;

        [XmlElement("DEPT_NAME")]
        public string Dept_Name { get; set; } = string.Empty;

        [XmlElement("LEAVE")]
        public string Leave { get; set; } = string.Empty;

        [XmlElement("Mode")]
        public string Mode { get; set; } = string.Empty;
    }
}
