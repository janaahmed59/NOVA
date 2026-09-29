using MediatR;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Products.Command.CreateProducts;
using NOVA.Application.Features.Products.Queries.GetProductById;
using NOVA.Domain.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace NOVA.Application.Features.Products.Command.UpdateProduct
{
    public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, Result<CreateProductResponse>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CreateProductResponse>> Handle(
            UpdateProductCommand request,
            CancellationToken ct)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == request.id && p.IsActive, ct);

            if (product is null)
            {
                return Result<CreateProductResponse>.Failure(
                    NovaErrors.NotFound);
            }

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId && c.IsActive, ct);

            if (!categoryExists)
            {
                return Result<CreateProductResponse>.Failure(
                    NovaErrors.NotFound);
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;
            product.StockQuantity = request.StockQuantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);

            var response = new CreateProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,

                Images = product.Images?
                    .Select(image => new ProductImageResponse
                    {
                        ImageUrl = image.ImageUrl
                    })
                    .ToList() ?? []
            };

            return Result<CreateProductResponse>.Success(response);
        }
    }
}
