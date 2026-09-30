using NOVA.Domain.Enums;

namespace NOVA.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusRequest(OrderStatus Status);
}