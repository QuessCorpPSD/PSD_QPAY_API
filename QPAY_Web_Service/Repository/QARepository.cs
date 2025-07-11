using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace QPay.BAL.Repository
{
    public class QARepository : IQARepository
    {
        private readonly DbRepository _dbRepository;

        public QARepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;///DB Inject
        }

        public async Task<List<CustomerSOPQuestion>> GetCustomerSOPQuestionAnswer()
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPQuestion>>(res)
                                                     ?? new List<CustomerSOPQuestion>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.questions = await GetCustomerSOPQuestions(question.CategoryId);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPQuestion>();
                }
            }

            return checklistQuestionAnswerDetails;
        }

        public async Task<List<CustomerSOPQuestions>> GetCustomerSOPQuestions(string CategoryId)
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPQuestions>>(res)
                                                     ?? new List<CustomerSOPQuestions>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer1s = await GetSOPAnswersById1(question.QuestionId);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPQuestions>();
                }
            }

            return checklistQuestionAnswerDetails;
        }

        private async Task<List<CustomerSOPAnswer1>> GetSOPAnswersById1(string questionId)
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer1>>(res)
                                                     ?? new List<CustomerSOPAnswer1>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer2s = await GetSOPAnswersById2(question.QuestionId, question.AnswerId_1);
                    }
                }
                catch (System.Text.Json.JsonException ex)
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

        private async Task<List<CustomerSOPAnswer2>> GetSOPAnswersById2(string questionId, string answerId1)
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer2>>(res)
                                                     ?? new List<CustomerSOPAnswer2>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer3s = await GetSOPAnswersById3(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer2>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer3>> GetSOPAnswersById3(string questionId, string answerId1, string answerId2)
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer3>>(res)
                                                     ?? new List<CustomerSOPAnswer3>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer4s = await GetSOPAnswersById4(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer3>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer4>> GetSOPAnswersById4(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer4>>(res)
                                                     ?? new List<CustomerSOPAnswer4>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer5s = await GetSOPAnswersById5(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer4>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer5>> GetSOPAnswersById5(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer5>>(res)
                                                     ?? new List<CustomerSOPAnswer5>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer6s = await GetSOPAnswersById6(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer5>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer6>> GetSOPAnswersById6(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer6>>(res)
                                                     ?? new List<CustomerSOPAnswer6>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer7s = await GetSOPAnswersById7(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer6>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer7>> GetSOPAnswersById7(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer7>>(res)
                                                     ?? new List<CustomerSOPAnswer7>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer8s = await GetSOPAnswersById8(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer7>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer8>> GetSOPAnswersById8(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer8>>(res)
                                                     ?? new List<CustomerSOPAnswer8>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer9s = await GetSOPAnswersById9(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer8>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer9>> GetSOPAnswersById9(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer9>>(res)
                                                     ?? new List<CustomerSOPAnswer9>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer10s = await GetSOPAnswersById10(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8, question.AnswerId_9);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer9>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer10>> GetSOPAnswersById10(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer10>>(res)
                                                     ?? new List<CustomerSOPAnswer10>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer11s = await GetSOPAnswersById11(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8, question.AnswerId_9,
                             question.AnswerId_10);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer10>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        private async Task<List<CustomerSOPAnswer11>> GetSOPAnswersById11(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer11>>(res)
                                                     ?? new List<CustomerSOPAnswer11>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer12s = await GetSOPAnswersById12(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8, question.AnswerId_9,
                             question.AnswerId_10, question.AnswerId_11);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer11>();
                }
            }

            return checklistQuestionAnswerDetails;

        }


        private async Task<List<CustomerSOPAnswer12>> GetSOPAnswersById12(string questionId, string answerId1, string answerId2,
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


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    checklistQuestionAnswerDetails = JsonConvert.DeserializeObject<List<CustomerSOPAnswer12>>(res)
                                                     ?? new List<CustomerSOPAnswer12>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var question in checklistQuestionAnswerDetails)
                    {
                        question.customersopanswer13s = await GetSOPAnswersById13(question.QuestionId, question.AnswerId_1,
                            question.AnswerId_2, question.AnswerId_3, question.AnswerId_4, question.AnswerId_5,
                             question.AnswerId_6, question.AnswerId_7, question.AnswerId_8, question.AnswerId_9,
                             question.AnswerId_10, question.AnswerId_11, question.AnswerId_12);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new List<CustomerSOPAnswer12>();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        public async Task<List<CustomerSOPAnswer13>> GetSOPAnswersById13(string questionId, string answerId1, string answerId2,
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

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

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

        public async Task<CompanyMaster> GetCompanyCode(int user_id)
        {
            var companyMasterDetails = new CompanyMaster();
            string storeProcedure = "sp_Get_Company_Details_SOP";
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", user_id);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<CompanyMaster>>(res);
                    companyMasterDetails = companyList?.FirstOrDefault() ?? new CompanyMaster();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    companyMasterDetails = new CompanyMaster();
                }
            }

            return companyMasterDetails;
        }

        public async Task<List<StateMaster>> GetState()
        {
            var stateMasterDetails = new List<StateMaster>();
            string storeProcedure = "sp_Get_State_Master_SOP";
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    stateMasterDetails = JsonConvert.DeserializeObject<List<StateMaster>>(res)
                                                     ?? new List<StateMaster>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    stateMasterDetails = new List<StateMaster>();
                }
            }

            return stateMasterDetails;
        }

        public async Task<List<CityMaster>> GetCity(int state_id)
        {
            var cityMasterDetails = new List<CityMaster>();
            string storeProcedure = "sp_Get_City_Master_SOP";
            var parameters = new DynamicParameters();
            parameters.Add("@StateId", state_id);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    cityMasterDetails = JsonConvert.DeserializeObject<List<CityMaster>>(res)
                                                     ?? new List<CityMaster>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    cityMasterDetails = new List<CityMaster>();
                }
            }

            return cityMasterDetails;
        }

        public async Task<List<DesignationMaster>> GetDesignation(string company_code)
        {
            var designationMasterDetails = new List<DesignationMaster>();
            string storeProcedure = "sp_Get_Designation_Master_SOP";
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyCode", company_code);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    designationMasterDetails = JsonConvert.DeserializeObject<List<DesignationMaster>>(res)
                                                     ?? new List<DesignationMaster>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    designationMasterDetails = new List<DesignationMaster>();
                }
            }

            return designationMasterDetails;
        }

        public async Task<List<FirstMonthPayroll>> GetFirstMonthPayroll(string companyId)
        {
            var firstMonthPayrollDetails = new List<FirstMonthPayroll>();
            string storeProcedure = "sp_Get_First_Month_Payroll_SOP";
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    firstMonthPayrollDetails = JsonConvert.DeserializeObject<List<FirstMonthPayroll>>(res)
                       ?? new List<FirstMonthPayroll>();

                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    firstMonthPayrollDetails = new List<FirstMonthPayroll>();
                }
            }

            return firstMonthPayrollDetails;
        }

        public async Task<List<Category>> GetCategory()
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
            catch (System.Text.Json.JsonException ex)
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


        public async Task<List<Question>> GetQuestion(int categoryId)
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
            catch (System.Text.Json.JsonException ex)
            {

                Questionlist = new List<Question>();
            }
            return Questionlist;
        }

        public async Task<List<Answer1>> GetSOPAnswer1(int QuestionId, string Createdby)
        {
            var AnswerDetails = new List<Answer1>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Client_Details_1";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer1>>(res)
                                                 ?? new List<Answer1>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer1>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer1(Answer1 answer1)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer1 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Client_Details_1";
                var parameters = new DynamicParameters();


                parameters.Add("@QuestionId", answer1.QuestionId);
                parameters.Add("@Company_Id", answer1.Company_Id ?? (object)DBNull.Value);
                parameters.Add("@Company_Code", !string.IsNullOrWhiteSpace(answer1.Company_Code) ? answer1.Company_Code : (object?)null, DbType.String);
                parameters.Add("@Company_Name", !string.IsNullOrWhiteSpace(answer1.Company_Name) ? answer1.Company_Name : (object?)null, DbType.String);
                parameters.Add("@SAP_Code", !string.IsNullOrWhiteSpace(answer1.SAP_Code) ? answer1.SAP_Code : (object?)null, DbType.String);
                parameters.Add("@MyContract_Reference_ID", !string.IsNullOrWhiteSpace(answer1.MyContract_Reference_ID) ? answer1.MyContract_Reference_ID : (object?)null, DbType.String);
                parameters.Add("@Client_Website_link", !string.IsNullOrWhiteSpace(answer1.Client_Website_link) ? answer1.Client_Website_link : (object?)null, DbType.String);
                parameters.Add("@First_Month_Payroll", !string.IsNullOrWhiteSpace(answer1.First_Month_Payroll) ? answer1.First_Month_Payroll : (object?)null, DbType.String);
                parameters.Add("@Client_Onboarding_Month", !string.IsNullOrWhiteSpace(answer1.Client_Onboarding_Month) ? answer1.Client_Onboarding_Month : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer1.CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer2> GetSOPAnswer2(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer2();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_2";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer2>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer2();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer2();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer2(Answer2 answer2)
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

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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


        public async Task<Answer3> GetSOPAnswer3(int QuestionId, int ComapnayId, string Createdby)
        {
            var AnswerDetails = new Answer3();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_3";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", ComapnayId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer3>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer3();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer3();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer3(Answer3 answer3)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer3 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_3";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer3.QuestionId);
                parameters.Add("@Company_Id", answer3.Company_Id);
                parameters.Add("@POC_Change", answer3.POC_Change);
                parameters.Add("@BU_Location_Change", answer3.BU_Location_Change);
                parameters.Add("@CreatedBy", answer3.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer6> GetSOPAnswer6(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer6();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_6";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer6>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer6();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer6();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer6(Answer6 answer6)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer6 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_6";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer6.QuestionId);
                parameters.Add("@Company_Id", answer6.Company_Id);
                parameters.Add("@FF_Payment_Mode", answer6.FF_Payment_Mode);
                parameters.Add("@CreatedBy", answer6.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer8> GetSOPAnswer8(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer8();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_8";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer8>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer8();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer8();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer8(Answer8 answer8)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer8 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_8";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer8.QuestionId);
                parameters.Add("@Company_Id", answer8.Company_Id);
                parameters.Add("@Sim_Card_Management_tracker", answer8.Sim_Card_Management_tracker);
                parameters.Add("@Email_Id_Management_tracker", answer8.Email_Id_Management_tracker);
                parameters.Add("@Id_Card_Management_tracker", answer8.Id_Card_Management_tracker);
                parameters.Add("@CreatedBy", answer8.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer9> GetSOPAnswer9(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer9();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_9";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer9>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer9();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer9();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer9(Answer9 answer3)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer3 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_9";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer3.QuestionId);
                parameters.Add("@Email_ID_Managemnet_Tracker", answer3.Email_ID_Managemnet_Tracker);
                parameters.Add("@CreatedBy", answer3.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer10> GetSOPAnswer10(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer10();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_10";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer10>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer10();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer10();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer10(Answer10 answer10)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer10 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_10";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer10.QuestionId);
                parameters.Add("@ID_Card_Managemnet_Tracker", answer10.ID_Card_Managemnet_Tracker);
                parameters.Add("@CreatedBy", answer10.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer5> GetSOPAnswer5(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer5();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Payroll_Calendar_5";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer5>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer5();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer5();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer5(Answer5 answer5)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer5 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Payroll_Calendar_5";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer5.QuestionId);
                parameters.Add("@Company_Id", answer5.Company_Id);
                parameters.Add("@Attendance_Cycle_From", answer5.Attendance_Cycle_From ?? (object)DBNull.Value);
                parameters.Add("@Attendance_Cycle_To", answer5.Attendance_Cycle_To ?? (object)DBNull.Value);
                parameters.Add("@PayRoll_Cycle_From", answer5.PayRoll_Cycle_From ?? (object)DBNull.Value);
                parameters.Add("@PayRoll_Cycle_To", answer5.PayRoll_Cycle_To ?? (object)DBNull.Value);
                parameters.Add("@Collection_Date", answer5.Collection_Date ?? (object)DBNull.Value);
                parameters.Add("@Group_Name_Site_Master", !string.IsNullOrWhiteSpace(answer5.Group_Name_Site_Master) ? answer5.Group_Name_Site_Master : (object?)null, DbType.String);
                parameters.Add("@PayOut_Date", answer5.PayOut_Date ?? (object)DBNull.Value);
                parameters.Add("@Payment_Proof", !string.IsNullOrWhiteSpace(answer5.Payment_Proof) ? answer5.Payment_Proof : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer5.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer7>> GetSOPAnswer7(int QuestionId, string Createdby)
        {
            var AnswerDetails = new List<Answer7>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_7";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer7>>(res)
                                                     ?? new List<Answer7>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer7>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer7(Answer7 answer7)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer7 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_7";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer7.QuestionId);
                parameters.Add("@CompanyId", answer7.CompanyId ?? (object)DBNull.Value);
                parameters.Add("@First_month_Payroll", answer7.First_month_Payroll ?? (object)DBNull.Value);
                parameters.Add("@CreatedBy", answer7.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer13> GetSOPAnswer13(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer13();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_13";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer13>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer13();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer13();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer13(Answer13 answer13)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer13 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_13";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer13.QuestionId);
                parameters.Add("@Company_Id", answer13.Company_Id);
                parameters.Add("@Attendance_Checking", answer13.Attendance_Checking);
                parameters.Add("@CreatedBy", answer13.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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


        public async Task<Answer14> GetSOPAnswer14(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer14();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_14";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer14>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer14();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer14();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer14(Answer14 answer14)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer14 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_14";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer14.QuestionId);
                parameters.Add("@Company_Id", answer14.Company_Id);
                parameters.Add("@Major_Correction", answer14.Major_Correction);
                parameters.Add("@Remarks", answer14.Remarks);
                parameters.Add("@CreatedBy", answer14.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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


        public async Task<Answer17> GetSOPAnswer17(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer17();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_17";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer17>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer17();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer17();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer17(Answer17 answer17)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer17 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_17";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer17.QuestionId);
                parameters.Add("@Company_Id", answer17.Company_Id);
                parameters.Add("@Inactive_Employee_Load", answer17.Inactive_Employee_Load);
                parameters.Add("@FF_Days", answer17.FF_Days);
                parameters.Add("@Remarks", answer17.Remarks);
                parameters.Add("@Gratuity", answer17.Gratuity);
                parameters.Add("@Date_Submission", DateTime.TryParse(answer17.Date_Submission, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@CreatedBy", answer17.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer16> GetSOPAnswer16(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer16();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_16";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer16>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer16();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer16();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer16(Answer16 answer16)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer16 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_16";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer16.QuestionId);
                parameters.Add("@Company_Id", answer16.Company_Id);
                parameters.Add("@Adhoc_Payment", answer16.Adhoc_Payment);
                parameters.Add("@Date_Of_Disbursal", DateTime.TryParse(answer16.Date_Of_Disbursal, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@Payment_proof", answer16.Payment_proof);
                parameters.Add("@Paycode", answer16.Paycode);
                parameters.Add("@Input_Type", answer16.Input_Type);
                parameters.Add("@Incentive_Calculation", answer16.Incentive_Calculation);
                parameters.Add("@CreatedBy", answer16.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer18> GetSOPAnswer18(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer18();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_18";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer18>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer18();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer18();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer18(Answer18 answer18)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer18 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_18";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer18.QuestionId);
                parameters.Add("@Company_Id", answer18.Company_Id);
                parameters.Add("@Payslip_Distribution", answer18.Payslip_Distribution);
                parameters.Add("@Quess_Ess", !string.IsNullOrWhiteSpace(answer18.Quess_Ess) ? answer18.Quess_Ess : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer18.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer19> GetSOPAnswer19(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer19();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_19";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer19>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer19();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer19();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer19(Answer19 answer19)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer19 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_19";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer19.QuestionId);
                parameters.Add("@Company_Id", answer19.Company_Id);
                parameters.Add("@Notice_Period_Pay", answer19.Notice_Period_Pay);
                parameters.Add("@Threshold_Day", answer19.Threshold_Day);
                parameters.Add("@Applicable_Wages_BASIC_DA", answer19.Applicable_Wages_BASIC_DA);
                parameters.Add("@Applicable_Wages_GROSS", answer19.Applicable_Wages_GROSS);
                parameters.Add("@CreatedBy", answer19.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer21> GetSOPAnswer21(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer21();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_21";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer21>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer21();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer21();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer21(Answer21 answer21)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer21 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_21";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer21.QuestionId);
                parameters.Add("@Company_Id", answer21.Company_Id);
                parameters.Add("@Maternity", !string.IsNullOrWhiteSpace(answer21.Maternity) ? answer21.Maternity : (object?)null, DbType.String);
                parameters.Add("@Remarks", !string.IsNullOrWhiteSpace(answer21.Remarks) ? answer21.Remarks : (object?)null, DbType.String);
                parameters.Add("@Applicable", !string.IsNullOrWhiteSpace(answer21.Applicable) ? answer21.Applicable : (object?)null, DbType.String);
                parameters.Add("@Billable", !string.IsNullOrWhiteSpace(answer21.Billable) ? answer21.Billable : (object?)null, DbType.String);
                parameters.Add("@Salary", !string.IsNullOrWhiteSpace(answer21.Salary) ? answer21.Salary : (object?)null, DbType.String);
                parameters.Add("@Approval", !string.IsNullOrWhiteSpace(answer21.Approval) ? answer21.Approval : (object?)null, DbType.String);
                parameters.Add("@Point_Of_Contact", !string.IsNullOrWhiteSpace(answer21.Point_Of_Contact) ? answer21.Point_Of_Contact : (object?)null, DbType.String);
                parameters.Add("@Email", !string.IsNullOrWhiteSpace(answer21.Email) ? answer21.Email : (object?)null, DbType.String);
                parameters.Add("@Mobile_Number", !string.IsNullOrWhiteSpace(answer21.Mobile_Number) ? answer21.Mobile_Number : (object?)null, DbType.String);
                parameters.Add("@Name", !string.IsNullOrWhiteSpace(answer21.Name) ? answer21.Name : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer21.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer23> GetSOPAnswer23(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer23();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_23";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer23>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer23();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer23();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer23(Answer23 answer23)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer23 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_23";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer23.QuestionId);
                parameters.Add("@Company_Id", answer23.Company_Id);
                parameters.Add("@BGV_Applicable", answer23.BGV_Applicable);
                parameters.Add("@Eligibility", answer23.Eligibility);
                parameters.Add("@Eligibility_By", answer23.Eligibility_By);
                parameters.Add("@Cost", answer23.Cost);
                parameters.Add("@CreatedBy", answer23.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer25> GetSOPAnswer25(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer25();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_25";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer25>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer25();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer25();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer25(Answer25 answer25)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer25 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_25";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer25.QuestionId);
                parameters.Add("@Company_Id", answer25.Company_Id);
                parameters.Add("@Billiable", answer25.Billiable);
                parameters.Add("@Calandar_Type", !string.IsNullOrWhiteSpace(answer25.Calandar_Type) ? answer25.Calandar_Type : (object?)null, DbType.String);
                parameters.Add("@Accumulated_FlushOut", !string.IsNullOrWhiteSpace(answer25.Accumulated_FlushOut) ? answer25.Accumulated_FlushOut : (object?)null, DbType.String);
                parameters.Add("@Billed_Paid", !string.IsNullOrWhiteSpace(answer25.Billed_Paid) ? answer25.Billed_Paid : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer25.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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


        public async Task<Answer28> GetSOPAnswer28(int QuestionId, string Createdby)
        {
            var AnswerDetails = new Answer28();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_28";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer28>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer28();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer28();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer28(Answer28 answer28)
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

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer29> GetSOPAnswer29(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer29();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_29";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer29>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer29();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer29();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer29(Answer29 answer29)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer29 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_29";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer29.QuestionId);
                parameters.Add("@Company_Id", answer29.Company_Id);
                parameters.Add("@Billable", answer29.Billable);
                parameters.Add("@Display_Register", answer29.Display_Register);
                parameters.Add("@CreatedBy", answer29.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer30> GetSOPAnswer30(int QuestionId,int CompanyId ,string Createdby)
        {
            var AnswerDetails = new Answer30();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_30";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer30>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer30();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer30();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer30(Answer30 answer30)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer30 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_30";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer30.QuestionId);
                parameters.Add("@Company_Id", answer30.Company_Id);
                parameters.Add("@PO_Type", answer30.PO_Type);
                parameters.Add("@PF_Calculated_15K_BASED_ON_ATTENDANCE", !string.IsNullOrWhiteSpace(answer30.PF_Calculated_15K_BASED_ON_ATTENDANCE) ? answer30.PF_Calculated_15K_BASED_ON_ATTENDANCE : (object?)null, DbType.String);
                parameters.Add("@PF_Calculated_Wages_Without_Any_Capping", !string.IsNullOrWhiteSpace(answer30.PF_Calculated_Wages_Without_Any_Capping) ? answer30.PF_Calculated_Wages_Without_Any_Capping : (object?)null, DbType.String);
                parameters.Add("@PF_Calculated_Earnings_Restricting_15K", !string.IsNullOrWhiteSpace(answer30.PF_Calculated_Earnings_Restricting_15K) ? answer30.PF_Calculated_Earnings_Restricting_15K : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer30.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer32> GetSOPAnswer32(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer32();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_32";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer32>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer32();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer32();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer32(Answer32 answer32)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer32 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_32";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer32.QuestionId);
                parameters.Add("@Company_Id", answer32.Company_Id);
                parameters.Add("@Calculation", answer32.Calculation);
                parameters.Add("@ATTRIBUTES", answer32.ATTRIBUTES);
                parameters.Add("@CreatedBy", answer32.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer36> GetSOPAnswer36(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer36();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_36";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer36>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer36();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer36();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer36(Answer36 answer36)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer36 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_36";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer36.QuestionId);
                parameters.Add("@Company_Id", answer36.Company_Id);
                parameters.Add("@Absorption_Fee", answer36.Absorption_Fee);
                parameters.Add("@Eligibility", answer36.Eligibility);
                parameters.Add("@TAT", answer36.TAT);
                parameters.Add("@Commercials", answer36.Commercials);
                parameters.Add("@Flat", answer36.Flat ?? (object)DBNull.Value);
                parameters.Add("@Pay_Code", !string.IsNullOrWhiteSpace(answer36.Pay_Code) ? answer36.Pay_Code : (object?)null, DbType.String);
                parameters.Add("@Designation", !string.IsNullOrWhiteSpace(answer36.Designation) ? answer36.Designation : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer36.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer37> GetSOPAnswer37(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer37();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_37";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer37>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer37();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer37();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer37(Answer37 answer37)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer37 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_37";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer37.QuestionId);
                parameters.Add("@Company_Id", answer37.Company_Id);
                parameters.Add("@Payment", answer37.Payment);
                parameters.Add("@Payment_Days", answer37.Payment_Days);
                parameters.Add("@CreatedBy", answer37.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer38> GetSOPAnswer38(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer38();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_38";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer38>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer38();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer38();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer38(Answer38 answer38)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer38 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_38";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer38.QuestionId);
                parameters.Add("@Company_Id", answer38.Company_Id);
                parameters.Add("@Penalty_Clause", answer38.Penalty_Clause);
                parameters.Add("@Payroll_Closure_Date", answer38.Payroll_Closure_Date ?? (object)DBNull.Value);
                parameters.Add("@CreatedBy", answer38.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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


        public async Task<List<Answer12>> GetSOPAnswer12(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer12>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_12";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer12>>(res)
                                                     ?? new List<Answer12>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer12>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer12(List<Answer12> answer12)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer12 != null)
            {
                string storeProcedure1 = "SP_Delete_tbl_Customer_SOP_Answer_Details_12";
                var parameters1 = new DynamicParameters();
                parameters1.Add("@QuestionId", answer12[0].QuestionId);
                parameters1.Add("@Company_Id", answer12[0].Company_Id);
                parameters1.Add("@CreatedBy", answer12[0].CreatedBy);


                var res1 = await this._dbRepository.GetItemsAsync(storeProcedure1, parameters1);
                if (!string.IsNullOrWhiteSpace(res1))
                {
                    foreach (var answer in answer12)
                    {
                        string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_12";
                        var parameters = new DynamicParameters();

                        parameters.Add("@QuestionId", answer.QuestionId);
                        parameters.Add("@Company_Id", answer.Company_Id);
                        parameters.Add("@SubId", answer.SubId);
                        parameters.Add("@Filling_Attendance", answer.Filling_Attendance);
                        parameters.Add("@CreatedBy", answer.CreatedBy);

                        var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            AnswerDetails.response = "Success";
                        }
                    }
                    AnswerDetails.response = "Success";
                }


            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public async Task<Answer27> GetSOPAnswer27(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer27();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_27";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer27>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer27();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer27();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer27(Answer27 answer27)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer27 != null)
            {
                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_27";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer27.QuestionId);
                parameters.Add("@Company_Id", answer27.Company_Id);
                parameters.Add("@Variable_Pay", answer27.Variable_Pay);
                parameters.Add("@Term", answer27.Term);
                parameters.Add("@Billing_Type", answer27.Billing_Type);
                parameters.Add("@CreatedBy", answer27.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<Answer39> GetSOPAnswer39(int QuestionId,int CompanyId, string Createdby)
        {

            var checklistQuestionAnswerDetails = new Answer39();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_39";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var answerList = JsonConvert.DeserializeObject<List<Answer39>>(res);
                    checklistQuestionAnswerDetails = answerList?.FirstOrDefault() ?? new Answer39();

                    // Get and set POUtilization list separately
                    checklistQuestionAnswerDetails.POUtiliziation = await GetSOPAnswer39_1(QuestionId,CompanyId, Createdby);
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    checklistQuestionAnswerDetails = new Answer39();
                }
            }

            return checklistQuestionAnswerDetails;

        }

        public async Task<List<Po_Utiliziation>> GetSOPAnswer39_1(int QuestionId,int CompanyId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new List<Po_Utiliziation>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_39_1";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

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


        public async Task<AnswerResponse> PostSOPAnswer39(Answer39 answer39)
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
                        parameters.Add("@Company_Id", answer39.Company_Id);
                        parameters.Add("@SubId", answer.SubId);
                        parameters.Add("@PO_Applicable", answer39.PO_Applicable);
                        parameters.Add("@PO_Type", answer39.PO_Type);
                        parameters.Add("@PO_Utiliziation", answer.PO_Utiliziation);
                        parameters.Add("@PO_Category", answer39.PO_Category);
                        parameters.Add("@Currency", answer39.Currency);
                        parameters.Add("@CreatedBy", answer39.CreatedBy);

                        var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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
                    parameters.Add("@Company_Id", answer39.Company_Id);
                    parameters.Add("@SubId", "");
                    parameters.Add("@PO_Applicable", answer39.PO_Applicable);
                    parameters.Add("@PO_Type", "");
                    parameters.Add("@PO_Utiliziation", "");
                    parameters.Add("@PO_Category", "");
                    parameters.Add("@Currency", "");
                    parameters.Add("@CreatedBy", answer39.CreatedBy);

                    var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Marked_Category>> GetmarkedQuestion(int CompanyId, string Createdby)
        {
            var categorydetails = new List<Marked_Category>();
            string storeProcedure = "SP_GET_Customer_SOP_Marked_Category";
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@Createdby", Createdby);
            


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    categorydetails = JsonConvert.DeserializeObject<List<Marked_Category>>(res)
                                                     ?? new List<Marked_Category>();

                    // Now populate cheklistAnswer1s for each question
                    foreach (var category in categorydetails)
                    {
                        category.Marked_Question = await GetmarkedQuestion(CompanyId,category.CategoryId, Createdby);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    categorydetails = new List<Marked_Category>();
                }
            }

            return categorydetails;

        }

        public async Task<List<Marked_Question>> GetmarkedQuestion(int CompanyId,string CategoryId, string Createdby)
        {
            var questionDetails = new List<Marked_Question>();
            string storeProcedure = "SP_GET_Customer_SOP_Marked_Question";
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CategoryId", CategoryId);
            parameters.Add("@Createdby", Createdby);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

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

        public async Task<AnswerResponse> PostSOPAnswer4(Answer4Request answer4)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer4 != null)
            {
                string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_4";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer4.QuestionId);
                parameters.Add("@Company_Id", answer4.Company_Id);
                parameters.Add("@CreatedBy", answer4.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

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
                        parameters.Add("@Company_Id", answer4.Company_Id);
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

        public async Task<Answer4RequestGet> GetSOPAnswer4(int QuestionId,int CompanyId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new Answer4RequestGet
            {
                QuestionId = QuestionId,
                Company_Id = CompanyId,
                CreatedBy = Createdby,
                Vertical = new List<Answer4Get>(),
                Department = new List<Answer4Get>(),
                Manager = new List<Answer4Get>(),
                Circle = new List<Answer4Get>()
            };

            string storedProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_4";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
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

        public async Task<AnswerResponse> PostSOPAnswer33(Answer33Request answer33)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer33 != null)
            {
                string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_33";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer33.QuestionId);
                parameters.Add("@Company_Id", answer33.Company_Id);
                parameters.Add("@CreatedBy", answer33.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

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
                        parameters.Add("@Company_Id", answer33.Company_Id);
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

        public async Task<Answer33RequestGet> GetSOPAnswer33(int QuestionId,int CompanyId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new Answer33RequestGet
            {
                QuestionId = QuestionId,
                Company_Id= CompanyId,
                CreatedBy = Createdby,
                Email = new List<Answer33Get>(),
                Portal = new List<Answer33Get>()
            };

            string storedProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_33";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
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

        public async Task<AnswerResponse> PostSOPAnswer31(IFormFile file, [FromForm] string billApplicable,
            [FromForm] int QuestionId, [FromForm] int CompanyId, [FromForm] string CreatedBy)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ReimbursementPolicyUploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"ReimbursementPolicy_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_31";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", QuestionId);
                parameters.Add("@Company_Id", CompanyId);
                parameters.Add("@Bill_Applicable", billApplicable);
                parameters.Add("@File_Path", filePath);
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<AnswerResponse> PostSOPAnswer31_1([FromForm] string billApplicable,
           [FromForm] int QuestionId, [FromForm] int CompanyId, [FromForm] string CreatedBy)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            string storeProcedure = "SP_Insert_Update_tbl_Customer_SOP_Answer_Details_31";
            var parameters = new DynamicParameters();

            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@Bill_Applicable", billApplicable);
            parameters.Add("@File_Path", "");
            parameters.Add("@CreatedBy", CreatedBy);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (!string.IsNullOrWhiteSpace(res))
            {
                AnswerDetails.response = "Success";
            }

            return AnswerDetails;
        }

        public async Task<Answer31> GetSOPAnswer31(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new Answer31();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_31";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var companyList = JsonConvert.DeserializeObject<List<Answer31>>(res);
                    AnswerDetails = companyList?.FirstOrDefault() ?? new Answer31();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new Answer31();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer11(Answer11Request answer11)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer11 != null)
            {
                string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_11";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer11.QuestionId);
                parameters.Add("@Company_Id", answer11.Company_Id);
                parameters.Add("@CreatedBy", answer11.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

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
                        parameters.Add("@Company_Id", answer11.Company_Id);
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

        public async Task<Answer11RequestGet> GetSOPAnswer11(int QuestionId,int CompanyId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new Answer11RequestGet
            {
                QuestionId = QuestionId,
                Company_Id = CompanyId,
                CreatedBy = Createdby,
                Email = new List<Answer11Get>(),
                Portal = new List<Answer11Get>(),
                Biometric = new List<Answer11Get>(),
                Others = new List<Answer11Get>()
            };

            string storedProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_11";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
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


        public async Task<AnswerResponse> PostSOPAnswer15(Answer15Request answer15)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer15 != null)
            {
                string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_15";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer15.QuestionId);
                parameters.Add("@Company_Id", answer15.Company_Id);
                parameters.Add("@CreatedBy", answer15.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

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
                        parameters.Add("@Company_Id", answer15.Company_Id);
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

        public async Task<Answer15RequestGet> GetSOPAnswer15(int QuestionId,int CompanyId, string Createdby)
        {
            var checklistQuestionAnswerDetails = new Answer15RequestGet
            {
                QuestionId = QuestionId,
                Company_Id = CompanyId,
                CreatedBy = Createdby,
                Email = new List<Answer15Get>(),
                Portal = new List<Answer15Get>(),
                Biometric = new List<Answer15Get>(),
                Others = new List<Answer15Get>()
            };

            string storedProcedure = "SP_GET_tbl_Customer_SOP_Answer_Details_15";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);

            var result = await _dbRepository.GetItemsAsync<Answer15Get>(storedProcedure, parameters);

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

        public async Task<AnswerResponse> PostSOPAnswer20(Answer20 answer20)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer20 != null)
            {

                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Notice_Period_Recovery_20";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer20.QuestionId);
                parameters.Add("@Applicable", !string.IsNullOrWhiteSpace(answer20.Applicable) ? answer20.Applicable : (object?)null, DbType.String);
                parameters.Add("@Eligible_days", answer20.Eligible_days ?? (object)DBNull.Value);
                parameters.Add("@Applicable_Desc_Client", !string.IsNullOrWhiteSpace(answer20.Applicable_Desc_Client) ? answer20.Applicable_Desc_Client : (object?)null, DbType.String);
                parameters.Add("@Designation_Id", answer20.Designation_Id ?? (object)DBNull.Value);
                parameters.Add("@CompanyId", !string.IsNullOrWhiteSpace(answer20.CompanyId) ? answer20.CompanyId : (object?)null, DbType.String);
                parameters.Add("@Designation_Name", !string.IsNullOrWhiteSpace(answer20.Designation_Name) ? answer20.Designation_Name : (object?)null, DbType.String);
                parameters.Add("@Designationwise_Days", answer20.Designationwise_Days ?? (object)DBNull.Value);
                parameters.Add("@Applicable_Wages_BASIC_DA", answer20.Applicable_Wages_BASIC_DA);
                parameters.Add("@Applicable_Wages_GROSS", answer20.Applicable_Wages_GROSS ?? (object)DBNull.Value);
                parameters.Add("@CreatedBy", answer20.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer20>> GetSOPAnswer20(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer20>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Notice_Period_Recovery_20";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CompanyId", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer20>>(res)
                                                      ?? new List<Answer20>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer20>();
                }
            }


            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer22(Answer22 answer22)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer22 != null)
            {

                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Leave_22";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer22.QuestionId);
                parameters.Add("@Company_Id", answer22.Company_Id);
                parameters.Add("@Applicable", answer22.Applicable);
                parameters.Add("@Leave_Management", !string.IsNullOrWhiteSpace(answer22.Leave_Management) ? answer22.Leave_Management : (object?)null, DbType.String);
                parameters.Add("@Calander_Type", !string.IsNullOrWhiteSpace(answer22.Calander_Type) ? answer22.Calander_Type : (object?)null, DbType.String);
                parameters.Add("@Leave_Type_Id", answer22.Leave_Type_Id ?? (object)DBNull.Value);
                parameters.Add("@Leave_Type", !string.IsNullOrWhiteSpace(answer22.Leave_Type) ? answer22.Leave_Type : (object?)null, DbType.String);
                parameters.Add("@No_Of_Leave", answer22.No_Of_Leave ?? (object)DBNull.Value);
                parameters.Add("@Carry_Forward", !string.IsNullOrWhiteSpace(answer22.Carry_Forward) ? answer22.Carry_Forward : (object?)null, DbType.String);
                parameters.Add("@Carry_Forward_Days", answer22.Carry_Forward_Days ?? (object)DBNull.Value);
                parameters.Add("@Encashment", !string.IsNullOrWhiteSpace(answer22.Encashment) ? answer22.Encashment : (object?)null, DbType.String);
                parameters.Add("@Leave_Encashment", !string.IsNullOrWhiteSpace(answer22.Leave_Encashment) ? answer22.Leave_Encashment : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer22.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer22>> GetSOPAnswer22(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer22>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Leave_22";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer22>>(res)
                                                      ?? new List<Answer22>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer22>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer24(Answer24 answer24)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer24 != null)
            {

                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Holiday_24";
                var parameters = new DynamicParameters();

                parameters.Add("@QuestionId", answer24.QuestionId);
                parameters.Add("@Calander_Type", answer24.Calander_Type);
                parameters.Add("@State_Id", answer24.State_Id);
                parameters.Add("@State_Name", answer24.State_Name);
                parameters.Add("@Leave_Type", answer24.Leave_Type);
                parameters.Add("@Holiday_Date", DateTime.TryParse(answer24.Holiday_Date, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@Leave_Description", answer24.Leave_Description);
                parameters.Add("@Is_Billable", answer24.Is_Billable);
                parameters.Add("@Billable_Type", answer24.Billable_Type);
                parameters.Add("@CreatedBy", answer24.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer24>> GetSOPAnswer24(int QuestionId, string Createdby)
        {
            var AnswerDetails = new List<Answer24>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Holiday_24";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer24>>(res)
                                                      ?? new List<Answer24>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer24>();
                }
            }

            return AnswerDetails;
        }

        public async Task<List<PermissionwiseCompanyModel>> GetUserWiseCompanyCode(int UserId)
        {
            if (UserId != 3)
            {
                UserId = 3;
            }
            var CompanyCodeDetails = new List<PermissionwiseCompanyModel>();
            string storeProcedure = "Sp_GetUserWiseCompanyCode";
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    CompanyCodeDetails = JsonConvert.DeserializeObject<List<PermissionwiseCompanyModel>>(res)
                                                     ?? new List<PermissionwiseCompanyModel>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    CompanyCodeDetails = new List<PermissionwiseCompanyModel>();
                }
            }

            return CompanyCodeDetails;
        }
        public async Task<List<PremiumTracker>> GetPremiumTracker26()
        {
            var PremiumTrackerDetails = new List<PremiumTracker>();
            string storeProcedure = "USP_CommonDropDowns";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "INSURANCE_PERMIUM_TRACKER");

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    PremiumTrackerDetails = JsonConvert.DeserializeObject<List<PremiumTracker>>(res)
                                                     ?? new List<PremiumTracker>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    PremiumTrackerDetails = new List<PremiumTracker>();
                }
            }

            return PremiumTrackerDetails;
        }

        public async Task<List<InsuranceCoverageType>> GetCoverageType26()
        {
            var InsuranceCoverageTypeDetails = new List<InsuranceCoverageType>();
            string storeProcedure = "sp_GetAllCoverageType";
            var parameters = new DynamicParameters();


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    InsuranceCoverageTypeDetails = JsonConvert.DeserializeObject<List<InsuranceCoverageType>>(res)
                                                     ?? new List<InsuranceCoverageType>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    InsuranceCoverageTypeDetails = new List<InsuranceCoverageType>();
                }
            }

            return InsuranceCoverageTypeDetails;
        }

        public async Task<List<Policy>> GetGPAPolicy26()
        {
            var PolicyDetails = new List<Policy>();
            string storeProcedure = "sp_GetAllInsurancePolicyTypeForOther";
            var parameters = new DynamicParameters();
            parameters.Add("@InsuranceType", "GPA");


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    PolicyDetails = JsonConvert.DeserializeObject<List<Policy>>(res)
                                                     ?? new List<Policy>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    PolicyDetails = new List<Policy>();
                }
            }

            return PolicyDetails;
        }

        public async Task<List<Policy>> GetGTLIPolicy26()
        {
            var PolicyDetails = new List<Policy>();
            string storeProcedure = "sp_GetAllInsurancePolicyTypeForOther";
            var parameters = new DynamicParameters();
            parameters.Add("@InsuranceType", "GTLI");


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    PolicyDetails = JsonConvert.DeserializeObject<List<Policy>>(res)
                                                     ?? new List<Policy>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    PolicyDetails = new List<Policy>();
                }
            }

            return PolicyDetails;
        }

        public async Task<List<Paycode>> GetDeductionPaycode26()
        {
            var PaycodeDetails = new List<Paycode>();
            string storeProcedure = "sp_GetAllInsurancePayCodes";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Deduction");


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    PaycodeDetails = JsonConvert.DeserializeObject<List<Paycode>>(res)
                                                     ?? new List<Paycode>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    PaycodeDetails = new List<Paycode>();
                }
            }

            return PaycodeDetails;
        }

        public async Task<List<Paycode>> GetBillingPaycode26()
        {
            var PaycodeDetails = new List<Paycode>();
            string storeProcedure = "sp_GetAllInsurancePayCodes";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Billing");


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    PaycodeDetails = JsonConvert.DeserializeObject<List<Paycode>>(res)
                                                     ?? new List<Paycode>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    PaycodeDetails = new List<Paycode>();
                }
            }

            return PaycodeDetails;
        }

        public async Task<List<string>> GetMartialStatus26()
        {
            return new List<string>() { "-Select-", "Single", "Married", "Divorced", "Widowed" };
        }

        public async Task<List<EmployeeType>> GetEmployeeType26()
        {
            var EmployeeTypeDetails = new List<EmployeeType>();
            string storeProcedure = "USP_CommonDropDowns";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetAllEmployeeType");


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    EmployeeTypeDetails = JsonConvert.DeserializeObject<List<EmployeeType>>(res)
                                                     ?? new List<EmployeeType>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    EmployeeTypeDetails = new List<EmployeeType>();
                }
            }

            return EmployeeTypeDetails;
        }

        public async Task<List<NewJoinee_Arrear>> GetNewJoinee26()
        {
            var NewJoineeDetails = new List<NewJoinee_Arrear>();
            string storeProcedure = "USP_CommonDropDowns";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetAllNewJoineeArrearType");


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    NewJoineeDetails = JsonConvert.DeserializeObject<List<NewJoinee_Arrear>>(res)
                                                     ?? new List<NewJoinee_Arrear>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    NewJoineeDetails = new List<NewJoinee_Arrear>();
                }
            }

            return NewJoineeDetails;
        }

        public async Task<List<GroupDetails>> GetAllGroupByCompany(int CompanyId)
        {
            var GroupDetails = new List<GroupDetails>();
            string storeProcedure = "sp_GetAllGroupNameByCompany";
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", CompanyId);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    GroupDetails = JsonConvert.DeserializeObject<List<GroupDetails>>(res)
                                                     ?? new List<GroupDetails>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    GroupDetails = new List<GroupDetails>();
                }
            }

            return GroupDetails;
        }

        public async Task<List<InsuranceVertical>> GetInsuranceVertical(int CompanyId)
        {
            var InsuranceVerticalDetails = new List<InsuranceVertical>();
            string storeProcedure = "sp_GetAllInsuranceVertical";
            var parameters = new DynamicParameters();
            parameters.Add("@Company_ID", CompanyId);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    InsuranceVerticalDetails = JsonConvert.DeserializeObject<List<InsuranceVertical>>(res)
                                                     ?? new List<InsuranceVertical>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    InsuranceVerticalDetails = new List<InsuranceVertical>();
                }
            }

            return InsuranceVerticalDetails;
        }

        public async Task<List<DesignationMaster>> GetAllDesignationByCompany(int CompanyId)
        {
            var DesignationMasterDetails = new List<DesignationMaster>();
            string storeProcedure = "sp_GetDesignationDetails";
            var parameters = new DynamicParameters();
            parameters.Add("@Companycode", CompanyId);
            parameters.Add("@Designation_Id", 0);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    DesignationMasterDetails = JsonConvert.DeserializeObject<List<DesignationMaster>>(res)
                                                     ?? new List<DesignationMaster>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    DesignationMasterDetails = new List<DesignationMaster>();
                }
            }

            return DesignationMasterDetails;
        }

        public async Task<List<GMCPolicyCondition>> GetPolicyConditionByCoverageType(int CoverageTypeId)
        {
            var GMCPolicyConditionDetails = new List<GMCPolicyCondition>();
            string storeProcedure = "sp_GetAllPolicyCondition";
            var parameters = new DynamicParameters();
            parameters.Add("@CoverageTypeId", CoverageTypeId);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    GMCPolicyConditionDetails = JsonConvert.DeserializeObject<List<GMCPolicyCondition>>(res)
                                                     ?? new List<GMCPolicyCondition>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    GMCPolicyConditionDetails = new List<GMCPolicyCondition>();
                }
            }

            return GMCPolicyConditionDetails;
        }

        public async Task<List<GMCPolicyNo>> GetPolicyNoByCondition(int CoverageTypeId, int PolicyConditionId)
        {
            var GMCPolicyNoDetails = new List<GMCPolicyNo>();
            string storeProcedure = "sp_GetAllInsurancePolicyType";
            var parameters = new DynamicParameters();
            parameters.Add("@CoverageTypeId", CoverageTypeId);
            parameters.Add("@PolicyConditionId", PolicyConditionId);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    GMCPolicyNoDetails = JsonConvert.DeserializeObject<List<GMCPolicyNo>>(res)
                                                     ?? new List<GMCPolicyNo>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    GMCPolicyNoDetails = new List<GMCPolicyNo>();
                }
            }

            return GMCPolicyNoDetails;
        }

        public async Task<AnswerResponse> InsuranceExists(InsuranceAlreadyExists answer26)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer26 != null)
            {
                string storeProcedure = "SP_tbl_Customer_SOP_Insurance_Policy_26_Exists";
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", answer26.CompanyId);
                parameters.Add("@GroupDetailId", answer26.GroupDetailId);
                parameters.Add("@PremiumTrackerId", answer26.PremiumTrackerId);
                parameters.Add("@EffectiveDate", answer26.EffectiveDate);
                parameters.Add("@Insurance_Vertical_ID", answer26.Insurance_Vertical_ID);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    var resultList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(res);

                    if (resultList != null && resultList.Count > 0)
                    {
                        var existsValue = resultList[0]["existss"];

                        if (existsValue == "0")
                        {
                            AnswerDetails.response = "Success";
                        }
                        else
                        {
                            AnswerDetails.response = "Already Exists";
                        }
                    }
                }

            }
            else
            {
                AnswerDetails = new AnswerResponse();
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> InsuranceCreate(InsuranceAdd answer26)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer26 != null)
            {
                string json = System.Text.Json.JsonSerializer.Serialize<object>(answer26);
                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Insurance_Policy_26_Exists";
                var parameters = new DynamicParameters();
                parameters.Add("@jsonInput", json);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<InsurancePolicy>> GetSOPInsurance26(int QuestionId, string Createdby)
        {
            var AnswerDetails = new List<InsurancePolicy>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Insurance_Policy_Mapping_26";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<InsurancePolicy>>(res)
                                                      ?? new List<InsurancePolicy>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<InsurancePolicy>();
                }
            }

            return AnswerDetails;
        }

        public async Task<List<Client>> GetClientName26()
        {
            var ClientDetails = new List<Client>();
            string storeProcedure = "USP_Client_Request_Full_Name_Organization";
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    ClientDetails = JsonConvert.DeserializeObject<List<Client>>(res)
                                                      ?? new List<Client>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    ClientDetails = new List<Client>();
                }
            }

            return ClientDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer35(Answer35 answer35)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer35 != null)
            {
                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Service_Fee_35";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer35.QuestionId);
                parameters.Add("@Client_ID", answer35.Client_ID ?? (object)DBNull.Value);
                parameters.Add("@Full_Name_Of_Organization", !string.IsNullOrWhiteSpace(answer35.Full_Name_Of_Organization) ? answer35.Full_Name_Of_Organization : (object?)null, DbType.String);
                parameters.Add("@Type_Of_Contact", !string.IsNullOrWhiteSpace(answer35.Type_Of_Contact) ? answer35.Type_Of_Contact : (object?)null, DbType.String);
                parameters.Add("@Credit_Days_Agreed", !string.IsNullOrWhiteSpace(answer35.Credit_Days_Agreed) ? answer35.Credit_Days_Agreed : (object?)null, DbType.String);
                parameters.Add("@Agreement_Start_Date", DateTime.TryParse(answer35.Agreement_Start_Date, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@Agreement_End_Date", DateTime.TryParse(answer35.Agreement_End_Date, out var parsedDate1) ? parsedDate1 : (object?)null, DbType.Date);
                parameters.Add("@Type_Of_Contact", !string.IsNullOrWhiteSpace(answer35.Type_Of_Contact) ? answer35.Type_Of_Contact : (object?)null, DbType.String);
                parameters.Add("@Agreement_Status", !string.IsNullOrWhiteSpace(answer35.Agreement_Status) ? answer35.Agreement_Status : (object?)null, DbType.String);
                parameters.Add("@Busniess_Head_Approval", !string.IsNullOrWhiteSpace(answer35.Busniess_Head_Approval) ? answer35.Busniess_Head_Approval : (object?)null, DbType.String);
                parameters.Add("@One_Time_Onboarding_Fees", decimal.TryParse(answer35.One_Time_Onboarding_Fees, out var fee) ? fee : (object?)null, DbType.Decimal);
                parameters.Add("@Service_Fee_Type", !string.IsNullOrWhiteSpace(answer35.Service_Fee_Type) ? answer35.Service_Fee_Type : (object?)null, DbType.String);
                parameters.Add("@Service_Fee", decimal.TryParse(answer35.Service_Fee, out var fee1) ? fee1 : (object?)null, DbType.Decimal);
                parameters.Add("@Sourcing_Fee", decimal.TryParse(answer35.Sourcing_Fee, out var fee2) ? fee2 : (object?)null, DbType.Decimal);
                parameters.Add("@Replacement_Clause", !string.IsNullOrWhiteSpace(answer35.Replacement_Clause) ? answer35.Replacement_Clause : (object?)null, DbType.String);
                parameters.Add("@Absorption_Fee", decimal.TryParse(answer35.Absorption_Fee, out var fee3) ? fee3 : (object?)null, DbType.Decimal);
                parameters.Add("@Upfront_Charges", decimal.TryParse(answer35.Upfront_Charges, out var fee4) ? fee4 : (object?)null, DbType.Decimal);
                parameters.Add("@InEdge_Charges", decimal.TryParse(answer35.InEdge_Charges, out var fee5) ? fee5 : (object?)null, DbType.Decimal);
                parameters.Add("@Supplementary_Fee_Type", !string.IsNullOrWhiteSpace(answer35.Supplementary_Fee_Type) ? answer35.Supplementary_Fee_Type : (object?)null, DbType.String);
                parameters.Add("@Supplementary_Charges", decimal.TryParse(answer35.Supplementary_Charges, out var fee6) ? fee6 : (object?)null, DbType.Decimal);
                parameters.Add("@LatePayment_Fee", decimal.TryParse(answer35.LatePayment_Fee, out var fee7) ? fee7 : (object?)null, DbType.Decimal);
                parameters.Add("@Other_Fees", !string.IsNullOrWhiteSpace(answer35.Other_Fees) ? answer35.Other_Fees : (object?)null, DbType.String);
                parameters.Add("@PAYROLL_WITH_DECIMAL", !string.IsNullOrWhiteSpace(answer35.PAYROLL_WITH_DECIMAL) ? answer35.PAYROLL_WITH_DECIMAL : (object?)null, DbType.String);
                parameters.Add("@SERVICE_FEE_WITH_DECIMAL", !string.IsNullOrWhiteSpace(answer35.SERVICE_FEE_WITH_DECIMAL) ? answer35.SERVICE_FEE_WITH_DECIMAL : (object?)null, DbType.String);
                parameters.Add("@OBApplicable", answer35.OBApplicable ?? (object)DBNull.Value);
                parameters.Add("@CreatedBy", answer35.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer35>> GetSOPAnswer35(int QuestionId, string Createdby)
        {
            var AnswerDetails = new List<Answer35>();
            string storeProcedure = "SP_Get_tbl_Customer_SOP_Service_Fee_35";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer35>>(res)
                                                      ?? new List<Answer35>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer35>();
                }
            }

            return AnswerDetails;
        }

        public async Task<List<Answer34>> GetSOPAnswer34(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer34>();
            string storeProcedure = "SP_Get_tbl_Customer_SOP_Gst_Certificate_34";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer34>>(res)
                                                      ?? new List<Answer34>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer34>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer34([FromForm] Answer34 answer34, IFormFile? fileGST, IFormFile? fileSEZ,
            IFormFile? fileLUT)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer34 != null)
            {
                string filePathGST = null;
                if (fileGST != null && fileGST.Length != 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "GST_Certificate");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var newFileName = $"GST_Certificate_{datePrefix}.pdf";

                    filePathGST = Path.Combine(uploadsFolder, newFileName);

                    using (var stream = new FileStream(filePathGST, FileMode.Create))
                    {
                        await fileGST.CopyToAsync(stream);
                    }
                }

                string filePathSEZ = null;

                if (fileSEZ != null && fileSEZ.Length != 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "SEZ_Certificate");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var newFileName = $"SEZ_Certificate_{datePrefix}.pdf";

                    filePathSEZ = Path.Combine(uploadsFolder, newFileName);

                    using (var stream = new FileStream(filePathSEZ, FileMode.Create))
                    {
                        await fileSEZ.CopyToAsync(stream);
                    }
                }

                string filePathLUT = null;
                if (fileLUT != null && fileLUT.Length != 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "LUT_Certificate");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var newFileName = $"LUT_Certificate_{datePrefix}.pdf";

                    filePathLUT = Path.Combine(uploadsFolder, newFileName);

                    using (var stream = new FileStream(filePathLUT, FileMode.Create))
                    {
                        await fileLUT.CopyToAsync(stream);
                    }
                }

                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Gst_Certificate_34";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer34.QuestionId);
                parameters.Add("@Company_Id", answer34.Company_Id);
                parameters.Add("@State_Id", answer34.State_Id ?? (object)DBNull.Value);
                parameters.Add("@State_Name", !string.IsNullOrWhiteSpace(answer34.State_Name) ? answer34.State_Name : (object?)null, DbType.String);
                parameters.Add("@Certificate_Type", !string.IsNullOrWhiteSpace(answer34.Certificate_Type) ? answer34.Certificate_Type : (object?)null, DbType.String);
                parameters.Add("@Invoice_Category", !string.IsNullOrWhiteSpace(answer34.Invoice_Category) ? answer34.Invoice_Category : (object?)null, DbType.String);
                parameters.Add("@Bill_To", !string.IsNullOrWhiteSpace(answer34.Bill_To) ? answer34.Bill_To : (object?)null, DbType.String);
                parameters.Add("@Bill_To_Pin", answer34.Bill_To_Pin ?? (object)DBNull.Value);
                parameters.Add("@Ship_To", !string.IsNullOrWhiteSpace(answer34.Ship_To) ? answer34.Ship_To : (object?)null, DbType.String);
                parameters.Add("@Ship_To_Pin", answer34.Ship_To_Pin ?? (object)DBNull.Value);
                parameters.Add("@GST_Certificate_Path", !string.IsNullOrWhiteSpace(filePathGST) ? filePathGST : (object?)null, DbType.String);
                parameters.Add("@GST_No", !string.IsNullOrWhiteSpace(answer34.GST_No) ? answer34.GST_No : (object?)null, DbType.String);
                parameters.Add("@PAN_No", !string.IsNullOrWhiteSpace(answer34.PAN_No) ? answer34.PAN_No : (object?)null, DbType.String);
                parameters.Add("@TAN_No", !string.IsNullOrWhiteSpace(answer34.TAN_No) ? answer34.TAN_No : (object?)null, DbType.String);
                parameters.Add("@SAC_Code", answer34.SAC_Code ?? (object)DBNull.Value);
                parameters.Add("@Client_Invoice_State", !string.IsNullOrWhiteSpace(answer34.Client_Invoice_State) ? answer34.Client_Invoice_State : (object?)null, DbType.String);
                parameters.Add("@Quess_Invoice_State", !string.IsNullOrWhiteSpace(answer34.Quess_Invoice_State) ? answer34.Quess_Invoice_State : (object?)null, DbType.String);
                parameters.Add("@SEZ_Certificate_path", !string.IsNullOrWhiteSpace(filePathSEZ) ? filePathSEZ : (object?)null, DbType.String);
                parameters.Add("@LUT_No",
    !string.IsNullOrWhiteSpace(answer34.LUT_No) && answer34.LUT_No != "34"
    ? answer34.LUT_No
    : (object?)null, DbType.String);

                parameters.Add("@LUT_From_Date",
    DateTime.TryParse(answer34.LUT_From_Date, out var parsedFrom) && parsedFrom != new DateTime(1900, 1, 1)
        ? parsedFrom
        : (object?)null,
    DbType.Date);

                parameters.Add("@LUT_End_Date",
    DateTime.TryParse(answer34.LUT_End_Date, out var parsedTo) && parsedTo != new DateTime(1900, 1, 1)
        ? parsedTo
        : (object?)null,
    DbType.Date);


                parameters.Add("@LUT_From_Date", DateTime.TryParse(answer34.LUT_From_Date, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@LUT_End_Date", DateTime.TryParse(answer34.LUT_End_Date, out var parsedDate1) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@LUT_Certificate_Path", !string.IsNullOrWhiteSpace(filePathLUT) ? filePathLUT : (object?)null, DbType.String);
                parameters.Add("@SUB_Code", answer34.SUB_Code ?? (object)DBNull.Value);
                parameters.Add("@CreatedBy", answer34.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Country>> GetCountry()
        {
            var CountryDetails = new List<Country>();
            string storeProcedure = "SP_Get_Country";
            var parameters = new DynamicParameters();


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    CountryDetails = JsonConvert.DeserializeObject<List<Country>>(res)
                                                     ?? new List<Country>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    CountryDetails = new List<Country>();
                }
            }

            return CountryDetails;
        }

        public async Task<List<Currency>> GetCurrency()
        {
            var CurrencyDetails = new List<Currency>();
            string storeProcedure = "SP_Get_Currency";
            var parameters = new DynamicParameters();


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    CurrencyDetails = JsonConvert.DeserializeObject<List<Currency>>(res)
                                                     ?? new List<Currency>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    CurrencyDetails = new List<Currency>();
                }
            }

            return CurrencyDetails;
        }

        public async Task<List<Answer40>> GetSOPAnswer40(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer40>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Vendor_Master_40";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer40>>(res)
                                                      ?? new List<Answer40>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer40>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer40(Answer40 answer40)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer40 != null)
            {
                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Vendor_Master_40";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer40.QuestionId);
                parameters.Add("@Company_Id", answer40.Company_Id);
                parameters.Add("@VendorCode", !string.IsNullOrWhiteSpace(answer40.VendorCode) ? answer40.VendorCode : (object?)null, DbType.String);
                parameters.Add("@VendorName", !string.IsNullOrWhiteSpace(answer40.VendorName) ? answer40.VendorName : (object?)null, DbType.String);
                parameters.Add("@CountryCode", answer40.CountryCode ?? (object)DBNull.Value);
                parameters.Add("@CountryName", !string.IsNullOrWhiteSpace(answer40.CountryName) ? answer40.CountryName : (object?)null, DbType.String);
                parameters.Add("@CityId", answer40.CityId ?? (object)DBNull.Value);
                parameters.Add("@CityName", !string.IsNullOrWhiteSpace(answer40.CityName) ? answer40.CityName : (object?)null, DbType.String);
                parameters.Add("@RegionId", answer40.RegionId ?? (object)DBNull.Value);
                parameters.Add("@RegionName", !string.IsNullOrWhiteSpace(answer40.RegionName) ? answer40.RegionName : (object?)null, DbType.String);
                parameters.Add("@GSTIN", !string.IsNullOrWhiteSpace(answer40.GSTIN) ? answer40.GSTIN : (object?)null, DbType.String);
                parameters.Add("@MSMENumber", !string.IsNullOrWhiteSpace(answer40.MSMENumber) ? answer40.MSMENumber : (object?)null, DbType.String);
                parameters.Add("@PANNumber", !string.IsNullOrWhiteSpace(answer40.PANNumber) ? answer40.PANNumber : (object?)null, DbType.String);
                parameters.Add("@PurchaseOrderCurrency", !string.IsNullOrWhiteSpace(answer40.PurchaseOrderCurrency) ? answer40.PurchaseOrderCurrency : (object?)null, DbType.String);
                parameters.Add("@VendorStatus", answer40.VendorStatus ?? (object)DBNull.Value);
                parameters.Add("@VendorCreationDate", DateTime.TryParse(answer40.VendorCreationDate, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@VendorAddress", !string.IsNullOrWhiteSpace(answer40.VendorAddress) ? answer40.VendorAddress : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer40.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer41>> GetSOPAnswer41(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer41>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_PIN_41";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer41>>(res)
                                                      ?? new List<Answer41>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer41>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswer41(Answer41 answer41)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer41 != null)
            {
                string storeProcedure = "SP_Insert_tbl_Customer_SOP_PIN_41";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer41.QuestionId);
                parameters.Add("@Company_Id", answer41.Company_Id);
                parameters.Add("@MasterChecklist", !string.IsNullOrWhiteSpace(answer41.MasterChecklist) ? answer41.MasterChecklist : (object?)null, DbType.String);
                parameters.Add("@SpocDetails", !string.IsNullOrWhiteSpace(answer41.SpocDetails) ? answer41.SpocDetails : (object?)null, DbType.String);
                parameters.Add("@CompletionActivity", DateTime.TryParse(answer41.CompletionActivity, out var parsedDate) ? parsedDate : (object?)null, DbType.Date);
                parameters.Add("@CreatedBy", answer41.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer42_1>> GetSOPAnswerCompliance42(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer42_1>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Compliance_42";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer42_1>>(res)
                                                      ?? new List<Answer42_1>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer42_1>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswerCompliance42(Answer42_1 answer42_1)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer42_1 != null)
            {
                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Compliance_42";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer42_1.QuestionId);
                parameters.Add("@Company_Id", answer42_1.Company_Id);
                parameters.Add("@IndustryType", !string.IsNullOrWhiteSpace(answer42_1.IndustryType) ? answer42_1.IndustryType : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer42_1.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer42_2>> GetSOPAnswerMinimumwages42(int QuestionId,int CompanyId ,string Createdby)
        {
            var AnswerDetails = new List<Answer42_2>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Minimumwages_42";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer42_2>>(res)
                                                      ?? new List<Answer42_2>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer42_2>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswerMinimumwages42(Answer42_2 answer42_2)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer42_2 != null)
            {
                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Minimumwages_42";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer42_2.QuestionId);
                parameters.Add("@Company_Id", answer42_2.Company_Id);
                parameters.Add("@Category", !string.IsNullOrWhiteSpace(answer42_2.Category) ? answer42_2.Category : (object?)null, DbType.String);
                parameters.Add("@StateId", answer42_2.StateId ?? (object)DBNull.Value);
                parameters.Add("@StateName", !string.IsNullOrWhiteSpace(answer42_2.StateName) ? answer42_2.StateName : (object?)null, DbType.String);
                parameters.Add("@Structure", !string.IsNullOrWhiteSpace(answer42_2.Structure) ? answer42_2.Structure : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer42_2.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer42_3>> GetSOPAnswerDesignation42(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer42_3>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_Designation_42";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CompanyId", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer42_3>>(res)
                                                      ?? new List<Answer42_3>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer42_3>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswerDesignation42(Answer42_3 answer42_3)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer42_3 != null)
            {
                string storeProcedure = "SP_Insert_tbl_Customer_SOP_Designation_42";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer42_3.QuestionId);
                parameters.Add("@CompanyId", answer42_3.CompanyId);
                parameters.Add("@Designationid", answer42_3.Designationid ?? (object)DBNull.Value);
                parameters.Add("@DesignationName", !string.IsNullOrWhiteSpace(answer42_3.DesignationName) ? answer42_3.DesignationName : (object?)null, DbType.String);
                parameters.Add("@SkilledCategoryId", answer42_3.SkilledCategoryId ?? (object)DBNull.Value);
                parameters.Add("@SkilledCategoryName", !string.IsNullOrWhiteSpace(answer42_3.SkilledCategoryName) ? answer42_3.SkilledCategoryName : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer42_3.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<List<Answer42_4>> GetSOPAnswerCLRA42(int QuestionId,int CompanyId, string Createdby)
        {
            var AnswerDetails = new List<Answer42_4>();
            string storeProcedure = "SP_GET_tbl_Customer_SOP_CLRA_42";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@Company_Id", CompanyId);
            parameters.Add("@CreatedBy", Createdby);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    AnswerDetails = JsonConvert.DeserializeObject<List<Answer42_4>>(res)
                                                      ?? new List<Answer42_4>();
                }
                catch (System.Text.Json.JsonException ex)
                {
                    // Log the error if needed
                    AnswerDetails = new List<Answer42_4>();
                }
            }

            return AnswerDetails;
        }

        public async Task<AnswerResponse> PostSOPAnswerCLRA42(Answer42_4 answer42_4)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            if (answer42_4 != null)
            {
                string storeProcedure = "SP_Insert_tbl_Customer_SOP_CLRA_42";
                var parameters = new DynamicParameters();
                parameters.Add("@QuestionId", answer42_4.QuestionId);
                parameters.Add("@Company_Id", answer42_4.Company_Id);
                parameters.Add("@StateId", answer42_4.StateId ?? (object)DBNull.Value);
                parameters.Add("@StateName", !string.IsNullOrWhiteSpace(answer42_4.StateName) ? answer42_4.StateName : (object?)null, DbType.String);
                parameters.Add("@CityId", answer42_4.CityId ?? (object)DBNull.Value);
                parameters.Add("@CityName", !string.IsNullOrWhiteSpace(answer42_4.CityName) ? answer42_4.CityName : (object?)null, DbType.String);
                parameters.Add("@HC", !string.IsNullOrWhiteSpace(answer42_4.HC) ? answer42_4.HC : (object?)null, DbType.String);
                parameters.Add("@CreatedBy", answer42_4.CreatedBy);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
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

        public async Task<AnswerResponse> DeleteSOPAnswer31(int QuestionId, string Createdby)
        {
            AnswerResponse AnswerDetails = new AnswerResponse();

            string storeProcedure = "SP_Delete_tbl_Customer_SOP_Answer_Details_31";
            var parameters = new DynamicParameters();
            parameters.Add("@QuestionId", QuestionId);
            parameters.Add("@CreatedBy", Createdby);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (!string.IsNullOrWhiteSpace(res))
            {
                AnswerDetails.response = "Success";
            }
            return AnswerDetails;
        }
    }
}
