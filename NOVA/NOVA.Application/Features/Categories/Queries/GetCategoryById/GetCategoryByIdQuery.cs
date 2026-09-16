using MediatR;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Categories.Queries.GetCategoryById
{
    public sealed record GetCategoryByIdQuery(int id)
        : IRequest<Result<CategoryResponse>>;
}
