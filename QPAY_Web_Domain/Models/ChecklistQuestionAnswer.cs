using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public class ChecklistQuestionAnswer
    {
        public string QUESTION_ID { get; set; }
        public string QUESTION_ORDER { get; set; }
        public string QUESTION_NAME { get; set; }
        public string ANSWER_TYPE { get; set; }
        public string MULTIPLE_ANSWER_FLAG { get; set; }
        public string ANSWER_FLAG { get; set; }

        public List<CheklistAnswer1> cheklistAnswer1s { get; set; }


    }

    public class CheklistAnswer1
    {
        public string ANSWER_ORDER { get; set; }
        public string QUESTION_ID { get; set; }
        public string ANSWER_ID { get; set; }
        public string ANSWER_NAME { get; set; }
        public string ANSWER_TITLE { get; set; }
        public string SUB_ANSWER_FLAG { get; set; }
        public string ANSWER_TYPE { get; set; }

    }

    public class ResponseChecklistQuestionAnswer
    {
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
        public List<ChecklistQuestionAnswer> checklistQuestionAnswers { get; set; }
    }


    public class CustomerSOPQuestion
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<CustomerSOPQuestions> questions { get; set; }

    }

    public class CustomerSOPQuestions
    {
        public string QuestionId { get; set; }
        public string QuestionOrder { get; set; }
        public string QuestionName { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public List<CustomerSOPAnswer1> customersopanswer1s { get; set; }
    }

    public class CustomerSOPAnswer1
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_1_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }

        public List<CustomerSOPAnswer2> customersopanswer2s { get; set; }
    }

    public class CustomerSOPAnswer2
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_2_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer3> customersopanswer3s { get; set; }
    }

    public class CustomerSOPAnswer3
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_3_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer4> customersopanswer4s { get; set; }
    }

    public class CustomerSOPAnswer4
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_4_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer5> customersopanswer5s { get; set; }
    }

    public class CustomerSOPAnswer5
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_5_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer6> customersopanswer6s { get; set; }
    }

    public class CustomerSOPAnswer6
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerId_6 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_6_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer7> customersopanswer7s { get; set; }
    }
    public class CustomerSOPAnswer7
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerId_6 { get; set; }
        public string AnswerId_7 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_7_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer8> customersopanswer8s { get; set; }
    }

    public class CustomerSOPAnswer8
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerId_6 { get; set; }
        public string AnswerId_7 { get; set; }
        public string AnswerId_8 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_8_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer9> customersopanswer9s { get; set; }
    }

    public class CustomerSOPAnswer9
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerId_6 { get; set; }
        public string AnswerId_7 { get; set; }
        public string AnswerId_8 { get; set; }
        public string AnswerId_9 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_9_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer10> customersopanswer10s { get; set; }
    }
    public class CustomerSOPAnswer10
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerId_6 { get; set; }
        public string AnswerId_7 { get; set; }
        public string AnswerId_8 { get; set; }
        public string AnswerId_9 { get; set; }
        public string AnswerId_10 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_10_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer11> customersopanswer11s { get; set; }
    }

    public class CustomerSOPAnswer11
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerId_6 { get; set; }
        public string AnswerId_7 { get; set; }
        public string AnswerId_8 { get; set; }
        public string AnswerId_9 { get; set; }
        public string AnswerId_10 { get; set; }
        public string AnswerId_11 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_11_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer12> customersopanswer12s { get; set; }
    }

    public class CustomerSOPAnswer12
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerId_6 { get; set; }
        public string AnswerId_7 { get; set; }
        public string AnswerId_8 { get; set; }
        public string AnswerId_9 { get; set; }
        public string AnswerId_10 { get; set; }
        public string AnswerId_11 { get; set; }
        public string AnswerId_12 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_12_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
        public List<CustomerSOPAnswer13> customersopanswer13s { get; set; }
    }

    public class CustomerSOPAnswer13
    {
        public string QuestionId { get; set; }
        public string AnswerId_1 { get; set; }
        public string AnswerId_2 { get; set; }
        public string AnswerId_3 { get; set; }
        public string AnswerId_4 { get; set; }
        public string AnswerId_5 { get; set; }
        public string AnswerId_6 { get; set; }
        public string AnswerId_7 { get; set; }
        public string AnswerId_8 { get; set; }
        public string AnswerId_9 { get; set; }
        public string AnswerId_10 { get; set; }
        public string AnswerId_11 { get; set; }
        public string AnswerId_12 { get; set; }
        public string AnswerId_13 { get; set; }
        public string AnswerType { get; set; }
        public string Answer_13_Name { get; set; }
        public string Title { get; set; }
        public string IsMandatory { get; set; }
        public string IsActive { get; set; }
        public string AddMultiple { get; set; }
        public string SubAnswerFlag { get; set; }
        public string API_URL { get; set; }
        public string API_Result_Id { get; set; }
        public string API_Result_Value { get; set; }
    }

    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
    }


    public class CompanyMaster
    {
        public string Company_Id { get; set; }
        public string Company_Code { get; set; }
        public string Client_Name { get; set; }
        public string SAP_Code { get; set; }
        public string MyContractReferenceID { get; set; }
    }


    public class StateMaster
    {
        public string State_Id { get; set; }
        public string State_Code { get; set; }
        public string State_Name { get; set; }

    }

    public class CityMaster
    {
        public string City_Id { get; set; }
        public string City_Code { get; set; }
        public string City_Name { get; set; }
    }

    public class DesignationMaster
    {
        public string Designation_Id { get; set; }
        public string Designation_Name { get; set; }
    }

    public class FirstMonthPayroll
    {
        public string Company_Code { get; set; }
        public string Pay_Period { get; set; }
    }

    //public class SOPModelUI
    //{
    //    public int UniqueId { get; set; }
    //    public int CategoryId { get; set; }
    //    public int QuestionOrder { get; set; }
    //    public int QuestionId { get; set; }
    //    public int SubId { get; set; }
    //    public string QuestionName { get; set; } = "";
    //    public string Attribute { get; set; } = "";
    //    public string IsMulti { get; set; } = "";
    //    public string IsMandatory { get; set; }

    //}

    public class SOPModelsUI
    {
        public int UniqueId { get; set; }
        public int CategoryId { get; set; }
        public int QuestionOrder { get; set; }
        public int QuestionId { get; set; }
        public int SubId { get; set; }
        public string QuestionName { get; set; } = "";
        public string Attribute { get; set; } = "";
        public string IsMulti { get; set; } = "";
        public string IsMandatory { get; set; }

        public List<SOPModelsUI> SOPModels { get; set; }

    }

    public class Marked_Category

    {

        public string CategoryId { get; set; }

        public string Perc { get; set; }

        public List<Marked_Question> Marked_Question { get; set; }

    }

    public class Marked_Question

    {

        public string QuestionId { get; set; }

    }


    public class Category
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
    }

    public class Question
    {
        public string CategoryId { get; set; }
        public string QuestionId { get; set; }
        public string QuestionOrder { get; set; }
        public string QuestionName { get; set; }
    }

    public class Answer1
    {
        public string QuestionId { get; set; }
        public string Client_website_link { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer2
    {
        public string QuestionId { get; set; }
        public string Company_Code { get; set; }
        public string Company_Name { get; set; }
        public string SAP_Code { get; set; }
        public string MyContract_Reference_ID { get; set; }
        public string CreatedBy { get; set; }
    }
    public class Answer3
    {
        public string QuestionId { get; set; }
        public string POC_Change { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer6
    {
        public string QuestionId { get; set; }
        public string FF_Payment_Mode { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer8
    {
        public string QuestionId { get; set; }
        public string Sim_Card_Management_tracker { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer9
    {
        public string QuestionId { get; set; }
        public string Email_ID_Managemnet_Tracker { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer10
    {
        public string QuestionId { get; set; }
        public string ID_Card_Managemnet_Tracker { get; set; }
        public string CreatedBy { get; set; }
    }
    public class Answer5
    {
        public string QuestionId { get; set; }
        public string Attendance_Cycle_From { get; set; }
        public string Attendance_Cycle_To { get; set; }
        public string PayRoll_Cycle_From { get; set; }
        public string PayRoll_Cycle_To { get; set; }
        public string Collection_Date_From { get; set; }
        public string Collection_Date_To { get; set; }
        public string Group_Name_Site_Master { get; set; }
        public string PayOut_Date { get; set; }
        public string Payment_Proof { get; set; }
        public string CreatedBy { get; set; }

    }

    public class Answer7
    {
        public string QuestionId { get; set; }
        public string First_month_Payroll { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer13
    {
        public string QuestionId { get; set; }
        public string Attendance_Checking { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer14
    {
        public string QuestionId { get; set; }
        public string Major_Correction { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer16
    {
        public string QuestionId { get; set; }
        public string Adhoc_Payment { get; set; }
        public string Date_Of_Disbursal { get; set; }
        public string Payment_proof { get; set; }
        public string Paycode { get; set; }
        public string Input_Type { get; set; }
        public string Incentive_Calculation { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer17
    {
        public string QuestionId { get; set; }
        public string Inactive_Employee_Load { get; set; }
        public string FF_Days { get; set; }
        public string Remarks { get; set; }
        public string Gratuity { get; set; }
        public string Date_Submission { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer18
    {
        public string QuestionId { get; set; }
        public string Payslip_Distribution { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer19
    {
        public string QuestionId { get; set; }
        public string Notice_Period_Pay { get; set; }
        public string Threshold_Day { get; set; }
        public string Applicable_Wages_BASIC_DA { get; set; }
        public string Applicable_Wages_GROSS { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer21
    {
        public string QuestionId { get; set; }
        public string Maternity { get; set; }
        public string Remarks { get; set; }
        public string Applicable { get; set; }
        public string Billable { get; set; }
        public string Salary { get; set; }
        public string Approval { get; set; }
        public string Point_Of_Contact { get; set; }
        public string Email { get; set; }
        public string Mobile_Number { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer23
    {
        public string QuestionId { get; set; }
        public string BGV_Applicable { get; set; }
        public string Eligibility { get; set; }
        public string Eligibility_By { get; set; }
        public string Cost { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer25
    {
        public string QuestionId { get; set; }
        public string Billiable { get; set; }
        public string Eligibility_Month { get; set; }
        public string Accumulated_FlushOut { get; set; }
        public string Billed_Paid { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer28
    {
        public string QuestionId { get; set; }
        public string Compensatory_Off { get; set; }
        public string Remarks { get; set; }
        public string Applicable { get; set; }
        public string Billable { get; set; }
        public string Salary { get; set; }
        public string Approval { get; set; }
        public string Point_Of_Contact { get; set; }
        public string Email { get; set; }
        public string Mobile_Number { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer29
    {
        public string QuestionId { get; set; }
        public string Billable { get; set; }
        public string Display_Register { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer30
    {
        public string QuestionId { get; set; }
        public string PO_Type { get; set; }
        public string PF_Calculated_15K_BASED_ON_ATTENDANCE { get; set; }
        public string PF_Calculated_Wages_Without_Any_Capping { get; set; }
        public string PF_Calculated_Earnings_Restricting_15K { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer32
    {
        public string QuestionId { get; set; }
        public string Calculation { get; set; }
        public string ATTRIBUTES { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer36
    {
        public string QuestionId { get; set; }
        public string Absorption_Fee { get; set; }
        public string Eligibility { get; set; }
        public string TAT { get; set; }
        public string Commercials { get; set; }
        public string Pay_Code { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer37
    {
        public string QuestionId { get; set; }
        public string Payment { get; set; }
        public string Payment_Days { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer38
    {
        public string QuestionId { get; set; }
        public string Penalty_Clause { get; set; }
        public string CreatedBy { get; set; }
    }

    public class Answer12
    {
        public string QuestionId { get; set; }
        public string SubId { get; set; }
        public string Filling_Attendance { get; set; }
        public string CreatedBy { get; set; }
    }
     public class Answer27
 {
     public string QuestionId { get; set; }
     public string Variable_Pay { get; set; }
     public string Term { get; set; }
     public string Billing_Type { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer39
 {
     public string QuestionId { get; set; }
     public string PO_Applicable { get; set; }
     public string PO_Type { get; set; }
     public string PO_Category { get; set; }
     public List<Po_Utiliziation> POUtiliziation { get; set; }
     public string Currency { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Po_Utiliziation
 {
     public string SubId { get; set; }
     public string PO_Utiliziation { get; set; }
 }

 public class Answer4
 {
     public string Input1 { get; set; }
     public string Input2 { get; set; }
     public string Input3 { get; set; }
     public string Input4 { get; set; }
 }

 public class Answer4Get
 {
     public string SectionType { get; set; }
     public string Input1 { get; set; }
     public string Input2 { get; set; }
     public string Input3 { get; set; }
     public string Input4 { get; set; }
 }

 public class Answer4RequestGet
 {
     public int QuestionId { get; set; }

     public List<Answer4Get> Vertical { get; set; }
     public List<Answer4Get> Department { get; set; }
     public List<Answer4Get> Manager { get; set; }
     public List<Answer4Get> Circle { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer4Request
 {
     public int QuestionId { get; set; }
     public List<Answer4> Vertical { get; set; }
     public List<Answer4> Department { get; set; }
     public List<Answer4> Manager { get; set; }
     public List<Answer4> Circle { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer33
 {
     public string Input1 { get; set; }
     public string Input2 { get; set; }
     public string Input3 { get; set; }
     public string Input4 { get; set; }
 }

 public class Answer33Get
 {
     public string SectionType { get; set; }
     public string Input1 { get; set; }
     public string Input2 { get; set; }
     public string Input3 { get; set; }
     public string Input4 { get; set; }
 }

 public class Answer33RequestGet
 {
     public int QuestionId { get; set; }
     public List<Answer33Get> Email { get; set; }
     public List<Answer33Get> Portal { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer33Request
 {
     public int QuestionId { get; set; }
     public List<Answer33> Email { get; set; }
     public List<Answer33> Portal { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer31
 {
     public string QuestionId { get; set; }
     public string Bill_Applicable { get; set; }

 }

 public class Answer11
 {
     public string Input1 { get; set; }
     public string Input2 { get; set; }
     public string Input3 { get; set; }
     public string Input4 { get; set; }
 }

 public class Answer11Get
 {
     public string Std_Working_Hours_Full_Day { get; set; }
     public string Std_Working_Hours_Half_Day { get; set; }
     public string SectionType { get; set; }
     public string Input1 { get; set; }
     public string Input2 { get; set; }
     public string Input3 { get; set; }
     public string Input4 { get; set; }
 }

 public class Answer11RequestGet
 {
     public int QuestionId { get; set; }
     public string Std_Working_Hours_Full_Day { get; set; }
     public string Std_Working_Hours_Half_Day { get; set; }
     public List<Answer11Get> Email { get; set; }
     public List<Answer11Get> Portal { get; set; }
     public List<Answer11Get> Biometric { get; set; }
     public List<Answer11Get> Others { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer11Request
 {
     public int QuestionId { get; set; }
     public string Std_Working_Hours_Full_Day { get; set; }
     public string Std_Working_Hours_Half_Day { get; set; }
     public List<Answer11> Email { get; set; }
     public List<Answer11> Portal { get; set; }
     public List<Answer11> Biometric { get; set; }
     public List<Answer11> Others { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer15
 {
     public string Input1 { get; set; }
     public string Input2 { get; set; }
     public string Input3 { get; set; }
     public string Input4 { get; set; }
 }

 public class Answer15Get
 {
     public string First_Input_date { get; set; }
     public string Revised_Input_date { get; set; }
     public string SectionType { get; set; }
     public string Input1 { get; set; }
     public string Input2 { get; set; }
     public string Input3 { get; set; }
     public string Input4 { get; set; }
 }

 public class Answer15RequestGet
 {
     public int QuestionId { get; set; }
     public string First_Input_date { get; set; }
     public string Revised_Input_date { get; set; }
     public List<Answer15Get> Email { get; set; }
     public List<Answer15Get> Portal { get; set; }
     public List<Answer15Get> Biometric { get; set; }
     public List<Answer15Get> Others { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer15Request
 {
     public int QuestionId { get; set; }
     public string First_Input_date { get; set; }
     public string Revised_Input_date { get; set; }
     public List<Answer15> Email { get; set; }
     public List<Answer15> Portal { get; set; }
     public List<Answer15> Biometric { get; set; }
     public List<Answer15> Others { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer20
 {
     public int Sopid { get; set; }
     public int QuestionId { get; set; }
     public string Applicable { get; set; }
     public string Eligible_days { get; set; }
     public string Applicable_Desc_Client { get; set; }
     public string Designation_Id { get; set; }
     public string Designation_Name { get; set; }
     public string Applicable_Wages_BASIC_DA { get; set; }
     public string Applicable_Wages_GROSS { get; set; }
     public string CreatedBy { get; set; }
 }

 public class Answer22
 {
     public int Sopid { get; set; }
     public int QuestionId { get; set; }
     public string Applicable { get; set; }
     public string Leave_Type_Id { get; set; }
     public string Leave_Type { get; set; }
     public string Carry_Forward { get; set; }
     public string Carry_Forward_Days { get; set; }
     public string Calander_Type { get; set; }
     public string Leave_Encashment { get; set; }
     public string Leave_Management { get; set; }
     public string CreatedBy { get; set; }
 }


    public class AnswerResponse
    {
        public string response { get; set; }
    }

}
