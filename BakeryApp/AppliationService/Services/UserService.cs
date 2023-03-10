using AppliationService.Contracts;
using AutoMapper;
using DataPersistence.Contracts;
using DataTransferObjects.DTOs.User;
using DomainModel;
using Microsoft.Extensions.Configuration;

namespace AppliationService.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, IRepositoryWrapper repository, ITokenService tokenService, IConfiguration configuration)
        {
            this._mapper = mapper;
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._tokenService = tokenService;
            this._configuration = configuration;
        }

        public async Task<Token?> ValidateCredentials(LoginUserDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);

            bool userExists = await _repository.UserRepository.ValidateCredentials(user).ConfigureAwait(false);

            if (userExists)
            {
                string? issuer = _configuration["JWT:Issuer"];
                string? audience = _configuration["JWT:Audience"];
                string? key = _configuration["JWT:SecretKey"];

                Token token =  _tokenService.GenerateToken(issuer, audience, key);

                return token;
            }
            else
            {
                return null;
            }
        }
    }
}
