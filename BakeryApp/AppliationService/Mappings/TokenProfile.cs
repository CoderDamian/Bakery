using AutoMapper;
using DataTransferObjects.DTOs.Token;
using DomainModel;

namespace AppliationService.Mappings
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<Token, TokenDTO>()
                .ForMember(dest => dest.AccessTokenValue, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.RefreshTokenValue, opt => opt.MapFrom(src => src.Refresh))
                .ReverseMap();

            // .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Date.Date))
            //.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            //   .ForMember(dest => dest.Refresh, opt => opt.MapFrom(src => src.Refresh))
        }
    }
}
