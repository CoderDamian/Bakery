using AutoMapper;
using Model;
using Model.DTOs;

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
