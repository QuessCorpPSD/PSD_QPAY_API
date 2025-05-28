using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Models;

namespace QPay.BAL.Repository
{
    public class QARepository : IQARepository
    {
        private readonly DbRepository _dbRepository;

        public QARepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public List<CustomerSOPQuestion> GetCustomerSOPQuestionAnswer()
        {
            var checklistQuestionAnswerDetails = new List<CustomerSOPQuestion>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", 0);
            parameters.Add("@AnswerId_1", 0);
            parameters.Add("@AnswerId_2", 0);
            parameters.Add("@AnswerId_3", 0);
            parameters.Add("@AnswerId_4", 0);
            parameters.Add("@AnswerId_5", 0);
            parameters.Add("@AnswerId_6", 0);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPQuestion>>(res)
                                                     ?? new List<CustomerSOPQuestion>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.questions = GetCustomerSOPQuestions(question.CategoryId);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPQuestion>();
                }
            }

            return checklistQuestionAnswerDetails;
        }

        public List<CustomerSOPQuestions> GetCustomerSOPQuestions(string CategoryId)
        {
            var checklistQuestionAnswerDetails = new List<CustomerSOPQuestions>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", CategoryId);
            parameters.Add("@QuestionId", 0);
            parameters.Add("@AnswerId_1", 0);
            parameters.Add("@AnswerId_2", 0);
            parameters.Add("@AnswerId_3", 0);
            parameters.Add("@AnswerId_4", 0);
            parameters.Add("@AnswerId_5", 0);
            parameters.Add("@AnswerId_6", 0);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPQuestions>>(res)
                                                     ?? new List<CustomerSOPQuestions>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer1s = GetSOPAnswersById1(question.QuestionId);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPQuestions>();
                }
            }

            return checklistQuestionAnswerDetails;
        }

        private List<CustomerSOPAnswer1> GetSOPAnswersById1(string questionId)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer1>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", 0);
            parameters.Add("@AnswerId_2", 0);
            parameters.Add("@AnswerId_3", 0);
            parameters.Add("@AnswerId_4", 0);
            parameters.Add("@AnswerId_5", 0);
            parameters.Add("@AnswerId_6", 0);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer1>>(res)
                                                     ?? new List<CustomerSOPAnswer1>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer2s = GetSOPAnswersById2(question.QuestionId, question.AnswerId_1);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer1>();
                }
            }

            return checklistQuestionAnswerDetails;


            //string procedure = "sp_Get_Checklist_Question_Answer_Master";
            //var parameters = new DynamicParameters();
            //parameters.Add("@QUESTION_ID", questionId);
            //parameters.Add("@ANSWER_ID1", 0);

            //var res = this._dbRepository.GetItemsAsync(procedure, parameters).Result;

            //if (!string.IsNullOrWhiteSpace(res))
            //{
            //    try
            //    {
            //        return JsonConvert.DeserializeObject<List<CheklistAnswer1>>(res)
            //               ?? new List<CheklistAnswer1>();
            //    }
            //    catch
            //    {
            //        return new List<CheklistAnswer1>();
            //    }
            //}

            //return new List<CheklistAnswer1>();
        }

        private List<CustomerSOPAnswer2> GetSOPAnswersById2(string questionId, string answerId1)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer2>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", 0);
            parameters.Add("@AnswerId_3", 0);
            parameters.Add("@AnswerId_4", 0);
            parameters.Add("@AnswerId_5", 0);
            parameters.Add("@AnswerId_6", 0);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer2>>(res)
                                                     ?? new List<CustomerSOPAnswer2>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer3s = GetSOPAnswersById3(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer2>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer3> GetSOPAnswersById3(string questionId, string answerId1, string answerId2)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer3>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", 0);
            parameters.Add("@AnswerId_4", 0);
            parameters.Add("@AnswerId_5", 0);
            parameters.Add("@AnswerId_6", 0);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer3>>(res)
                                                     ?? new List<CustomerSOPAnswer3>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer4s = GetSOPAnswersById4(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer3>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer4> GetSOPAnswersById4(string questionId, string answerId1, string answerId2,
            string answerId3)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer4>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", 0);
            parameters.Add("@AnswerId_5", 0);
            parameters.Add("@AnswerId_6", 0);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer4>>(res)
                                                     ?? new List<CustomerSOPAnswer4>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer5s = GetSOPAnswersById5(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer4>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer5> GetSOPAnswersById5(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer5>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", 0);
            parameters.Add("@AnswerId_6", 0);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer5>>(res)
                                                     ?? new List<CustomerSOPAnswer5>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer6s = GetSOPAnswersById6(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer5>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer6> GetSOPAnswersById6(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4, string answerId5)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer6>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", answerId5);
            parameters.Add("@AnswerId_6", 0);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer6>>(res)
                                                     ?? new List<CustomerSOPAnswer6>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer7s = GetSOPAnswersById7(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer6>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer7> GetSOPAnswersById7(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4, string answerId5, string answerId6)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer7>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", answerId5);
            parameters.Add("@AnswerId_6", answerId6);
            parameters.Add("@AnswerId_7", 0);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer7>>(res)
                                                     ?? new List<CustomerSOPAnswer7>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer8s = GetSOPAnswersById8(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer7>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer8> GetSOPAnswersById8(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4, string answerId5, string answerId6, string answerId7)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer8>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", answerId5);
            parameters.Add("@AnswerId_6", answerId6);
            parameters.Add("@AnswerId_7", answerId7);
            parameters.Add("@AnswerId_8", 0);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer8>>(res)
                                                     ?? new List<CustomerSOPAnswer8>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer9s = GetSOPAnswersById9(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer8>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer9> GetSOPAnswersById9(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4, string answerId5, string answerId6, string answerId7, string answerId8)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer9>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", answerId5);
            parameters.Add("@AnswerId_6", answerId6);
            parameters.Add("@AnswerId_7", answerId7);
            parameters.Add("@AnswerId_8", answerId8);
            parameters.Add("@AnswerId_9", 0);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer9>>(res)
                                                     ?? new List<CustomerSOPAnswer9>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer10s = GetSOPAnswersById10(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8, question.AnswerId_9);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer9>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer10> GetSOPAnswersById10(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4, string answerId5, string answerId6, string answerId7, string answerId8,
            string answerId9)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer10>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", answerId5);
            parameters.Add("@AnswerId_6", answerId6);
            parameters.Add("@AnswerId_7", answerId7);
            parameters.Add("@AnswerId_8", answerId8);
            parameters.Add("@AnswerId_9", answerId9);
            parameters.Add("@AnswerId_10", 0);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer10>>(res)
                                                     ?? new List<CustomerSOPAnswer10>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer11s = GetSOPAnswersById11(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8, question.AnswerId_9,
                             question.AnswerId_10);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer10>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private List<CustomerSOPAnswer11> GetSOPAnswersById11(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4, string answerId5, string answerId6, string answerId7, string answerId8,
            string answerId9, string answerId10)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer11>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", answerId5);
            parameters.Add("@AnswerId_6", answerId6);
            parameters.Add("@AnswerId_7", answerId7);
            parameters.Add("@AnswerId_8", answerId8);
            parameters.Add("@AnswerId_9", answerId9);
            parameters.Add("@AnswerId_10", answerId10);
            parameters.Add("@AnswerId_11", 0);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer11>>(res)
                                                     ?? new List<CustomerSOPAnswer11>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer12s = GetSOPAnswersById12(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8, question.AnswerId_9,
                             question.AnswerId_10, question.AnswerId_11);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer11>();
                }
            }

            return checklistQuestionAnswerDetails;

        }


        private List<CustomerSOPAnswer12> GetSOPAnswersById12(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4, string answerId5, string answerId6, string answerId7, string answerId8,
            string answerId9, string answerId10, string answerId11)
        {

            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer12>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", answerId5);
            parameters.Add("@AnswerId_6", answerId6);
            parameters.Add("@AnswerId_7", answerId7);
            parameters.Add("@AnswerId_8", answerId8);
            parameters.Add("@AnswerId_9", answerId9);
            parameters.Add("@AnswerId_10", answerId10);
            parameters.Add("@AnswerId_11", answerId11);
            parameters.Add("@AnswerId_12", 0);
            parameters.Add("@AnswerId_13", 0);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer12>>(res)
                                                     ?? new List<CustomerSOPAnswer12>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer13s = GetSOPAnswersById13(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8, question.AnswerId_9,
                             question.AnswerId_10, question.AnswerId_11, question.AnswerId_12);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer12>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        public List<CustomerSOPAnswer13> GetSOPAnswersById13(string questionId, string answerId1, string answerId2,
            string answerId3, string answerId4, string answerId5, string answerId6, string answerId7, string answerId8,
            string answerId9, string answerId10, string answerId11, string answerId12)
        {
            var checklistQuestionAnswerDetails = new List<CustomerSOPAnswer13>();
            string storeProcedure = "sp_Get_Cusromer_SOP_Question_Answer_Master";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", 0);
            parameters.Add("@QuestionId", questionId);
            parameters.Add("@AnswerId_1", answerId1);
            parameters.Add("@AnswerId_2", answerId2);
            parameters.Add("@AnswerId_3", answerId3);
            parameters.Add("@AnswerId_4", answerId4);
            parameters.Add("@AnswerId_5", answerId5);
            parameters.Add("@AnswerId_6", answerId6);
            parameters.Add("@AnswerId_7", answerId7);
            parameters.Add("@AnswerId_8", answerId8);
            parameters.Add("@AnswerId_9", answerId9);
            parameters.Add("@AnswerId_10", answerId10);
            parameters.Add("@AnswerId_11", answerId11);
            parameters.Add("@AnswerId_12", answerId12);
            parameters.Add("@AnswerId_13", 0);

            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<CustomerSOPAnswer13>>(res)
                           ?? new List<CustomerSOPAnswer13>();
                }
                catch
                {
                    return new List<CustomerSOPAnswer13>();
                }
            }

            return new List<CustomerSOPAnswer13>();
        }
    }
}
