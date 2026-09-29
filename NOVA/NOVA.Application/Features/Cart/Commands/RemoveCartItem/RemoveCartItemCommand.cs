using MediatR;
using NOVA.Application.Features.Cart.Common;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Cart.Commands.RemoveCartItem
{
    public record RemoveCartItemCommand(int CartItemId) : IRequest<Result<CartResponse>>;
}