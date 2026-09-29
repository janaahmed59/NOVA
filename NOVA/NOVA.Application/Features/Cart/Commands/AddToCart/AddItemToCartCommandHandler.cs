using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Cart.Common;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;

namespace NOVA.Application.Features.Cart.Commands.AddToCart
{
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Result<CartResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public AddItemToCartCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<CartResponse>> Handle(AddItemToCartCommand request, CancellationToken ct)
        {
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<CartResponse>.Failure(NovaErrors.Unauthenticated);
            }

            // 1. check the product if it is exixt or not
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsActive, ct);

            if (product is null)
            {
                return Result<CartResponse>.Failure(NovaErrors.NotFound);
            }

            // 2. get the user cart or create new one if there is no cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)!
                    .ThenInclude(ci => ci.Product)!
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId.Value, ct);

            if (cart is null)
            {
                cart = new Domain.Entities.Cart
                {
                    UserId = userId.Value,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }

            // 3. check if the product is already in the cart?
            cart.CartItems ??= new List<CartItem>();
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);

            var totalDesiredQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;

            // 4. check the quntity
            if (product.StockQuantity < totalDesiredQuantity)
            {
                return Result<CartResponse>.Failure(
                    Error.Conflict("INSUFFICIENT_STOCK", $"Only {product.StockQuantity} items available in stock."));
            }

            // 5. add and update
            if (existingItem is not null)
            {
                existingItem.Quantity = totalDesiredQuantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    Product = product
                });
            }

            await _context.SaveChangesAsync(ct);

            // 6. the response
            var response = new CartResponse
            {
                Id = cart.Id,
                Items = cart.CartItems.Select(ci => new CartItemResponse
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name,
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    ProductImage = ci.Product?.Images?
                        .OrderByDescending(i => i.IsMain)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                }).ToList()
            };

            return Result<CartResponse>.Success(response);
        }
    }
}