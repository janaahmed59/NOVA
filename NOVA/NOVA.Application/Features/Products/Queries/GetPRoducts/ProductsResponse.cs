using NOVA.Domain.Entities;
namespace NOVA.Application.Features.Products.Queries.GetPRoducts
{
    public class ProductsResponse
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? CategoryName { get; set; }
        public string? ProductImage { get; set; }
    }
}
