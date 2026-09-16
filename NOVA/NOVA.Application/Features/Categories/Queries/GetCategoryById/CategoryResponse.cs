using NOVA.Application.Features.Products.Queries.GetPRoducts;

namespace NOVA.Application.Features.Categories.Queries.GetCategoryById
{
    public class CategoryResponse
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductsResponse>? Products { get; set; }

    }
}
