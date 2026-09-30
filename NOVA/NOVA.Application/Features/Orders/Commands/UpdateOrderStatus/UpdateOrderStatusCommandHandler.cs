using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Enums;

namespace NOVA.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateOrderStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(UpdateOrderStatusCommand request, CancellationToken ct)
        {
            // 1. جلب الطلب مع عناصره
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);

            if (order is null)
            {
                return Result<Unit>.Failure(NovaErrors.NotFound);
            }

            // 2. التحقق من القواعد المنطقية لتغيير الحالة
            if (order.Status == OrderStatus.Cancelled)
            {
                return Result<Unit>.Failure(
                    Error.Conflict("ORDER_ALREADY_CANCELLED", "Cannot update the status of an already cancelled order."));
            }

            if (order.Status == OrderStatus.Delivered && request.Status == OrderStatus.Cancelled)
            {
                return Result<Unit>.Failure(
                    Error.Conflict("CANNOT_CANCEL_DELIVERED", "Cannot cancel an order that has already been delivered."));
            }

            // 3. إذا تم إلغاء الطلب، نعيد المنتجات إلى المخزن تلقائياً
            if (request.Status == OrderStatus.Cancelled && order.OrderItems is not null)
            {
                var productIds = order.OrderItems.Select(oi => oi.ProductId).ToList();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync(ct);

                foreach (var item in order.OrderItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product is not null)
                    {
                        product.StockQuantity += item.Quantity; // استرجاع الكمية للمخزن
                    }
                }
            }

            // 4. تحديث الحالة وحفظ التغييرات
            order.Status = request.Status;
            await _context.SaveChangesAsync(ct);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}