using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Orders.Common;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Enums;

namespace NOVA.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public GetOrderByIdQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken ct)
        {
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<OrderResponse>.Failure(NovaErrors.Unauthenticated);
            }

            var isAdmin = _currentUser.Role == GlobalRole.Admin;

            // إذا كان أدمن يرى الطلب، وإذا كان مستخدم عادي يراه فقط لو كان ملكه
            var order = await _context.Orders.AsNoTracking()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.Id && (isAdmin || o.UserId == userId.Value), ct);

            if (order is null)
            {
                return Result<OrderResponse>.Failure(NovaErrors.NotFound);
            }

            var response = new OrderResponse
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                AddressId = order.AddressId,
                Items = order.OrderItems?.Select(oi => new OrderItemResponse
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList() ?? []
            };

            return Result<OrderResponse>.Success(response);
        }
    }
}