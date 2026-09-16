using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Products.Queries.GetPRoducts;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;
namespace NOVA.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
    {
        private readonly IApplicationDbContext db;
        public GetCategoryByIdQueryHandler(IApplicationDbContext _db)
        {
            db = _db;
        }
        public async Task<Result<CategoryResponse>> Handle(
            GetCategoryByIdQuery request, CancellationToken ct)
        {
            
            var response = await db.Categories
                .Where(c => c.Id == request.id)
                .Select(c => new CategoryResponse
                {
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    Products = c.Products.Select(p => new ProductsResponse
                    {
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryName = c.Name,
                        ProductImage = p.Images.Where(i=> i.IsMain)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                    }).ToList()
                })
                .FirstOrDefaultAsync(ct);
            if (response is null)
            {
                return Result<CategoryResponse>.Failure(NovaErrors.NotFound);
            }
            return Result<CategoryResponse>.Success(response);
        }
    }
}
