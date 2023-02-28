using DomainModel;

namespace AppliationService.Contracts
{
    public interface ITokenService
    {
        Token Authenticate(User user);
    }
}
