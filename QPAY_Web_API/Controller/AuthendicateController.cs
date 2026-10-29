using Azure;
using Azure.Core;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.Repository;
using QPay.UI.Admin;
using QPay.UI.Common;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPAY_Web_API.Models;
using StackExchange.Redis;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using static QRCoder.PayloadGenerator;
using static System.Net.WebRequestMethods;


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
        //private readonly IDatabase _db;
        private readonly HttpClient _http;
        private readonly string ACCESS_TOKEN_KEY;
        private readonly string ACCESS_EXPIRY_KEY;
        private readonly string API_KEY_KEY;
        private readonly string API_KEY_EXPIRY_KEY;
        private readonly string LOCK_RESOURCE;
        private readonly ILoggerManager _logger;
        private static readonly TimeSpan ACCESS_SAFETY =
            TimeSpan.FromSeconds(5);

        private static readonly TimeSpan APIKEY_SAFETY =
            TimeSpan.FromMinutes(1);

        public AuthendicateController(ILoginRepository loginRepository, ILoggerManager logger,
            IAssignmentRepository assignment, IConfiguration configuration, IJwtTokenService jwtTokenService,IEmailService emailService,  HttpClient http)
        {
            this._loginRepository = loginRepository;
            this._assignment = assignment;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
            //_db= db;
            this.ACCESS_TOKEN_KEY =configuration["MailOtp:AccessToken:Format"] ?? string.Empty;
            this.ACCESS_EXPIRY_KEY =configuration["MailOtp:AccessTokeExpiry:Format"] ?? string.Empty;
            this.API_KEY_KEY =configuration["MailOtp:ApiKey:Format"] ?? string.Empty;
            this.API_KEY_EXPIRY_KEY =configuration["MailOtp:ApiKey:FormatExpiry"] ?? string.Empty;
            this.LOCK_RESOURCE =configuration["MailOtp:TokenRefreshLock:Format"] ?? string.Empty;
            this._http = http;
            this._logger = logger;

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
        private static DateTime? TryGetDateTime(RedisValue value)
        {
            if (value.IsNullOrEmpty)
                return null;

            return DateTime.TryParse(
                value.ToString(),
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.RoundtripKind,
                out DateTime result)
                    ? result
                    : null;
        }
        private async Task<bool> TryRefreshUsingApiKeyAsync(
     string apiKey,
     CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            try
            {
                var baseUrl = _configuration["MailOtp:BaseUrl"]?.TrimEnd('/');

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                  //  _logger.LogError("MailOtp:BaseUrl configuration is missing.");
                    return false;
                }

                var url = $"{baseUrl}/api/auth/RefreshApiKey";

                var payload = new { apiKey };

                var json = JsonConvert.SerializeObject(payload);

                using var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                using var response = await _http.PostAsync(
                    url,
                    content,
                    cancellationToken);

                if (response.StatusCode is HttpStatusCode.Unauthorized
                    or HttpStatusCode.BadRequest)
                {
                    this._logger.LogInfo(string.Format(
                       "API key refresh rejected with status code {StatusCode}.",
                        (int)response.StatusCode));

                    return false;
                }

                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                var tokenResp =
                    JsonConvert.DeserializeObject<TokenResponse>(body);

                if (tokenResp == null ||
                    string.IsNullOrWhiteSpace(tokenResp.accessToken) ||
                    string.IsNullOrWhiteSpace(tokenResp.apiKey) ||
                    tokenResp.expiresIn <= 0 ||
                    (tokenResp.apiKeyExpiresIn.HasValue &&
                     tokenResp.apiKeyExpiresIn.Value <= 0))
                {
                 //   _logger.LogWarning("API key refresh returned an invalid token response.");

                    return false;
                }

                var now = DateTimeOffset.UtcNow;

                var accessExpiry = now
                    .AddSeconds(tokenResp.expiresIn)
                    .Subtract(ACCESS_SAFETY);

                var apiKeyExpiry = now
                    .AddSeconds(tokenResp.apiKeyExpiresIn ?? 86400)
                    .Subtract(APIKEY_SAFETY);

                // Update all Redis keys atomically.
                //var transaction = _db.CreateTransaction();

                //_ = transaction.StringSetAsync(
                //    ACCESS_TOKEN_KEY,
                //    tokenResp.accessToken);

                //_ = transaction.StringSetAsync(
                //    ACCESS_EXPIRY_KEY,
                //    accessExpiry.ToString("o"));

                //_ = transaction.StringSetAsync(
                //    API_KEY_KEY,
                //    tokenResp.apiKey);

                //_ = transaction.StringSetAsync(
                //    API_KEY_EXPIRY_KEY,
                //    apiKeyExpiry.ToString("o"));

                //var committed = await transaction.ExecuteAsync();

                //if (!committed)
                //{
                //    this._logger.LogInfo(
                //        "Redis transaction failed while saving refreshed tokens.");

                //    return false;
                //}

                return true;
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                // Preserve caller-requested cancellation.
                throw;
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogInfo(                    
                    "HTTP request failed while refreshing the API key.");

                return false;
            }
            catch (OperationCanceledException ex)
            {
                this._logger.LogInfo(
                 
                    "API key refresh timed out.");

                return false;
            }
            catch (RedisException ex)
            {
                this._logger.LogInfo(
                 
                    "Redis error while refreshing tokens.");

                return false;
            }
            catch (JsonException ex)
            {
                this._logger.LogInfo(                  
                    "Failed to deserialize the token refresh response.");

                return false;
            }
        }

        //public async Task<string> GetValidAccessTokenAsync()
        //{
        //    var accessToken = ACCESS_TOKEN_KEY; // _db.StringGet(ACCESS_TOKEN_KEY);
        //    var accessExpiry = TryGetDateTime(ACCESS_EXPIRY_KEY); //TryGetDateTime(_db.StringGet(ACCESS_EXPIRY_KEY));
        //    var apiKey = API_KEY_KEY; //_db.StringGet(API_KEY_KEY);
        //    var apiKeyExpiry = TryGetDateTime(API_KEY_EXPIRY_KEY); ; //TryGetDateTime(_db.StringGet(API_KEY_EXPIRY_KEY));


        //    //bool apiKeyNearExpiry = apiKey.IsNullOrEmpty || apiKeyExpiry == null || apiKeyExpiry <= DateTime.UtcNow.Add(APIKEY_SAFETY);
        //    //bool accessNearExpiry = accessToken.IsNullOrEmpty || accessExpiry == null || accessExpiry <= DateTime.UtcNow.Add(ACCESS_SAFETY);

        //    bool apiKeyNearExpiry = apiKeyExpiry <= DateTime.UtcNow.Add(APIKEY_SAFETY);
        //    bool accessNearExpiry =  accessExpiry <= DateTime.UtcNow.Add(ACCESS_SAFETY);

        //    if (!accessNearExpiry && !apiKeyNearExpiry)
        //    {
        //        return accessToken;
        //    }
        //    try
        //    {




        //        //accessToken = _db.StringGet(ACCESS_TOKEN_KEY);
        //        //accessExpiry = TryGetDateTime(_db.StringGet(ACCESS_EXPIRY_KEY));
        //        //apiKey = _db.StringGet(API_KEY_KEY);
        //        //apiKeyExpiry = TryGetDateTime(_db.StringGet(API_KEY_EXPIRY_KEY));

        //        //apiKeyNearExpiry = apiKey.IsNullOrEmpty || apiKeyExpiry == null || apiKeyExpiry <= DateTime.UtcNow.Add(APIKEY_SAFETY);
        //        //accessNearExpiry = accessToken.IsNullOrEmpty || accessExpiry == null || accessExpiry <= DateTime.UtcNow.Add(ACCESS_SAFETY);

        //        //if (!accessNearExpiry && !apiKeyNearExpiry)
        //        //{
        //        //    return accessToken;
        //        //}

        //        //if (!apiKey.IsNullOrEmpty && !apiKeyNearExpiry)
        //        //{
        //        //    var refreshed = TryRefreshUsingApiKeyAsync(apiKey).Result;
        //        //    if (refreshed)
        //        //    {
        //        //        return _db.StringGet(ACCESS_TOKEN_KEY);
        //        //    }
        //        //}

        //        //var generated = await TryGenerateApiKeyWithCredentialsAsync();
        //        //if (generated)
        //        //{
        //        //    return _db.StringGet(ACCESS_TOKEN_KEY);
        //        //}

        //        //var final = _db.StringGet(ACCESS_TOKEN_KEY);
        //        //if (!final.IsNullOrEmpty)
        //        //    return final;

        //        //throw new Exception("Unable to obtain access token: refresh and re-login failed.");
        //        ////}
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        private async Task<TokenResponse> TryGenerateApiKeyWithCredentialsAsync(
            CancellationToken cancellationToken = default)
        {
            TokenResponse tokenResponse = new TokenResponse();
            try
            {
                var baseUrl = _configuration["MailOtp:BaseUrl"]?.TrimEnd('/');
                var primaryKey = _configuration["MailOtp:PrimaryKey"];
                var secondaryKey = _configuration["MailOtp:SecondaryKey"];
                var appId = _configuration["MailOtp:AppId"];

                if (string.IsNullOrWhiteSpace(baseUrl) ||
                    string.IsNullOrWhiteSpace(primaryKey) ||
                    string.IsNullOrWhiteSpace(secondaryKey) ||
                    string.IsNullOrWhiteSpace(appId))
                {
                    _logger.LogError(
                        "MailOtp configuration is missing or incomplete.");

                    return tokenResponse;
                }

                var payload = new
                {
                    primaryKey,
                    secondaryKey,
                    appId
                };

                var json = JsonConvert.SerializeObject(payload);

                using var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                using var response = await _http.PostAsync(
                    $"{baseUrl}/api/auth/GenerateApiKey",
                    content,
                    cancellationToken);

                if (response.StatusCode is HttpStatusCode.BadRequest
                    or HttpStatusCode.Unauthorized)
                {
                    this._logger.LogInfo(string.Format(
                        "API key generation rejected. Status: {StatusCode}",
                        (int)response.StatusCode));

                    return tokenResponse;
                }

                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                var tokenResp =
                    JsonConvert.DeserializeObject<TokenResponse>(body);

                

                

                return tokenResp?? new TokenResponse();
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogInfo(
                   
                    "HTTP error during API key generation.");

                return tokenResponse;
            }
            catch (RedisException ex)
            {
                this._logger.LogInfo(
                   "Redis error during API key generation.");

                return tokenResponse;
            }
            catch (JsonException ex)
            {
                this._logger.LogInfo(
                   "Invalid JSON received during API key generation.");

                return tokenResponse;
            }
            catch (OperationCanceledException ex)
            {
                this._logger.LogInfo(
                    "API key generation request timed out.");

                return tokenResponse;
            }
            catch (Exception ex)
            {
                this._logger.LogInfo(
                    "Unexpected error during API key generation.");

                return tokenResponse;
            }
        }
        //[HttpPost,Route("VerifyEmailOTPAsync")]
        //public async Task<(bool isValidOTP, string message)> VerifyEmailOTPAsync(MailValidationRequest mailValidationRequest)
        //{
        //    try
        //    {
        //        var accessToken = await GetValidAccessTokenAsync();

        //        var baseUrl = _configuration["MailOtp:BaseUrl"]?.TrimEnd('/');

        //        if (string.IsNullOrWhiteSpace(baseUrl))
        //        {
        //            return (false, "Mail OTP service URL is not configured.");
        //        }

        //        var payload = new
        //        {
        //            Application = "Qpay",
        //            Environment = _configuration["Environment"],
        //            UserId = mailValidationRequest.userId,
        //            Otp = mailValidationRequest.otp
        //        };

        //        using var request = new HttpRequestMessage(
        //            HttpMethod.Post,
        //            $"{baseUrl}/api/OtpManager/ValidateOtp");

        //        request.Headers.Authorization =
        //            new AuthenticationHeaderValue("Bearer", accessToken);

        //        request.Content = new StringContent(
        //            JsonConvert.SerializeObject(payload),
        //            Encoding.UTF8,
        //            "application/json");

        //        using var response = await _http.SendAsync(request);

        //        var responseContent =
        //            await response.Content.ReadAsStringAsync();

        //        var message = "Service is down! Check with admin.";

        //        if (!string.IsNullOrWhiteSpace(responseContent))
        //        {
        //            try
        //            {
        //                var responseObject =
        //                    JsonConvert.DeserializeObject<dynamic>(responseContent);

        //                message = (string?)responseObject?.message ?? message;
        //            }
        //            catch (JsonException)
        //            {
        //                message = responseContent;
        //            }
        //        }

        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            return (true, message);
        //        }

        //        return (false, message);
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return (false, $"HTTP Request Error: {ex.Message}");
        //    }
        //    catch (TaskCanceledException)
        //    {
        //        return (false, "OTP verification request timed out.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return (false, $"General Error: {ex.Message}");
        //    }
        //}
        

        public async Task<string> SendOtp(MailTokenContent mailTokenContent,string payload,CancellationToken cancellationToken = default)
        {
            var baseUrl = _configuration["MailOtp:BaseUrl"]?.TrimEnd('/')??"";
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            
            try
            {
                //_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mailTokenContent.AccessToken);
                //using var response = await _http.PostAsync($"{baseUrl.TrimEnd('/')}/api/OtpManager/SendOtp",
                //                    content, cancellationToken) ;
                //                    

                using var request = new HttpRequestMessage(HttpMethod.Post,
                $"{baseUrl.TrimEnd('/')}/api/OtpManager/SendOtp")
                {
                    Content = content
                };
                request.Headers.Authorization =new AuthenticationHeaderValue("Bearer",mailTokenContent.AccessToken);
                request.Headers.Accept.ParseAdd("application/json");
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(60));
                using var response = await _http.SendAsync(request,HttpCompletionOption.ResponseHeadersRead,
                cts.Token);
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        [Route("VerifyEmailOTPAsync")]
        public async Task<IActionResult> VerifyEmailOTPAsync(UserOTPValidatedRequest userOTPValidatedRequest)
        {
            var baseUrl = _configuration["MailOtp:BaseUrl"]?.TrimEnd('/') ?? "";
            var payload = new
            {
                Application = "Qpay",
                Environment =_configuration["MailOtp:Environment"],
                UserId = userOTPValidatedRequest.userId.ToString(),
                Otp = userOTPValidatedRequest.otp
            };
            var requests = JsonConvert.SerializeObject(payload);
            var content = new StringContent(requests, Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Post,
                $"{baseUrl.TrimEnd('/')}/api/OtpManager/ValidateOtp")
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userOTPValidatedRequest.mailTokenContent.AccessToken);
            request.Headers.Accept.ParseAdd("application/json");
            CancellationToken cancellationToken = new CancellationToken(default);
            using var cts = new CancellationTokenSource(
      TimeSpan.FromSeconds(60));
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
            cts.Token);
            var responseContent = await response.Content.ReadFromJsonAsync<UserOTPValidatedResponse>();
            return Ok(responseContent);

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
                    if (user?.Is_MFA_Enabled == true)
                    {
                        user.Otp_required = true;
                        var statusToken = await TryGenerateApiKeyWithCredentialsAsync();
                        if(statusToken!=null)
                        {
                            MailTokenContent mailTokenContent = new MailTokenContent()
                            {
                                AccessToken = statusToken.accessToken,
                                Apikey = statusToken.apiKey,
                                expiresIn = statusToken.expiresIn
                            };
                            user.mailToken = mailTokenContent;                            
                            var payload = new
                            {
                                Application = "Qpay",
                                Environment = _configuration["MailOtp:Environment"],
                                UserId = user.User_Id.ToString(),
                                UserName = user.UserName,
                                EmailId =user.Mail_Id.ToString(),
                            };
                          var request= JsonConvert.SerializeObject(payload);
                            var Otp_status = SendOtp(mailTokenContent, request);
                        }
                    }

                    var identity = new ClaimsIdentity(new[]
                        {
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.Role_Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Mail_Id ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()??string.Empty)
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
