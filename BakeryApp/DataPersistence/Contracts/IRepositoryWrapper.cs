using DomainModel.Contracts;

namespace DataPersistence.Contracts
{
    public interface IRepositoryWrapper
    {
        public IProductRepository ProductRepository { get; }     
    }
}
