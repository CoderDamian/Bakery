namespace DomainModel.Contracts
{
    public interface IUserRepository
    {
        Task<bool> ValidateCredentials(User user);
    }
}
