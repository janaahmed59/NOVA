using MediatR;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Enums;

namespace NOVA.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(
        int OrderId,
        OrderStatus Status
    ) : IRequest<Result<Unit>>;
}