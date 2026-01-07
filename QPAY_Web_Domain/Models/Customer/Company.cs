using QPay.UI.Common;
using System.Xml.Serialization;

namespace QPay.UI.Customer
{
    public class Company
    {
        public class CompanyDetails
        {
            //Standing Data Enum
            public List<EnumModel> GetModeOfPayment { get; set; }

            public List<EnumModel> GetYesno { get; set; }
            public List<EnumModel> GetDuesBasedOn { get; set; }
            public List<EnumModel> GetIncentivetype { get; set; }
            public List<EnumModel> GetBillingtype { get; set; }
            public List<EnumModel> GetCustomertype { get; set; }
            public List<EnumModel> GetReimbursementType { get; set; }
            public List<EnumModel> GetSourcingFee { get; set; }
            public List<EnumModel> GetAbsorptionFee { get; set; }


            //Fetch Data From Database
            public List<EnumModel> GetCompanyName { get; set; } //Company Code and Name
            public List<EnumModel> GetCompanyGroupCode { get; set; } //Company Group Code and Name

            public List<EnumModel> GetEntityName { get; set; } //Business Unit Name
            public List<EnumModel> GetBankName { get; set; }
            public List<EnumModel> GetSegmentName { get; set; }
            public List<EnumModel> GetSubSegmentName { get; set; }
            public List<EnumModel> GetFinancialYear { get; set; }
            public List<EnumModel> GetAllCity { get; set; }
            public List<EnumModel> GetAllState { get; set; }
            public List<EnumModel> GetAllRegion { get; set; } // By dipuna

            public List<EnumModel> GetPfCode { get; set; }
            public List<EnumModel> GetCompanyType { get; set; }
            public List<EnumModel> GetReimbPayment { get; set; }
            public List<EnumModel> GetPayrollWithDecimal { get; set; }
            public List<EnumModel> GetPfCategory { get; set; }
            public List<EnumModel> GetServiceFeeWithDecimal { get; set; }

            public List<EnumModel> GetBankAdvice { get; set; }
            public List<EnumModel> GetVerticals { get; set; }
            public List<EnumModel> GetServiceChargeClubbing { get; set; }
            public List<EnumModel> GetBillingCompanyCodeList { get; set; }

            //public List<EnumModel> GetServicechargemaster { get; set; }
            public void Getallbinddata()
            {
                GetCompanyType = Common.Common.GetEnumList(new StandingDataEnum.CompanyType(), 0);
                GetModeOfPayment = Common.Common.GetEnumList(new StandingDataEnum.ModeOfPayment(), 0);
                GetYesno = Common.Common.GetEnumList(new StandingDataEnum.Yes_No(), 0);
                GetDuesBasedOn = Common.Common.GetEnumList(new StandingDataEnum.DuesBasedOn(), 0);
                GetIncentivetype = Common.Common.GetEnumList(new StandingDataEnum.Incentivetype(), 1);
                GetBillingtype = Common.Common.GetEnumList(new StandingDataEnum.BillingType(), 0);
                GetCustomertype = Common.Common.GetEnumList(new StandingDataEnum.CustomerType(), 1);
                GetReimbursementType = Common.Common.GetEnumList(new StandingDataEnum.ReimbursementType(), 1);
                GetSourcingFee = Common.Common.GetEnumList(new StandingDataEnum.SourcingFee(), 0);
                GetAbsorptionFee = Common.Common.GetEnumList(new StandingDataEnum.AbsorptionFee(), 0);
            }
        }

        public class CompanyAddress
        {
            //Company Address Details
            public string Map_Name { get; set; }

            public int Cost_Center_Mapping_Id { get; set; }

            public int Company_Address_Detail_Id { get; set; }
            public string Certificate_Number { get; set; }
            public string Fax_Number { get; set; }
            public int Location { get; set; }
            public string Location_Name { get; set; }
            public string Website_Name { get; set; }
            public int Invoice_Location { get; set; }
            public string Invoice_Location_Name { get; set; }
            public string Phone_Number { get; set; }
            public string Address { get; set; }
            public string TAN_Number { get; set; }
            public int City_Id { get; set; }
            public string City_Name { get; set; }
            public string PF_Code { get; set; }
            public int State_Id { get; set; }
            public string State_Name { get; set; }
            public string PT_Code { get; set; }
            public string Pin_Code { get; set; }
            public string PAN_Number { get; set; }
            public string Cost_Code { get; set; }
            public string Service_Tax_Number { get; set; }
            public string Circle_Code { get; set; }
            public string ESI_Code { get; set; }
            public string Billing_Client_Name { get; set; }
            public string Shipment_Client_Name { get; set; }
            public string Billing_Client_Address1 { get; set; }
            public string Shipment_Client_Address1 { get; set; }
            public string Billing_Client_Address2 { get; set; }
            public string Shipment_Client_Address2 { get; set; }
            public string Email_Id { get; set; }
        }

