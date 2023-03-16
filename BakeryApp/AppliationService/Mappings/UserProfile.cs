using AutoMapper;
using DataTransferObjects.DTOs.Token;
using DataTransferObjects.DTOs.User;
using DomainModel;

namespace AppliationService.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, LoginUserDTO>()
                .ReverseMap();
        }
    }
}
