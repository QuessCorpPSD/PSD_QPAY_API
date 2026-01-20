using Dapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.DAL.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QPay.API.Extensions
{
    public interface IJwtTokenService
    {
        public string GenerateAccessToken(ClaimsIdentity identity);
        public string GenerateRefreshToken();
        Task<RefreshToken> GetRefreshToken(RefreshToken refreshToken);

    }
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly DbRepository _dbRepository;
        public JwtTokenService(JwtSettings jwtSettings, DbRepository dbRepository)
        {
            _jwtSettings = jwtSettings;
            this._dbRepository = dbRepository;
        }

        public string GenerateAccessToken(ClaimsIdentity identity)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: identity.Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {

            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        }

        public async Task<RefreshToken> GetRefreshToken(RefreshToken refreshToken)
        {
            RefreshToken refresh = new RefreshToken();
            string procedure = "SP_JWT_Token_refreshToken" ?? "";
            var parameters = new DynamicParameters();
            parameters.Add("@Token", refreshToken.Token);             
            parameters.Add("@userId", refreshToken.UserId);
            parameters.Add("@ExpiryDate", refreshToken.ExpiryDate);
            //if (refreshToken.ExpiryDate != null)
            //{

            //}
            parameters.Add("@ActionType", refreshToken.ActionType);
            var res =await this._dbRepository.GetItemsAsync(procedure, parameters);
            if (res != "")
            {
                refresh = JsonConvert.DeserializeObject<List<RefreshToken>>(res).FirstOrDefault() ?? new RefreshToken();
            }
            return refresh;
        }


    }
}
