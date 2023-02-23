namespace Model.DTOs
{
    public class ListProductsDTO
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public IEnumerable<Product> Products { get; set; } = null!;
    }
}
