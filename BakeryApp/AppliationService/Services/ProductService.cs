using AppliationService.Contracts;
using AutoMapper;
using DataPersistence.Contracts;
using DataTransferObjects.DTOs;
using DomainModel;

namespace AppliationService.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public ProductService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<ListProductsDTO> GetAllAsync()
        {
            IEnumerable<Product> products = await _repositoryWrapper.ProductRepository.GetAllProducts().ConfigureAwait(false);

            if (products.Any())
            {
                Product FeaturedProduct = products.ElementAt(new Random().Next(products.Count()));

                ListProductsDTO productsDTO = new ListProductsDTO()
                {
                    Id = FeaturedProduct.ID,
                    Name = FeaturedProduct.Name,
                    Description = FeaturedProduct.Description,
                    Price = FeaturedProduct.Price,
                    ImageName = FeaturedProduct.ImageName
                };

                productsDTO.Products = _mapper.Map<IEnumerable<ProductDTO>>(products);

                return productsDTO;
            }
            else
            {
                throw new NullReferenceException(nameof(products));
            }
        }

        public async Task<ProductDTO> GetByIDAsync(int id)
        {
            Product? product = await _repositoryWrapper.ProductRepository.GetProductByID(id).ConfigureAwait(false);

            if (product == null)
                throw new NullReferenceException(nameof(product));

            ProductDTO productDTO = _mapper.Map<ProductDTO>(product);

            return productDTO;
        }
    }
}