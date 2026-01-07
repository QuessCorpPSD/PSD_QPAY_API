using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.Repository;
using QPay.UI.Admin;
using QPay.UI.Common;
using QPay.UI.Models;
using QPAY_Web_API.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;


namespace QPAY_Web_API.Controller
{
   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
   
    public class AuthendicateController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IAssignmentRepository _assignment;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;

        public AuthendicateController(ILoginRepository loginRepository, IAssignmentRepository assignment, IConfiguration configuration, IJwtTokenService jwtTokenService,IEmailService emailService)
        {
            this._loginRepository = loginRepository;
            this._assignment = assignment;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
        }

        [HttpPost,Route("UserCreate")]
        public async Task<ActionResult> UserCreate(QPay.UI.Models.Users users)
        {

            var status=await this._loginRepository.UserCreate(users);
            return Ok(status);

        }
        [HttpGet,Route("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId) =>
        Ok((await _loginRepository.GetAllActiveUsers())
         .Where(u => u.User_Id == userId)
         .FirstOrDefault());

        [HttpGet, Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser() =>
        Ok((await _loginRepository.GetAllActiveUsers()));


        public async Task<EmailSensStatus> OTPSend(string Name,string emailId)
        {
            int otp = RandomNumberGenerator.GetInt32(100000, 1000000);
            string safeUserName = HtmlEncoder.Default.Encode(Name);

            string html = $"""
<!DOCTYPE html>
<html>
<head>
    <title>mail</title>
</head>
<body>
    <h2>Hi {safeUserName}</h2>
    <p>
        <b>{otp}</b> {_configuration["mailContent"]}
    </p>
</body>
</html>
""";

            var mail = await _emailService.SendEmailAsync(emailId, "One-Time Passcode for accessing your PSD Application", html);
            mail.Otp = otp;
            return mail;
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDetailsModel loginDetailsModel)
        {
            if (loginDetailsModel == null ||
                loginDetailsModel.username==0 ||
                string.IsNullOrWhiteSpace(loginDetailsModel.password))
            {
                return BadRequest("Invalid login request.");
            }

                try
                {
                    var user = await _loginRepository.UserLogin(
                        loginDetailsModel.username,
                        loginDetailsModel.password,
                       loginDetailsModel.ipAddress,
                       loginDetailsModel.Cname
                    );

                    
                if (user is { User_Id: > 0 })
                    {

                   // var mail =await OTPSend(user.UserName, user.Mail_Id);

                    //user.otp = mail.Otp;
                    //user.expirytime = 3;
                    var identity = new ClaimsIdentity(new[]
                        {
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role_Id.ToString()),
            new Claim(ClaimTypes.Email, user.Mail_Id ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        });

                        user.token = _jwtTokenService.GenerateAccessToken(identity);
                        user.refreshtoken = _jwtTokenService.GenerateRefreshToken();

                        int userId = user.User_Id ?? 0;

                       

                        var refreshToken = new RefreshToken
                        {
                            Token = user.refreshtoken,
                            UserId = userId,
                            ExpiryDate = DateTime.UtcNow.AddDays(1),
                            ActionType = "I"
                        };

                        var status = _jwtTokenService.GetRefreshToken(refreshToken);

                        return Ok(user);
                    }
                    else
                    {
                    if (user != null)
                    {
                        user.Error_Message ??= "Invalid user credentials or user not found.";
                    }
                    return Ok(user);

                }


               // return Ok(user);
                }
                catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest model)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                Token=model.RefreshToken,
                UserId=model.User_Id
            };
            var storedToken = await _jwtTokenService.GetRefreshToken(refreshToken);

            if (storedToken == null)
                return Unauthorized();

            var user = (await this._loginRepository.GetAllActiveUsers())
                    .FirstOrDefault(u => u.User_Id == storedToken.UserId);
            if (user == null) return Unauthorized();

            var identity = new ClaimsIdentity(new[]
                     {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role_Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Mail_Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                    });

            user.token = _jwtTokenService.GenerateAccessToken(identity);
            user.refreshtoken = _jwtTokenService.GenerateRefreshToken();

            // Invalidate old refresh token
            storedToken.IsRevoked = true;
            RefreshToken refreshTokens = new RefreshToken()
            {
                Token = user.refreshtoken,
                UserId = user.User_Id ?? 0,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                ActionType = "I"
            };
            var status = _jwtTokenService.GetRefreshToken(refreshTokens);

            return Ok(new { accessToken = user.token, refreshToken = user.refreshtoken });
        }


        [HttpGet,Route("GetReporting")]
        public async Task<IActionResult> GetReporting()
        {
            var reprting = await this._loginRepository.GetAllActiveUsers();
            return Ok(reprting);
        }
        [HttpGet,Route("GetAllTeamLead")]
        public async Task<IActionResult> GetAllTeamLead() =>
        Ok((await _loginRepository.GetAllActiveUsers())
         .Where(u => u.Role_Id == int.Parse(_configuration["Roles:TeamLeader"] ?? "0"))
         .ToList());

        [HttpGet("GetAllManager")]
        public async Task<IActionResult> GetAllManager() =>
        Ok((await _loginRepository.GetAllActiveUsers())
         .Where(u => u.Role_Id == int.Parse(_configuration["Roles:Managers"] ?? "0"))
         .ToList());

        [HttpGet,Route("GetAllFunctionalityHead")]
        public async Task<IActionResult> GetAllFunctionalityHead()
        {
            var roleIds = _configuration.GetSection("Roles:FunHead").Get<List<int>>() ?? new List<int>();
            var users=await _loginRepository.GetAllActiveUsers();

            var fun = users.Where(u => roleIds.Contains(u.User_Id??0));
            return Ok(fun);
        }
        [HttpGet, Route("GetAllActiveUsers")]
        public async Task<IActionResult> GetAllActiveUsers(int employeeId) =>
       Ok((await _loginRepository.GetAllActiveUsers()));

        [HttpGet, Route("UserByEmployeeId/{employeeId}")]
        public async Task<IActionResult> UserByEmployeeId(int employeeId) =>
       Ok((await _loginRepository.GetAllActiveUsers())
        .Where(u => u.EmployeeID == employeeId)
        .ToList());

        [HttpPost,Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var status=await this._loginRepository.ChangePasswordAsync(changePassword);
            return Ok(status);
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
