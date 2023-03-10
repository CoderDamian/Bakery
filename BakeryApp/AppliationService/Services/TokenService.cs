using AppliationService.Contracts;
using DomainModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppliationService.Services
{
    public class TokenService : ITokenService
    {
        public Token GenerateToken(string publisher, string audience, string key)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var signingCredential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: publisher,
                audience: audience,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: signingCredential
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new Token()
            {
                Value = tokenString
            };
        }
    }
}
