using MediatR;
using NOVA.Application.Features.Orders.Common;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Orders.Commands.CreateOrder
{
    // يحتاج العميل فقط لتحديد عنوان الشحن لإتمام الشراء
    public record CreateOrderCommand(
        int AddressId
    ) : IRequest<Result<OrderResponse>>;
}