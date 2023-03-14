using DomainModel;
using System.Security.Claims;

namespace AppliationService.Contracts
{
    public interface ITokenService
    {
        Token GetNewToken(IEnumerable<Claim> claims);
        Task<Token> GetNewRefreshToken(string accessTokenValue, string refreshTokenValue);
    }
}
