using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public class ChecklistQuestionAnswer
    {
        public string QUESTION_ID { get; set; } = string.Empty;
        public string QUESTION_ORDER { get; set; } = string.Empty;
        public string QUESTION_NAME { get; set; } = string.Empty;
        public string ANSWER_TYPE { get; set; } = string.Empty;
        public string MULTIPLE_ANSWER_FLAG { get; set; } = string.Empty;
        public string ANSWER_FLAG { get; set; } = string.Empty;
        public List<CheklistAnswer1> cheklistAnswer1s { get; set; } = new List<CheklistAnswer1>();

    }

    public class CheklistAnswer1
    {
        public string ANSWER_ORDER { get; set; } = string.Empty;
        public string QUESTION_ID { get; set; } = string.Empty;
        public string ANSWER_ID { get; set; } = string.Empty;
        public string ANSWER_NAME { get; set; } = string.Empty;
        public string ANSWER_TITLE { get; set; } = string.Empty;
        public string SUB_ANSWER_FLAG { get; set; } = string.Empty;
        public string ANSWER_TYPE { get; set; } = string.Empty;

    }

    public class ResponseChecklistQuestionAnswer
    {
        public string statusCode { get; set; } = string.Empty;
        public string statusMessage { get; set; } = string.Empty;
        public List<ChecklistQuestionAnswer> checklistQuestionAnswers { get; set; } = new List<ChecklistQuestionAnswer>();
    }


    public class CustomerSOPQuestion
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public List<CustomerSOPQuestions> questions { get; set; } = new List<CustomerSOPQuestions>();

    }

    public class CustomerSOPQuestions
    {
        public string QuestionId { get; set; } = string.Empty;
        public string QuestionOrder { get; set; } = string.Empty;
        public string QuestionName { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public List<CustomerSOPAnswer1> customersopanswer1s { get; set; } = new List<CustomerSOPAnswer1>();
    }

    public class CustomerSOPAnswer1
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_1_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;

        public List<CustomerSOPAnswer2> customersopanswer2s { get; set; } = new List<CustomerSOPAnswer2>();
    }

    public class CustomerSOPAnswer2
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_2_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer3> customersopanswer3s { get; set; } = new List<CustomerSOPAnswer3>();
    }

    public class CustomerSOPAnswer3
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_3_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer4> customersopanswer4s { get; set; } = new List<CustomerSOPAnswer4>();
    }

    public class CustomerSOPAnswer4
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_4_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer5> customersopanswer5s { get; set; } = new List<CustomerSOPAnswer5>();
    }

    public class CustomerSOPAnswer5
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_5_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer6> customersopanswer6s { get; set; } = new List<CustomerSOPAnswer6>();
    }

    public class CustomerSOPAnswer6
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerId_6 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_6_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer7> customersopanswer7s { get; set; } = new List<CustomerSOPAnswer7>();
    }
    public class CustomerSOPAnswer7
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerId_6 { get; set; } = string.Empty;
        public string AnswerId_7 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_7_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer8> customersopanswer8s { get; set; } = new List<CustomerSOPAnswer8>();
    }

    public class CustomerSOPAnswer8
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerId_6 { get; set; } = string.Empty;
        public string AnswerId_7 { get; set; } = string.Empty;
        public string AnswerId_8 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_8_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer9> customersopanswer9s { get; set; } = new List<CustomerSOPAnswer9>();
    }

    public class CustomerSOPAnswer9
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerId_6 { get; set; } = string.Empty;
        public string AnswerId_7 { get; set; } = string.Empty;
        public string AnswerId_8 { get; set; } = string.Empty;
        public string AnswerId_9 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_9_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer10> customersopanswer10s { get; set; } = new List<CustomerSOPAnswer10>();
    }
    public class CustomerSOPAnswer10
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerId_6 { get; set; } = string.Empty;
        public string AnswerId_7 { get; set; } = string.Empty;
        public string AnswerId_8 { get; set; } = string.Empty;
        public string AnswerId_9 { get; set; } = string.Empty;
        public string AnswerId_10 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_10_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer11> customersopanswer11s { get; set; } = new List<CustomerSOPAnswer11>();
    }

    public class CustomerSOPAnswer11
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerId_6 { get; set; } = string.Empty;
        public string AnswerId_7 { get; set; } = string.Empty;
        public string AnswerId_8 { get; set; } = string.Empty;
        public string AnswerId_9 { get; set; } = string.Empty;
        public string AnswerId_10 { get; set; } = string.Empty;
        public string AnswerId_11 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_11_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer12> customersopanswer12s { get; set; } = new List<CustomerSOPAnswer12>();
    }

    public class CustomerSOPAnswer12
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerId_6 { get; set; } = string.Empty;
        public string AnswerId_7 { get; set; } = string.Empty;
        public string AnswerId_8 { get; set; } = string.Empty;
        public string AnswerId_9 { get; set; } = string.Empty;
        public string AnswerId_10 { get; set; } = string.Empty;
        public string AnswerId_11 { get; set; } = string.Empty;
        public string AnswerId_12 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_12_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
        public List<CustomerSOPAnswer13> customersopanswer13s { get; set; } = new List<CustomerSOPAnswer13>();
    }

    public class CustomerSOPAnswer13
    {
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId_1 { get; set; } = string.Empty;
        public string AnswerId_2 { get; set; } = string.Empty;
        public string AnswerId_3 { get; set; } = string.Empty;
        public string AnswerId_4 { get; set; } = string.Empty;
        public string AnswerId_5 { get; set; } = string.Empty;
        public string AnswerId_6 { get; set; } = string.Empty;
        public string AnswerId_7 { get; set; } = string.Empty;
        public string AnswerId_8 { get; set; } = string.Empty;
        public string AnswerId_9 { get; set; } = string.Empty;
        public string AnswerId_10 { get; set; } = string.Empty;
        public string AnswerId_11 { get; set; } = string.Empty;
        public string AnswerId_12 { get; set; } = string.Empty;
        public string AnswerId_13 { get; set; } = string.Empty;
        public string AnswerType { get; set; } = string.Empty;
        public string Answer_13_Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IsMandatory { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
        public string AddMultiple { get; set; } = string.Empty;
        public string SubAnswerFlag { get; set; } = string.Empty;
        public string API_URL { get; set; } = string.Empty;
        public string API_Result_Id { get; set; } = string.Empty;
        public string API_Result_Value { get; set; } = string.Empty;
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
        public string Company_Id { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public string Client_Name { get; set; } = string.Empty;
        public string SAP_Code { get; set; } = string.Empty;
        public string MyContractReferenceID { get; set; } = string.Empty;
    }


    public class StateMaster
    {
        public string State_Id { get; set; } = string.Empty;
        public string State_Code { get; set; } = string.Empty;
        public string State_Name { get; set; } = string.Empty;

    }

    public class CityMaster
    {
        public string City_Id { get; set; } = string.Empty;
        public string City_Code { get; set; } = string.Empty;
        public string City_Name { get; set; } = string.Empty;
    }

    public class DesignationMaster
    {
        public string Designation_Id { get; set; } = string.Empty;
        public string Designation_Name { get; set; } = string.Empty;
    }

    public class FirstMonthPayroll
    {
        public string Company_Code { get; set; } = string.Empty;
        public string Company_Name { get; set; } = string.Empty;
        public string SAP_Code { get; set; } = string.Empty;
        public string MyContract_Reference_ID { get; set; } = string.Empty;
        public string Client_Website_link { get; set; } = string.Empty;
        public string First_Month_Payroll { get; set; } = string.Empty;
        public string Client_Onboarding_Month { get; set; } = string.Empty;
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
        public string IsMandatory { get; set; } = string.Empty;

        public List<SOPModelsUI> SOPModels { get; set; } = new List<SOPModelsUI>();

    }

    public class Marked_Category

    {
        public string CategoryId { get; set; } = string.Empty;
        public string Perc { get; set; } = string.Empty;
        public List<Marked_Question> Marked_Question { get; set; } = new List<Marked_Question>();

    }

    public class Marked_Question
    {
        public string QuestionId { get; set; } = string.Empty;

    }


    public class Category
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; } = string.Empty;
    }

    public class Question
    {
        public string CategoryId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string QuestionOrder { get; set; } = string.Empty;
        public string QuestionName { get; set; } = string.Empty;
    }

    public class Answer1
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public string Company_Name { get; set; } = string.Empty;
        public string SAP_Code { get; set; } = string.Empty;
        public string MyContract_Reference_ID { get; set; } = string.Empty;
        public string Client_Website_link { get; set; } = string.Empty;
        public string First_Month_Payroll { get; set; } = string.Empty;
        public string Client_Onboarding_Month { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer2
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public string Company_Name { get; set; } = string.Empty;
        public string SAP_Code { get; set; } = string.Empty;
        public string MyContract_Reference_ID { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
    public class Answer3
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string POC_Change { get; set; } = string.Empty;
        public string BU_Location_Change { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer6
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string FF_Payment_Mode { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer8
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Sim_Card_Management_tracker { get; set; } = string.Empty;
        public string Email_Id_Management_tracker { get; set; } = string.Empty;
        public string Id_Card_Management_tracker { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer9
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Email_ID_Managemnet_Tracker { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer10
    {
        public string QuestionId { get; set; } = string.Empty;
        public string ID_Card_Managemnet_Tracker { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
    public class Answer5
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Attendance_Cycle_From { get; set; } = string.Empty;
        public string Attendance_Cycle_To { get; set; } = string.Empty;
        public string PayRoll_Cycle_From { get; set; } = string.Empty;
        public string PayRoll_Cycle_To { get; set; } = string.Empty;
        public string Collection_Date { get; set; } = string.Empty;
        public string Group_Name_Site_Master { get; set; } = string.Empty;
        public string PayOut_Date { get; set; } = string.Empty;
        public string Payment_Proof { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

    }

    public class Answer7
    {
        public string QuestionId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public string First_month_Payroll { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer13
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Attendance_Checking { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer14
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Major_Correction { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer16
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Adhoc_Payment { get; set; } = string.Empty;
        public string Date_Of_Disbursal { get; set; } = string.Empty;
        public string Payment_proof { get; set; } = string.Empty;
        public string Paycode { get; set; } = string.Empty;
        public string Input_Type { get; set; } = string.Empty;
        public string Incentive_Calculation { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer17
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Inactive_Employee_Load { get; set; } = string.Empty;
        public string FF_Days { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string Gratuity { get; set; } = string.Empty;
        public string Date_Submission { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer18
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Payslip_Distribution { get; set; } = string.Empty;
        public string Quess_Ess { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer19
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Notice_Period_Pay { get; set; } = string.Empty;
        public string Threshold_Day { get; set; } = string.Empty;
        public string Applicable_Wages_BASIC_DA { get; set; } = string.Empty;
        public string Applicable_Wages_GROSS { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer21
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Maternity { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string Applicable { get; set; } = string.Empty;
        public string Billable { get; set; } = string.Empty;
        public string Salary { get; set; } = string.Empty;
        public string Approval { get; set; } = string.Empty;
        public string Point_Of_Contact { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile_Number { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer23
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string BGV_Applicable { get; set; } = string.Empty;
        public string Eligibility { get; set; } = string.Empty;
        public string Eligibility_By { get; set; } = string.Empty;
        public string Cost { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer25
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Billiable { get; set; } = string.Empty;
        public string Calandar_Type { get; set; } = string.Empty;
        public string Accumulated_FlushOut { get; set; } = string.Empty;
        public string Billed_Paid { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer28
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Compensatory_Off { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string Applicable { get; set; } = string.Empty;
        public string Billable { get; set; } = string.Empty;
        public string Salary { get; set; } = string.Empty;
        public string Approval { get; set; } = string.Empty;
        public string Point_Of_Contact { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile_Number { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer29
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Billable { get; set; } = string.Empty;
        public string Display_Register { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer30
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string PO_Type { get; set; } = string.Empty;
        public string PF_Calculated_15K_BASED_ON_ATTENDANCE { get; set; } = string.Empty;
        public string PF_Calculated_Wages_Without_Any_Capping { get; set; } = string.Empty;
        public string PF_Calculated_Earnings_Restricting_15K { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer32
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Calculation { get; set; } = string.Empty;
        public string ATTRIBUTES { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer36
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Absorption_Fee { get; set; } = string.Empty;
        public string Eligibility { get; set; } = string.Empty;
        public string TAT { get; set; } = string.Empty;
        public string Commercials { get; set; } = string.Empty;
        public string Pay_Code { get; set; } = string.Empty;
        public string Flat { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer37
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Payment { get; set; } = string.Empty;
        public string Payment_Days { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer38
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Penalty_Clause { get; set; } = string.Empty;
        public string Payroll_Closure_Date { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer12
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string SubId { get; set; } = string.Empty;
        public string Filling_Attendance { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
    public class Answer27
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Variable_Pay { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public string Billing_Type { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer39
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string PO_Applicable { get; set; } = string.Empty;
        public string PO_Type { get; set; } = string.Empty;
        public string PO_Category { get; set; } = string.Empty;
        public List<Po_Utiliziation> POUtiliziation { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Po_Utiliziation
    {
        public string SubId { get; set; } = string.Empty;
        public string PO_Utiliziation { get; set; } = string.Empty;
    }

    public class Answer4
    {
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Input3 { get; set; } = string.Empty;
        public string Input4 { get; set; } = string.Empty;
    }

    public class Answer4Get
    {
        public string SectionType { get; set; } = string.Empty;
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Input3 { get; set; } = string.Empty;
        public string Input4 { get; set; } = string.Empty;
    }

    public class Answer4RequestGet
    {
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public List<Answer4Get> Vertical { get; set; } = new List<Answer4Get>();
        public List<Answer4Get> Department { get; set; } = new List<Answer4Get>();
        public List<Answer4Get> Manager { get; set; } = new List<Answer4Get>();
        public List<Answer4Get> Circle { get; set; } = new List<Answer4Get>();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer4Request
    {
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public List<Answer4> Vertical { get; set; } = new List<Answer4>();
        public List<Answer4> Department { get; set; } = new List<Answer4>();
        public List<Answer4> Manager { get; set; } = new List<Answer4>();
        public List<Answer4> Circle { get; set; } = new List<Answer4>();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer33
    {
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Input3 { get; set; } = string.Empty;
        public string Input4 { get; set; } = string.Empty;
    }

    public class Answer33Get
    {
        public string Company_Id { get; set; } = string.Empty;
        public string SectionType { get; set; } = string.Empty;
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Input3 { get; set; } = string.Empty;
        public string Input4 { get; set; } = string.Empty;
    }

    public class Answer33RequestGet
    {
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public List<Answer33Get> Email { get; set; } = new List<Answer33Get>();
        public List<Answer33Get> Portal { get; set; } = new List<Answer33Get>();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer33Request
    {
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public List<Answer33> Email { get; set; } = new List<Answer33>();
        public List<Answer33> Portal { get; set; } = new List<Answer33>();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer31
    {
        public string QuestionId { get; set; } = string.Empty;
        public string Bill_Applicable { get; set; } = string.Empty;
        public string File_Path { get; set; } = string.Empty;


    }

    public class Answer11
    {
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Input3 { get; set; } = string.Empty;
        public string Input4 { get; set; } = string.Empty;
    }

    public class Answer11Get
    {
        public string Company_Id { get; set; } = string.Empty;
        public string Std_Working_Hours_Full_Day { get; set; } = string.Empty;
        public string Std_Working_Hours_Half_Day { get; set; } = string.Empty;
        public string SectionType { get; set; } = string.Empty;
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Input3 { get; set; } = string.Empty;
        public string Input4 { get; set; } = string.Empty;
    }

    public class Answer11RequestGet
    {
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public string Std_Working_Hours_Full_Day { get; set; } = string.Empty;
        public string Std_Working_Hours_Half_Day { get; set; } = string.Empty;
        public List<Answer11Get> Email { get; set; } = new List<Answer11Get>();
        public List<Answer11Get> Portal { get; set; } = new List<Answer11Get>();
        public List<Answer11Get> Biometric { get; set; } = new List<Answer11Get>();
        public List<Answer11Get> Others { get; set; } = new List<Answer11Get>();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer11Request
    {
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public string Std_Working_Hours_Full_Day { get; set; } = string.Empty;
        public string Std_Working_Hours_Half_Day { get; set; } = string.Empty;
        public List<Answer11> Email { get; set; } = new List<Answer11>();
        public List<Answer11> Portal { get; set; } = new List<Answer11>();
        public List<Answer11> Biometric { get; set; } = new List<Answer11>();
        public List<Answer11> Others { get; set; } = new List<Answer11>();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer15
    {
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Input3 { get; set; } = string.Empty;
        public string Input4 { get; set; } = string.Empty;
    }

    public class Answer15Get
    {
        public string First_Input_date { get; set; } = string.Empty;
        public string Revised_Input_date { get; set; } = string.Empty;
        public string SectionType { get; set; } = string.Empty;
        public string Input1 { get; set; } = string.Empty;
        public string Input2 { get; set; } = string.Empty;
        public string Input3 { get; set; } = string.Empty;
        public string Input4 { get; set; } = string.Empty;
    }

    public class Answer15RequestGet
    {
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public string First_Input_date { get; set; } = string.Empty;
        public string Revised_Input_date { get; set; } = string.Empty;
        public List<Answer15Get> Email { get; set; } = new List<Answer15Get>();
        public List<Answer15Get> Portal { get; set; } = new List<Answer15Get>();
        public List<Answer15Get> Biometric { get; set; } = new List<Answer15Get>();
        public List<Answer15Get> Others { get; set; } = new List<Answer15Get>();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer15Request
    {
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public string First_Input_date { get; set; } = string.Empty;
        public string Revised_Input_date { get; set; } = string.Empty;
        public List<Answer15> Email { get; set; } = new List<Answer15>();
        public List<Answer15> Portal { get; set; } = new List<Answer15>();
        public List<Answer15> Biometric { get; set; } = new List<Answer15>();
        public List<Answer15> Others { get; set; } = new List<Answer15>();
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer20
    {
        public int Sopid { get; set; }
        public int QuestionId { get; set; }
        public string Applicable { get; set; } = string.Empty;
        public string Eligible_days { get; set; } = string.Empty;
        public string Applicable_Desc_Client { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public string Designation_Id { get; set; } = string.Empty;
        public string Designation_Name { get; set; } = string.Empty;
        public string Designationwise_Days { get; set; } = string.Empty;
        public string Applicable_Wages_BASIC_DA { get; set; } = string.Empty;
        public string Applicable_Wages_GROSS { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer22
    {
        public int Sopid { get; set; }
        public int QuestionId { get; set; }
        public int Company_Id { get; set; }
        public string Applicable { get; set; } = string.Empty;
        public string Leave_Management { get; set; } = string.Empty;
        public string Calander_Type { get; set; } = string.Empty;
        public string Leave_Type_Id { get; set; } = string.Empty;
        public string Leave_Type { get; set; } = string.Empty;
        public string No_Of_Leave { get; set; } = string.Empty;
        public string Carry_Forward { get; set; } = string.Empty;
        public string Carry_Forward_Days { get; set; } = string.Empty;
        public string Encashment { get; set; } = string.Empty;        
        public string Leave_Encashment { get; set; } = string.Empty;
        
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer24
    {
        public int Sopid { get; set; }
        public int QuestionId { get; set; }
        public string Calander_Type { get; set; } = string.Empty;
        public string State_Id { get; set; } = string.Empty;
        public string State_Name { get; set; } = string.Empty;
        public string Leave_Type { get; set; } = string.Empty;
        public string Holiday_Date { get; set; } = string.Empty;
        public string Leave_Description { get; set; } = string.Empty;
        public string Is_Billable { get; set; } = string.Empty;
        public string Billable_Type { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class PermissionwiseCompanyModel
    {
        public string Company_Id { get; set; }
        public string Company_Code { get; set; }
        public string User_Id { get; set; }
    }

    public class PremiumTracker
    {
        public string GEN_iID { get; set; }
        public string GEN_vDescription { get; set; }
    }

    public class InsuranceCoverageType
    {
        public string CoverageTypeId { get; set; }
        public string CoverageType { get; set; }
    }

    public class Policy
    {
        public string Id { get; set; }
        public string PolicyNumber { get; set; }
    }

    public class Paycode
    {
        public string Paycode_Id { get; set; }
        public string paycode_Code { get; set; }
    }

    public class EmployeeType
    {
        public string EmployeeTypeId { get; set; }
        public string EmployeeTypeValue { get; set; }
    }

    public class NewJoinee_Arrear
    {
        public string NewJoineeArrearId { get; set; }
        public string NewJoineeArrear { get; set; }
    }

    public class GroupDetails
    {
        public string Group_Detail_Id { get; set; }
        public string Group_Name { get; set; }
    }

    public class InsuranceVertical
    {
        public string Company_ID { get; set; }
        public string Insurance_Vertical_ID { get; set; }
        public string Insurance_Vertical { get; set; }
    }

    public class GMCPolicyCondition
    {
        public string PolicyConditionId { get; set; }
        public string PolicyCondition { get; set; }
    }

    public class GMCPolicyNo
    {
        public string PolicyNumberId { get; set; }
        public string PolicyNumber { get; set; }
    }

    public class InsuranceAlreadyExists
    {
        public string CompanyId { get; set; }
        public string GroupDetailId { get; set; }
        public string PremiumTrackerId { get; set; }
        public string EffectiveDate { get; set; }
        public string Insurance_Vertical_ID { get; set; }
    }

    public class InsuranceAdd
    {
        public string QuestionId { get; set; }
        public string CompanyId { get; set; }
        public string GroupDetailId { get; set; }
        public string PremiumTrackerId { get; set; }
        public string Designation_Id { get; set; }
        public string CoverageTypeId { get; set; }
        public string PolicyConditionId { get; set; }
        public string GMCAmount { get; set; }
        public string PolicyNumberId { get; set; }
        public string GPAAmount { get; set; }
        public string GPAPolicyNumberId { get; set; }
        public string GTLIAmount { get; set; }
        public string GTLIPolicyNumberId { get; set; }
        public string PayCodeId { get; set; }
        public string DeductionAmount { get; set; }
        public string BillingPayCodeId { get; set; }
        public string BillingAmount { get; set; }
        public string EffectiveDate { get; set; }
        public string Remarks { get; set; }
        public string MaritalStatus { get; set; }
        public string Is_ESIApplicable { get; set; }
        public string Is_ESINonApplicable { get; set; }
        public string EmployeeTypeId { get; set; }
        public string Insurance_Vertical_ID { get; set; }
        public string NewJoineeArrearId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class InsurancePolicy
    {
        public string Serial_No { get; set; }
        public string SopId { get; set; }
        public string CompanyId { get; set; }
        public string Company_Code { get; set; }
        public string GroupDetailId { get; set; }
        public string Group_Name { get; set; }
        public string PremiumTrackerId { get; set; }
        public string MappingTypeDesc { get; set; }
        public string CoverageType { get; set; }
        public string CoverageTypeId { get; set; }
        public string PolicyConditionId { get; set; }
        public string PolicyCondition { get; set; }
        public string PolicyNumberId { get; set; }
        public string PolicyNumber { get; set; }
        public string PayCodeId { get; set; }
        public string Paycode { get; set; }
        public string BillingPayCodeId { get; set; }
        public string DeductionAmount { get; set; }
        public string BillingAmount { get; set; }
        public string BillingPaycode { get; set; }
        public string EffectiveDate { get; set; }
        public string GMCAmount { get; set; }
        public string GPAPolicyNumberId { get; set; }
        public string GPAPolicyNumber { get; set; }
        public string GPAAmount { get; set; }
        public string GTLIPolicyNumberId { get; set; }
        public string GTLIPolicyNumber { get; set; }
        public string GTLIAmount { get; set; }
        public string Remarks { get; set; }
        public string MaritalStatus { get; set; }
        public string Is_ESIApplicable { get; set; }
        public string Is_ESINonApplicable { get; set; }
        public string EmployeeTypeId { get; set; }
        public string EmployeeTypeValue { get; set; }
        public string Designation_Id { get; set; }
        public string Designation_Name { get; set; }
        public string Insurance_Vertical { get; set; }
        public string Insurance_Vertical_ID { get; set; }
        public string NewJoineeArrear { get; set; }
        public string NewJoineeArrearId { get; set; }

    }

    public class Client
    {
        public string Client_Id { get; set; }
        public string Client_Name { get; set; }
    }

    public class Answer35
    {
        public string sr_no { get; set; }
        public string QuestionId { get; set; }
        public string Client_ID { get; set; }
        public string Full_Name_Of_Organization { get; set; }
        public string Type_Of_Contact { get; set; }
        public string Credit_Days_Agreed { get; set; }
        public string Agreement_Start_Date { get; set; }
        public string Agreement_End_Date { get; set; }
        public string Agreement_Status { get; set; }
        public string Busniess_Head_Approval { get; set; }
        public string One_Time_Onboarding_Fees { get; set; }
        public string Service_Fee_Type { get; set; }
        public string Service_Fee { get; set; }
        public string Sourcing_Fee { get; set; }
        public string Replacement_Clause { get; set; }
        public string Absorption_Fee { get; set; }
        public string Upfront_Charges { get; set; }
        public string InEdge_Charges { get; set; }
        public string Supplementary_Fee_Type { get; set; }
        public string Supplementary_Charges { get; set; }
        public string LatePayment_Fee { get; set; }
        public string Other_Fees { get; set; }
        public string PAYROLL_WITH_DECIMAL { get; set; }
        public string SERVICE_FEE_WITH_DECIMAL { get; set; }
        public string OBApplicable { get; set; }
        public string CreatedBy { get; set; }

    }

    public class Answer34
    {
        public string sr_no { get; set; } = string.Empty;
        public string SOPId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string State_Id { get; set; } = string.Empty;
        public string State_Name { get; set; } = string.Empty;
        public string Certificate_Type { get; set; } = string.Empty;
        public string Invoice_Category { get; set; } = string.Empty;
        public string Bill_To { get; set; } = string.Empty;
        public string Bill_To_Pin { get; set; } = string.Empty;
        public string Ship_To { get; set; } = string.Empty;
        public string Ship_To_Pin { get; set; } = string.Empty;
        public string GST_Certificate_Path { get; set; } = string.Empty;
        public string GST_No { get; set; } = string.Empty;
        public string PAN_No { get; set; } = string.Empty;
        public string TAN_No { get; set; } = string.Empty;
        public string SAC_Code { get; set; } = string.Empty;
        public string Client_Invoice_State { get; set; } = string.Empty;
        public string Quess_Invoice_State { get; set; } = string.Empty;
        public string SEZ_Certificate_path { get; set; } = string.Empty;
        public string LUT_No { get; set; } = string.Empty;
        public string LUT_From_Date { get; set; } = string.Empty;
        public string LUT_End_Date { get; set; } = string.Empty;
        public string LUT_Certificate_Path { get; set; } = string.Empty;
        public string SUB_Code { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

    }

    public class Country
    {
        public string Country_Id { get; set; } = string.Empty;
        public string Country_Name { get; set; } = string.Empty;
    }

    public class Currency
    {
        public string CurrencyCode { get; set; } = string.Empty;
    }

    public class Answer40
    {
        public string SOPId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string CityId { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string RegionId { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string GSTIN { get; set; } = string.Empty;
        public string MSMENumber { get; set; } = string.Empty;
        public string PANNumber { get; set; } = string.Empty;
        public string PurchaseOrderCurrency { get; set; } = string.Empty;
        public string VendorStatus { get; set; } = string.Empty;
        public string VendorCreationDate { get; set; } = string.Empty;
        public string VendorAddress { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

    }

    public class Answer41
    {
        public string SOPId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string MasterChecklist { get; set; } = string.Empty;
        public string SpocDetails { get; set; } = string.Empty;
        public string CompletionActivity { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;


    }

    public class Answer42_1
    {
        public string SOPId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string IndustryType { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer42_2
    {
        public string SOPId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string StateId { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty;
        public string Structure { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer42_3
    {
        public string SOPId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public string Designationid { get; set; } = string.Empty;
        public string DesignationName { get; set; } = string.Empty;
        public string SkilledCategoryId { get; set; } = string.Empty;
        public string SkilledCategoryName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class Answer42_4
    {
        public string SOPId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string Company_Id { get; set; } = string.Empty;
        public string StateId { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty;
        public string CityId { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string HC { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
    public class AnswerResponse
    {
        public string response { get; set; } = string.Empty;
    }

}
