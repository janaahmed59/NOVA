using MediatR;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;


namespace NOVA.Application.Features.Categories.Queries.GetCategories
{
    public sealed record GetCategoriesQuery(int Page, int PageSize)
        : IRequest<Result<PagedResult<CategoriesResponse>>>;
}
