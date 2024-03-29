﻿namespace DataTransferObjects.DTOs.Product
{
    public class ListProductsDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public IEnumerable<ProductDTO> Products { get; set; } = null!;
    }
}
