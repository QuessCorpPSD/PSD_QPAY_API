using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.PurchaseOrder
{
    [Serializable]
    public class PurchaseOrderViewModel : BaseModel
    {

        [Display(Name = "SL No")]
        public int SLNo { get; set; }

        [Display(Name = "Purchase Order No")]
        public int Purchase_Order_Id { get; set; }

        //[Required(ErrorMessage = "Company Code Required")]
        public int? Company_Id { get; set; }

        [Display(Name = "Company Code")]
        public string Company_Code { get; set; }

        [Display(Name = "Company Name")]
        public string Company_Name { get; set; }

        [Display(Name = "Client Code")]
        public int? Client_Id { get; set; }

        [Display(Name = "Client Code")]
        public string Client_Code { get; set; }

        [Required(ErrorMessage = "Enter the PO Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "PO Date")]
        public DateTime PO_Date { get; set; }

        [Display(Name = "Purchase Request No")]
        [Required(ErrorMessage = "Required Purchase Request No")]
        public string Purchase_Request_No { get; set; }

        [Required(ErrorMessage = "Required PO Amount")]
        [Display(Name = "PO Amount")]
        public decimal PO_Amount { get; set; }

        [Required(ErrorMessage = "Enter PO Valid From Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "PO Valid From")]
        public DateTime PO_Valid_From { get; set; }

        [Required(ErrorMessage = "PO Valid To Date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "PO Valid To")]
        public DateTime PO_Valid_To { get; set; }


        [Display(Name = "Invoiced Amount")]
        public decimal Invoiced_Amount { get; set; }


        [Display(Name = "Transfered Amount")]
        public decimal Transfered_Amount { get; set; }


        [Display(Name = "PO Based On")]
        [Required(ErrorMessage = "PO Based_ On")]
        public int? PO_Based_On { get; set; }


        [Display(Name = "Client PO Ref No")]
        public string Client_PO_Ref_No { get; set; }


        public int? City_Id { get; set; }


        [Display(Name = "City Name")]
        public string City_Name { get; set; }


        [Display(Name = "Remarks")]
        public string Remarks { get; set; }


        public bool IsActive { get; set; }

        public string Error_Message { get; set; }

        // [Required]
        [Display(Name = "Created By")]
        public Int32 CreatedBy { get; set; }

        ////  [Required]
        //  [Display(Name = "Created On")]
        //  public DateTime CreatedOn { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        //  [Display(Name = "Modified On")]
        //  public DateTime ModifiedOn { get; set; }

        //Rudra
        [Display(Name = "CompanyGroupId")]
        public int CompanyGroup_Id { get; set; }
        [Display(Name = "CompanyGroupCode")]
        public string CompanyGroupCode { get; set; }
        //Rudra

        public int IsCompanyGroupId { get; set; }
      //  public virtual ICollection<PurchaseOrderDetailViewModel> PurchaseOrderDetailViewModel { get; set; }
    }
    [DataContract]
    public class BaseModel
    {
        public BaseModel()
        {
            PageNo = 1;
            PageSize = 10;
        }

        // Default parameters
        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string SearchText { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string XmlData { get; set; }

        // Paging parameters
        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public int PageNo { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public string SortField { get; set; }
        [DataMember]
        public string SortDirection { get; set; }
    }
    public class PurchaseOrderRequest
    {
        public string? Action { get; set; } = "";
        public int? Company_Id { get; set; }
        public string? Purchase_Request_No { get; set; }
        public string? Purchase_Order_Id {  get; set; }
        public DateTime? PODateFrom {  get; set; }
        public DateTime? PODateTo {  get; set; }
        public int? PageNo {  get; set; }
        public int? PageSize { get; set; }
        public string? SortField { get; set; }
        public string? SortDirection {  get; set; }
        public int? TotalCount {  get; set; }
    }

    public class Column_Constants
    {
        #region Columns_Constants

        public const string Columns_Name_Region_Id = "Region_Id";
        public const string Columns_Name_Region_Name = "Region_Name";
        public const string Columns_Name_State_Id = "State_Id";

        public const string JOB_Category_ID = "JOB_Category_ID";
        public const string JOB_Category = "JOB_Category";

        public const string Columns_Name_State_Name = "State_Name";
        public const string Column_Name_City_Id = "City_Id";
        public const string Column_Name_City_Name = "City_Name";
        public const string Columns_Name_Entity_Id = "Entity_Id";
        public const string Columns_Name_Entity_Name = "Entity_Name";
        public const string Column_Name_Location = "Location";
        public const string Column_Name_Paycode_Id = "Paycode_Id";
        public const string Column_Name_Paycode_Code = "Paycode_Code";

        public const string Column_Name_PayPeriod_id = "PayPeriod_id";
        public const string Column_Name_PayPeriod = "PayPeriod";
        public const string Column_Name_Company_Id = "Company_Id";
        public const string Column_Name_Company_Code = "Company_Code";
        public const string Column_Name_PayType_Id = "PayType_Id";
        public const string Column_Name_PayType = "PayType";

        public const string Column_Name_Previous_Employment_Id = "Previous_Employment_Id";
        public const string Column_Name_Financial_Year_Id = "Financial_Year_Id";
        public const string Column_Name_Financial_Year_Name = "Financial_Year_Name";
        public const string Column_Name_Date = "Date";
        public const string Column_Name_FROM_DATE = "From_Date";
        public const string Column_Name_TO_DATE = "To_Date";
        public const string Column_Name_Frequencey_Id = "Frequencey_Id";
        public const string Column_Name_Pay_Period = "Pay_Period";
        public const string Column_Income_After_Exemption_10 = "Income_After_Exemption_10";

        // public const string Column_Name_Type = "Type";
        public const string Column_Date_Of_Joining = "Date_Of_Joining";

        public const string Column_Total_tax_paid = "Total_Tax_Paid";
        public const string Column_Name_Surcharge = "Surcharge";
        public const string Column_Name_Pt_paid = "PT_Paid";
        public const string Column_Name_Education_Cess = "Education_Cess";
        public const string Column_Name_PF_Paid = "PF_Paid";
        public const string Column_Name_Tax_Paid = "Tax_Paid";
        public const string Column_Name_Children_Education_Allowance_Id = "Children_Education_Allowance_Id";
        public const string Column_Name_Tuition_Eligibility = "Tuition_Eligibility";
        public const string Column_name_Hostel_Eligibility = "Hostel_Eligibility";
        public const string Column_name_Declaration_Date = "Declaration_Date";
        public const string Column_Name_Children_Education_Allowance_Detail_Id = "Children_Education_Allowance_Detail_Id";
        public const string Column_Name_Claim_Amount = "Claim_Amount";
        public const string Column_Name_Number_Of_Children = "Number_Of_Children";
        public const string Column_Name_Student_Name = "Student_Name";
        public const string Column_Name_School_Name = "School_Name";
        public const string Column_Name_Hostel_Name = "Hostel_Name";
        public const string Column_Name_Phone_Number = "Phone_Number";
        public const string Column_Name_Exemption_Amount = "Exemption_Amount";
        public const string Column_Name_Is_Tuition_Eligible = "Is_Tuition_Eligible";
        public const string Column_Name_Is_Hostel_Eligible = "Is_Hostel_Eligible";
        public const string Column_Name_Purchase_Request_Id = "Purchase_Request_Id";
        public const string Column_Name_Request_Date = "Request_Date";
        public const string Column_Name_Request_Number = "Request_Number";
        public const string Column_Name_Company_Address_Detail_Id = "Company_Address_Detail_Id";
        public const string Column_Name_Group_Detail_Id = "Group_Detail_Id";
        public const string Column_Name_Group_Name = "Group_Name";
        #endregion Columns_Constants

        #region Common

        public const string Columns_Name_Error_Message = "Error_Message";
        public const string Columns_Name_Serial_No = "Serial_No";
        public const string Columns_Name_Client_Id = "Client_Id";

        //public const string Columns_Name_Employee_Name = "Employee_Name";
        public const string Columns_Name_Client_Code = "Client_Code";

        public const string Columns_Name_Company_Code = "Company_Code";
        public const string Columns_Name_Payroll_Type = "Payroll_Type";
        public const string Column_Name_Is_NonInvoice = "Is_NonInvoice";

        public const string Columns_Name_Pay_Category_Name = "Pay_Category_Name";
        public const string Columns_Name_New_Pay_Category_Name = "New_Pay_Category_Name";
        public const string Columns_Name_Pay_Period = "Pay_Period";
        public const string Columns_Name_Pay_Frequency_Detail_Id = "Pay_Frequency_Detail_Id";
        public const string Columns_Name_Description = "Description";
        public const string Columns_Name_Formula_Name = "Formula_Name";
        public const string Columns_Name_Formula_ID = "Formula_ID";
        public const string Column_Name_Employee_Name = "Employee_Name";
        public const string Column_Name_Client_Name = "Client_Name";
        public const string Column_Name_Middle_Name = "First_Name";
        public const string Column_Name_Last_Name = "";
        public const string Column_Name_Date_Of_Joining = "Date_Of_Joining";
        public const string Columns_Name_New_Pay_Category_Id = "New_Pay_Category_Id";
        public const string Columns_Name_Old_CTC = "Old_CTC";
        public const string Column_Name_Revision_Date = "Revision_Date";
        public const string Columns_Name_Pay_Sequence_Number = "Pay_Sequence_Number";
        public const string Columns_Name_Company_Name = "Company_Name";

        public const string Columns_Name_Declaration_Type_Id = "Declaration_Type_Id";
        public const string Columns_Name_Declaration_Type_Name = "Declaration_Type_Name";
        public const string Columns_Name_Group_Name = "Group_Name";
        public const string Column_Name_Quote_Amount = "Quote_Amount";
        public const string Column_Name_Group_ID = "Group_ID";
        public const string Column_Name_Is_PO_Applicable = "Is_PO_Applicable";

        #endregion Common

        #region Tbl_invoice

        public const string Column_Name_MAP_NAME = "Map_Name";
        public const string Column_Name_Invoice_Value = "Invoice_Value";
        public const string Column_Name_Invoice_Date = "Invoice_Date";

        #endregion Tbl_invoice

        #region tbl_Invoice_Collection_Detail

        // public const string Column_Name_Record_Id = "Record_Id";
        public const string Column_Name_Mode_of_Payment = "Mode_of_Payment";

        public const string Column_Name_Salary_period = "Salary_period";
        public const string Column_Name_Type_of_Invoice = "Type_of_Invoice";
        public const string Column_Name_Total_Invoice_Amount = "Total_Invoice_Amount";
        public const string Column_Name_TDS_Amount = "TDS_Amount";
        public const string Column_Name_Deposit_Cheque_Date = "Deposit_Cheque_Date";
        public const string Column_Name_Cheque_No = "Cheque_No";
        public const string Column_Name_Salary_Release_Date = "Salary_Release_Date";
        public const string Column_Name_Credit_Note = "Credit_Note";
        public const string Column_Name_Pending_Amount = "Pending_Amount";
        public const string Column_Name_Invoice_Type = "Invoice_Type";

        #endregion tbl_Invoice_Collection_Detail

        #region tbl_PO_Balance_Transfer

        public const string Column_Name_PO_Balance_Transfer_Id = "PO_Balance_Transfer_Id";
        public const string Column_Name_Company_Name = "Company_Name";
        public const string Column_Name_Transfer_From_PO = "Transfer_From_PO";
        public const string Column_Name_Balance_Amount = "Balance_Amount";
        public const string Column_Name_Transfer_To_PO = "Transfer_To_PO";
        public const string Column_Name_Transfer_Amount = "Transfer_Amount";

        #endregion tbl_PO_Balance_Transfer

        #region tbl_increment

        public const string Columns_Name_Increment_Id = "Increment_Id";
        public const string Columns_Name_Employee_Id = "Employee_Id";
        public const string Column_Name_Employee_Code = "Employee_Code";
        public const string Column_Name_Pay_Category_Id = "Pay_Category_Id";

        public const string Column_Name_New_Pay_Category_Id = "New_Pay_Category_Id";
        public const string Column_Name_Old_CTC = "Old_CTC";
        public const string Columns_Name_New_CTC = "New_CTC";
        public const string Columns_Name_Revision_Date = "Revision_Date";
        public const string Columns_Name_Effective_Date = "Effective_Date";
        public const string Column_Name_Salary_Month = "Salary_Month";
        public const string Column_Name_Is_Annum = "Is_Annum";
        public const string Column_Name_Post_CF = "Post_CF";
        public const string Column_Name_Work_Location_Id = "Work_Location_Id";

        public const string Column_Name_Employee_ID = "Column_Name_Employee_ID";
        //  public const string Column_Name_Employee_Code = "Employee_code";

        #endregion tbl_increment

        #region tbl_increment_Details

        public const string Columns_Name_Increment_Detail_Id = "Increment_Detail_Id";
        public const string Column_Name_Old_Amount = "Old_Amount";
        public const string Column_Name_Amount = "Amount";
        public const string Column_Name_Difference = "Difference";

        #endregion tbl_increment_Details

        #region LTA

        public const string TableConstant_LTA_BLOCK_ID = "LTA_Block_Id";
        public const string TableConstant_BLOCK_PERIOD = "Block_Period";
        public const string TableConstant_FROM_DATE = "From_Date";
        public const string TableConstant_TO_DATE = "To_Date";
        public const string TableConstant_SERIAL_NO = "SERIAL_NO";

        #endregion LTA

        #region CostCenterMapping

        public const string TableConstant_COST_CENTER_MAPPING_ID = "Cost_Center_Mapping_Id";
        public const string TableConstant_MAP_NAME = "Map_Name";
        public const string TableConstant_BUSINESS_UNIT_ID = "Business_Unit_Id";
        public const string TableConstant_BUSINESS_UNIT_NAME = "Business_Unit_Name";
        public const string TableConstant_COMPANY_ID = "Company_Id";
        public const string TableConstant_SPOC_NAME = "SPOC_Name";
        public const string TableConstant_COST_CENTER_NAME = "Cost_Center_Name";
        public const string TableConstant_GRN_NUMBER = "GRN_Number";
        public const string TableConstant_COMPANY_NAME = "Company_Name";

        #endregion CostCenterMapping

        #region ITCalender

        public const string TableConstant_IT_Calender_Id = "IT_Calender_Id";
        public const string TableConstant_Company_Id = "Company_Id";
        public const string TableConstant_Company_Code = "Company_Code";
        public const string TableConstant_Financial_Year_Id = "Financial_Year_Id";
        public const string TableConstant_Financial_Year_Name = "Financial_Year_Name";
        public const string TableConstant_Declaration_CutOff_Date = "Declaration_CutOff_Date";
        public const string TableConstant_Submission_CutOff_Date = "Submission_CutOff_Date";
        public const string TableConstant_IsActive = "IsActive";
        public const string TableConstant_ERROR_MESSAGE = "Error_Message";

        #endregion ITCalender

        public const string Column_Name_Declaration_CutOff_Date = "Declaration_CutOff_Date";
        public const string Column_Name_Submission_CutOff_Date = "Submission_CutOff_Date";
        public const string Column_Name_Fore_Cast_Id = "Fore_Cast_Id";
        public const string Column_Name_Vendor = "Vendor";
        public const string Column_Name_Sbu = "Sbu";
        public const string Column_Name_Region = "Region";
        public const string Column_Name_Projection_Amount = "Projection_Amount";
        public const string Column_Name_Collected_Amount = "Collected_Amount";

        //public const string Column_Name_Balance_Amount = "Balance_Amount";
        public const string Column_Name_Collected_Pay_Period = "Pay_Period";

        public const string Column_Name_Collected_Company_Code = "Company_Code";

        #region tbl_Income_Loss_On_House_Property

        public const string Column_Name_Income_Loss_On_House_Property_Id = "Income_Loss_On_House_Property_Id";
        public const string Column_Name_Declaration_Date = "Declaration_Date";

        //public const string Column_Name_Tax_Code = "Tax_Code";
        public const string Column_Name_Income_On_House_Property = "Income_On_House_Property";

        public const string Column_Name_Municipal_Tax_Paid = "Municipal_Tax_Paid";
        public const string Column_Name_Insurance_Charge_Paid = "Insurance_Charge_Paid";
        public const string Column_Name_Number_Letout_Property = "Number_Letout_Property";
        public const string Column_Name_Letout_Eligible_Interest = "Letout_Eligible_Interest";
        public const string Column_Name_Letout_Effective_Date = "Letout_Effective_Date";
        public const string Column_Name_Number_Of_SelfOccupied_Property = "Number_Of_SelfOccupied_Property";
        public const string Column_Name_Interest_On_Housing_Loan = "Interest_On_Housing_Loan";
        public const string Column_Name_SelfOccupied_Effective_Date = "SelfOccupied_Effective_Date";
        public const string Column_Name_Eligible_Interest_On_Housing_Loan = "Eligible_Interest_On_Housing_Loan";
        public const string Column_Name_Additional_Exemption = "Additional_Exemption";
        // public const string Column_Name_Declaration_Type_Id = "Declaration_Type_Id";
        //public const string Column_Name_Declaration_Type_Name = "Declaration_Type_Name";

        public const string Column_Name_Eligible_Housing_Exemption = "Eligible_Housing_Exemption";
        public const string Column_Name_Eligible_Let_Out_Exemption = "Eligible_Let_Out_Exemption";
        public const string Column_Name_Net_Annual_Value = "Net_Annual_Value";
        public const string Column_Name_Repair_Collection_30_Percent = "Repair_Collection_30_Percent";
        public const string Column_Name_Net_income_on_House_property = "Net_income_on_House_property";

        #endregion tbl_Income_Loss_On_House_Property

        #region CompanyProvidedBenefits

        //  public const string Column_Name_Employee_Id = "Employee_Id";
        //public const string Column_Name_Employee_Code = "Employee_code";
        //public const string Column_Name_EmployeeName = "EmployeeName";

        //for the procedure spSearchCompanyProvidedBenefits
        //  public const string Column_Name_Perk_Code = "PerkCode";
        public const string Column_Name_Perk_Code = "PerkCode";

        public const string Column_Name_Perk_Code_Id = "Perk_Code_Id";
        public const string Column_Name_Perk_Amount = "Perk_Amount";
        public const string Column_Name_Perk_Type = "Perk_Type";
        public const string Column_Name_Perk_Id = "Perk_Id";
        public const string Column_Name_Company_Provided_Benefit_Id = "Company_Provided_Benefit_Id";
        public const string Column_Name_Company_Provided_Benefit_Date = "Company_Provided_Benefit_Date";
        //  public const string Column_Name_Company_Provided_Benefit_DateForSearch = "Date";

        #endregion CompanyProvidedBenefits

        #region tbl_Tax_Declaration_Actual

        public const string Column_Name_Computation_Rule_Id = "Computation_Rule_Id";
        public const string Column_Name_Computation_Rule = "Computation_Rule";
        public const string Column_Name_Computation_Rule_Category_Name = "Computation_Rule_Category_Name";
        public const string Column_Name_Tax_Id = "Tax_Id";
        public const string Column_Name_Declaration_Type_Id = "Declaration_Type_Id";
        public const string Column_Name_Declaration_Type_Name = "Declaration_Type_Name";
        public const string Column_Name_Section = "Section";
        public const string Column_Name_Description = "Description";
        public const string Column_Name_Formula = "Formula";
        public const string Column_Name_Tax_Declaration_Actual_Id = "Tax_Declaration_Actual_Id";
        public const string Column_Name_SNo = "SNo";
        public const string Column_Name_Financial_Year = "Financial_Year";
        public const string Column_Name_Tax_Declaration_Actual_Date = "Tax_Declaration_Actual_Date";
        public const string Column_Name_Tax_Code = "Tax_Code";
        public const string Column_Name_Type = "Type";
        public const string Column_Name_EmployeeName = "EmployeeName";
        public const string Column_Name_Citizen_Category = "Citizen_Category";
        public const string Column_Name_Eligible_Amount = "Eligible_Amount";

        #endregion tbl_Tax_Declaration_Actual

        #region tbl_Gratuity

        public const string Column_Name_Gratuity_Id = "Gratuity_Id";
        public const string Column_Name_Gratuity_Date = "Gratuity_Date";
        public const string Column_Name_Year_Of_Service = "Year_Of_Service";
        public const string Column_Name_Tax_Exemption = "Tax_Exemption";

        public const string Column_Name_Resignation_Date = "Resignation_Date";

        #endregion tbl_Gratuity

        #region tbl_Pay_Transaction

        public const string Column_Name_Pay_Transaction_Id = "Pay_Transaction_Id";
        public const string Column_Name_Pay_Transaction_Detail_Id = "Pay_Transaction_Detail_Id";
        public const string Column_Name_Pay_Sequence = "Pay_Sequence";

        #endregion tbl_Pay_Transaction

        #region tbl_PO_Topup

        public const string Column_Name_PO_Topup_Id = "PO_Topup_Id";
        public const string Column_Name_System_Id = "System_Id";
        public const string Column_Name_System_Date = "System_Date";
        public const string Column_Name_PO_Reference_Number = "PO_Reference_Number";
        public const string Column_Name_Topup_Amount = "Topup_Amount";
        public const string Column_Name_Client_PO_Mapping_Id = "Client_PO_Mapping_Id";

        #endregion tbl_PO_Topup

        #region tbl_Client_TDS_Slab

        public const string Column_Name_Client_TDS_Slab_Id = "Client_TDS_Slab_Id";
        public const string Column_Name_TDS_Percentage = "TDS_Percentage";
        public const string Column_Name_TDS_Exemption_Certification_Value = "TDS_Exemption_Certification_Value";

        #endregion tbl_Client_TDS_Slab

        #region tbl_Invoice

        public const string Column_Name_Invoice_Due_Date = "Invoice_Due_Date";
        public const string Column_Name_Client_PO = "Client_PO";
        public const string Column_Name_Utilized_Amount = "Utilized_Amount";
        public const string Column_Name_Customer_Name = "Customer_Name";
        public const string Column_Name_Input_Date = "Input_Date";
        public const string Column_Name_Output_Date = "Output_Date";
        public const string Column_Name_Particulars = "Particulars";
        public const string Column_Name_Charge_Name = "Charge_Name";
        public const string Column_Name_Reimbursement_Amount = "Reimbursement_Amount";
        public const string Columns_Name_Invoice_Type_Id = "Invoice_Type_Id";
        public const string Column_Name_Invoice_Type_Name = "Invoice_Type_Name";
        public const string Column_Name_Sourcing_Fee_Emp_Count = "Sourcing_Fee_Emp_Count";
        public const string Column_Name_Sourcing_Fee_Percentage = "Sourcing_Fee_Percentage";
        public const string Column_Name_One_time_Charges = "One_time_Charges";
        public const string Columns_Name_Service_Charge_Fixed_Per = "Service_Charge_Fixed_Per";
        public const string Columns_Name_No_of_Employees = "No_of_Employees";
        public const string Column_Name_Service_Charge_Amt = "Service_Charge_Amt";
        public const string Column_Name_Absorption_Fee = "Absorption_Fee";
        public const string Columns_Name_Absorption_Fee_Per = "Absorption_Fee_Per";
        public const string Column_Name_Absorption_Amt = "Absorption_Amt";
        public const string Column_Name_Tax_Scheme = "Tax_Scheme";
        public const string Columns_Name_Service_Tax_Rate_Per = "Service_Tax_Rate_Per";
        public const string Column_Name_Service_Tax_Amount = "Service_Tax_Amount";
        public const string Column_Name_Net_Amount = "Net_Amount";
        public const string Columns_Name_Edu_Cess_Rate_Per = "Edu_Cess_Rate_Per";
        public const string Column_Name_Edu_Cess_Amount = "Edu_Cess_Amount";
        public const string Column_Name_CTC_Amt_Adjusted = "CTC_Amt_Adjusted";
        public const string Column_Name_SB_Chess = "SB_Chess";
        public const string Columns_Name_Sec_Higher_Edu_Rate_Per = "Sec_Higher_Edu_Rate_Per";
        public const string Column_Name_Net_Amt_Adjusted = "Net_Amt_Adjusted";
        public const string Column_Name_CTC_Adj_Note = "CTC_Adj_Note";
        public const string Column_Name_Net_Adj_Note = "Net_Adj_Note";
        public const string Column_Name_Amount_In_Words = "Amount_In_Words";
        public const string Column_Name_Invoice_Raised_By = "Invoice_Raised_By";

        public const string Column_Name_Invoice_Detail_Id = "Invoice_Detail_Id";
        public const string Column_Name_Issue_log = "Issue_log";
        public const string Column_Name_Revised_By = "Revised_By";

        #endregion tbl_Invoice

        #region tbl_Invoice_Collection

        public const string Column_Name_Invoice_Collection_Id = "Invoice_Collection_Id";
        public const string Column_Name_Mode_Of_Collection = "Mode_Of_Collection";
        public const string Column_Name_Search_Mode_Of_Collection = "Search_Mode_Of_Collection";
        public const string Column_Name_Invoice_Location_Id = "Invoice_Location_Id";
        public const string Column_Name_Invoice_Location = "Invoice_Location";
        public const string Column_Name_Invoice_Map_Name = "Invoice_Map_Name";
        public const string Column_Name_Note = "Note";
        public const string Column_Name_Collection_Against = "Collection_Against";
        public const string Column_Name_On_Account_Amount = "On_Account_Amount";
        public const string Column_Name_Invoice_Id = "Invoice_Id";
        public const string Column_Name_Invoice_Number = "Invoice_Number";
        public const string Column_Name_Collection_Amount = "Collection_Amount";
        public const string Column_Name_Amount_Adjusted = "Amount_Adjusted";
        public const string Column_Name_Round_Off_Amount = "Round_Off_Amount";
        public const string Column_Name_Credit_Note_Date = "Credit_Note_Date";
        public const string Column_Name_Credit_Note_Amount = "Credit_Note_Amount";
        public const string Column_Name_Bank_Name = "Bank_Name";
        public const string Column_Name_Deposit_Bank_Id = "Deposit_Bank_Id";
        public const string Column_Name_Collection_Received_Date = "Collection_Received_Date";
        public const string Column_Name_Cheque_Number = "Cheque_Number";
        public const string Column_Name_Cheque_Date = "Cheque_Date";
        public const string Column_Name_Remarks = "Remarks";
        public const string Column_Name_Invoice_Collection_Detail_Id = "Invoice_Collection_Detail_Id";
        public const string Column_Name_Invoice_Amount = "Invoice_Amount";
        public const string Column_Name_TDSAmount = "TDSAmount";
        public const string Column_Name_RecAmount = "RecAmount";
        // public const string Column_Name_Invoice_Type = "Invoice_Type";

        #endregion tbl_Invoice_Collection

        #region tbl_Reimbursement

        public const string Column_Name_Reimbursement_Code = "Reimbursement_Code";
        public const string Column_Name_Reimbursement_Detail_Id = "Reimbursement_Detail_Id";
        public const string Column_Name_Reimbursement_Id = "Reimbursement_Id";
        public const string Column_Name_Reimbursement_Date = "Reimbursement_Date";

        #endregion tbl_Reimbursement

        #region SalaryReleaseDetails

        public const string Column_Name_Salary_Release_Batch_Id = "Salary_Release_Batch_Id";
        public const string Column_Name_Salary_Release_Batch_Name = "Salary_Release_Batch_Name";
        public const string Column_Name_Unique_No = "Unique_No";
        public const string Column_Name_Bank_Account_Number = "Salary_Release_Batch_Name";
        public const string Column_Name_IFSC_Code = "IFSC_Code";
        public const string Column_Name_PT_Code = "PT_Code";
        public const string Column_Name_Net_Pay = "Net_Pay";
        public const string Column_Name_Work_Location = "Work_Location";

        #endregion SalaryReleaseDetails

        #region BankAdviceGeneration

        public const string Column_Name_Status = "Status";
        public const string Column_Name_No_Of_Associates = "No_Of_Associates";
        public const string Column_Name_Email_Id = "Email_Id";
        public const string Column_Name_Invoice_Start_Range = "Invoice_Start_Range";
        public const string Column_Name_Invoice_End_Range = "Invoice_End_Range";

        #endregion BankAdviceGeneration

        #region Conveyance

        public const string Column_Name_Conveyance_Id = "Conveyance_Id";

        //  public const string Column_Name_Date_Of_Joining = "Date_Of_Joining";
        public const string Column_Name_From_Date = "From_Date";

        public const string Column_Name_To_Date = "To_Date";
        public const string Column_Name_Conveyance_Date = "Conveyance_Date";
        public const string Column_Name_Monthly_Eligible_Amount = "Monthly_Eligible_Amount";
        public const string Column_Name_Conveyance_Received = "Conveyance_Received";
        public const string Column_Name_Pay_Frequency_Detail_Id = "Pay_Frequency_Detail_Id";
        public const string Column_Name_Conveyance_Exemption = "Conveyance_Exemption";
        public const string Column_Name_Conveyance_Detail_Id = "Conveyance_Detail_Id";
        public const string Column_Name_Tax_Code_Id = "Tax_Code_Id";
        public const string Column_Name_ConveyanceAmount = "ConveyanceAmount";
        //  public const string Column_Name_Effective_Date = "Effective_Date";

        #endregion Conveyance

        #region MedicalBill

        public const string Column_Name_Medical_Bill_Id = "Medical_Bill_Id";
        public const string Column_Name_Medical_Bill_Date = "Medical_Bill_Date";
        public const string Column_Name_Disability = "Disability";
        public const string Column_Name_Medical_Exemption = "Medical_Exemption";
        public const string Column_Name_Medical_Bill_Detail_Id = "Medical_Bill_Detail_Id";
        public const string Column_Name_Medical_Received = "Medical_Received";
        public const string Column_Name_Disability_NonDisability = "Disability_NonDisability";
        public const string Column_Name_Effective_Date = "Effective_Date";
        public const string Column_Name_CheckAmount = "CheckAmount";

        #endregion MedicalBill

        #region tbl_HRA_And_HRA_Calculation_Details

        public const string Column_Name_HRA_Calculation_Id = "HRA_Calculation_Id";
        public const string Column_Name_HRA_Calculation_Date = "HRA_Calculation_Date";
        public const string Column_Name_Employee_Id = "Employee_Id";

        // public const string Column_Name_From_Date = "From_Date";
        // public const string Column_Name_To_Date = "To_Date";
        public const string Column_Name_Monthly_Rent_Paid = "Monthly_Rent_Paid";

        public const string Column_Name_Eligible_Basic = "Eligible_Basic";
        public const string Column_Name_Eligible_HRA = "Eligible_HRA";
        public const string Column_Name_Residing_Location = "Residing_Location";
        public const string Column_Name_HRA_Calculation_Detail_Id = "HRA_Calculation_Detail_Id";
        public const string Column_Name_Rent_Paid_Minus_Basic = "Rent_Paid_Minus_Basic";
        public const string Column_Name_Percentage_Of_Basic = "Percentage_Of_Basic";
        public const string Column_Name_HRA_Exemption = "HRA_Exemption";

        #endregion tbl_HRA_And_HRA_Calculation_Details

        #region Insurance

        public const string Column_Name_Insurance_Id = "Insurance_Id";
        public const string Column_Name_Insurance_Type = "Insurance_Type";
        public const string Column_Name_Insurance_Code = "Insurance_Code";
        public const string Column_Name_Insurance_Company_Name = "Insurance_Company_Name";
        public const string Column_Name_Insurance_Company_Premium = "Insurance_Company_Premium";
        public const string Column_Name_Monthly_Premium = "Monthly_Premium";
        public const string Column_Name_Coverage_Type = "Coverage_Type";
        public const string Column_Name_TPA_Name = "TPA_Name";
        public const string Column_Name_Sum_Assured = "Sum_Assured";
        public const string Column_Name_Policy_Number = "Policy_Number";
        public const string Column_Name_Insurance_Detail_Id = "Insurance_Detail_Id";
        public const string Column_Name_Company_Location_Id = "Company_Location_Id";
        public const string Column_Name_CoverageType = "CoverageType";
        public const string Column_Name_InsuranceType = "InsuranceType";

        #endregion Insurance

        #region InsuranceEmployeeReport

        public const string Column_Name_Employee_Insurance_Id = "Employee_Insurance_Id";
        public const string Column_Name_Name = "Name";
        public const string Column_Name_Relationship = "Relationship";
        public const string Column_Name_Gender = "Gender";
        public const string Column_Name_Insurance_Number = "Insurance_Number";
        public const string Column_Name_Rejoining_Date = "Rejoining_Date";
        public const string Column_Name_Date_Of_Birth = "Date_Of_Birth";
        public const string Column_Name_IKYA_Location = "IKYA_Location";
        public const string Column_Name_Policy_Start_Date = "Policy_Start_Date";
        public const string Column_Name_NoOfDaysToBeCovered = "NoOfDaysToBeCovered";
        public const string Column_Name_NoOfDaysToBeReCovered = "NoOfDaysToBeReCovered";
        public const string Column_Name_Policy_End_Date = "Policy_End_Date";
        public const string Column_Name_Insurance_Premium = "Insurance_Premium";
        public const string Column_Name_GMC_Sum_Assured = "GMC_Sum_Assured";
        public const string Column_Name_FixedAnnualPremium = "FixedAnnualPremium";
        public const string Column_Name_Prorata = "Prorata";

        #endregion InsuranceEmployeeReport

        public const string Column_Client_code = "Client_code";
        public const string Column_Company_ID = "Company_ID";
        public const string Column_Pay_Frequency_Id = "Pay_Frequency_Id";
        public const string Column_Pay_Period_Days = "Pay_Period_Days";
        public const string Column_Pay_Sequence_Number = "Pay_Sequence_Number";
        public const string Column_Working_Days = "Working_Days";
        public const string Column_Company_Name = "Company_Name";
        public const string Column_Employee_ID = "Employee_Id";
        public const string Column_Employee_Name = "Employee_Name";

        #region LOP Adjustment

        public const string Column_Name_Serial_No = "Serial_No";
        public const string Column_Name_LOP_Adjustment_Id = "LOP_Adjustment_Id";
        public const string Column_Name_LOP_Adjustment_Detail_Id = "LOP_Adjustment_Detail_Id";
        public const string Column_Name_Working_Days = "Working_Days";
        public const string Column_Name_Loss_Of_Pay_Days = "Loss_Of_Pay_Days";
        public const string Column_Name_Month_Days = "Month_Days";

        // public const string Column_Name_MonthDays= "MonthDays";
        public const string Column_Name_LOP = "LOP";

        public const string Column_Name_Pay_Period_Id = "Pay_Period_Id";
        public const string Column_Name_LOP_Restoration = "LOP_Restoration";

        // public const string Column_Pay_Period_Id ="Pay_Period_Id";
        public const string Column_Name_LOP_Month = "LOP_Month";

        public const string Column_Name_MonthDays = "MonthDays";
        public const string Column_Name_Work_Days = "Work_Days";
        public const string Column_Name_WorkDays = "WorkDays";
        public const string Column_Name_WorkDays_Atten = "Atten_Work_days";

        #endregion LOP Adjustment

        #region FullFinalSettelment

        public const string Columns_Name_Full_Final_Settlement_Id = "Full_Final_Settlement_Id";
        public const string Columns_Name_Component_Detail_Id = "Component_Detail_Id";

        public const string Columns_Name_LOP_Recovery = "LOP_Recovery";
        public const string Columns_Name_Notice_Pay_Salary = "Notice_Pay_Salary";
        public const string Columns_Name_Paid_Leave = "Paid_Leave";
        public const string Columns_Name_Notice_Pay_Recovery = "Notice_Pay_Recovery";
        public const string Columns_Name_Recovered_Days = "Recovered_Days";

        #endregion FullFinalSettelment

        #region LoanAndAdvancesDetails

        public const string Column_Name_Employee_Loan_Id = "Employee_Loan_Id";
        public const string Column_Name_Employee_Loan_Date = "Employee_Loan_Date";

        //public const string Column_Name_Employee_Id = "Employee_Id";
        public const string Column_Name_Loan_Type_Id = "Loan_Type_Id";

        public const string Column_Name_Loan_Number = "Loan_Number";
        public const string Column_Name_Opening_Balance = "Opening_Balance";
        public const string Column_Name_Loan_Advance_Amount = "Loan_Advance_Amount";
        public const string Column_Name_Start_Date = "Start_Date";
        public const string Column_Name_Interest_Type = "Interest_Type";
        public const string Column_Name_Bank_Interest = "Bank_Interest";
        public const string Column_Name_Interest_Rate_Given = "Interest_Rate_Given";
        public const string Column_Name_Perk_Percentage = "Perk_Percentage";
        public const string Column_Name_Number_Of_Installment = "Number_Of_Installment";
        public const string Column_Name_EMI = "EMI";
        public const string Column_Name_Pay_Sequence_Number = "Pay_Sequence_Number";
        public const string Column_Name_Outstanding_Principal = "Outstanding_Principal";
        //public const string Column_Name_Pay_Category_Id = "Pay_Category_Id";
        //public const string Column_Name_IsActive = "IsActive";
        //public const string Column_Name_CreatedBy = "CreatedBy";
        //public const string Column_Name_CreatedOn = "CreatedOn";
        //public const string Column_Name_ModifiedBy = "ModifiedBy";
        //public const string Column_Name_ModifiedOn = "ModifiedOn";

        //public const string Column_Name_Company_Id = "Company_Id";
        //public const string Column_Name_Company_Code = "Company_Code";
        //public const string Column_Name_Employee_Name = "Employee_Name";
        public const string Column_Name_Department_Name = "Department_Name";

        public const string Column_Name_Designation_Name = "Designation_Name";
        public const string Column_Name_Loan_Type = "Loan_Type";
        public const string Column_Name_Employee_Loan_Detail_Id = "Employee_Loan_Detail_Id";
        public const string Column_Name_Interest = "Interest";
        public const string Column_Name_Principal = "Principal";
        public const string Column_Name_Interest_Percentage = "Interest_Percentage";

        #endregion LoanAndAdvancesDetails

        #region Reimbursements

        //public const string Column_Name_Reimbursement_Code = "Reimbursement_Code";
        //public const string Column_Name_Reimbursement_Detail_Id = "Reimbursement_Detail_Id";
        //public const string Column_Name_Reimbursement_Id = "Reimbursement_Id";

        #endregion Reimbursements

        #region PayRegisterBuilder

        public const string Columns_Name_Type_Name = "Type_Name";
        public const string Column_Name_Type_Id = "Type_Id";

        #endregion PayRegisterBuilder

        #region PTBlock

        public const string Column_Name_PT_Block_Id = "PT_Block_Id";
        public const string Column_Name_PT_Block_Details_Id = "PT_Block_Details_Id";
        public const string Column_Name_Month = "Month";
        public const string Column_Name_Quaterly = "Quaterly";
        public const string Column_Name_Half_Yearly = "Half_Yearly";
        public const string Column_Name_Monthly = "Monthly";

        #endregion PTBlock

        #region PTType

        public const string Column_Name_PT_Type_Id = "PT_Type_Id";
        public const string Column_Name_PT_Type_Name = "PT_Type_Name";
        public const string Column_Name_PT_Type = "PT_Type";

        #endregion PTType

        #region MTD

        public const string Columns_Name_MTD = "MTD";
        public const string Columns_Client_Name = "Client_Name";
        public const string Columns_Name_Today = "Today";
        public const string Columns_Name_Total = "Total";

        #endregion MTD

        #region Approved Invoices For BankAdviceDetails

        public const string Column_Name_Pay_Peroid = "Pay_Peroid";
        public const string Column_Name_Billing_Location = "Billing_Location";
        public const string Column_Name_Mode_Of_Payment = "Mode_Of_Payment";
        public const string Column_Name_Head_Count = "Head_Count";
        public const string Column_Name_Hold_Head_Count = "Hold_Head_Count";
        public const string Column_Name_Hold_Salary_Amount = "Hold_Salary_Amount";
        public const string Column_Name_HDFC = "HDFC";
        public const string Column_Name_ICICI = "ICICI";
        public const string Column_Name_ICICI_PayDirect = "ICICI_PayDirect";
        public const string Column_Name_AXIS = "AXIS";
        public const string Column_Name_NEFT = "NEFT";
        public const string Column_Name_AC_Payee = "AC_Payee";
        public const string Column_Name_B_cheque = "B_cheque";
        public const string Column_Name_SBICheque = "SBICheque";
        //public const string Column_Name_Net_Pay = "Net_Pay";

        #endregion Approved Invoices For BankAdviceDetails

        #region Criteria_Type

        public const string Column_Name_Criteria_Type_Id = "Criteria_Type_Id";
        public const string Column_Name_Criteria_Type_Name = "Criteria_Type_Name";
        public const string Column_Name_Category = "Category";
        public const string Column_Name_From_Value = "From_Value";
        public const string Column_Name_To_Value = "To_Value";
        public const string Column_Name_Criteria = "Criteria";
        public const string Column_Name_Parameter_Id = "Parameter_Id";

        #endregion Criteria_Type

        #region PoNumber

        public const string Column_Name_Purchase_Order_Id = "Purchase_Order_Id";

        //  public const string Column_Name_Company_Id = "Company_Id";
        public const string Column_Name_Client_PO_Ref_No = "Client_PO_Ref_No";

        public const string Column_Name_PO_Date = "PO_Date";
        public const string Column_Name_PO_Based_On = "PO_Based_On";
        public const string Column_Name_Purchase_Request_No = "Purchase_Request_No";
        public const string Column_Name_Vertical_Type = "Vertical_Type";
        public const string Column_Name_PO_Amount = "PO_Amount";
        public const string Column_Name_PO_Valid_From = "PO_Valid_From";
        public const string Column_Name_Invoiced_Amount = "Invoiced_Amount";
        public const string Column_Name_Transfered_Amount = "Transfered_Amount";
        public const string Column_Name_PO_Valid_To = "PO_Valid_To";
        public const string Column_Name_Client_Id = "Client_Id";
        public const string Column_Name_Vertical_Type_Id = "Vertical_Type_Id";
        public const string Column_Name_Vertical_Type_Name = "Vertical_Type_Name";
        // public const string Column_Name_Purchase_Request_Id = "Purchase_Request_Id";

        #endregion PoNumber

        #region PoNumber

        //public const string Column_Name_Company_Id = "Company_Id";
        //public const string Column_Name_Company_Code = "Company_Code";
        //public const string Column_Name_PO_Based_On = "PO_Based_On";

        #endregion PoNumber

        #region PoEmployeeMapping

        public const string Column_Name_PO_Employee_Mapping_Id = "PO_Employee_Mapping_Id";
        public const string Column_Name_Vertical_Name = "Vertical_Name";
        public const string Column_Name_PO_Number = "PO_Number";
        public const string Column_Name_PO_Employee_Mapping_Details_Id = "PO_Employee_Mapping_Details_Id";

        #endregion PoEmployeeMapping

        #region PoEmployeeMapping

        // public const string Column_Name_Vertical_Type_Id = "Vertical_Type_Id";
        public const string Column_Name_Vertical_Type_Code = "Vertical_Type_Code";

        //  public const string Column_Name_Vertical_Type_Name = "Vertical_Type_Name";

        #endregion PoEmployeeMapping

        public const string Column_Service_Charge_Master_Id = "Service_Charge_Master_Id";
        public const string Column_Service_Charge_Master_Name = "Service_Charge_Master_Name";
        public const string Column_Service_Charge_Slab_Inner_Item_Id = "Service_Charge_Slab_Inner_Item_Id";
        public const string Column_Service_Charge_Slab_Inner_Item_Name = "Service_Charge_Slab_Inner_Item_Name";
        public const string Column_Service_Charge_Slab_Item_Id = "Service_Charge_Slab_Item_Id";
        public const string Column_Service_Charge_Slab_Item_Name = "Service_Charge_Slab_Item_Name";

        public const string Column_InvoiceType_Id = "InvoiceType_Id";
        public const string Column_InvoiceType = "InvoiceType";
        public const string Column_Cost_Center_Mapping_Id = "Cost_Center_Mapping_Id";
        public const string Column_Map_Name = "Map_Name";
        public const string Column_Map_Name_Id = "Map_Name_Id";
        public const string Column_Type_Of_Invoice_Id = "Type_Of_Invoice_Id";
        public const string Column_Type_Of_Type_Of_Invoice_Name = "Type_Of_Invoice_Name";

        public const string Column_Service_Charge_Type_Name = "Service_Charge_Type_Name";
        public const string Column_Service_Charge_Type_Id = "Service_Charge_Type_Id";


        //Remittance Upload

        public const string Columns_Name_Remittancedropdown_id = "Remittancedropdown_id";
        public const string Columns_Name_Remittancedropdown_Name = "Remittancedropdown_Name";



        //Compilance Upload

        public const string Columns_Name_CompilanceUpload_id = "Compilancedropdown_id";
        public const string Columns_Name_CompilanceUpload_Name = "Compilancedropdown_Name";
    }
}
