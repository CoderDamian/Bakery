using DataTransferObjects.DTOs.User;
using DomainModel;

namespace AppliationService.Contracts
{
    public interface IUserService
    {
        Task<bool> ValidateUserExistence(LoginUserDTO userDTO);
    }
}
