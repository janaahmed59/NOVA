using MediatR;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Products.Command.CreateProducts
{
    public record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    int CategoryId,
    int StockQuantity,
    List<string>? Images
) : IRequest<Result<CreateProductResponse>>;
}
