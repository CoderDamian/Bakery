using Model.DTOs;

namespace AppliationService.Contracts
{
    public interface IProductService
    {
        Task<ProductDTO> GetByIDAsync(int id);
        Task<ListProductsDTO> GetAllAsync();
    }
}
