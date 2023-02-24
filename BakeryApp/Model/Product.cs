using DomainModel.Seedwork;

namespace DomainModel
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageName { get; set; } = string.Empty;
    }
}