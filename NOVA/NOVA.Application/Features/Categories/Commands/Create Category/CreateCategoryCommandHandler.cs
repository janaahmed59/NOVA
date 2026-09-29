using MediatR;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;
namespace NOVA.Application.Features.Categories.Commands.Create_Category
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand , Result<CreateCategoryResponse>>
    {
        private readonly IApplicationDbContext context;
        public CreateCategoryCommandHandler(IApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand Request, CancellationToken ct)
        {
            var NewCategory = new Category
            {
                Name = Request.Name,
                Description = Request.Description,
                ImageUrl = Request.IamgeUrl,
                IsActive = true
            };
            context.Categories.Add(NewCategory);
            await context.SaveChangesAsync(ct);

            var response = new CreateCategoryResponse
            {
                Id = NewCategory.Id,
                Name = NewCategory.Name,
                Description = NewCategory.Description,
                ImageUrl = NewCategory.ImageUrl,
                IsActive = NewCategory.IsActive
            };

            return Result<CreateCategoryResponse>.Success(response);
        }
    }
}
