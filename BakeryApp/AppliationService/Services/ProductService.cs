using AppliationService.Contracts;
using AutoMapper;
using DataPersistence.Contracts;
using DataTransferObjects.DTOs.Product;
using DomainModel;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace AppliationService.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public ProductService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IDistributedCache cache)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<ListProductsDTO> GetAllAsync()
        {
            string? serializedData = null;
            byte[]? dataAsByteArray = await _cache.GetAsync("allProducts");
            IEnumerable<Product>? products;

            if ((dataAsByteArray?.Count() ?? 0) > 0)
            {
                serializedData = Encoding.UTF8.GetString(dataAsByteArray);

                products = JsonSerializer.Deserialize<IEnumerable<Product>>(serializedData);
            }
            else
            {
                products = await _repositoryWrapper.ProductRepository.GetAllProducts().ConfigureAwait(false);

                serializedData = JsonSerializer.Serialize<IEnumerable<Product>>(products);
                dataAsByteArray = Encoding.UTF8.GetBytes(serializedData);
                await _cache.SetAsync("allProducts", dataAsByteArray).ConfigureAwait(false);
            }

            if (products.Any())
            {
                Product FeaturedProduct = products.ElementAt(new Random().Next(products.Count()));

                ListProductsDTO productsDTO = new()
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