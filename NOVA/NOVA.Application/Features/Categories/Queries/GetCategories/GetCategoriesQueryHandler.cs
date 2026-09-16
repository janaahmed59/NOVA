using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;
using NOVA.Application.Features.Products.Queries.GetPRoducts;
using NOVA.Domain.Entities;

namespace NOVA.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<PagedResult<CategoriesResponse>>>
    {
        private readonly IApplicationDbContext db;
        public GetCategoriesQueryHandler(IApplicationDbContext _db)
        {
            db = _db;
        }
        public async Task<Result<PagedResult<CategoriesResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var Categories = db.Categories.AsNoTracking()
                .Where(c => c.IsActive)
                .Select(cr => new CategoriesResponse
                {
                    Name = cr.Name,
                    Description = cr.Description,
                    ImageUrl = cr.ImageUrl,
                    Products = cr.Products.Select(p => new ProductsResponse
                    {
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryName = cr.Name,
                        ProductImage = p.Images
                        .Where(i => i.IsMain).
                        Select(pi => pi.ImageUrl)
                        .FirstOrDefault()
                    }).ToList()
                });
            var TotalCount = await Categories.CountAsync(cancellationToken);
            var pagedCategories = await Categories
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResult<CategoriesResponse>
            {
                Items = pagedCategories,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = TotalCount,
                TotalPages = (int)Math.Ceiling((double)TotalCount / request.PageSize)
            };
            return Result<PagedResult<CategoriesResponse>>.Success(pagedResult);
        }
    }
}
