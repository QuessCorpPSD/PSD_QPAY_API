using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.BAL.IRepository;
using QPAY_Web_API.Models;


namespace QPAY_Web_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthendicateController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IAssignmentRepository _assignment;

        public AuthendicateController(ILoginRepository loginRepository, IAssignmentRepository assignment)
        {
            this._loginRepository=loginRepository;
            this._assignment=assignment;
        }


        [HttpPost,Route("UserLogin")]
        public async Task<IActionResult> UserLogin(LoginDetailsModel loginDetailsModel)
        {
            var res = this._loginRepository.UserLogin(loginDetailsModel.username, loginDetailsModel.password, "::1", "Selvaraj");
            if(res!=null)
            {
                if(res.User_Id>0)
                {
                    this._assignment.AutoAllocationLots(res.User_Id);
                }
            }
            return Ok(res);
        }

        [HttpGet,Route("GetData")]
        public IActionResult GetData()
        {
            var res = this._loginRepository.GetCompanies();
            return Ok(res);
        }
       
       
        public string GetsheetName(int i)
        {

            switch (i)
            {
                case 0: return "Pay Register";
                //case 1: return "UnProcessed List";
                default: return "UnProcessed List";


            }

        }
        //[HttpPost,Route("ValidateLogin")]
        //public async Task<IActionResult> ValidateLogin(LoginDetailsModel loginDetails)
        //{
        //   // var result = await _apiService.MakeAuthenticatedRequest(loginDetails);
        //    APIResponses responses = new APIResponses();
        //    responses.StatusCode=200;
        //    responses.Message="successfully logged In...";
        //    responses.Data=loginDetails;
        //    return Ok(responses);
        //}
    }
}
