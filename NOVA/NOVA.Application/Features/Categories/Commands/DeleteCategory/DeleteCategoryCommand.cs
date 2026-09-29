using MediatR;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int Id) : IRequest<Result<Unit>>;

}
