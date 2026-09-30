using MediatR;
using NOVA.Application.Features.Orders.Common;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(int Id) : IRequest<Result<OrderResponse>>;
}