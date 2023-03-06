using DataPersistence.Contracts;
using DataPersistence.Repositories;
using DomainModel.Contracts;
using Persistence;

namespace DataPersistence
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly BakeryDbContext _bakeryDbContext;

        public RepositoryWrapper(BakeryDbContext bakeryDbContext)
        {
            this._bakeryDbContext = bakeryDbContext ?? throw new ArgumentNullException(nameof(bakeryDbContext));
        }

        private IProductRepository productRepository = null!;

        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(_bakeryDbContext);

                return productRepository;
            }
        }

        private IUserRepository userRepository;

        public IUserRepository UserRepository
        {
            get 
            { 
                if (userRepository == null)
                    userRepository = new UserRepository(_bakeryDbContext);

                return userRepository;
            }
        }
    }
}
