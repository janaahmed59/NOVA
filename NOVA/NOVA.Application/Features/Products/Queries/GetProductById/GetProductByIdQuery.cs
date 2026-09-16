using MediatR;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Products.Queries.GetProductById
{
    public sealed record GetProductByIdQuery(int Id) : IRequest<Result<ProductResponse>>;
   
}
