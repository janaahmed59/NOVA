using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
