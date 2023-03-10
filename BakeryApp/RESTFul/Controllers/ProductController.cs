using AppliationService.Contracts;
using Microsoft.AspNetCore.Mvc;
using DataTransferObjects.DTOs.Product;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet, Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ListProductsDTO productsDTO = await _productService.GetAllAsync().ConfigureAwait(false);

                return Ok(productsDTO);
            }
            catch (Exception)
            {
                return BadRequest();

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
