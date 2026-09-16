using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;

namespace NOVA.Application.Features.Products.Queries.GetPRoducts
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<PagedResult<ProductsResponse>>>
    {
        private readonly IApplicationDbContext DB;
        //represent the product with images + Reviews.
        public GetProductQueryHandler(IApplicationDbContext _DB)
        {
            DB = _DB;
        }
        public async Task<Result<PagedResult<ProductsResponse>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var products = DB.Products.AsNoTracking()
                .Where(p => p.IsActive)
                .Select(p => new ProductsResponse
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryName = p.Category.Name,
                    ProductImage = p.Images.Where(i => i.IsMain)
                        .Select(pi => pi.ImageUrl)
                        .FirstOrDefault()
                });
            var totalCount = await products.CountAsync(cancellationToken);
            var pagedProducts = await products
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new PagedResult<ProductsResponse>
            {
                Items = pagedProducts,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
