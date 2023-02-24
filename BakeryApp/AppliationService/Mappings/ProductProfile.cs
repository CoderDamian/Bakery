using AutoMapper;
using DomainModel;
using DataTransferObjects.DTOs;

namespace AppliationService.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ListProductsDTO>();

            CreateMap<Product, ProductDTO>();
        }
    }
}
