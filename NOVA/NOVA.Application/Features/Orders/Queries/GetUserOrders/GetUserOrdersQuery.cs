using MediatR;
using NOVA.Application.Features.Orders.Common;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Orders.Queries.GetUserOrders
{
    public record GetUserOrdersQuery(
        int Page = 1,
        int PageSize = 10
    ) : IRequest<Result<PagedResult<OrderResponse>>>;
}