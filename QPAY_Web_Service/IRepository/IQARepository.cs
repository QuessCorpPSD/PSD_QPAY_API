using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models;

namespace QPay.BAL.IRepository
{
    public interface IQARepository
    {
        List<CustomerSOPQuestion> GetCustomerSOPQuestionAnswer();
        CompanyMaster GetCompanyCode(int user_id);
        List<StateMaster> GetState();
        List<CityMaster> GetCity(int state_id);
        List<DesignationMaster> GetDesignation(string company_code);
        FirstMonthPayroll GetFirstMonthPayroll(string company_code);
        List<Category> GetCategory();
        List<Question> GetQuestion(int categoryId);
        Answer1 GetSOPAnswer1(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer1(Answer1 answer1);
        Answer2 GetSOPAnswer2(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer2(Answer2 answer2);
        Answer3 GetSOPAnswer3(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer3(Answer3 answer2);
        Answer6 GetSOPAnswer6(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer6(Answer6 answer6);
        Answer8 GetSOPAnswer8(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer8(Answer8 answer8);
        Answer9 GetSOPAnswer9(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer9(Answer9 answer9);
        Answer10 GetSOPAnswer10(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer10(Answer10 answer10);
        Answer5 GetSOPAnswer5(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer5(Answer5 answer5);
        Answer7 GetSOPAnswer7(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer7(Answer7 answer7);
        Answer13 GetSOPAnswer13(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer13(Answer13 answer13);
        Answer14 GetSOPAnswer14(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer14(Answer14 answer14);
        Answer16 GetSOPAnswer16(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer16(Answer16 answer16);
        Answer17 GetSOPAnswer17(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer17(Answer17 answer17);
        Answer18 GetSOPAnswer18(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer18(Answer18 answer18);
        Answer19 GetSOPAnswer19(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer19(Answer19 answer19);
        Answer21 GetSOPAnswer21(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer21(Answer21 answer21);
        Answer23 GetSOPAnswer23(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer23(Answer23 answer23);
        Answer25 GetSOPAnswer25(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer25(Answer25 answer25);
        Answer28 GetSOPAnswer28(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer28(Answer28 answer28);
        Answer29 GetSOPAnswer29(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer29(Answer29 answer29);
        Answer30 GetSOPAnswer30(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer30(Answer30 answer30);
        Answer32 GetSOPAnswer32(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer32(Answer32 answer32);
        Answer36 GetSOPAnswer36(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer36(Answer36 answer36);
        Answer37 GetSOPAnswer37(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer37(Answer37 answer37);
        Answer38 GetSOPAnswer38(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer38(Answer38 answer38);
        Answer12 GetSOPAnswer12(int QuestionId, string Createdby);
        AnswerResponse PostSOPAnswer12(List<Answer12> answer12);
        Answer27 GetSOPAnswer27(int QuestionId, string Createdby);
AnswerResponse PostSOPAnswer27(Answer27 answer27);
Answer39 GetSOPAnswer39(int QuestionId, string Createdby);
AnswerResponse PostSOPAnswer39(Answer39 answer39);
List<Marked_Category> GetmarkedQuestion(string Createdby);
Answer4RequestGet GetSOPAnswer4(int QuestionId, string Createdby);
AnswerResponse PostSOPAnswer4(Answer4Request answer4);
Answer33RequestGet GetSOPAnswer33(int QuestionId, string Createdby);
AnswerResponse PostSOPAnswer33(Answer33Request answer33);
AnswerResponse PostSOPAnswer31(IFormFile file, [FromForm] string billApplicable, 
    [FromForm] int QuestionId, [FromForm] string CreatedBy);
AnswerResponse PostSOPAnswer31_1([FromForm] string billApplicable,
   [FromForm] int QuestionId, [FromForm] string CreatedBy);
Answer31 GetSOPAnswer31(int QuestionId, string Createdby);
Answer11RequestGet GetSOPAnswer11(int QuestionId, string Createdby);
AnswerResponse PostSOPAnswer11(Answer11Request answer11);
Answer15RequestGet GetSOPAnswer15(int QuestionId, string Createdby);
AnswerResponse PostSOPAnswer15(Answer15Request answer15);
AnswerResponse PostSOPAnswer20(Answer20 answer20);
List<Answer20> GetSOPAnswer20(int QuestionId, string Createdby);
AnswerResponse PostSOPAnswer22(Answer22 answer22);
List<Answer22> GetSOPAnswer22(int QuestionId, string Createdby);

    }
}
