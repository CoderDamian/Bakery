using AutoMapper;
using DomainModel;
using DataTransferObjects.DTOs.Product;

namespace AppliationService.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ListProductsDTO>();

            CreateMap<Product, ProductDTO>();

            // .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Date.Date))

        }
    }
}
