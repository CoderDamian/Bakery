using AppliationService.Contracts;
using AutoMapper;
using DataPersistence.Contracts;
using DataTransferObjects.DTOs.User;
using DomainModel;

namespace AppliationService.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public UserService(IMapper mapper, IRepositoryWrapper repository)
        {
            this._mapper = mapper;
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<bool> ValidateUserExistence(LoginUserDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);

            return await _repository.UserRepository.ValidateCredentials(user).ConfigureAwait(false);
        }
    }
}
