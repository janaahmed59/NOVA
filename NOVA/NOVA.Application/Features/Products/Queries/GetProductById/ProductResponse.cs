using NOVA.Domain.Entities;
namespace NOVA.Application.Features.Products.Queries.GetProductById
{
    public class ProductResponse
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? CategoryName { get; set; }
        public List<ProductImageResponse>? ProductImages { get; set; } = new();
    }
}
