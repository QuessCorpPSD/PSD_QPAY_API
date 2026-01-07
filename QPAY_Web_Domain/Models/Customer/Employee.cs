using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Customer
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "EmployeeDetails")]
    [System.Serializable()]
    public class EmployeeResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("Employees")]
        public EmployeeWithDetails[] EmployeeDetails { get; set; }
    }

    //public class EmployeeEmploymentDetailResponse
    //{
    //    [System.Xml.Serialization.XmlElementAttribute("EmployeeEmploymentDetailDetails")]
    //    public EmployeeEmploymentDetail[] EmployeeEmploymentDetailDetails { get; set; }
    //}
    public class EmployeeBankDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeBankDetails")]
        public EmployeeBankDetail[] EmployeeBankDetails { get; set; }
    }

    public class EmployeeContactResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeContactDetails")]
        public EmployeeContactDetail[] EmployeeContactDetails { get; set; }
    }

    public class EmployeePersonalResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeePersonalDetails")]
        public EmployeePersonalDetail[] EmployeePersonalDetails { get; set; }
    }



    public class EmployeePreviousEmploymentResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeePreviousEmploymentDetails")]
        public EmployeePreviousEmployment[] EmployeePreviousEmploymentDetails { get; set; }
    }

    public class EmployeeInformationResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeInformationDetails")]
        public EmployeeInformation[] EmployeeInformationDetails { get; set; }
    }



    public class EmployeeSalaryInformationResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeSalaryInformation")]
        public EmployeeSalaryInformation[] EmployeeSalaryInformationeDetails { get; set; }
    }

    public class EmployeeSalaryInformationDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeSalaryInformationDetail")]
        public EmployeeSalaryInformationDetail[] EmployeeSalaryInformationDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Employee")]
    public class Employees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Id { get; set; }

        public string Employee_Code { get; set; }
        public DateTime Effective_Date { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Father_Name { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public bool Gender { get; set; }
        public string Identification { get; set; }
        public string Languages_Known { get; set; }
        public string Blood_Group { get; set; }
        public bool Disability { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public int Cost_Center_Mapping_Id { get; set; }
        public string Cost_Center_Name { get; set; }
        public string Marital_Status { get; set; }
        public string Hiring_Status { get; set; }
        public string Deputee_Id { get; set; }
        public string DeputeeIdExists { get; set; }
        public string ETDS_Sequence { get; set; }
        public string DMS_Id { get; set; }
        public int Entity_Location_Id { get; set; }
        public string EntityLocation { get; set; }
        public int Employee_Employment_Detail_Id { get; set; }
        public int Group_Detail_Id { get; set; }
        public string Group_Name { get; set; }
        public string Business_Head { get; set; }

        //public string Instituition { get; set; }
        public string Report_Manager { get; set; }

        public string Reporting_Head_Email { get; set; }
        public string IKYA_Location { get; set; }
        public string Reason_Of_Leaving { get; set; }
        public DateTime Date_Of_Joining { get; set; }
        public DateTime Rejoinee_Date { get; set; }
        public string Joining_Pay_Period { get; set; }
        public int Department_Id { get; set; }
        public string Department_Name { get; set; }
        public int Band_Id { get; set; }
        public string Band_Name { get; set; }

        // public int User_Group { get; set; } user group is role here
        //instead of user group we r picking role coz we r using role table jst to avoid confusion.
        //public int Role_Id { get; set; }
        //public string Role_Name { get; set; }
        public string Axpert_Id { get; set; }

        public int User_Group_Id { get; set; }
        public string User_Group_Name { get; set; }
        public int PT_State_Id { get; set; }
        public string PT_State { get; set; }
        public int LWF_State_Id { get; set; }
        public string LWF_State { get; set; }
        public string Rejoin_Month { get; set; }
        public bool Stop_Payment { get; set; }
        public int Designation_Id { get; set; }
        public string Designation_Name { get; set; }
        public int Work_Location_Id { get; set; }
        public string Work_Location { get; set; }
        public bool Is_PT_Applicable { get; set; }
        public bool Is_Metro_City { get; set; }

        public int Billing_Designation_Id { get; set; }

        public int Billing_Designation_Name { get; set; }

        //public int Cost_Center_Id { get; set; }
        //cost center is vertical here as it is fetched from vertical tale
        public int Vertical_Id { get; set; }

        public string Vertical_Name { get; set; }
        public bool Is_PF_Applicable { get; set; }
        public bool Is_Insurance_Applicable { get; set; }
        public bool Is_ESI_Applicable { get; set; }
        public DateTime Resignation_Date { get; set; }
        public DateTime Last_Working_Day { get; set; }
        public string Resign_Period { get; set; }
        public int EntityID { get; set; }
        public string Entity_Name { get; set; }
        public int Employee_Bank_Detail_Id { get; set; }
        public int Bank_Id { get; set; }
        public int Serial_No { get; set; }
        public string Error_Message { get; set; }
        public bool EActive { get; set; }
        public string EActiveText { get; set; }
        public bool CapNonCap { get; set; }
        public bool New_Tax_Regime { get; set; }
    }

    [Table("tbl_Employee_Bank_Detail")]
    public class EmployeeBankDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Bank_Detail_Id { get; set; }

        public int Employee_Id { get; set; }
        public int Bank_Id { get; set; }
        public string Bank_Name { get; set; }
        public string Branch_Name { get; set; }
        public string Bank_Account_Number { get; set; }
        public string Swift_Code { get; set; }
        public string Bank_Code { get; set; }
        public string Branch_Code { get; set; }
        public string Nominee_Name { get; set; }
        public string Nominee_Relationship { get; set; }
        public string Nominee_Age { get; set; }
        public string Nominee_Swift_Code { get; set; }

        public string Nominee_Bank_Account { get; set; }
        public string Nominee_Bank_Account_Number { get; set; }
        public string Nominee_Bank_Code { get; set; }
        public string Nominee_Branch_Code { get; set; }

    }

    [Table("tbl_Employee_Contact_Detail")]
    public class EmployeeContactDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Contact_Detail_Id { get; set; }

        public int Employee_Id { get; set; }
        public string Address { get; set; }
        // public string City { get; set; }
        public string Pin_Code { get; set; }
        public string Mobile_Number { get; set; }
        //  public string Telephone { get; set; }
        public string Email_Id { get; set; }
        // public int State_Id { get; set; }
        public string Contact_Person { get; set; }
        public string Emergency_Contact_Person { get; set; }
    }

    [Table("tbl_Employee_Information")]
    public class EmployeeInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Information_Id { get; set; }

        public int Employee_Id { get; set; }
        public string Passport_Number { get; set; }
        public DateTime Passport_Expiry_Date { get; set; }
        public string Place_Of_Issue { get; set; }
        //public string PAN_Number { get; set; }
        public string Gun_License_Number { get; set; }
        public string Driving_License_Number { get; set; }
        //public string ESI_Number { get; set; }
        //public string PF_Number { get; set; }
        //public string PF_Number_Backup { get; set; }
        //public string UAN_Number { get; set; }
        //public string Aadhaar_Number { get; set; }

        //public string PRAN_Number { get; set; }
        //public string UAN_Type { get; set; }


        public string NRIC_FIN_NUMBER { get; set; }
        public string FUND_LEVY { get; set; }
        public int SPR_STATUS_ID { get; set; }
        public string SPR_APPROVE_DATE { get; set; }
        //  public string LEAVE_SCHEME { get; set; }
        //  public string PROFESSION_CODE { get; set; }
        //public string PROG_CODE1 { get; set; }
        //public string PROG_CODE2 { get; set; }
        public string VISA_NUMBER { get; set; }
        public string INSURANCE_NUMBER { get; set; }
        public string VISA_DURATION_START_DATE { get; set; }
        public string VISA_DURATION_END_DATE { get; set; }
        public string WORK_PASS_ID { get; set; }
        public string RELIGION { get; set; }

    }

    [Table("tbl_Employee_Personal_Detail")]
    public class EmployeePersonalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Personal_Detail_Id { get; set; }

        public int Employee_Id { get; set; }
        public string Birth_Place { get; set; }
        // public int Home_State { get; set; }
        public string Nominee { get; set; }
        public string Relationship { get; set; }
        // public string Recruiter_Name { get; set; }
        //public string Religion { get; set; }
        // public string Nationality { get; set; }
        //public bool Is_Ex_Service { get; set; }

        public string RACE_CODE { get; set; }
        public string NATIONAL_CODE { get; set; }
        public int RFUND_CODE1 { get; set; }
        public int RFUND_CODE2 { get; set; }
        public string COUNTRY_OF_BIRTH { get; set; }

    }

    [Table("tbl_Employee_Previous_Employment_Detail")]
    public class EmployeePreviousEmployment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Previous_Employment_Detail_Id { get; set; }

        //public int Employee_Id { get; set; }
        // public string Employee_Code { get; set; }

        public string Company_Name { get; set; }
        public string Designation { get; set; }
        public decimal Experience_In_Years { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        // public string Error_Message { get; set; }

    }


    public class EmployeeWithDetails
    {
        //public string F_Resign_Period { get; set; } 
        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }

        public int SPR_Status { get; set; }
        public DateTime Effective_Date { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Father_Name { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public bool Gender { get; set; }
        //public string GenderText { get; set; } 
        //public string Identification { get; set; } 
        public string Languages_Known { get; set; }
        public string Blood_Group { get; set; }
        public bool Disability { get; set; }
        //public string DisabilityText { get; set; } 
        //public decimal Height { get; set; } 
        //public decimal Weight { get; set; } 
        public DateTime Date_Of_Birth { get; set; }
        public int Cost_Center_Mapping_Id { get; set; }
        // public string Cost_Center_Name { get; set; } 
        public string Marital_Status { get; set; }
        public string Hiring_Status { get; set; }
        public string Deputee_Id { get; set; }
        //public string ETDS_Sequence { get; set; } 
        public string DMS_Id { get; set; }
        public int Entity_Location_Id { get; set; }
        //public string EntityLocation { get; set; } 
        //public int Employee_Employment_Detail_Id { get; set; } 
        public int Group_Detail_Id { get; set; }
        //public string Group_Name { get; set; } 
        public string Business_Head { get; set; }

        //public string Instituition { get; set; } 
        public string Report_Manager { get; set; }

        public string Reporting_Head_Email { get; set; }
        // public string IKYA_Location { get; set; } 
        public string Reason_Of_Leaving { get; set; }
        public DateTime Date_Of_Joining { get; set; }
        public DateTime Rejoinee_Date { get; set; }
        public string Joining_Pay_Period { get; set; }
        public int Department_Id { get; set; }
        // public string Department_Name { get; set; } 
        public int Band_Id { get; set; }
        // public string Band_Name { get; set; } 
        // public int User_Group_Id { get; set; } 
        // public string User_Group_Name { get; set; } 
        // public int PT_State_Id { get; set; } 
        // public string PT_State { get; set; } 
        //public int LWF_State_Id { get; set; } 
        // public string LWF_State { get; set; } 
        public string Rejoin_Month { get; set; }
        public bool Stop_Payment { get; set; }
        public int Designation_Id { get; set; }
        //public string Designation_Name { get; set; } 

        //public int Billing_Designation_Id { get; set; } 

        //public string Billing_Designation_Name { get; set; } 
        //public int Work_Location_Id { get; set; } 
        public string Work_Location { get; set; }
        //public bool Is_PT_Applicable { get; set; } 
        //public bool Is_Metro_City { get; set; } 
        //public int Vertical_Id { get; set; } 
        //public string Vertical_Name { get; set; } 
        public bool Is_PF_Applicable { get; set; }
        //public bool Is_ESI_Applicable { get; set; } 
        public bool Is_Insurance_Applicable { get; set; }
        public DateTime Resignation_Date { get; set; }
        public DateTime Last_Working_Day { get; set; }
        public string Resign_Period { get; set; }
        public int EntityID { get; set; }
        //public string Entity_Name { get; set; } 
        // public int Employee_Bank_Detail_Id { get; set; } 
        //public int Bank_Id { get; set; } 
        //public string Bank_Name { get; set; } 
        //public string Branch_Name { get; set; } 
        //public string PF_Number_Backup { get; set; } 
        //public string UAN_Number { get; set; } 
        //public string Nominee_Bank_Account { get; set; } 
        //public string Nominee_Bank_Account_Number { get; set; } 
        //public string Bank_Account_Number { get; set; } 
        //public string IFSC_Code { get; set; } 
        //public int Employee_Contact_Detail_Id { get; set; } 
        //public string Address { get; set; } 
        //public string City { get; set; } 
        //public string Pin_Code { get; set; } 
        //public string Mobile_Number { get; set; } 
        //public string Telephone { get; set; } 
        //public string Email_Id { get; set; } 
        //public string Contact_Person { get; set; } 
        //public string Emergency_Contact_Person { get; set; } 
        //public int State_Id { get; set; } 
        //public string State_Name { get; set; } 
        //public string Country { get; set; } 
        //public int Employee_Personal_Detail_Id { get; set; } 
        //public string Birth_Place { get; set; } 
        //public string Nominee { get; set; } 
        //public string Relationship { get; set; } 
        //public string Recruiter_Name { get; set; } 
        //public int Home_State { get; set; } 
        //public string Home_State_Name { get; set; } 
        //public string Religion { get; set; } 
        //public string Nationality { get; set; } 
        //public bool Is_Ex_Service { get; set; } 
        //public int Employee_Information_Id { get; set; } 
        //public string Passport_Number { get; set; } 
        //public DateTime Passport_Expiry_Date { get; set; } 
        //public string Place_Of_Issue { get; set; } 
        //public string PAN_Number { get; set; } 
        //public string Gun_License_Number { get; set; } 
        //public string Driving_License_Number { get; set; } 
        //public string ESI_Number { get; set; } 
        //public string PF_Number { get; set; } 
        //public string Aadhaar_Number { get; set; } 
        //public int Serial_No { get; set; } 
        //public string Error_Message { get; set; } 
        //public int Asset_Id { get; set; } 
        //public string Gun_Issued_Serial_No { get; set; } 
        //public string Gun_License_No { get; set; } 
        //public string Number_of_Cartridges { get; set; } 
        //public string AMobile_Number { get; set; } 
        //public string Sim_Number { get; set; } 
        //public string IMEI_Number { get; set; } 
        //public string Mobile_Model_Number { get; set; } 
        //public string Laptop_Model_Number { get; set; } 
        //public string Laptop_Serial_Number { get; set; } 
        //public string Data_Card_Sim_Number { get; set; } 
        //public string Data_Card_Serial_Number { get; set; } 
        public bool EActive { get; set; }
        // public string EActiveText { get; set; } 

        //public bool CapNonCap { get; set; } 
        // public int CapNonCap { get; set; }//Mahadev 

        // public string CapNonCaptext { get; set; }//mahadev 
        public string Axpert_Id { get; set; }
        //public Int32 TotalNoofRows { get; set; } 
        //public string INSURANCE_NUMBER { get; set; } 
        //public int EMPLOYMENT_TYPE { get; set; } 

        ////#4 
        //public string Nominee_Name { get; set; } 

        //public string Nominee_Relationship { get; set; } 
        //public string Nominee_Age { get; set; } 
        //public string Nominee_Ifsc_Code { get; set; } 

        ////#5 
        public bool Is_Black_Listed { get; set; }

        //public string Black_Listed_Remarks { get; set; } 

        //public DateTime Contract_Expiry_Date { get; set; } //Rudra Changes 

        //public string GRADE { get; set; } 
        //public bool New_Tax_Regime { get; set; } 

        //public int VerticalId { get; set; } 
        //public int SubVerticalId { get; set; } 
        //public int ProductId { get; set; } 
        //public int ChannelId { get; set; } 
        //public string Alternate_Bank { get; set; } 
        //public string Alternate_Account_Number { get; set; } 
        //public string Alternate_Ifsc_Code { get; set; } 
        //public DateTime Abscond_Reporting_Date { get; set; } 
        //public string PRAN_Number { get; set; } 
        //public string UAN_Type { get; set; } 
        public string Date_Of_Death { get; set; }
        public string Death_DocPath { get; set; }
        public int? Invoice_Legal_Entity { get; set; }
    }


    [Table("tbl_Security")]
    public class EmployeeSecurity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Security_Id { get; set; }

        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }
        public string Gun_Issued_Serial_No { get; set; }
        public string Gun_License_No { get; set; }
        public string Number_of_Cartridges { get; set; }
        public string Error_Message { get; set; }
    }

    public class EmployeeSecurityResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeSecurityDetails")]
        public EmployeeSecurity[] EmployeeSecurityDetails { get; set; }
    }

    [Table("tbl_Assets")]
    public class EmployeeAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Asset_Id { get; set; }

        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }
        public string AMobile_Number { get; set; }
        public string Sim_Number { get; set; }
        public string IMEI_Number { get; set; }
        public string Mobile_Model_Number { get; set; }
        public string Laptop_Model_Number { get; set; }
        public string Laptop_Serial_Number { get; set; }
        public string Data_Card_Sim_Number { get; set; }
        public string Data_Card_Serial_Number { get; set; }
        public string Gun_Issued_Serial_No { get; set; }
        public string Gun_License_No { get; set; }
        public string Number_of_Cartridges { get; set; }
        public string Error_Message { get; set; }
    }

    public class EmployeeAssetResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeAssetDetails")]
        public EmployeeAsset[] EmployeeAssetDetails { get; set; }
    }

    [Table("tbl_Employee_Salary_Information_Detail")]
    public class EmployeeSalaryInformationDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Salary_Information_Detail_Id { get; set; }

        public int Employee_Salary_Information_Id { get; set; }
        public int Paycode_Id { get; set; }
        public string Paycode_Code { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Amount_Per_Annum { get; set; }
    }

    [Table("tbl_Employee_Salary_Information")]
    public class EmployeeSalaryInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Salary_Information_Id { get; set; }

        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }
        public DateTime Effective_Date { get; set; }
        public decimal Total_CTC { get; set; }
        public DateTime Applied_On { get; set; }
        public string Error_Message { get; set; }
    }

    public class EmployeeSalaryInformationWithDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Salary_Information_Detail_Id { get; set; }

        public int Employee_Salary_Information_Id { get; set; }
        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }
        public DateTime Effective_Date { get; set; }
        public decimal Total_CTC { get; set; }
        public DateTime Applied_On { get; set; }
        public int Paycode_Id { get; set; }
        public string Paycode_Code { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Amount_Per_Annum { get; set; }
        public int Serial_No { get; set; }
        public string Error_Message { get; set; }
    }

    public class NewJoineeSalaryUpload
    {
        [Key]
        public string XML_File { get; set; }

        public int CreatedBy { get; set; }
        public string message { get; set; }
        public List<EmployeeSalaryInformationWithDetail> SalaryUploadMsg { get; set; }
    }

    [Table("tbl_Employee_Insurance")]
    public class EmployeeInsurance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Insurance_Id { get; set; }

        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }
        public Boolean Is_Insurance_Applicable { get; set; }

        //public int Financial_Year_Id { get; set; }
        //public string Financial_Year_Name { get; set; }
        public decimal Monthly_Premium { get; set; }

        public decimal Insurance_Premium { get; set; }
        public string Insurance_Status { get; set; }
        public Boolean Is_ESI_Applicable { get; set; }
        public decimal GMC_Sum_Assured { get; set; }
        public decimal GTLI_Sum_Assured { get; set; }
        public decimal GPA_Sum_Assured { get; set; }
    }

    [Table("tbl_Employee_Insurance_Detail")]
    public class EmployeeInsuranceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_Insurance_Detail_Id { get; set; }

        public int Employee_Insurance_Id { get; set; }
        public string Name { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public Boolean Gender { get; set; }
        public string IsGender { get; set; }
        public int Age { get; set; }
        public string Relationship { get; set; }
        public int InsuranceType_Id { get; set; }
        public string Insurance_Number { get; set; }
        public string Policy_Number { get; set; }
        public string Policy_Upload_Path { get; set; }
        public DateTime Policy_Start_Date { get; set; }
        public DateTime Policy_End_Date { get; set; }
        public string Insurance_File_Path { get; set; }
        public string Insurance_File_Name { get; set; }
        public string Insurance_Actual_File_Name { get; set; }
    }

    public class EmployeeInsuranceWithDetail
    {
        public int Employee_Insurance_Detail_Id { get; set; }
        public int Employee_Insurance_Id { get; set; }
        public string Name { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public Boolean Gender { get; set; }
        public int Age { get; set; }
        public string Relationship { get; set; }
        public int InsuranceType_Id { get; set; }
        public string Insurance_Number { get; set; }
        public string Policy_Number { get; set; }
        public string Policy_Upload_Path { get; set; }
        public DateTime Policy_Start_Date { get; set; }
        public DateTime Policy_End_Date { get; set; }
        public string Insurance_File_Path { get; set; }
        public string Insurance_File_Name { get; set; }
        public string Insurance_Actual_File_Name { get; set; }
        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }
        public Boolean Is_Insurance_Applicable { get; set; }

        //public int Financial_Year_Id { get; set; }
        //public string Financial_Year_Name { get; set; }
        public decimal Monthly_Premium { get; set; }

        public decimal Insurance_Premium { get; set; }
        public string Insurance_Status { get; set; }
        public Boolean Is_ESI_Applicable { get; set; }
        public decimal GMC_Sum_Assured { get; set; }
        public decimal GTLI_Sum_Assured { get; set; }
        public int Serial_No { get; set; }
        public string Error_Message { get; set; }
        public string IsGender { get; set; }
        public string Is_Insurance_ApplicableText { get; set; }
        public string Is_ESI_ApplicableText { get; set; }
        public string Insurance_Type_Name { get; set; }
        public decimal GPA_Sum_Assured { get; set; }
    }

    public class EmployeeInsuranceResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeInsurance")]
        public EmployeeInsurance[] EmployeeInsuranDetails { get; set; }
    }

    public class EmployeeInsuranceDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("EmployeeInsuranceDetail")]
        public EmployeeInsuranceDetail[] EmployeeInsuranceDetails { get; set; }
    }

    public class EmployeeImport
    {
        public string Comp_Id { get; set; }
        public string Name { get; set; }
        public string Father_Name { get; set; }
        public string Gender { get; set; }
        public string Blood { get; set; }
        public DateTime DOJ { get; set; }
        public DateTime DOB { get; set; }
        public string Active { get; set; }
        public string ExService { get; set; }
        public string Marital { get; set; }
        public string Disability { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string PayCategory { get; set; }
        public string PTState { get; set; }
        public string PT { get; set; }
        public string PF { get; set; }
        public string ESI { get; set; }
        public string Stop { get; set; }
        public string PAN { get; set; }
        public string Bank_Name { get; set; }
        public string ACNO { get; set; }
        public string Work_location { get; set; }
        public string Email { get; set; }
        public string Date_Of_Join_PayPayPeriod { get; set; }
        public string IFSCode { get; set; }
        public string Hiring_Status { get; set; }
        public string Ikya_Location { get; set; }
        public string Map_Name { get; set; }
        public string Recruiters_Name { get; set; }
        public string MobileNo { get; set; }
        public string DeputeeId { get; set; }
        public string ESINumber { get; set; }
        public string DMIID { get; set; }
        public string Error_Message { get; set; }



    }

    public class EmployeeUpload
    {
        [Key]
        public string XML_File { get; set; }

        public int CreatedBy { get; set; }
        public string message { get; set; }
        public List<EmployeeImport> _EmployeeMessage { get; set; }
    }

    public class EmployeeRequest
    {

        public int createdBy { get; set; }
        public EmployeeWithDetails detail { get; set; }
    }

    public class EmployeeBankRequest
    {

        public int createdBy { get; set; }
        public EmployeeBankDetail detail { get; set; }
    }

    public class EmployeePersonalRequest
    {

        public int createdBy { get; set; }
        public EmployeePersonalDetail detail { get; set; }
    }

    public class EmployeeContactRequest
    {

        public int createdBy { get; set; }
        public EmployeeContactDetail detail { get; set; }
    }

    public class EmployeeInformationRequest
    {

        public int createdBy { get; set; }
        public EmployeeInformation detail { get; set; }
    }

    public class EmployeePreviousRequest
    {

        public int createdBy { get; set; }
        public EmployeePreviousEmployment detail { get; set; }
    }

    public class LegalEntityUI
    {
        public string? LegalEntityId { get; set; }
        public string? LegalEntityName { get; set; }
    }

    public class EmployeeApiResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }

}