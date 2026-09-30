using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Orders.Common;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Orders.Queries.GetUserOrders
{
    public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, Result<PagedResult<OrderResponse>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public GetUserOrdersQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<PagedResult<OrderResponse>>> Handle(GetUserOrdersQuery request, CancellationToken ct)
        {
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<PagedResult<OrderResponse>>.Failure(NovaErrors.Unauthenticated);
            }

            var query = _context.Orders.AsNoTracking()
                .Where(o => o.UserId == userId.Value)
                .OrderByDescending(o => o.CreatedAt); // أحدث الطلبات أولاً

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