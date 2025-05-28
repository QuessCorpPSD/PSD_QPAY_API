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
        public string  CategoryId { get; set; }
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
    }
}
