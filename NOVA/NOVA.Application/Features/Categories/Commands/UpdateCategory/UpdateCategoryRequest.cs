
namespace NOVA.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
