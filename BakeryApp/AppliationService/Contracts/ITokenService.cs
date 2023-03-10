using DomainModel;

namespace AppliationService.Contracts
{
    public interface ITokenService
    {
        Token GenerateToken(string issuer, string audience, string key);
    }
}
