using MediatR;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(
        int id,
        string? Name,
        string? Description,
        string? ImageUrl) : IRequest<Result<UpdateCategoryResponse>>;
   
}
