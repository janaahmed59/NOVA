using MediatR;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Products.Command.DeleteProduct
{
    public record DeleteProductCommand(int id) : IRequest<Result<Unit>>;
}
