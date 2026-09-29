using MediatR;
using NOVA.Application.Features.Cart.Common;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Cart.Queries.GetCart
{
    public record GetCartQuery : IRequest<Result<CartResponse>>;
}