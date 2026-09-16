using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
    {
        private readonly IApplicationDbContext context;
        public GetProductByIdQueryHandler(IApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await context.Products.AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(product => new ProductResponse
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CategoryName = product.Category.Name,
                    ProductImages = product.Images.Select(i => new ProductImageResponse
                    {
                        ImageUrl = i.ImageUrl
                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
            if (response == null)
            {
                return Result<ProductResponse>.Failure(NovaErrors.NotFound);
            }
            return Result<ProductResponse>.Success(response);
        }
    }
}
