using DomainModel;
using DomainModel.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace DataPersistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BakeryDbContext _bakeryDbContext;

        public UserRepository(BakeryDbContext bakeryDbContext)
        {
            this._bakeryDbContext = bakeryDbContext ?? throw new ArgumentNullException(nameof(bakeryDbContext));
        }

        public async Task<bool> ValidateCredentials(User user)
        {
            return await _bakeryDbContext.Users.AnyAsync(u => u.Name == user.Name && u.Password == user.Password).ConfigureAwait(false);
        }
    }
}
