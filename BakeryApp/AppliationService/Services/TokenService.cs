using AppliationService.Contracts;
using DomainModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AppliationService.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenParamenter = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("")),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenParamenter, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            else
            {
                return principal; 
            }
        }

        public Token GetNewToken()
        {
            Token token = new();

            token.Value = GetValue();
            token.Refresh = GetRefresh();
            token.ExpiryTime = GetExpiryTime();

            return token;
        }

        public string GetRefresh()
        {
            byte[] randomNumber = new byte[32];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GetValue()
        {
            string? issuer = _configuration["JWT:Issuer"];
            string? audience = _configuration["JWT:Audience"];
            string? key = _configuration["JWT:SecretKey"];

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var signingCredential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: signingCredential
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        public DateTime GetExpiryTime()
        {
            return DateTime.Now.AddDays(1);
        }
    }
}