        public class ContactPerson
        {
            public int Company_Contact_Id { get; set; }
            public int Company_Id { get; set; }
            public string Contact_Name { get; set; }
            public string Designation_Name { get; set; }
            public string Department_Name { get; set; }
            public string Email_Id { get; set; }
            public string Phone_Number { get; set; }

            public List<ContactPerson> lstContactPerson { get; set; }
        }

        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "ContactPerson")]
        [System.Serializable()]
        public class ContactPersonResponse
        {
            [System.Xml.Serialization.XmlElementAttribute("ContactPerson")]
            public ContactPerson[] ContactPerson { get; set; }
        }

        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "CompanyAddress")]
        [System.Serializable()]
        public class CompanyAddressResponse
        {
            [System.Xml.Serialization.XmlElementAttribute("CompanyAddress")]
            public CompanyAddress[] CompanyAddress { get; set; }
        }


        public class CreateCompany
        {
            public CompanyDetails companyDetails { get; set; }
            public string? Contactdata { get; set; }
            public string? Companyaddress { get; set; }
            public int? userId { get; set; }
        }
        //Company.CompanyDetails companyDetails, string? Contactdata, string? Companyaddress, int? userId

        [XmlRoot("Company")]
        public class CompanyRequest
        {
            public int? Client_Id { get; set; }
            public int? Financial_Year_Id { get; set; }
            public string? Client_Since { get; set; }
            public int? Company_Active { get; set; }
            public int? Is_Zip_Documents { get; set; }
            public int? Payroll_Type { get; set; }
            public int? Invoicing_Type { get; set; }
            public string? Investment_Block_Date { get; set; }
            public string? Business_Unit_Name_Id { get; set; }
            public string? Month_Days { get; set; }
            public string? Salary_Fix_Days { get; set; }
            public string? Business_Unit_Location_Id { get; set; }
            public string? Attendance_Cycle_From { get; set; }
            public string? Attendance_Cycle_To { get; set; }
            public string? Is_PF_Remittance { get; set; }
            public string? Input_Date { get; set; }
            public string? Output_Date { get; set; }
            public string? Work_Days_Based_On { get; set; }
            public string? CTC { get; set; }
            public string? Sourcing_Fee_Criteria_Type { get; set; }
            public string? Sourcing_Fee { get; set; }
            public string? Absorption_Fee_Criteria_Type { get; set; }
            public string? Absorption_Fee { get; set; }
            public string? Incentive_Type { get; set; }
            public string? Is_PO_Applicable { get; set; }
            public string? Salary_SMS { get; set; }
            public string? Dues_Based_On { get; set; }
            public string? Is_Insurance_Applicable { get; set; }
            public int? ReimbInvoiceFormat_Id { get; set; }
            public string? Segment_Id { get; set; }
            public string? SubSegment_Id { get; set; }
            public string? Payslip_Format { get; set; }
            public string? Mode_Of_Payment { get; set; }
            public string? TAT { get; set; }
            public string? Billing_Type { get; set; }
            public string? Is_RoundOff_Applicable { get; set; }
            public string? Deviation { get; set; }
            public string? Incharge { get; set; }
            public string? Credit_Days_Upfront { get; set; }
            public string? Customer_Type { get; set; }
            public string? Incentive_Date { get; set; }
            public int? Service_Tax_Applicable { get; set; }
            public string? Reimbursement_Type { get; set; }
            public string? Salary_Transfer_Date { get; set; }
            public string? Effective_Date { get; set; }
            public string? Sales_Person { get; set; }
            public string? Branch_Location { get; set; }
            public string? Reimbursement_Date { get; set; }
            public string? SAP_Code { get; set; }
            public string? Pin_Code { get; set; }
            public string? Address { get; set; }
            public string? Phone_Number { get; set; }
            public string? PAN_Number { get; set; }
            public string? TAN_Number { get; set; }
            public string? Service_Tax_Number { get; set; }
            public string? PF_Code { get; set; }
            public string? ESI_Code { get; set; }
            public string? PT_Code { get; set; }
            public string? Email_Id { get; set; }
            public string? Certificate_Number { get; set; }
            public string? Fax_Number { get; set; }
            public string? Website_Name { get; set; }
            public string? Wages { get; set; }
            public string? Particulars { get; set; }
            public string? Is_NonInvoice { get; set; }
            public string? Mis_Name { get; set; }
            public string? Zone_Tagging { get; set; }
            public string? IsHeaderFooter { get; set; }
            public string? Sap_Customer_Code { get; set; }
            public string? Profit_Center_Code { get; set; }
            public string? Inedge_charges { get; set; }
            public string? Inedge_charges_Criteria_Type { get; set; }
            public string? CompanyGroupCode { get; set; }
            public string? OnBoarding_Category { get; set; }
            public string? InEdge_Category { get; set; }
            public string? Is_PO_Wise_Batch { get; set; }
            public int? IsBonusPayThroughFF { get; set; }
            public int? IsExtraWorkingDaysServiceFee { get; set; }
            public int? AttendanceInputWithLeave { get; set; }
            public string? Management_MIS { get; set; }
            public int? PfCode_Id { get; set; }
            public string? IsDecimal { get; set; }
            public string? IsProforma { get; set; }
            public int? CompanyType { get; set; }
            public string? Manual_NewJoinee { get; set; }
            public int? Invoice_Submission_Date { get; set; }
            public int? Collection_Date { get; set; }
            public string? PE_User_ID { get; set; }
            public string? PE_Name { get; set; }
            public string? PE_Email_Id { get; set; }
            public string? RM_User_ID { get; set; }
            public string? RM_Name { get; set; }
            public string? RM_Email_Id { get; set; }
            public string? Client_SPOC_Name { get; set; }
            public string? Client_SPOC_Email_Id { get; set; }
            public string? Client_SPOC_Mobile_No { get; set; }
            public string? Client_Escalation_Manager_Name { get; set; }
            public string? Client_Escalation_Manager_Email_Id { get; set; }
            public string? Client_Escalation_Manager_Mobile_No { get; set; }
            public string? Portal_Payslip_Format { get; set; }
            public string? IsNewJoinee { get; set; }
            public int? ReimbPaymentId { get; set; }
            public int? PayrollWithDecimalId { get; set; }
            public int? PfCategoryId { get; set; }
            public int? IsSignature { get; set; }
            public int? ServiceFeeWithDecimalId { get; set; }
            public string? Qdemy_charges { get; set; }
            public int? IsCurrencyConversion { get; set; }
            public string? TechSubscriptionCharges { get; set; }
            public string? Tech_Subscription_Charges_Criteria_Type { get; set; }
            public int? DigitalPlatformConsent { get; set; }
            public int? DGPSF { get; set; }
            public int? Vertical_Id { get; set; }
            public int? ServiceChargeClubbing { get; set; }
            public int? IsOneTouchInvoicing { get; set; }
            public int? IsInvoicePoBased { get; set; }
            public int? IS_ESI_split { get; set; }
            public int? WorkingHours { get; set; }
            public int? Is40BillingModel { get; set; }
            public int? BillingCompanyId { get; set; }
            public string? Contract_Start_Date { get; set; }
            public string? Contract_End_Date { get; set; }
            public string? Contract_File_Path { get; set; }
            public string? Contract_File_Name { get; set; }
            public string? Contract_Uploaded_File_Name { get; set; }
            public string? Service_Tax_Date { get; set; }
            public string? Service_Tax_File_Path { get; set; }
            public string? Service_Tax_File_Name { get; set; }
            public string? Service_Tax_Uploaded_File_Name { get; set; }
            public string? Bank_Id { get; set; }
            public string? IFSC_Code { get; set; }
            public string? Account_Number { get; set; }
            public string? Bank_Address { get; set; }
            public string? Branch { get; set; }
            public string? BranchCode { get; set; }
            public string? BankCode { get; set; }
            public string? BankAdviceId { get; set; }
            
            public int? OT_WEEK_DAY_TYPE { get; set; }
            public string? OT_WEEK_DAY_VLAUE { get; set; }
            public string? OT_WEEK_DAY_FORMULA { get; set; }
            public int? OT_NIGHT_SHIFT_TYPE { get; set; }
            public string? OT_NIGHT_SHIFT_VLAUE { get; set; }
            public string? OT_NIGHT_SHIFT_FORMULA { get; set; }
            public int? OT_WEEKEND_TYPE { get; set; }
            public string? OT_WEEKEND_VLAUE { get; set; }
            public string? OT_WEEKEND_FORMULA { get; set; }
            public int? OT_HOLIDAY_TYPE { get; set; }
            public string? OT_HOLIDAY_VLAUE { get; set; }
            public string? OT_HOLIDAY_FORMULA { get; set; }
            public string? Adhoc_Service_Fee { get; set; }
            public string? Adhoc_Service_Formula { get; set; }
            public string? Portal_Type { get; set; }
        }

        public class CompanyCreateRequest
        {
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
            public CompanyRequest companyrequest { get; set; }
        }

        public class CompanyUpdateRequestPayload
        {
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
            public CompanyUpdateRequest companyrequest { get; set; }
        }

        public class CompanyDeleteRequest
        {
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
            public int? Company_Id { get; set; }
        }

        [XmlRoot("Main")]
        public class CompanyCreateRequestWrapper
        {
            public CompanyRequest Company { get; set; }
        }

        [XmlRoot("Main")]
        public class CompanyUpdateRequestWrapper
        {
            public CompanyUpdateRequest Company { get; set; }
        }

        [XmlRoot("Company")]
        public class CompanyUpdateRequest
        {
            public string? Company_Id { get; set; }
            public string? Company_Contract_Id { get; set; }
            public int? Client_Id { get; set; }
            public int? Financial_Year_Id { get; set; }
            public string? Client_Since { get; set; }
            public int? Company_Active { get; set; }
            public int? Is_Zip_Documents { get; set; }
            public int? Payroll_Type { get; set; }
            public int? Invoicing_Type { get; set; }
            public string? Investment_Block_Date { get; set; }
            public string? Business_Unit_Name_Id { get; set; }
            public string? Month_Days { get; set; }
            public string? Salary_Fix_Days { get; set; }
            public string? Business_Unit_Location_Id { get; set; }
            public string? Attendance_Cycle_From { get; set; }
            public string? Attendance_Cycle_To { get; set; }
            public string? Is_PF_Remittance { get; set; }
            public string? Input_Date { get; set; }
            public string? Output_Date { get; set; }
            public string? Work_Days_Based_On { get; set; }
            public string? CTC { get; set; }
            public string? Sourcing_Fee_Criteria_Type { get; set; }
            public string? Sourcing_Fee { get; set; }
            public string? Absorption_Fee_Criteria_Type { get; set; }
            public string? Absorption_Fee { get; set; }
            public string? Incentive_Type { get; set; }
            public string? Is_PO_Applicable { get; set; }
            public string? Salary_SMS { get; set; }
            public string? Dues_Based_On { get; set; }
            public string? Is_Insurance_Applicable { get; set; }
            public int? ReimbInvoiceFormat_Id { get; set; }
            public string? Segment_Id { get; set; }
            public string? SubSegment_Id { get; set; }
            public string? Payslip_Format { get; set; }
            public string? Mode_Of_Payment { get; set; }
            public string? TAT { get; set; }
            public string? Billing_Type { get; set; }
            public string? Is_RoundOff_Applicable { get; set; }
            public string? Deviation { get; set; }
            public string? Incharge { get; set; }
            public string? Credit_Days_Upfront { get; set; }
            public string? Customer_Type { get; set; }
            public string? Incentive_Date { get; set; }
            public int? Service_Tax_Applicable { get; set; }
            public string? Reimbursement_Type { get; set; }
            public string? Salary_Transfer_Date { get; set; }
            public string? Effective_Date { get; set; }
            public string? Sales_Person { get; set; }
            public string? Branch_Location { get; set; }
            public string? Reimbursement_Date { get; set; }
            public string? SAP_Code { get; set; }
            public string? Pin_Code { get; set; }
            public string? Address { get; set; }
            public string? Phone_Number { get; set; }
            public string? PAN_Number { get; set; }
            public string? TAN_Number { get; set; }
            public string? Service_Tax_Number { get; set; }
            public string? PF_Code { get; set; }
            public string? ESI_Code { get; set; }
            public string? PT_Code { get; set; }
            public string? Email_Id { get; set; }
            public string? Certificate_Number { get; set; }
            public string? Fax_Number { get; set; }
            public string? Website_Name { get; set; }
            public string? Wages { get; set; }
            public string? Particulars { get; set; }
            public string? Is_NonInvoice { get; set; }
            public string? Mis_Name { get; set; }
            public string? Zone_Tagging { get; set; }
            public string? IsHeaderFooter { get; set; }
            public string? Sap_Customer_Code { get; set; }
            public string? Profit_Center_Code { get; set; }
            public string? Inedge_charges { get; set; }
            public string? Inedge_charges_Criteria_Type { get; set; }
            public string? CompanyGroupCode { get; set; }
            public string? OnBoarding_Category { get; set; }
            public string? InEdge_Category { get; set; }
            public string? Is_PO_Wise_Batch { get; set; }
            public int? IsBonusPayThroughFF { get; set; }
            public int? IsExtraWorkingDaysServiceFee { get; set; }
            public int? AttendanceInputWithLeave { get; set; }
            public string? Management_MIS { get; set; }
            public int? PfCode_Id { get; set; }
            public string? IsDecimal { get; set; }
            public string? IsProforma { get; set; }
            public int? CompanyType { get; set; }
            public string? Manual_NewJoinee { get; set; }
            public int? Invoice_Submission_Date { get; set; }
            public int? Collection_Date { get; set; }
            public string? PE_User_ID { get; set; }
            public string? PE_Name { get; set; }
            public string? PE_Email_Id { get; set; }
            public string? RM_User_ID { get; set; }
            public string? RM_Name { get; set; }
            public string? RM_Email_Id { get; set; }
            public string? Client_SPOC_Name { get; set; }
            public string? Client_SPOC_Email_Id { get; set; }
            public string? Client_SPOC_Mobile_No { get; set; }
            public string? Client_Escalation_Manager_Name { get; set; }
            public string? Client_Escalation_Manager_Email_Id { get; set; }
            public string? Client_Escalation_Manager_Mobile_No { get; set; }
            public string? Portal_Payslip_Format { get; set; }
            public string? IsNewJoinee { get; set; }
            public int? ReimbPaymentId { get; set; }
            public int? PayrollWithDecimalId { get; set; }
            public int? PfCategoryId { get; set; }
            public int? IsSignature { get; set; }
            public int? ServiceFeeWithDecimalId { get; set; }
            public string? Qdemy_charges { get; set; }
            public int? IsCurrencyConversion { get; set; }
            public string? TechSubscriptionCharges { get; set; }
            public string? Tech_Subscription_Charges_Criteria_Type { get; set; }
            public int? DigitalPlatformConsent { get; set; }
            public int? DGPSF { get; set; }
            public int? Vertical_Id { get; set; }
            public int? ServiceChargeClubbing { get; set; }
            public int? IsOneTouchInvoicing { get; set; }
            public int? IsInvoicePoBased { get; set; }
            public int? IS_ESI_split { get; set; }
            public int? WorkingHours { get; set; }
            public int? Is40BillingModel { get; set; }
            public int? BillingCompanyId { get; set; }
            public string? Contract_Start_Date { get; set; }
            public string? Contract_End_Date { get; set; }
            public string? Contract_File_Path { get; set; }
            public string? Contract_File_Name { get; set; }
            public string? Contract_Uploaded_File_Name { get; set; }
            public string? Service_Tax_Date { get; set; }
            public string? Service_Tax_File_Path { get; set; }
            public string? Service_Tax_File_Name { get; set; }
            public string? Service_Tax_Uploaded_File_Name { get; set; }
            public string? Bank_Id { get; set; }
            public string? IFSC_Code { get; set; }
            public string? Account_Number { get; set; }
            public string? Bank_Address { get; set; }
            public string? Branch { get; set; }
            public string? BranchCode { get; set; }
            public string? BankCode { get; set; }
            public string? BankAdviceId { get; set; }

            public int? OT_WEEK_DAY_TYPE { get; set; }
            public string? OT_WEEK_DAY_VLAUE { get; set; }
            public string? OT_WEEK_DAY_FORMULA { get; set; }
            public int? OT_NIGHT_SHIFT_TYPE { get; set; }
            public string? OT_NIGHT_SHIFT_VLAUE { get; set; }
            public string? OT_NIGHT_SHIFT_FORMULA { get; set; }
            public int? OT_WEEKEND_TYPE { get; set; }
            public string? OT_WEEKEND_VLAUE { get; set; }
            public string? OT_WEEKEND_FORMULA { get; set; }
            public int? OT_HOLIDAY_TYPE { get; set; }
            public string? OT_HOLIDAY_VLAUE { get; set; }
            public string? OT_HOLIDAY_FORMULA { get; set; }
            public string? Adhoc_Service_Fee { get; set; }
            public string? Adhoc_Service_Formula { get; set; }
            public string? Portal_Type { get; set; }

        }

    }
}
