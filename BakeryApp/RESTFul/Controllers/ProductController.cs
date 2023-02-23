using AppliationService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;

namespace RESTFul.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync().ConfigureAwait(false);

                return Ok(products);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            try
            {
                ProductDTO productDTO = await _productService.GetByIDAsync(id).ConfigureAwait(false);

                return Ok(productDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
