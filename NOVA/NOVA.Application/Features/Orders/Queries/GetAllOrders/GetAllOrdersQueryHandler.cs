using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Orders.Common;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Result<PagedResult<OrderResponse>>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllOrdersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagedResult<OrderResponse>>> Handle(GetAllOrdersQuery request, CancellationToken ct)
        {
            var query = _context.Orders.AsNoTracking().AsQueryable();

            // إذا حدد الأدمن حالة معينة (مثل Pending أو Processing) نقوم بالفلترة
            if (request.Status.HasValue)
            {
                query = query.Where(o => o.Status == request.Status.Value);
            }

            query = query.OrderByDescending(o => o.CreatedAt);

            var totalCount = await query.CountAsync(ct);

            var orders = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(o => new OrderResponse
                {
                    Id = o.Id,
                    Status = o.Status.ToString(),
                    TotalAmount = o.TotalAmount,
                    CreatedAt = o.CreatedAt,
                    AddressId = o.AddressId,
                    Items = o.OrderItems!.Select(oi => new OrderItemResponse
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        UnitPrice = oi.UnitPrice,
                        Quantity = oi.Quantity
                    }).ToList()
                })
                .ToListAsync(ct);

            var pagedResult = new PagedResult<OrderResponse>
            {
                Items = orders,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };

            return Result<PagedResult<OrderResponse>>.Success(pagedResult);
        }
    }
}