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

        public async Task SaveTokenByUserID(int userID, string refreshToken, DateTime expiryTime)
        {
            //await _bakeryDbContext.Database
            //    .ExecuteSqlInterpolatedAsync($"UPDATEUSERTOKEN @USERID={userID}, @REFRESHTOKEN={refreshToke}, @EXPITYTIME={expiryTime}")
            //    .ConfigureAwait(false);

            //OracleParameter userId = new OracleParameter("userId", OracleDbType.Int32, ParameterDirection.Input);
            //OracleParameter refreshToken = new OracleParameter("refreshToken", OracleDbType.NVarchar2, ParameterDirection.Input);
            //OracleParameter expiryTime = new OracleParameter("expiryTime", OracleDbType.Date, ParameterDirection.Input);

            OracleParameter p_userId = new OracleParameter("userId", userID);
            OracleParameter p_refreshToken = new OracleParameter("refreshToken", refreshToken);
            OracleParameter p_expiryTime = new OracleParameter("expiryTime", expiryTime);

            string sql = "Begin UpdateUserToken(:userId, :refreshToken, :expiryTime); End;";

#warning possible sql inject
            await _bakeryDbContext.Database
                .ExecuteSqlRawAsync(sql, p_userId, p_refreshToken, p_expiryTime)
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
