using DataTransferObjects.DTOs.User;
using DomainModel;

namespace AppliationService.Contracts
{
    public interface IUserService
    {
        Task<Token?> ValidateCredentials(LoginUserDTO userDTO);
    }
}
