using AppliationService.Contracts;
using AutoMapper;
using DataPersistence.Contracts;
using DataTransferObjects.DTOs.User;
using DomainModel;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace AppliationService.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ITokenService _tokenService;

        public UserService(IMapper mapper, IRepositoryWrapper repository, ITokenService tokenService)
        {
            this._mapper = mapper;
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._tokenService = tokenService;
        }

        public async Task<Token?> ValidateCredentials(LoginUserDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);

            int userID = await _repository.UserRepository
                .ValidateCredentials(user.Name, user.Password)
                .ConfigureAwait(false);

            if (userID == -1)
            {
                return null;
            }
            else
            {
                IEnumerable<Claim> claims = GetClaims(user.Name);

                Token token = _tokenService.GetNewToken(claims);

                await _repository.UserRepository
                    .SaveTokenByUserID(userID, token.Refresh, token.ExpiryTime)
                    .ConfigureAwait(false);

                await _repository.Save()
                    .ConfigureAwait(false);

                return token;
            }
        }

        private IEnumerable<Claim> GetClaims(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "Manager")
            };

            return claims;
        }
    }
}
