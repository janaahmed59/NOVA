using MediatR;
using NOVA.Application.Features.Cart.Common;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Cart.Commands.AddToCart
{
    public record AddItemToCartCommand(
            int ProductId,
            int Quantity = 1
        ) : IRequest<Result<CartResponse>>;
}
