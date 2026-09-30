using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Orders.Common;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;
using NOVA.Domain.Enums;

namespace NOVA.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public CreateOrderCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            // 1. التحقق من هوية المستخدم
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<OrderResponse>.Failure(NovaErrors.Unauthenticated);
            }

            // 2. التحقق من أن عنوان الشحن موجود ويخص هذا المستخدم
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == request.AddressId && a.UserId == userId.Value, ct);

            if (address is null)
            {
                return Result<OrderResponse>.Failure(NovaErrors.NotFound);
            }

            // 3. جلب سلة المستخدم الحالية
            var cart = await _context.Carts
                .Include(c => c.CartItems)!
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId.Value, ct);

            if (cart is null || cart.CartItems is null || !cart.CartItems.Any())
            {
                return Result<OrderResponse>.Failure(NovaErrors.CartIsEmpty);
            }

            // 4. التحقق من سلامة المنتجات وتوفر المخزون الكافي
            foreach (var item in cart.CartItems)
            {
                if (item.Product is null || !item.Product.IsActive)
                {
                    return Result<OrderResponse>.Failure(
                        Error.Business("PRODUCT_UNAVAILABLE", $"Product '{item.Product?.Name ?? item.ProductId.ToString()}' is no longer available."));
                }

                if (item.Product.StockQuantity < item.Quantity)
                {
                    return Result<OrderResponse>.Failure(
                        Error.Conflict("INSUFFICIENT_STOCK",
                            $"Insufficient stock for '{item.Product.Name}'. Available: {item.Product.StockQuantity}, Requested: {item.Quantity}."));
                }
            }

            // 5. إنشاء كائن الطلب الجديد
            var order = new Order
            {
                UserId = userId.Value,
                AddressId = address.Id,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            // 6. تحويل عناصر السلة إلى OrderItems وخصم المخزون
            foreach (var item in cart.CartItems)
            {
                // خصم الكمية من مخزون المنتج
                item.Product.StockQuantity -= item.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    UnitPrice = item.Product.Price, // تسجيل السعر في لحظة الشراء
                    Quantity = item.Quantity,
                    Order = order
                };

                totalAmount += orderItem.UnitPrice * orderItem.Quantity;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            // 7. تفريغ السلة بعد نجاح الطلب
            _context.CartItems.RemoveRange(cart.CartItems);

            // 8. حفظ الطلب وتحديث المخزون ومسح السلة دفعة واحدة
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(ct);

            // 9. تجهيز الـ Response
            var response = new OrderResponse
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                AddressId = order.AddressId,
                ShippingAddress = $"{address.Street}, {address.City}",
                Items = order.OrderItems.Select(oi => new OrderItemResponse
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            };

            return Result<OrderResponse>.Success(response);
        }
    }
}