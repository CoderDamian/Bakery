using DomainModel;
using System.Security.Claims;

namespace AppliationService.Contracts
{
    public interface ITokenService
    {
        Token GetNewToken(string userName);
        Task<Token> GetNewRefreshToken(string accessTokenValue, string refreshTokenValue);
    }
}
