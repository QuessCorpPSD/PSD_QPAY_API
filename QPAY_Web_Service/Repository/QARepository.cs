using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public CompanyMaster GetCompanyCode(int user_id)
        {
            var companyMasterDetails = new CompanyMaster();
            string storeProcedure = "sp_Get_Company_Details_SOP";
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", user_id);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<CompanyMaster>>(res);
                    companyMasterDetails = companyList?.FirstOrDefault() ?? new CompanyMaster();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    companyMasterDetails = new CompanyMaster();
                }
            }

            return companyMasterDetails;
        }

        public List<StateMaster> GetState()
        {
            var stateMasterDetails = new List<StateMaster>();
            string storeProcedure = "sp_Get_State_Master_SOP";
            var parameters = new DynamicParameters();

            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    stateMasterDetails = JsonConvert.DeserializeObject<List<StateMaster>>(res)
                                                     ?? new List<StateMaster>();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    stateMasterDetails = new List<StateMaster>();
                }
            }

            return stateMasterDetails;
        }

        public List<CityMaster> GetCity(int state_id)
        {
            var cityMasterDetails = new List<CityMaster>();
            string storeProcedure = "sp_Get_City_Master_SOP";
            var parameters = new DynamicParameters();
            parameters.Add("@StateId", state_id);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    cityMasterDetails = JsonConvert.DeserializeObject<List<CityMaster>>(res)
                                                     ?? new List<CityMaster>();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    cityMasterDetails = new List<CityMaster>();
                }
            }

            return cityMasterDetails;
        }

        public List<DesignationMaster> GetDesignation(string company_code)
        {
            var designationMasterDetails = new List<DesignationMaster>();
            string storeProcedure = "sp_Get_Designation_Master_SOP";
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyCode", company_code);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    designationMasterDetails = JsonConvert.DeserializeObject<List<DesignationMaster>>(res)
                                                     ?? new List<DesignationMaster>();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    designationMasterDetails = new List<DesignationMaster>();
                }
            }

            return designationMasterDetails;
        }

        public FirstMonthPayroll GetFirstMonthPayroll(string company_code)
        {
            var firstMonthPayrollDetails = new FirstMonthPayroll();
            string storeProcedure = "sp_Get_First_Month_Payroll_SOP";
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyCode", company_code);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var firstMonthPayrollList = JsonConvert.DeserializeObject<List<FirstMonthPayroll>>(res);
                    firstMonthPayrollDetails = firstMonthPayrollList?.FirstOrDefault() ?? new FirstMonthPayroll();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    firstMonthPayrollDetails = new FirstMonthPayroll();
                }
            }

            return firstMonthPayrollDetails;
        }

        public List<Category> GetCategory()
        {
            var Categorylist = new List<Category>();
            string Query = "Select * from tbl_Customer_SOP_Category_Master_New";
            var parameters = new DynamicParameters();

            var res = this._dbRepository.QueryMultiAsync(Query).Result;

            //if (!string.IsNullOrWhiteSpace(res))
            //{
            try
            {
                Categorylist = JsonConvert.DeserializeObject<List<Category>>(res)
                                                 ?? new List<Category>();
            }
            catch (JsonException ex)
            {
                // Log the error if needed
                Categorylist = new List<Category>();
            }
            //}

            return Categorylist;
        }
        //public async Task<List<SOPModelsUI>> GetMenuAsync(int categoryId)
        //{

        //    var items = await GetQuestion(categoryId).ConfigureAwait(false);

        //   // return items;
        //    return BuildMenuTree(items.ToList(), 0);
        //}

        //private List<SOPModelsUI> BuildMenuTree(List<SOPModelUI> items, int parentId)
        //{
        //    if (items.Count > 0)
        //    {
        //        return GetQuestionBySubId(parentId)
        //            .Where(x => x.SubId == parentId)
        //            .Select(x => new SOPModelsUI
        //            {
        //                UniqueId = x.UniqueId,
        //                SubId = x.SubId,
        //                CategoryId = x.CategoryId,
        //                QuestionOrder = x.QuestionOrder,
        //                QuestionId = x.QuestionId,
        //                QuestionName = x.QuestionName,
        //                Attribute = x.Attribute,
        //                SOPModels = BuildMenuTree(items, x.UniqueId)
        //            })
        //            .ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


        public List<Question> GetQuestion(int categoryId)
        {
            var Questionlist = new List<Question>();
            string Query = "Select * from tbl_Customer_SOP_Question_Master_New Where CategoryId='" + categoryId + "' AND IsActive=1";
            var parameters = new DynamicParameters();

            var res = this._dbRepository.QueryMultiAsync(Query).Result;

            try
            {
                Questionlist = JsonConvert.DeserializeObject<List<Question>>(res)
                                                 ?? new List<Question>();
            }
            catch (JsonException ex)
            {

                Questionlist = new List<Question>();
            }
            return Questionlist;
        }

        public Answer1 GetSOPAnswer1(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer1();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_1";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer1>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer1();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer1();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer1(Answer1 answer1)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer1 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_1";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer1.QuestionId);
                parameters.Add("@Client_website_link", answer1.Client_website_link);
                parameters.Add("@CreatedBy", answer1.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }

            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer2 GetSOPAnswer2(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer2();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_2";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer2>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer2();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer2();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer2(Answer2 answer2)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer2 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_2";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer2.QuestionId);
                parameters.Add("@Company_Code", answer2.Company_Code);
                parameters.Add("@Company_Name", answer2.Company_Name);
                parameters.Add("@SAP_Code", answer2.SAP_Code);
                parameters.Add("@MyContract_Reference_ID", answer2.MyContract_Reference_ID);
                parameters.Add("@CreatedBy", answer2.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }

            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }


        public Answer3 GetSOPAnswer3(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer3();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_3";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer3>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer3();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer3();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer3(Answer3 answer3)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer3 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_3";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer3.QuestionId);
                parameters.Add("@POC_Change", answer3.POC_Change);
                parameters.Add("@CreatedBy", answer3.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }

            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer6 GetSOPAnswer6(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer6();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_6";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer6>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer6();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer6();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer6(Answer6 answer6)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer6 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_6";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer6.QuestionId);
                parameters.Add("@FF_Payment_Mode", answer6.FF_Payment_Mode);
                parameters.Add("@CreatedBy", answer6.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }

            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer8 GetSOPAnswer8(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer8();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_8";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer8>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer8();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer8();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer8(Answer8 answer8)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer8 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_8";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer8.QuestionId);
                parameters.Add("@Sim_Card_Management_tracker", answer8.Sim_Card_Management_tracker);
                parameters.Add("@CreatedBy", answer8.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }

            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer9 GetSOPAnswer9(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer9();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_9";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer9>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer9();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer9();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer9(Answer9 answer3)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer3 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_9";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer3.QuestionId);
                parameters.Add("@Email_ID_Managemnet_Tracker", answer3.Email_ID_Managemnet_Tracker);
                parameters.Add("@CreatedBy", answer3.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer10 GetSOPAnswer10(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer10();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_10";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer10>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer10();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer10();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer10(Answer10 answer10)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer10 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_10";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer10.QuestionId);
                parameters.Add("@ID_Card_Managemnet_Tracker", answer10.ID_Card_Managemnet_Tracker);
                parameters.Add("@CreatedBy", answer10.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer5 GetSOPAnswer5(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer5();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_5";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer5>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer5();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer5();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer5(Answer5 answer5)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer5 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_5";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer5.QuestionId);
                parameters.Add("@Attendance_Cycle_From", DateTime.TryParse(answer5.Attendance_Cycle_From, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@Attendance_Cycle_To", DateTime.TryParse(answer5.Attendance_Cycle_To, out var parsedDate1) ? parsedDate1 : (object?)null, DbType.Date);
                parameters.Add("@PayRoll_Cycle_From", DateTime.TryParse(answer5.PayRoll_Cycle_From, out var parsedDate2) ? parsedDate2 : (object?)null, DbType.Date);
                parameters.Add("@PayRoll_Cycle_To", DateTime.TryParse(answer5.PayRoll_Cycle_To, out var parsedDate3) ? parsedDate3 : (object?)null, DbType.Date);
                parameters.Add("@Collection_Date_From", DateTime.TryParse(answer5.Collection_Date_From, out var parsedDate4) ? parsedDate4 : (object?)null, DbType.Date);
                parameters.Add("@Collection_Date_To", DateTime.TryParse(answer5.Collection_Date_To, out var parsedDate5) ? parsedDate5 : (object?)null, DbType.Date);
                parameters.Add("@Group_Name_Site_Master", answer5.Group_Name_Site_Master);
                parameters.Add("@PayOut_Date", DateTime.TryParse(answer5.PayOut_Date, out var parsedDate6) ? parsedDate6 : (object?)null, DbType.Date);
                parameters.Add("@Payment_Proof", answer5.Payment_Proof);
                parameters.Add("@CreatedBy", answer5.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer7 GetSOPAnswer7(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer7();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_7";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer7>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer7();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer7();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer7(Answer7 answer7)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer7 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_7";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer7.QuestionId);
                parameters.Add("@First_month_Payroll", answer7.First_month_Payroll);
                parameters.Add("@CreatedBy", answer7.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer13 GetSOPAnswer13(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer13();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_13";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer13>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer13();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer13();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer13(Answer13 answer13)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer13 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_13";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer13.QuestionId);
                parameters.Add("@Attendance_Checking", answer13.Attendance_Checking);
                parameters.Add("@CreatedBy", answer13.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }


        public Answer14 GetSOPAnswer14(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer14();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_14";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer14>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer14();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer14();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer14(Answer14 answer14)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer14 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_14";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer14.QuestionId);
                parameters.Add("@Major_Correction", answer14.Major_Correction);
                parameters.Add("@Remarks", answer14.Remarks);
                parameters.Add("@CreatedBy", answer14.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }


        public Answer17 GetSOPAnswer17(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer17();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_17";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer17>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer17();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer17();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer17(Answer17 answer17)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer17 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_17";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer17.QuestionId);
                parameters.Add("@Inactive_Employee_Load", answer17.Inactive_Employee_Load);
                parameters.Add("@FF_Days", answer17.FF_Days);
                parameters.Add("@Remarks", answer17.Remarks);
                parameters.Add("@Gratuity", answer17.Gratuity);
                parameters.Add("@Date_Submission", DateTime.TryParse(answer17.Date_Submission, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@CreatedBy", answer17.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer16 GetSOPAnswer16(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer16();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_16";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer16>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer16();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer16();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer16(Answer16 answer16)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer16 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_16";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer16.QuestionId);
                parameters.Add("@Adhoc_Payment", answer16.Adhoc_Payment);
                parameters.Add("@Date_Of_Disbursal", DateTime.TryParse(answer16.Date_Of_Disbursal, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@Payment_proof", answer16.Payment_proof);
                parameters.Add("@Paycode", answer16.Paycode);
                parameters.Add("@Input_Type", answer16.Input_Type);
                parameters.Add("@Incentive_Calculation", answer16.Incentive_Calculation);
                parameters.Add("@CreatedBy", answer16.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer18 GetSOPAnswer18(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer18();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_18";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer18>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer18();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer18();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer18(Answer18 answer18)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer18 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_18";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer18.QuestionId);
                parameters.Add("@Payslip_Distribution", answer18.Payslip_Distribution);
                parameters.Add("@CreatedBy", answer18.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer19 GetSOPAnswer19(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer19();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_19";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer19>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer19();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer19();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer19(Answer19 answer19)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer19 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_19";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer19.QuestionId);
                parameters.Add("@Notice_Period_Pay", answer19.Notice_Period_Pay);
                parameters.Add("@Threshold_Day", answer19.Threshold_Day);
                parameters.Add("@Applicable_Wages_BASIC_DA", answer19.Applicable_Wages_BASIC_DA);
                parameters.Add("@Applicable_Wages_GROSS", answer19.Applicable_Wages_GROSS);
                parameters.Add("@CreatedBy", answer19.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer21 GetSOPAnswer21(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer21();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_21";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer21>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer21();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer21();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer21(Answer21 answer21)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer21 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_21";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer21.QuestionId);
                parameters.Add("@Maternity", answer21.Maternity);
                parameters.Add("@Remarks", answer21.Remarks);
                parameters.Add("@Applicable", answer21.Applicable);
                parameters.Add("@Billable", answer21.Billable);
                parameters.Add("@Salary", answer21.Salary);
                parameters.Add("@Approval", answer21.Approval);
                parameters.Add("@Point_Of_Contact", answer21.Point_Of_Contact);
                parameters.Add("@Email", answer21.Email);
                parameters.Add("@Mobile_Number", answer21.Mobile_Number);
                parameters.Add("@Name", answer21.Name);
                parameters.Add("@CreatedBy", answer21.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer23 GetSOPAnswer23(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer23();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_23";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer23>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer23();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer23();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer23(Answer23 answer23)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer23 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_23";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer23.QuestionId);
                parameters.Add("@BGV_Applicable", answer23.BGV_Applicable);
                parameters.Add("@Eligibility", answer23.Eligibility);
                parameters.Add("@Eligibility_By", answer23.Eligibility_By);
                parameters.Add("@Cost", answer23.Cost);
                parameters.Add("@CreatedBy", answer23.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer25 GetSOPAnswer25(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer25();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_25";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer25>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer25();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer25();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer25(Answer25 answer25)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer25 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_25";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer25.QuestionId);
                parameters.Add("@Billiable", answer25.Billiable);
                parameters.Add("@Eligibility_Month", answer25.Eligibility_Month);
                parameters.Add("@Accumulated_FlushOut", answer25.Accumulated_FlushOut);
                parameters.Add("@Billed_Paid", answer25.Billed_Paid);
                parameters.Add("@CreatedBy", answer25.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }


        public Answer28 GetSOPAnswer28(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer28();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_28";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer28>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer28();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer28();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer28(Answer28 answer28)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer28 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_28";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer28.QuestionId);
                parameters.Add("@Compensatory_Off", answer28.Compensatory_Off);
                parameters.Add("@Remarks", answer28.Remarks);
                parameters.Add("@Applicable", answer28.Applicable);
                parameters.Add("@Billable", answer28.Billable);
                parameters.Add("@Salary", answer28.Salary);
                parameters.Add("@Approval", answer28.Approval);
                parameters.Add("@Point_Of_Contact", answer28.Point_Of_Contact);
                parameters.Add("@Email", answer28.Email);
                parameters.Add("@Mobile_Number", answer28.Mobile_Number);
                parameters.Add("@Name", answer28.Name);
                parameters.Add("@CreatedBy", answer28.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer29 GetSOPAnswer29(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer29();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_29";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer29>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer29();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer29();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer29(Answer29 answer29)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer29 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_29";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer29.QuestionId);
                parameters.Add("@Billable", answer29.Billable);
                parameters.Add("@Display_Register", answer29.Display_Register);
                parameters.Add("@CreatedBy", answer29.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer30 GetSOPAnswer30(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer30();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_30";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer30>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer30();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer30();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer30(Answer30 answer30)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer30 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_30";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer30.QuestionId);
                parameters.Add("@PO_Type", answer30.PO_Type);
                parameters.Add("@PF_Calculated_15K_BASED_ON_ATTENDANCE", answer30.PF_Calculated_15K_BASED_ON_ATTENDANCE);
                parameters.Add("@PF_Calculated_Wages_Without_Any_Capping", answer30.PF_Calculated_Wages_Without_Any_Capping);
                parameters.Add("@PF_Calculated_Earnings_Restricting_15K", answer30.PF_Calculated_Earnings_Restricting_15K);
                parameters.Add("@CreatedBy", answer30.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer32 GetSOPAnswer32(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer32();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_32";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer32>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer32();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer32();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer32(Answer32 answer32)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer32 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_32";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer32.QuestionId);
                parameters.Add("@Calculation", answer32.Calculation);
                parameters.Add("@ATTRIBUTES", answer32.ATTRIBUTES);
                parameters.Add("@CreatedBy", answer32.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer36 GetSOPAnswer36(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer36();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_36";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer36>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer36();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer36();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer36(Answer36 answer36)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer36 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_36";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer36.QuestionId);
                parameters.Add("@Absorption_Fee", answer36.Absorption_Fee);
                parameters.Add("@Eligibility", answer36.Eligibility);
                parameters.Add("@TAT", answer36.TAT);
                parameters.Add("@Commercials", answer36.Commercials);
                parameters.Add("@Pay_Code", answer36.Pay_Code);
                parameters.Add("@CreatedBy", answer36.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer37 GetSOPAnswer37(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer37();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_37";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer37>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer37();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer37();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer37(Answer37 answer37)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer37 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_37";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer37.QuestionId);
                parameters.Add("@Payment", answer37.Payment);
                parameters.Add("@Payment_Days", answer37.Payment_Days);
                parameters.Add("@CreatedBy", answer37.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer38 GetSOPAnswer38(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer38();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_38";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer38>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer38();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer38();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer38(Answer38 answer38)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer38 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_38";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer38.QuestionId);
                parameters.Add("@Penalty_Clause", answer38.Penalty_Clause);
                parameters.Add("@CreatedBy", answer38.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }


        public Answer12 GetSOPAnswer12(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer12();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_12";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer12>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer12();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer12();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer12(List<Answer12> answer12)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer12 != null)
            {
                foreach (var answer in answer12)
                {
                    string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_12";
                    var parameters = new DynamicParameters();

                    parameters.Add("@QuestionId", answer.QuestionId);
                    parameters.Add("@SubId", answer.SubId);
                    parameters.Add("@Filling_Attendance", answer.Filling_Attendance);
                    parameters.Add("@CreatedBy", answer.CreatedBy);

                    var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        AnswerDetails.response = "Success";
                    }
                }
                AnswerDetails.response = "Success";
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer27 GetSOPAnswer27(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer27();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_27";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer27>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer27();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer27();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer27(Answer27 answer27)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer27 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_27";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer27.QuestionId);
                parameters.Add("@Variable_Pay", answer27.Variable_Pay);
                parameters.Add("@Term", answer27.Term);
                parameters.Add("@Billing_Type", answer27.Billing_Type);
                parameters.Add("@CreatedBy", answer27.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer39 GetSOPAnswer39(int QuestionId, string Createdby)
        {

            var checklistQuestionAnswerDetails = new Answer39();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_39";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var answerList = JsonConvert.DeserializeObject<List<Answer39>>(res);
                    checklistQuestionAnswerDetails = answerList?.FirstOrDefault() ?? new Answer39();

                    // Get and set POUtilization list separately
                    checklistQuestionAnswerDetails.POUtiliziation = GetSOPAnswer39_1(QuestionId, Createdby);
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new Answer39();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        public List<Po_Utiliziation> GetSOPAnswer39_1(int QuestionId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new List<Po_Utiliziation>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_39_1";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);

            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Po_Utiliziation>>(res)
                           ?? new List<Po_Utiliziation>();
                }
                catch
                {
                    return new List<Po_Utiliziation>();
                }
            }

            return new List<Po_Utiliziation>();
        }


        public AnswerResponse PostSOPAnswer39(Answer39 answer39)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer39 != null)
            {
                if (answer39.POUtiliziation.Count > 0)
                {
                    foreach (var answer in answer39.POUtiliziation)
                    {
                        string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_39";
                        var parameters = new DynamicParameters();

                        parameters.Add("@QuestionId", answer39.QuestionId);
                        parameters.Add("@SubId", answer.SubId);
                        parameters.Add("@PO_Applicable", answer39.PO_Applicable);
                        parameters.Add("@PO_Type", answer39.PO_Type);
                        parameters.Add("@PO_Utiliziation", answer.PO_Utiliziation);
                        parameters.Add("@PO_Category", answer39.PO_Category);
                        parameters.Add("@Currency", answer39.Currency);
                        parameters.Add("@CreatedBy", answer39.CreatedBy);

                        var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            AnswerDetails.response = "Success";
                        }
                    }
                }
                else
                {
                    string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_39";
                    var parameters = new DynamicParameters();

                    parameters.Add("@QuestionId", answer39.QuestionId);
                    parameters.Add("@SubId", "");
                    parameters.Add("@PO_Applicable", answer39.PO_Applicable);
                    parameters.Add("@PO_Type", "");
                    parameters.Add("@PO_Utiliziation", "");
                    parameters.Add("@PO_Category", "");
                    parameters.Add("@Currency", "");
                    parameters.Add("@CreatedBy", answer39.CreatedBy);

                    var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        AnswerDetails.response = "Success";
                    }
                }
                AnswerDetails.response = "Success";
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public List<Marked_Category> GetmarkedQuestion(string Createdby)
        {
            var categorydetails = new List<Marked_Category>();
            string storeProcedure = "SP_GET_Customer_SOP_Marked_Category";
            var parameters = new DynamicParameters();
            parameters.Add("@Createdby", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    categorydetails = JsonConvert.DeserializeObject<List<Marked_Category>>(res)
                                                     ?? new List<Marked_Category>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var category in categorydetails)
                    {
                        category.Marked_Question = GetmarkedQuestion(category.CategoryId, Createdby);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    categorydetails = new List<Marked_Category>();
                }
            }

            return categorydetails;

        }

        public List<Marked_Question> GetmarkedQuestion(string CategoryId, string Createdby)
        {
            var questionDetails = new List<Marked_Question>();
            string storeProcedure = "SP_GET_Customer_SOP_Marked_Question";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", CategoryId);
            parameters.Add("@Createdby", Createdby);

            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Marked_Question>>(res)
                           ?? new List<Marked_Question>();
                }
                catch
                {
                    return new List<Marked_Question>();
                }
            }

            return new List<Marked_Question>();
        }

        public AnswerResponse PostSOPAnswer4(Answer4Request answer4)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer4 != null)
            {
                string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_4";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer4.QuestionId);
                parameters.Add("@CreatedBy", answer4.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

                if (answer4.Vertical.Count > 0)
                {
                    SaveSection(answer4.Vertical, "Vertical");

                }
                if (answer4.Manager.Count > 0)
                {
                    SaveSection(answer4.Manager, "Manager");

                }
                if (answer4.Department.Count > 0)
                {
                    SaveSection(answer4.Department, "Department");
                }
                if (answer4.Circle.Count > 0)
                {
                    SaveSection(answer4.Circle, "Circle");
                }

                void SaveSection(List<Answer4> list, string sectionType)
                {


                    foreach (var item in list)
                    {
                        string storeProcedure = "SP_Insert_tbl_Customer_SOP_Answer_Details_4";
                        var parameters = new DynamicParameters();

                        parameters.Add("@QuestionId", answer4.QuestionId);
                        parameters.Add("@SectionType", sectionType);
                        parameters.Add("@Input1", item.Input1);
                        parameters.Add("@Input2", item.Input2);
                        parameters.Add("@Input3", item.Input3);
                        parameters.Add("@Input4", item.Input4);
                        parameters.Add("@CreatedBy", answer4.CreatedBy);

                        var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            AnswerDetails.response = "Success";
                        }
                    }
                }
                AnswerDetails.response = "Success";
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer4RequestGet GetSOPAnswer4(int QuestionId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new Answer4RequestGet
            {
                QuestionId = QuestionId,
                CreatedBy = Createdby,
                Vertical = new List<Answer4Get>(),
                Department = new List<Answer4Get>(),
                Manager = new List<Answer4Get>(),
                Circle = new List<Answer4Get>()
            };

            string storedProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_4";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);

            var result = _dbRepository.GetItemsAsync<Answer4Get>(storedProcedure, parameters).Result;

            if (result != null)
            {
                foreach (var item in result)
                {
                    switch (item.SectionType?.ToLower())
                    {
                        case "vertical":
                            checklistQuestionAnswerDetails.Vertical.Add(item);
                            break;
                        case "department":
                            checklistQuestionAnswerDetails.Department.Add(item);
                            break;
                        case "manager":
                            checklistQuestionAnswerDetails.Manager.Add(item);
                            break;
                        case "circle":
                            checklistQuestionAnswerDetails.Circle.Add(item);
                            break;
                    }
                }
            }

            return checklistQuestionAnswerDetails;
        }

        public AnswerResponse PostSOPAnswer33(Answer33Request answer33)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer33 != null)
            {
                string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_33";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer33.QuestionId);
                parameters.Add("@CreatedBy", answer33.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

                if (answer33.Email.Count > 0)
                {
                    SaveSection(answer33.Email, "Email");

                }
                if (answer33.Portal.Count > 0)
                {
                    SaveSection(answer33.Portal, "Portal");
                }


                void SaveSection(List<Answer33> list, string sectionType)
                {
                    foreach (var item in list)
                    {
                        string storeProcedure = "SP_Insert_tbl_Customer_SOP_Answer_Details_33";
                        var parameters = new DynamicParameters();

                        parameters.Add("@QuestionId", answer33.QuestionId);
                        parameters.Add("@SectionType", sectionType);
                        parameters.Add("@Input1", item.Input1);
                        parameters.Add("@Input2", item.Input2);
                        parameters.Add("@Input3", item.Input3);
                        parameters.Add("@Input4", item.Input4);
                        parameters.Add("@CreatedBy", answer33.CreatedBy);

                        var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            AnswerDetails.response = "Success";
                        }
                    }
                }
                AnswerDetails.response = "Success";
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer33RequestGet GetSOPAnswer33(int QuestionId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new Answer33RequestGet
            {
                QuestionId = QuestionId,
                CreatedBy = Createdby,
                Email = new List<Answer33Get>(),
                Portal = new List<Answer33Get>()
            };

            string storedProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_33";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);

            var result = _dbRepository.GetItemsAsync<Answer33Get>(storedProcedure, parameters).Result;

            if (result != null)
            {
                foreach (var item in result)
                {
                    switch (item.SectionType?.ToLower())
                    {
                        case "email":
                            checklistQuestionAnswerDetails.Email.Add(item);
                            break;
                        case "portal":
                            checklistQuestionAnswerDetails.Portal.Add(item);
                            break;
                    }
                }
            }

            return checklistQuestionAnswerDetails;
        }

        public AnswerResponse PostSOPAnswer31(IFormFile file, [FromForm] string billApplicable,
            [FromForm] int QuestionId, [FromForm] string CreatedBy)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ReimbursementPolicyUploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }

                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_31";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", QuestionId);
                parameters.Add("@Bill_Applicable", billApplicable);
                parameters.Add("@File_Path", filePath);
                parameters.Add("@CreatedBy", CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }
            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer31_1([FromForm] string billApplicable,
           [FromForm] int QuestionId, [FromForm] string CreatedBy)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_31";
            var parameters = new DynamicParameters();

            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Bill_Applicable", billApplicable);
            parameters.Add("@File_Path", "");
            parameters.Add("@CreatedBy", CreatedBy);

            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (!string.IsNullOrWhiteSpace(res))
            {
                AnswerDetails.response = "Success";
            }

            return AnswerDetails;
        }

        public Answer31 GetSOPAnswer31(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer31();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_31";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer31>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer31();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer31();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer11(Answer11Request answer11)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer11 != null)
            {
                string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_11";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer11.QuestionId);
                parameters.Add("@CreatedBy", answer11.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

                if (answer11.Email.Count > 0)
                {
                    SaveSection(answer11.Email, "Email");

                }
                if (answer11.Portal.Count > 0)
                {
                    SaveSection(answer11.Portal, "Portal");

                }
                if (answer11.Biometric.Count > 0)
                {
                    SaveSection(answer11.Biometric, "Biometric");
                }
                if (answer11.Others.Count > 0)
                {
                    SaveSection(answer11.Others, "Others");
                }

                void SaveSection(List<Answer11> list, string sectionType)
                {


                    foreach (var item in list)
                    {
                        string storeProcedure = "SP_Insert_tbl_Customer_SOP_Answer_Details_11";
                        var parameters = new DynamicParameters();

                        parameters.Add("@QuestionId", answer11.QuestionId);
                        parameters.Add("@Std_Working_Hours_Full_Day", answer11.Std_Working_Hours_Full_Day);
                        parameters.Add("@Std_Working_Hours_Half_Day", answer11.Std_Working_Hours_Half_Day);
                        parameters.Add("@SectionType", sectionType);
                        parameters.Add("@Input1", item.Input1);
                        parameters.Add("@Input2", item.Input2);
                        parameters.Add("@Input3", item.Input3);
                        parameters.Add("@Input4", item.Input4);
                        parameters.Add("@CreatedBy", answer11.CreatedBy);

                        var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            AnswerDetails.response = "Success";
                        }
                    }
                }
                AnswerDetails.response = "Success";
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer11RequestGet GetSOPAnswer11(int QuestionId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new Answer11RequestGet
            {
                QuestionId = QuestionId,
                CreatedBy = Createdby,
                Email = new List<Answer11Get>(),
                Portal = new List<Answer11Get>(),
                Biometric = new List<Answer11Get>(),
                Others = new List<Answer11Get>()
            };

            string storedProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_11";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);

            var result = _dbRepository.GetItemsAsync<Answer11Get>(storedProcedure, parameters).Result;

            if (result != null)
            {
                foreach (var item in result)
                {
                    checklistQuestionAnswerDetails.Std_Working_Hours_Full_Day = item.Std_Working_Hours_Full_Day;
                    checklistQuestionAnswerDetails.Std_Working_Hours_Half_Day = item.Std_Working_Hours_Half_Day;
                    switch (item.SectionType?.ToLower())
                    {
                        case "email":
                            checklistQuestionAnswerDetails.Email.Add(item);
                            break;
                        case "portal":
                            checklistQuestionAnswerDetails.Portal.Add(item);
                            break;
                        case "biometric":
                            checklistQuestionAnswerDetails.Biometric.Add(item);
                            break;
                        case "others":
                            checklistQuestionAnswerDetails.Others.Add(item);
                            break;
                    }
                }
            }

            return checklistQuestionAnswerDetails;
        }


        public AnswerResponse PostSOPAnswer15(Answer15Request answer15)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer15 != null)
            {
                string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_15";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer15.QuestionId);
                parameters.Add("@CreatedBy", answer15.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

                if (answer15.Email.Count > 0)
                {
                    SaveSection(answer15.Email, "Email");

                }
                if (answer15.Portal.Count > 0)
                {
                    SaveSection(answer15.Portal, "Portal");

                }
                if (answer15.Biometric.Count > 0)
                {
                    SaveSection(answer15.Biometric, "Biometric");
                }
                if (answer15.Others.Count > 0)
                {
                    SaveSection(answer15.Others, "Others");
                }

                void SaveSection(List<Answer15> list, string sectionType)
                {


                    foreach (var item in list)
                    {
                        string storeProcedure = "SP_Insert_tbl_Customer_SOP_Answer_Details_15";
                        var parameters = new DynamicParameters();

                        parameters.Add("@QuestionId", answer15.QuestionId);
                        parameters.Add("@First_Input_date", DateTime.TryParse(answer15.First_Input_date, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                        parameters.Add("@Revised_Input_date", DateTime.TryParse(answer15.Revised_Input_date, out var parsedDate1) ? parsedDate1 : (object?)null, DbType.Date);
                        parameters.Add("@SectionType", sectionType);
                        parameters.Add("@Input1", item.Input1);
                        parameters.Add("@Input2", item.Input2);
                        parameters.Add("@Input3", item.Input3);
                        parameters.Add("@Input4", item.Input4);
                        parameters.Add("@CreatedBy", answer15.CreatedBy);

                        var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            AnswerDetails.response = "Success";
                        }
                    }
                }
                AnswerDetails.response = "Success";
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public Answer15RequestGet GetSOPAnswer15(int QuestionId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new Answer15RequestGet
            {
                QuestionId = QuestionId,
                CreatedBy = Createdby,
                Email = new List<Answer15Get>(),
                Portal = new List<Answer15Get>(),
                Biometric = new List<Answer15Get>(),
                Others = new List<Answer15Get>()
            };

            string storedProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_15";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);

            var result = _dbRepository.GetItemsAsync<Answer15Get>(storedProcedure, parameters).Result;

            if (result != null)
            {
                foreach (var item in result)
                {
                    checklistQuestionAnswerDetails.First_Input_date = item.First_Input_date;
                    checklistQuestionAnswerDetails.Revised_Input_date = item.Revised_Input_date;
                    switch (item.SectionType?.ToLower())
                    {
                        case "email":
                            checklistQuestionAnswerDetails.Email.Add(item);
                            break;
                        case "portal":
                            checklistQuestionAnswerDetails.Portal.Add(item);
                            break;
                        case "biometric":
                            checklistQuestionAnswerDetails.Biometric.Add(item);
                            break;
                        case "others":
                            checklistQuestionAnswerDetails.Others.Add(item);
                            break;
                    }
                }
            }

            return checklistQuestionAnswerDetails;
        }

        public AnswerResponse PostSOPAnswer20(Answer20 answer20)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer20 != null)
            {

                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Notice_Period_Recovery_20";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer20.QuestionId);
                parameters.Add("@Applicable", answer20.Applicable);
                parameters.Add("@Eligible_days", answer20.Eligible_days);
                parameters.Add("@Applicable_Desc_Client", answer20.Applicable_Desc_Client);
                parameters.Add("@Designation_Id", answer20.Designation_Id);
                parameters.Add("@Designation_Name", answer20.Designation_Name);
                parameters.Add("@Applicable_Wages_BASIC_DA", answer20.Applicable_Wages_BASIC_DA);
                parameters.Add("@Applicable_Wages_GROSS", answer20.Applicable_Wages_GROSS);
                parameters.Add("@CreatedBy", answer20.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }
            return AnswerDetails;
        }

        public List<Answer20> GetSOPAnswer20(int QuestionId, string Createdby)
        {
            var AnswerDetails = new List<Answer20>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Notice_Period_Recovery_20";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer20>>(res)
                                                      ?? new List<Answer20>();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer20>();
                }
            }

            return AnswerDetails;
        }

        public AnswerResponse PostSOPAnswer22(Answer22 answer22)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer22 != null)
            {

                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Leave_22";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer22.QuestionId);
                parameters.Add("@Applicable", answer22.Applicable);
                parameters.Add("@Leave_Type_Id", answer22.Leave_Type_Id);
                parameters.Add("@Leave_Type", answer22.Leave_Type);
                parameters.Add("@Carry_Forward", answer22.Carry_Forward);
                parameters.Add("@Carry_Forward_Days", answer22.Carry_Forward_Days);
                parameters.Add("@Calander_Type", answer22.Calander_Type);
                parameters.Add("@Leave_Encashment", answer22.Leave_Encashment);
                parameters.Add("@Leave_Management", answer22.Leave_Management);
                parameters.Add("@CreatedBy", answer22.CreatedBy);

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
                if (!string.IsNullOrWhiteSpace(res))
                {
                    AnswerDetails.response = "Success";
                }
            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }
            return AnswerDetails;
        }

        public List<Answer22> GetSOPAnswer22(int QuestionId, string Createdby)
        {
            var AnswerDetails = new List<Answer22>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Leave_22";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer22>>(res)
                                                      ?? new List<Answer22>();
                }
                catch (JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer22>();
                }
            }

            return AnswerDetails;
        }

    }
}
