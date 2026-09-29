using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;
using NOVA.Application.Features.Products.Queries.GetProductById;
namespace NOVA.Application.Features.Products.Command.CreateProducts
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
    {
        private readonly IApplicationDbContext dbContext;
        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            dbContext = context;
        }
        public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var category = await dbContext.Categories
            .FirstOrDefaultAsync(
                c => c.Id == request.CategoryId,
                ct);

            if(category is null)
            {
                return Result<CreateProductResponse>.Failure(NovaErrors.NotFound);
            }
            var Product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                StockQuantity = request.StockQuantity,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            };
            if(request.Images is not null)
            {
                Product.Images = request.Images
                            .Select((url, index) => new ProductImage
                            {
                                ImageUrl = url,
                                IsMain = index == 0
                            }).ToList();
            }
            dbContext.Products.Add(Product);

            await dbContext.SaveChangesAsync(ct);

            var response = new CreateProductResponse
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                CategoryId = Product.CategoryId,
                StockQuantity = Product.StockQuantity,
                IsActive = Product.IsActive,
                CreatedAt = Product.CreatedAt,
                Images = Product.Images?.Select(image => new ProductImageResponse
                {
                    ImageUrl = image.ImageUrl
                }).ToList() ?? []
            };

            return Result<CreateProductResponse>.Success(response);
        }
    }
}
