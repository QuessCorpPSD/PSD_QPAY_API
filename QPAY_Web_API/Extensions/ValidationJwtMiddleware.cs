using Microsoft.IdentityModel.Tokens;
using QPAY_Web_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace QPay.API.Extensions
{
    public class ValidationJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret;
        private readonly string[] _whitelistedPaths = ["/api/authendicate/userlogin", "/api/authendicate/refresh"]; // Add your public paths here

        public ValidationJwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _secret = configuration["JwtSettings:SecretKey"]??"";
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            if (_whitelistedPaths.Contains(path))
            {
                await _next(context);
                return;
            }
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_secret);

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                    context.Items["UserId"] = userId;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        success = false,
                        message = "Invalid or expired token.",
                        details = ex.Message // optionally remove for security
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                    return;
                    // Invalid token; nothing to attach
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var apiresponse = new APIResponses((HttpStatusCode)context.Response.StatusCode, "Authorization token is missing", null, null);
               

                
                await context.Response.WriteAsync(JsonSerializer.Serialize(apiresponse));
                return;
            }

                await _next(context);
        }
    }

}

