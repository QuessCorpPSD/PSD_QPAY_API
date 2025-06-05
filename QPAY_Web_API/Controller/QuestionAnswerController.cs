using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;
using QPay.UI.Models;
using QPAY_Web_API.Models;

namespace QPay.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionAnswerController : ControllerBase
    {
        private readonly IQARepository _qaRepository;
        public QuestionAnswerController(IQARepository qaRepository)
        {
            this._qaRepository = qaRepository;
        }


        [HttpGet, Route("GetCustomerSOPQuestionAnswer")]
        public async Task<IActionResult> GetCustomerSOPQuestionAnswer()
        {
            var res = this._qaRepository.GetCustomerSOPQuestionAnswer();
            return Ok(res);
        }

        [HttpGet, Route("GetCompanyCode")]
        public async Task<IActionResult> GetCompanyCode(int user_id)
        {
            var res = this._qaRepository.GetCompanyCode(user_id);
            return Ok(res);
        }

        [HttpGet, Route("GetState")]
        public async Task<IActionResult> GetState()
        {
            var res = this._qaRepository.GetState();
            return Ok(res);
        }

        [HttpGet, Route("GetCity")]
        public async Task<IActionResult> GetCity(int state_id)
        {
            var res = this._qaRepository.GetCity(state_id);
            return Ok(res);
        }

        [HttpGet, Route("GetDesignation/{company_code}")]
        public async Task<IActionResult> GetDesignation(string company_code)
        {
            var res = this._qaRepository.GetDesignation(company_code);
            return Ok(res);
        }

        [HttpGet, Route("GetFirstMonthPayroll")]
        public async Task<IActionResult> GetFirstMonthPayroll(string company_code)
        {
            var res = this._qaRepository.GetFirstMonthPayroll(company_code);
            return Ok(res);
        }

        [HttpGet, Route("GetCategory")]
        public async Task<IActionResult> GetCategory()
        {
            var res = this._qaRepository.GetCategory();
            return Ok(res);
        }

        [HttpGet, Route("GetQuestion/{categoryId}")]
        public async Task<IActionResult> GetQuestion(int categoryId)
        {
            var res = this._qaRepository.GetQuestion(categoryId);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer1/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer1(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer1(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer1")]
        public async Task<IActionResult> PostSOPAnswer1(Answer1 answer1)
        {
            var res = this._qaRepository.PostSOPAnswer1(answer1);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer2/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer2(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer2(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer2")]
        public async Task<IActionResult> PostSOPAnswer2(Answer2 answer2)
        {
            var res = this._qaRepository.PostSOPAnswer2(answer2);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer3/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer3(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer3(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer3")]
        public async Task<IActionResult> PostSOPAnswer3(Answer3 answer3)
        {
            var res = this._qaRepository.PostSOPAnswer3(answer3);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer6/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer6(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer6(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer6")]
        public async Task<IActionResult> PostSOPAnswer6(Answer6 answer6)
        {
            var res = this._qaRepository.PostSOPAnswer6(answer6);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer8/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer8(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer8(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer8")]
        public async Task<IActionResult> PostSOPAnswer8(Answer8 answer8)
        {
            var res = this._qaRepository.PostSOPAnswer8(answer8);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer9/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer9(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer9(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer9")]
        public async Task<IActionResult> PostSOPAnswer9(Answer9 answer9)
        {
            var res = this._qaRepository.PostSOPAnswer9(answer9);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer10/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer10(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer10(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer10")]
        public async Task<IActionResult> PostSOPAnswer10(Answer10 answer10)
        {
            var res = this._qaRepository.PostSOPAnswer10(answer10);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer5/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer5(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer5(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer5")]
        public async Task<IActionResult> PostSOPAnswer5(Answer5 answer5)
        {
            var res = this._qaRepository.PostSOPAnswer5(answer5);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer7/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer7(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer7(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer7")]
        public async Task<IActionResult> PostSOPAnswer7(Answer7 answer7)
        {
            var res = this._qaRepository.PostSOPAnswer7(answer7);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer13/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer13(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer13(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer13")]
        public async Task<IActionResult> PostSOPAnswer13(Answer13 answer13)
        {
            var res = this._qaRepository.PostSOPAnswer13(answer13);
            return Ok(res);
        }


        [HttpGet, Route("GetSOPAnswer14/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer14(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer14(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer14")]
        public async Task<IActionResult> PostSOPAnswer14(Answer14 answer14)
        {
            var res = this._qaRepository.PostSOPAnswer14(answer14);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer16/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer16(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer16(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer16")]
        public async Task<IActionResult> PostSOPAnswer16(Answer16 answer16)
        {
            var res = this._qaRepository.PostSOPAnswer16(answer16);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer17/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer17(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer17(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer17")]
        public async Task<IActionResult> PostSOPAnswer17(Answer17 answer17)
        {
            var res = this._qaRepository.PostSOPAnswer17(answer17);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer18/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer18(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer18(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer18")]
        public async Task<IActionResult> PostSOPAnswer18(Answer18 answer18)
        {
            var res = this._qaRepository.PostSOPAnswer18(answer18);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer19/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer19(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer19(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer19")]
        public async Task<IActionResult> PostSOPAnswer19(Answer19 answer19)
        {
            var res = this._qaRepository.PostSOPAnswer19(answer19);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer21/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer21(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer21(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer21")]
        public async Task<IActionResult> PostSOPAnswer21(Answer21 answer21)
        {
            var res = this._qaRepository.PostSOPAnswer21(answer21);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer23/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer23(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer23(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer23")]
        public async Task<IActionResult> PostSOPAnswer23(Answer23 answer23)
        {
            var res = this._qaRepository.PostSOPAnswer23(answer23);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer25/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer25(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer25(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer25")]
        public async Task<IActionResult> PostSOPAnswer25(Answer25 answer25)
        {
            var res = this._qaRepository.PostSOPAnswer25(answer25);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer28/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer28(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer28(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer28")]
        public async Task<IActionResult> PostSOPAnswer28(Answer28 answer28)
        {
            var res = this._qaRepository.PostSOPAnswer28(answer28);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer29/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer29(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer29(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer29")]
        public async Task<IActionResult> PostSOPAnswer29(Answer29 answer29)
        {
            var res = this._qaRepository.PostSOPAnswer29(answer29);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer30/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer30(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer30(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer30")]
        public async Task<IActionResult> PostSOPAnswer30(Answer30 answer30)
        {
            var res = this._qaRepository.PostSOPAnswer30(answer30);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer32/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer32(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer32(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer32")]
        public async Task<IActionResult> PostSOPAnswer32(Answer32 answer32)
        {
            var res = this._qaRepository.PostSOPAnswer32(answer32);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer36/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer36(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer36(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer36")]
        public async Task<IActionResult> PostSOPAnswer36(Answer36 answer36)
        {
            var res = this._qaRepository.PostSOPAnswer36(answer36);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer37/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer37(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer37(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer37")]
        public async Task<IActionResult> PostSOPAnswer37(Answer37 answer37)
        {
            var res = this._qaRepository.PostSOPAnswer37(answer37);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer38/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer38(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer38(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer38")]
        public async Task<IActionResult> PostSOPAnswer38(Answer38 answer38)
        {
            var res = this._qaRepository.PostSOPAnswer38(answer38);
            return Ok(res);
        }

        [HttpGet, Route("GetSOPAnswer12/{QuestionId}/{Createdby}")]
        public async Task<IActionResult> GetSOPAnswer12(int QuestionId, string Createdby)
        {
            var res = this._qaRepository.GetSOPAnswer12(QuestionId, Createdby);
            return Ok(res);
        }

        [HttpPost, Route("PostSOPAnswer12")]
        public async Task<IActionResult> PostSOPAnswer12([FromBody] List<Answer12> answer12)
        {
            var res = this._qaRepository.PostSOPAnswer12(answer12);
            return Ok(res);
        }

         [HttpGet, Route("GetSOPAnswer27/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer27(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer27(QuestionId, Createdby);
     return Ok(res);
 }

 [HttpPost, Route("PostSOPAnswer27")]
 public async Task<IActionResult> PostSOPAnswer27(Answer27 answer27)
 {
     var res = this._qaRepository.PostSOPAnswer27(answer27);
     return Ok(res);
 }


 [HttpGet, Route("GetSOPAnswer39/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer39(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer39(QuestionId, Createdby);
     return Ok(res);
 }

 [HttpPost, Route("PostSOPAnswer39")]
 public async Task<IActionResult> PostSOPAnswer39(Answer39 answer39)
 {
     var res = this._qaRepository.PostSOPAnswer39(answer39);
     return Ok(res);
 }

 [HttpGet, Route("GetmarkedQuestion/{Createdby}")]
 public async Task<IActionResult> GetmarkedQuestion(string Createdby)
 {
     var res = this._qaRepository.GetmarkedQuestion(Createdby);
     return Ok(res);
 }

 [HttpGet, Route("GetSOPAnswer4/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer4(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer4(QuestionId, Createdby);
     return Ok(res);
 }

 [HttpPost, Route("PostSOPAnswer4")]
 public async Task<IActionResult> PostSOPAnswer4(Answer4Request answer4)
 {
     var res = this._qaRepository.PostSOPAnswer4(answer4);
     return Ok(res);
 }

 [HttpGet, Route("GetSOPAnswer33/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer33(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer33(QuestionId, Createdby);
     return Ok(res);
 }

 [HttpPost, Route("PostSOPAnswer33")]
 public async Task<IActionResult> PostSOPAnswer33(Answer33Request answer33)
 {
     var res = this._qaRepository.PostSOPAnswer33(answer33);
     return Ok(res);
 }

 [HttpPost]
 [Route("PostSOPAnswer31")]
 public async Task<IActionResult> PostSOPAnswer31(IFormFile file, [FromForm] string billApplicable, 
     [FromForm] int QuestionId, [FromForm] string CreatedBy)
 {
     if (file == null || file.Length == 0)
         return BadRequest("File is missing.");

     var res =  _qaRepository.PostSOPAnswer31(file, billApplicable,QuestionId,CreatedBy);
     return Ok(res);
 }

 [HttpPost]
 [Route("PostSOPAnswer31_1")]
 public async Task<IActionResult> PostSOPAnswer31_1([FromForm] string billApplicable,
     [FromForm] int QuestionId, [FromForm] string CreatedBy)
 {
   
     var res = _qaRepository.PostSOPAnswer31_1(billApplicable, QuestionId, CreatedBy);
     return Ok(res);
 }

 [HttpGet, Route("GetSOPAnswer31/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer31(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer31(QuestionId, Createdby);
     return Ok(res);
 }

 [HttpGet, Route("GetSOPAnswer11/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer11(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer11(QuestionId, Createdby);
     return Ok(res);
 }

 [HttpPost, Route("PostSOPAnswer11")]
 public async Task<IActionResult> PostSOPAnswer11(Answer11Request answer11)
 {
     var res = this._qaRepository.PostSOPAnswer11(answer11);
     return Ok(res);
 }

 [HttpGet, Route("GetSOPAnswer15/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer15(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer15(QuestionId, Createdby);
     return Ok(res);
 }

 [HttpPost, Route("PostSOPAnswer15")]
 public async Task<IActionResult> PostSOPAnswer15(Answer15Request answer15)
 {
     var res = this._qaRepository.PostSOPAnswer15(answer15);
     return Ok(res);
 }

 [HttpPost, Route("PostSOPAnswer20")]
 public async Task<IActionResult> PostSOPAnswer20(Answer20 answer20)
 {
     var res = this._qaRepository.PostSOPAnswer20(answer20);
     return Ok(res);
 }


 [HttpGet, Route("GetSOPAnswer20/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer20(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer20(QuestionId, Createdby);
     return Ok(res);
 }

 [HttpPost, Route("PostSOPAnswer22")]
 public async Task<IActionResult> PostSOPAnswer22(Answer22 answer22)
 {
     var res = this._qaRepository.PostSOPAnswer22(answer22);
     return Ok(res);
 }


 [HttpGet, Route("GetSOPAnswer22/{QuestionId}/{Createdby}")]
 public async Task<IActionResult> GetSOPAnswer22(int QuestionId, string Createdby)
 {
     var res = this._qaRepository.GetSOPAnswer22(QuestionId, Createdby);
     return Ok(res);
 }

    }
}
