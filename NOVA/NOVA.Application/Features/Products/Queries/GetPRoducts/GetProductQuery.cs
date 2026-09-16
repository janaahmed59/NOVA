using MediatR;
using NOVA.Domain.Common.Results;
using NOVA.Application.Pagination;

namespace NOVA.Application.Features.Products.Queries.GetPRoducts
{
    // page
    // pageSize

    public sealed record GetProductQuery(int Page, int PageSize) : IRequest<Result<PagedResult<ProductsResponse>>>;

}
