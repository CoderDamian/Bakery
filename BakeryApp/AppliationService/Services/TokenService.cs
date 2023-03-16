using AppliationService.Contracts;
using DataPersistence.Contracts;
using DomainModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IRepositoryWrapper _repository;

        public TokenService(IConfiguration configuration, IRepositoryWrapper repository)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._repository = repository;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string tokenValue)
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

            var principal = tokenHandler.ValidateToken(tokenValue, tokenParamenter, out securityToken);

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

        public Token GetNewToken(string userName)
        {
            IEnumerable<Claim> claims = GetClaims(userName);

            Token token = new();

            token.Value = GetValue(claims);
            token.Refresh = GetRefresh();
            token.ExpiryTime = GetExpiryTime();

            return token;
        }

        private Token GetNewTokenByRefreshToken(IEnumerable<Claim> claims)
        {
            Token token = new();

            token.Value = GetValue(claims);
            token.Refresh = GetRefresh();

            return token;
        }
        
        private IEnumerable<Claim> GetClaims(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "Manager")
            };

            return claims;
        }

        private string GetRefresh()
        {
            byte[] randomNumber = new byte[32];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        private string GetValue(IEnumerable<Claim> claims)
        {
            string? issuer = _configuration["JWT:Issuer"];
            string? audience = _configuration["JWT:Audience"];
            string? key = _configuration["JWT:SecretKey"];

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var signingCredential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: signingCredential
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        private DateTime GetExpiryTime()
        {
            return DateTime.Now.AddDays(1);
        }

        public async Task<Token> GetNewRefreshToken(string accessTokenValue, string refreshTokenValue)
        {
            try
            {
                ClaimsPrincipal? principal = GetPrincipalFromExpiredToken(accessTokenValue);

                string userName = principal.Identity.Name; // this is mapped to the Name claim by default

                if (String.IsNullOrEmpty(userName))
                    throw new NullReferenceException(nameof(userName));

                User? user = await _repository.UserRepository.GetUserByName(userName);

                if (user is null || user.RefreshToken != refreshTokenValue || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    throw new ArgumentException("invalid client request");
                }

                Token newToken = GetNewTokenByRefreshToken(principal.Claims);

                await _repository.UserRepository.UpdateRefreshTokenByUserID(user.Id, newToken.Refresh);

                return newToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
