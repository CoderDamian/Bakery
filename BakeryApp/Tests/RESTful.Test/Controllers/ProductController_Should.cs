using AppliationService.Contracts;
using DataTransferObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFul.Controllers;

namespace RESTful.Test.Controllers
{
    public class ProductController_Should
    {
        private readonly Mock<IProductService> _mockProductService;

        public ProductController_Should()
        {
            // Create an mock instance.
            _mockProductService = new Mock<IProductService>();

            // Set method.
            _mockProductService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(GetProducts());
        }

        [Fact]
        public async Task be_of_type_ListProductDTO()
        {
            var productController = new ProductController(_mockProductService.Object);

            var result = await productController.GetAll().ConfigureAwait(false);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            var model = Assert.IsAssignableFrom<ListProductsDTO>(objectResult.Value);

            Assert.Single(model.Products);
        }

        private ListProductsDTO GetProducts()
        {
            return new ListProductsDTO()
            {
                 Id = 1, Name = "Prod 01", Description = "description", Price = 1, Products = new List<ProductDTO>()
                 {
                     new ProductDTO{Id = 1, Name = "Prod 01", Description = "description"}
                 }
            };
        }
    }
}
