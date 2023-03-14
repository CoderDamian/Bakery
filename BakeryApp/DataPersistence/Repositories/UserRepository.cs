using DomainModel;
using DomainModel.Contracts;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Persistence;
using System.Data;

namespace DataPersistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BakeryDbContext _bakeryDbContext;

        public UserRepository(BakeryDbContext bakeryDbContext)
        {
            this._bakeryDbContext = bakeryDbContext ?? throw new ArgumentNullException(nameof(bakeryDbContext));
        }

        public async Task<User?> GetUserByName(string name)
        {
            return await _bakeryDbContext.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(name))
                .ConfigureAwait(false);
        }

        public async Task SaveTokenByUserID(int userID, string refreshToken, DateTime expiryTime)
        {
            OracleParameter p_userId = new ("userId", userID);
            OracleParameter p_refreshToken = new ("refreshToken", refreshToken);
            OracleParameter p_expiryTime = new ("expiryTime", expiryTime);

            string sql = "Begin UpdateUserToken(:userId, :refreshToken, :expiryTime); End;";

            await _bakeryDbContext.Database
                .ExecuteSqlRawAsync(sql, p_userId, p_refreshToken, p_expiryTime)
                .ConfigureAwait(false);
        }

        public async Task UpdateRefreshTokenByUserID(int userId, string refreshToken)
        {
            OracleParameter p_userId = new("userid", userId);
            OracleParameter p_refreshToken = new("refreshtoken", refreshToken);

            string sql = "Begin updaterefreshtoken(:userid, :refreshtoken); End;";

            await _bakeryDbContext.Database
                .ExecuteSqlRawAsync(sql, p_userId, p_refreshToken)
                .ConfigureAwait(false);
        }

        public async Task<int> ValidateCredentials(string userName, string password)
        {
            int? userID = await _bakeryDbContext.Users
                .Where(u => u.Name == userName && u.Password == password)
                .Select(u => u.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (userID == null)
            {
                return -1; // This means the user doesn't exists.
            }
            else
            {
                return userID.Value;
            }
        }
    }
}
