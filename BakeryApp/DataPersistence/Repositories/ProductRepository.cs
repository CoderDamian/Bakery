using DomainModel;
using DomainModel.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace DataPersistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BakeryDbContext _bakeryDbContext;

        public ProductRepository(BakeryDbContext bakeryDbContext)
        {
            this._bakeryDbContext = bakeryDbContext ?? throw new ArgumentNullException(nameof(bakeryDbContext));
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _bakeryDbContext.Products.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Product?> GetProductByID(int id)
        {
            Product? product = await _bakeryDbContext.Products.FindAsync(id).ConfigureAwait(false);

            if (product == null)
            {
                throw new NullReferenceException(nameof(product));
            }
            else
            {
                return product;
            }
        }
    }
}
