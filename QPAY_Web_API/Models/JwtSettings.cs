namespace QPay.API.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int AccessTokenExpiryMinutes { get; set; }
        public int RefreshTokenExpiryDays { get; set; }
    }
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public int? UserId { get; set; } 
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }

        public string ActionType { get; set; } = string.Empty;
    }
    public class TokenRequest
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

}
