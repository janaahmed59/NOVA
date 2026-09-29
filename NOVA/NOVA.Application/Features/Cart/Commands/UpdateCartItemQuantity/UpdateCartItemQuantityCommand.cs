using MediatR;
using NOVA.Application.Features.Cart.Common;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Cart.Commands.UpdateCartItemQuantity
{
    public record UpdateCartItemQuantityCommand(
        int CartItemId,
        int Quantity
    ) : IRequest<Result<CartResponse>>;
}