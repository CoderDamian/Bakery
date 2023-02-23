using AppliationService.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using Persistence;

namespace AppliationService
{
    public class ProductService : IProductService
    {
        private readonly BakeryDbContext _bakeryDbContext;
        private readonly IMapper _mapper;

        public ProductService(BakeryDbContext bakeryDbContext, IMapper mapper)
        {
            this._bakeryDbContext = bakeryDbContext;
            this._mapper = mapper;
        }

        public async Task<ListProductsDTO> GetAllAsync()
        {
            IEnumerable<Product> products = await _bakeryDbContext.Products.ToListAsync().ConfigureAwait(false);

            if (products.Any())
            {
                Product FeaturedProduct = products.ElementAt(new Random().Next(products.Count()));

                ListProductsDTO productsDTO = new ListProductsDTO()
                {
                    ID = FeaturedProduct.ID,
                    Name = FeaturedProduct.Name,
                    Description = FeaturedProduct.Description,
                    Price = FeaturedProduct.Price,
                    ImageName = FeaturedProduct.ImageName
                };

                productsDTO.Products = _mapper.Map<IEnumerable<Product>>(products);

                return productsDTO;
            }
            else
            {
                throw new NullReferenceException(nameof(products));
            }
        }

        public async Task<ProductDTO> GetByIDAsync(int id)
        {
            Product? product = await _bakeryDbContext.Products.FindAsync(id).ConfigureAwait(false);

            if (product == null)
                throw new NullReferenceException(nameof(product));

            ProductDTO productDTO = _mapper.Map<ProductDTO>(product);

            return productDTO;
        }
    }
}