using DomainModel;
using System.Security.Claims;

namespace AppliationService.Contracts
{
    public interface ITokenService
    {
        Token GetNewToken();
        string GetValue();
        string GetRefresh(); 
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        DateTime GetExpiryTime();
    }
}
