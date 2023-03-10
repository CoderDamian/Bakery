namespace DomainModel.Contracts
{
    public interface IUserRepository
    {
        Task<int> ValidateCredentials(string userName, string password);
        Task SaveTokenByUserID(int userID, string refreshToken, DateTime expiryTime);
    }
}
