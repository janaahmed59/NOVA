using MediatR;
using NOVA.Domain.Common.Results;
using NOVA.Application.Features.Products.Command.CreateProducts;

namespace NOVA.Application.Features.Products.Command.UpdateProduct
{
    public record UpdateProductCommand(
     int id,
     string Name,
     string? Description,
     decimal Price,
     int CategoryId,
     int StockQuantity
 ) : IRequest<Result<CreateProductResponse>>;
}
