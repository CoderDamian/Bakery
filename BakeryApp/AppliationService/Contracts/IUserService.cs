using DataTransferObjects.DTOs.Token;
using DataTransferObjects.DTOs.User;
using DomainModel;

namespace AppliationService.Contracts
{
    public interface IUserService
    {
        Task<TokenDTO?> ValidateCredentials(LoginUserDTO userDTO);
    }
}
