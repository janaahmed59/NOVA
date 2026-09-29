using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;


namespace NOVA.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<UpdateCategoryResponse>>
    {
        private readonly IApplicationDbContext context;
        public UpdateCategoryCommandHandler(IApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<Result<UpdateCategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken ct )
        {
            var Category = await context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.id
                        && c.IsActive, ct);

            if(Category is null)
            {
                return Result<UpdateCategoryResponse>.Failure(NovaErrors.NotFound);
            }
            Category.Name = request.Name;
            Category.Description = request.Description;
            Category.ImageUrl = request.ImageUrl;

            await context.SaveChangesAsync(ct);
            var response = new UpdateCategoryResponse
            {
                Id = request.id,
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IsActive = Category.IsActive
            };
            return Result<UpdateCategoryResponse>.Success(response);

        }
    }
}
