using MediatR;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Cart.Commands.ClearCart
{
    public record ClearCartCommand : IRequest<Result<Unit>>;
}