
using NOVA.Application.Features.Products.Queries.GetProductById;

namespace NOVA.Application.Features.Products.Command.CreateProducts
{
    public class CreateProductResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductImageResponse> Images { get; set; } = [];
    }
}
